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

const NodeList: FC<NodeListProps> = (props) => {

    const [loading, setLoading] = useState<boolean>(true);
    const [data, setData] = useState<Array<NodeIDMapSummaryInfo>>();

    const fetchData = async () => {
        var dataList = await GetAllNodeList();
        setLoading(false);
        setData(dataList.data as Array<NodeIDMapSummaryInfo>);
        dataList.data?.forEach(async (item) => {
            let res = await GetNodeTraceCount(item.aliasName);
            item.traceCount = res.data?.toString() as string;
            setData([...(dataList.data as Array<NodeIDMapSummaryInfo>)])
        });
    }

    useEffect(() => {
        fetchData();
    }, []);

    return <Table
        bordered
        loading={loading}
        columns={columns}
        dataSource={data}
    />
}

export default NodeList;