import { Button, Drawer } from "antd";
import React, { FC, useState } from "react"
import PathTable from "./pathTable"
import TraceItemTable from "./traceItemTable"
import "./index.less"

interface indexProps {
    show: boolean;
    onOk?: () => void;
    onCancel?: () => void;
    nodeAlias: string;
    nodeID: string;
    onSelectTraceID: (traceID: string) => void;
}

const index: FC<indexProps> = (props) => {
    if (!props.show) {
        return null;
    }
    const [pathItem, setPathItem] = useState<string | null>(null);

    return <Drawer
        maskClosable={false}
        className="node-path"
        title={`节点${props.nodeID}的Path列表`}
        width="100vw"
        visible={true}
        closable={false}
        footer={<Button onClick={() => {
            if (props.onOk != undefined) {
                props.onOk();
            }
        }}>关闭</Button>}
    >
        <PathTable nodeAlias={props.nodeAlias} onSelectPathItem={(item) => { setPathItem(item) }} />
        <TraceItemTable nodeAlias={props.nodeAlias} pathAlias={pathItem} onSelectTraceItem={(traceID: string) => {
            props.onSelectTraceID(traceID);
        }} />

    </Drawer>
}

export default index;