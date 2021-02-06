import { Button, Descriptions, Popover, Space } from "antd";
import React, { FC } from "react"
import { MethodInfoResponse } from '../../../../../api/model/methods'
import { CloseOutlined } from "@ant-design/icons"


interface indexProps {
    data: MethodInfoResponse | null;
    nodeX: number;
    nodeY: number;
    nodeSizeHeight: number;
    nodeSizeWidth: number;
    showClose?: boolean;
    onShowMethodClicked?: (eventID: string) => void;
    onCloseClicked?: () => void;
    popoverDivID: string;
}

const index: FC<indexProps> = (props) => {
    if (props.data == null) { return <></>; }
    let content = <Descriptions
        title={`函数名:${props.data.methodName} 事件ID:${props.data.methodEventID}`}
        bordered
        layout="vertical"
        extra={<Space>
            {props.showClose === true ?
                <Button
                    size="small"
                    danger
                    icon={<CloseOutlined />}
                    shape="circle"
                    type="dashed"
                    onClick={() => { props.onCloseClicked?.(); }} />
                : null}
        </Space>}
    >
        <Descriptions.Item label="来源文件">{props.data.fileName}</Descriptions.Item>
        <Descriptions.Item label="行号">{props.data.lineNumber}</Descriptions.Item>
        <Descriptions.Item label="总耗时(天:时:分:秒.秒的小数部分)">{props.data.duration}</Descriptions.Item>
        <Descriptions.Item label="请求时间区间" span={3}>{props.data.beginTime}至{props.data.endTime == null ? "未完成" : props.data.endTime} </Descriptions.Item>
        <Descriptions.Item label="直系节点">{props.data.children.length}个</Descriptions.Item>
    </Descriptions>;
    return <Popover overlayClassName={"node-trace-popover-overlay"} content={content}
        getPopupContainer={() => document.getElementById(props.popoverDivID) as HTMLElement}
        visible={true} >
        <div id={props.popoverDivID}
            style={{
                position: "fixed",
                left: props.nodeX,
                top: props.nodeY,
                height: props.nodeSizeHeight,
                width: props.nodeSizeWidth,
                cursor: "default"
            }}
            onClick={() => {
                props.data?.switchCollapsedState();
            }}
        >　</div>
    </Popover>
}

export default index;