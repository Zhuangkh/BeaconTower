import { Button, Descriptions, Popover, Space } from "antd";
import React, { FC } from "react"
import { GetNodeTypeStr, NodeTraceItemResponse } from '../../../../api/model/nodes'
import { CloseOutlined } from "@ant-design/icons"


interface indexProps {
    data: NodeTraceItemResponse | null;
    nodeX: number;
    nodeY: number;
    nodeSizeHeight: number;
    nodeSizeWidth: number;
    showClose?: boolean;
    onShowMethodClicked?: (eventID: string) => void;
    onShowNodeLogClicked?: (eventID: string) => void;
    onCloseClicked?: () => void;
    showMethodGraphBtn?: boolean;
    showNodeLogBtn?: boolean;
    popoverDivID: string;
}

const index: FC<indexProps> = (props) => {
    if (props.data == null) { return <></>; }
    let content = <Descriptions
        title={`节点:${props.data.nodeID} 事件ID:${props.data.eventID}`}
        bordered
        layout="vertical"
        extra={<Space>
            {props.showMethodGraphBtn === false ? null :
                <Button shape="round" onClick={() => {
                    props.onShowMethodClicked?.(props.data?.eventID!);
                }}>节点函数</Button>
            }
            {props.showNodeLogBtn === true ?
                <Button shape="round" onClick={() => {
                    props.onShowNodeLogClicked?.(props.data?.eventID!);
                }}>节点日志</Button>
                : null
            }
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
        <Descriptions.Item label="请求路径" span={3}>{props.data.path}</Descriptions.Item>
        <Descriptions.Item label="请求时间区间" span={3}>{props.data.beginTime}至{props.data.endTime == null ? "未完成" : props.data.endTime} </Descriptions.Item>
        <Descriptions.Item label="节点类型">{GetNodeTypeStr(props.data.type)}</Descriptions.Item>
        <Descriptions.Item label="总耗时(天:时:分:秒.秒的小数部分)">{props.data.duration}</Descriptions.Item>
        <Descriptions.Item label="直系节点">{props.data.nextNode.length}个</Descriptions.Item>
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