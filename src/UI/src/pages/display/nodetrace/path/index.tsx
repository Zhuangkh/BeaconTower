import { Button, Drawer } from "antd";
import React, { FC } from "react"
import PathTable from "./pathTable"
import "./index.less"

interface indexProps {
    show: boolean;
    onOk?: () => void;
    onCancel?: () => void;
    nodeAlias: string;
}

const nodeTraceItemColumns = [{
    title: "TraceID",
    width: "300px",
    dataIndex: "orignalPath"
}, {
    title: "操作",
    render: (item: any) => {
        return <Button type="primary" shape="round">在图中查看</Button>
    }
}];
const index: FC<indexProps> = (props) => {
    if (!props.show) {
        return null;
    }
    
    return <Drawer
        maskClosable={false}
        className="node-path"
        title="Path列表"
        width="100vw"
        visible={true}
        closable={false}
        footer={<Button onClick={() => {
            if (props.onOk != undefined) {
                props.onOk();
            }
        }}>关闭</Button>}
    >
        <PathTable nodeAlias={props.nodeAlias} />


    </Drawer>
}

export default index;