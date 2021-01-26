import { Badge, Descriptions, PageHeader, Tooltip } from "antd"
import React, { FC, useEffect, useState } from "react"
import {GetAliasName, GetBlockCount, GetFolderName, GetFolderPath, GetSliceCount, GetState, GetTraceCount} from "../../api/resource/nodes"
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

    const fetchData = async () => {
        var getAliasName = GetAliasName();
        var getState = GetState();
        var getSliceCount = GetSliceCount();
        var getBlockCount = GetBlockCount();
        var getTraceCount = GetTraceCount();
        var getFolderPath = GetFolderPath();
        var getFolderName = GetFolderName();
        await Promise.all([getAliasName, getState, getSliceCount, getBlockCount, getTraceCount, getFolderPath, getFolderName]);
        setAliasName((await getAliasName).data as string);
        setState((await getState).data as boolean);
        setSliceCount((await getSliceCount).data as number);
        setBlockCount((await getBlockCount).data as number);
        setTraceCount((await getTraceCount).data as number);
        setFolderPath((await getFolderPath).data as string);
        setFolderName((await getFolderName).data as string);
    }

    useEffect(() => {
        fetchData();
    }, []);
    return <PageHeader
        ghost={false}
        title="节点数据库"
        subTitle="当前系统存储的节点追踪信息"
    >
        <Descriptions
            bordered
            title={<>别名:</>}
            size="small"
        >
            <DescriptionsItem label="分块总数">1</DescriptionsItem>
            <DescriptionsItem label="切片总数">2</DescriptionsItem>
            <DescriptionsItem label="存储数据量">3</DescriptionsItem>
            <DescriptionsItem label="运行状态">{true ? <Badge status="processing" text="运行中" /> : <Badge status="error" text="已停止" />}</DescriptionsItem>
            <DescriptionsItem label="文件根目录" >
                <Tooltip title={"asd"}>
                    <div className="root-folder-path">{"asd"}</div>
                </Tooltip>
            </DescriptionsItem>
            <DescriptionsItem label="对应文件夹">{"zxc"}</DescriptionsItem>
        </Descriptions>
    </PageHeader>
}

export default Node;