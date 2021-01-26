import { Badge, Descriptions, Divider, PageHeader, Skeleton, Space, Spin, Tooltip } from "antd"
import React, { FC, useEffect, useState } from "react"
import { GetAliasName, GetBlockCount, GetFolderName, GetFolderPath, GetNodeCount, GetSliceCount, GetState, GetTraceCount, GetUnhandledItemCount } from "../../api/resource/nodes"
import NodeSummaryInfo from "./summary"
import NodeList from "./list"
import "./index.less"



interface NodeProps {

}

const Node: FC<NodeProps> = (props) => {
    const [aliasName, setAliasName] = useState<string>("");
    const [state, setState] = useState<boolean>(false);
    const [sliceCount, setSliceCount] = useState<number>(0);
    const [blockCount, setBlockCount] = useState<number>(0);
    const [traceCount, setTraceCount] = useState<number>(0);
    const [folderPath, setFolderPath] = useState<string>("");
    const [folderName, setFolderName] = useState<string>("");
    const [nodesCount, setNodesCount] = useState<number>(0);
    const [unhandledItemCount, setUnhandledItemCount] = useState<number>(0);
    const [loading, setLoading] = useState<boolean>(true);


    const fetchData = async () => {
        var getAliasName = GetAliasName();
        var getState = GetState();
        var getSliceCount = GetSliceCount();
        var getBlockCount = GetBlockCount();
        var getTraceCount = GetTraceCount();
        var getFolderPath = GetFolderPath();
        var getFolderName = GetFolderName();
        var getNodesCount = GetNodeCount();
        var getUnhandledItemCount = GetUnhandledItemCount();
        await Promise.all([getAliasName
            , getState
            , getSliceCount
            , getBlockCount
            , getTraceCount
            , getFolderPath
            , getFolderName
            , getUnhandledItemCount]);
        setAliasName((await getAliasName).data as string);
        setState((await getState).data as boolean);
        setSliceCount((await getSliceCount).data as number);
        setBlockCount((await getBlockCount).data as number);
        setTraceCount((await getTraceCount).data as number);
        setFolderPath((await getFolderPath).data as string);
        setFolderName((await getFolderName).data as string);
        setNodesCount((await getNodesCount).data as number);
        setUnhandledItemCount((await getUnhandledItemCount).data as number);

        setLoading(false);
    }
    const showSkeleton = () => {
        return <Skeleton active />
    }

    useEffect(() => {
        fetchData();
    }, []);
    return <PageHeader
        ghost={false}
        title="节点数据库"
        className="node-instance"
        subTitle="当前系统存储的节点追踪信息"
    >
        {loading === true ? showSkeleton() :
            <NodeSummaryInfo
                aliasName={aliasName}
                state={state}
                sliceCount={sliceCount}
                blockCount={blockCount}
                traceCount={traceCount}
                folderPath={folderPath}
                folderName={folderName}
                nodesCount={nodesCount}
                unhandledItemCount={unhandledItemCount}

            />}
        <Divider />
        <NodeList />
    </PageHeader>
}

export default Node;