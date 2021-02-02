import { Button, Space, Table } from "antd";
import React, { FC, useEffect, useState } from "react"
import { NodeIDMapSummaryInfo } from "../../../api/model/nodes";
import { GetAllNodeList, GetNodeTraceCount } from "../../../api/resource/nodes"
import { FundTwoTone } from "@ant-design/icons"
import "./index.less"

interface NodeListProps {

}

const columns = [{
    title: '节点名称',
    width: `400px`,
    dataIndex: 'orignalID',
}, {
    title: '已追踪',
    width: `100px`,
    dataIndex: 'traceCount',
}, {
    title: '操作',
    render: (item: NodeIDMapSummaryInfo) => {
        return <Space>
            <Button icon={<FundTwoTone />} type="primary" shape="round"
                onClick={() => {
                    window.open(`/#/display/${item.aliasName}`)
                }}
            >查看Node详情</Button>
        </Space>
    }
},];

const pageSize = 10;
const NodeList: FC<NodeListProps> = (props) => {

    const [loading, setLoading] = useState<boolean>(true);
    const [data, setData] = useState<Array<NodeIDMapSummaryInfo>>();
    const [pageIndex, setPageIndex] = useState<number>(1);
    const getAllCount = async (tempData: Array<NodeIDMapSummaryInfo>) => {
        let allJob = [];
        for (let index = 0; index < tempData.length; index++) {
            const element = tempData[index];
            var load = async () => {
                let countRes = await GetNodeTraceCount(element.aliasName);
                if (countRes.data != undefined && countRes.data != null) {
                    element.traceCount = countRes.data.toString();
                }
            }
            allJob.push(load());
        }
        await Promise.all(allJob);
        setData([...tempData]);
    }
    const fetchData = async (index: number) => {
        var dataList = await GetAllNodeList(pageSize, index);
        setLoading(false);
        setData(dataList.data as Array<NodeIDMapSummaryInfo>);
        getAllCount(dataList.data as Array<NodeIDMapSummaryInfo>); 
    }

    useEffect(() => {
        fetchData(1);
    }, []);

    return <Table
        bordered
        loading={loading}
        columns={columns}
        dataSource={data}
        pagination={{
            current: pageIndex,
            onChange: (page) => {
                setPageIndex(page);
                fetchData(pageIndex);
            }
        }}
    />
}

export default NodeList;