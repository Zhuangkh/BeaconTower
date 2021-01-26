import { Badge, Descriptions, PageHeader, Tooltip } from "antd"
import React, { FC, useEffect, useState } from "react"
import { GetAliasName, GetBlockCount, GetFolderName, GetFolderPath, GetNodeCount, GetSliceCount, GetState, GetTraceCount, GetUnhandledItemCount } from "../../api/resource/nodes"
import "./index.less"


const DescriptionsItem = Descriptions.Item;

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
        <Descriptions
            bordered
            title={<>DB别名:{aliasName}</>}
            size="small"
        >
            <DescriptionsItem label="分块总数">{blockCount}</DescriptionsItem>
            <DescriptionsItem label="切片总数">{sliceCount}</DescriptionsItem>
            <DescriptionsItem label="存储数据量">{traceCount}</DescriptionsItem>
            <DescriptionsItem label="运行状态">{state ? <Badge status="processing" text="运行中" /> : <Badge status="error" text="已停止" />}</DescriptionsItem>
            <DescriptionsItem label="文件根目录" >
                <Tooltip title={folderPath}>
                    <div className="root-folder-path">{folderPath}</div>
                </Tooltip>
            </DescriptionsItem>
            <DescriptionsItem label="对应文件夹">{folderName}</DescriptionsItem>
            <DescriptionsItem label="已追踪Node个数">{nodesCount}</DescriptionsItem>
            <DescriptionsItem label="当前未落盘数据个数">{unhandledItemCount}</DescriptionsItem>
        </Descriptions>
    </PageHeader>
}

export default Node;