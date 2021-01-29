import { Modal } from "antd";
import React, { FC, useEffect } from "react"
import "./index.less"

interface indexProps {
    show: boolean;
    onOk?: () => void;
    onCancel?: () => void;
}

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
        <p>Some contents...</p>
        <p>Some contents...</p>
        <p>Some contents...</p>
    </Modal>
}

export default index;