import { Table } from "antd";
import React, { FC, useEffect, useState } from "react"
import { NodeIDMapSummaryInfo } from "../../../api/model/nodes";
import { GetAllNodeList, GetNodeTraceCount } from "../../../api/resource/nodes"
import "./index.less"

interface NodeListProps {

}

const columns = [{
    title: '节点名称',
    dataIndex: 'orignalID',
}, {
    title: '已有追踪数',
    dataIndex: 'traceCount',
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