import React, { FC, useState, useEffect } from "react"
import { Badge, Descriptions, Tooltip } from "antd"
import "./index.less"

const DescriptionsItem = Descriptions.Item;

interface NodeSummaryInfoProps {
    aliasName: string;
    state: boolean;
    sliceCount: number;
    blockCount: number;
    traceCount: number;
    folderPath: string;
    folderName: string;
    nodesCount: number;
    unhandledItemCount: number;
}
const NodeSummaryInfo: FC<NodeSummaryInfoProps> = (props) => {


    return <Descriptions
        bordered
        title={<>DB别名:{props.aliasName}</>}
        size="small"
    >
        <DescriptionsItem label="分块总数">{props.blockCount}</DescriptionsItem>
        <DescriptionsItem label="切片总数">{props.sliceCount}</DescriptionsItem>
        <DescriptionsItem label="存储数据量">{props.traceCount}</DescriptionsItem>
        <DescriptionsItem label="运行状态">{props.state ? <Badge status="processing" text="运行中" /> : <Badge status="error" text="已停止" />}</DescriptionsItem>
        <DescriptionsItem label="文件根目录" >
            <Tooltip title={props.folderPath}>
                <div className="root-folder-path">{props.folderPath}</div>
            </Tooltip>
        </DescriptionsItem>
        <DescriptionsItem label="对应文件夹">{props.folderName}</DescriptionsItem>
        <DescriptionsItem label="已追踪Node个数">{props.nodesCount}</DescriptionsItem>
        <DescriptionsItem label="当前未落盘数据个数">{props.unhandledItemCount}</DescriptionsItem>
    </Descriptions>
}


export default NodeSummaryInfo;