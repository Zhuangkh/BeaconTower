import { Drawer } from "antd";
import React, { FC, useState, useEffect } from "react"
import MethodGraph from "./methodGraph"
import { GetMethodInfoByEventID } from "../../../../api/resource/methods"
import { NodeTraceItemResponse } from "../../../../api/model/nodes";

import "./index.less"
import { MethodInfoResponse } from "../../../../api/model/methods";
import { ResponseCode } from "../../../../api/common";
import NodeitemPopover from "../itemPopover"

interface indexProps {
    eventID: string | null;
    onClose: () => void;
    item: NodeTraceItemResponse | null;
}

const index: FC<indexProps> = (props) => {
    if (props.eventID == null) { return null; }
    const [data, setData] = useState<Array<MethodInfoResponse>>([]);
    const [nodeItemData, setNodeItemData] = useState<NodeTraceItemResponse | null>(null);
    const [methodItemData, setMethodItemData] = useState<MethodInfoResponse | null>(null);
    const [nodeX, setNodeX] = useState<number>(0);
    const [nodeY, setNodeY] = useState<number>(0);
    const [nodeSizeHeight, setNodeSizeHeight] = useState<number>(0);
    const [nodeSizeWidth, setNodeSizeWidth] = useState<number>(0);

    const fetch = async () => {
        const res = await GetMethodInfoByEventID(props.item?.traceID as string, props.eventID as string);
        if (res.code == ResponseCode.Success) {
            setData(res.data!);
        }

    }

    useEffect(() => {
        fetch();
    }, [])

    return <Drawer
        visible={true}
        title={"函数追踪详情"}
        onClose={() => { props.onClose() }}
        width="100vw"
        bodyStyle={{ padding: "0px" }}
    >
        <MethodGraph
            item={props.item!}
            data={data}
            showNodeTooltips={(x, y, currentWidth, currentHeight) => {
                setNodeX(x);
                setNodeY(y + 55);
                setMethodItemData(null);
                setNodeSizeWidth(currentWidth);
                setNodeSizeHeight(currentHeight);
                setNodeItemData(props.item);
            }}
            showMethodTooltips={(x, y, data, currentWidth, currentHeight) => {
                setNodeX(x);
                setNodeY(y + 55);
                setMethodItemData(data);
                setNodeItemData(null);
                setNodeSizeWidth(currentWidth);
                setNodeSizeHeight(currentHeight);
                setNodeItemData(props.item);
            }}
        />
        <NodeitemPopover
            data={nodeItemData}
            nodeX={nodeX}
            nodeY={nodeY}
            nodeSizeHeight={nodeSizeHeight}
            nodeSizeWidth={nodeSizeWidth}
            showClose={true}
            showMethodGraphBtn={false}
            onCloseClicked={() => {
                setNodeItemData(null);
            }}
            popoverDivID={"methodItemTraceNodeTraceItemPopover"}
        />

    </Drawer>
}
export default index;