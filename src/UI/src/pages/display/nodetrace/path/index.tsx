import { Button, Modal, Table } from "antd";
import React, { FC, useEffect } from "react"
import "./index.less"

interface indexProps {
    show: boolean;
    onOk?: () => void;
    onCancel?: () => void;
}
const columns = [{
    title: "路径名称",
    width: "300px",
    dataIndex: "path"
}, {
    title: "已追踪",
    width: "150px",
    dataIndex: "traceItemCount"
}, {
    title: "操作",
    render: (item: any) => {
        return <Button>操作</Button>
    }
}]
const index: FC<indexProps> = (props) => {
    if (!props.show) {
        return null;
    }
    const fetch = async () => {

    }
    useEffect(() => {
        fetch();
    }, []);

    return <Modal
        className="node-path"
        title="Path列表"
        width="80vw"
        visible={true}
        onOk={() => {
            if (props.onOk != undefined) {
                props.onOk();
            }
        }}
        onCancel={() => {
            if (props.onCancel != undefined) {
                props.onCancel();
            }
        }}>
        <Table bordered columns={columns} />
    </Modal>
}

export default index;