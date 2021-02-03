import { Button, Descriptions, PageHeader, Popover, Spin, Tooltip } from 'antd'
import React, { Component } from "react"
import { RouteComponentProps, withRouter } from "react-router-dom"
import { GetNodeTypeStr, NodeIDMapSummaryInfo, NodeTraceItemResponse } from '../../../api/model/nodes'
import { GetNodeSummaryInfo, GetNodeTrace } from '../../../api/resource/nodes'
import MyGraph from "./mygraph"
import PathModal from "./path"
import "./index.less"


interface NodeTraceDisplayProps extends RouteComponentProps {

}
interface NodeTraceDisplayState {
    nodeInfo?: NodeIDMapSummaryInfo;
    loading: boolean;
    showPathModel: boolean;
    currentTraceInfo: NodeTraceItemResponse | null;
    showItemTooltip: NodeTraceItemResponse | null;
    nodeX: number;
    nodeY: number;
    nodeSizeWidth: number;
    nodeSizeHeight: number;
}

class nodeTraceDisplay extends Component<NodeTraceDisplayProps, NodeTraceDisplayState>{
    index: number = 2;
    constructor(props: NodeTraceDisplayProps) {
        super(props);
        this.state = {
            nodeX: 0,
            nodeY: 0,
            loading: true,
            nodeInfo: undefined,
            showPathModel: false,
            currentTraceInfo: null,
            showItemTooltip: null,
            nodeSizeWidth: 32,
            nodeSizeHeight: 32,
        }
    }
    fetchData = async () => {
        let res = await GetNodeSummaryInfo((this.props.match.params as any).nodeAlias);
        this.setState({
            nodeInfo: res.data,
            loading: false
        });

    }

    traceIDSelected = async (traceID: string) => {
        let res = await GetNodeTrace(traceID);
        this.setState({
            currentTraceInfo: res.data as NodeTraceItemResponse,
            showPathModel: false
        });
    }
    componentDidMount() {
        this.fetchData();
    }

    getPopoverInfo = () => {
        if (this.state.showItemTooltip == null) { return <></>; }
        let content = <Descriptions
            title={`TraceID:${this.state.showItemTooltip.traceID} NodeID:${this.state.showItemTooltip.nodeID}`}
            bordered
            layout="vertical">
            <Descriptions.Item label="请求路径" span={3}>{this.state.showItemTooltip.path}</Descriptions.Item>
            <Descriptions.Item label="请求时间区间" span={3}>{this.state.showItemTooltip.beginTime}至{this.state.showItemTooltip.endTime == null ? "未完成" : this.state.showItemTooltip.endTime} </Descriptions.Item>
            <Descriptions.Item label="节点类型">{GetNodeTypeStr(this.state.showItemTooltip.type)}</Descriptions.Item>
            <Descriptions.Item label="总耗时">{this.state.showItemTooltip.useMS}ms</Descriptions.Item>
            <Descriptions.Item label="子节点个数">{this.state.showItemTooltip.nextNode.length}个</Descriptions.Item>

        </Descriptions>;
        return <Popover overlayClassName={"node-trace-popover-overlay"} content={content}
            getPopupContainer={() => document.getElementById("toolTipsDiv") as HTMLElement}
            visible={true} >
            <div id="toolTipsDiv" style={{
                position: "fixed",
                left: this.state.nodeX,
                top: this.state.nodeY,
                height: this.state.nodeSizeHeight,
                width: this.state.nodeSizeWidth, 
                cursor: "default"
            }} >　</div>
        </Popover>
    }

    render() {
        return <Spin spinning={this.state.loading} tip="加载中...">
            <PathModal show={this.state.showPathModel}
                onOk={() => { this.setState({ showPathModel: false }) }}
                onCancel={() => { this.setState({ showPathModel: false }) }}
                nodeAlias={(this.props.match.params as any).nodeAlias}
                onSelectTraceID={this.traceIDSelected}
            />
            <div className={"display"} >
                <PageHeader
                    ghost={false}
                    title={this.state.nodeInfo != undefined ? `${this.state.nodeInfo.orignalID}详情` : ""}
                    subTitle="当前页面可以查阅该节点的具体详细信息"
                    className="page-header"
                    extra={[
                        <Button key="3" onClick={() => {
                            this.setState({
                                showPathModel: true
                            })
                        }}>查看Trace列表</Button>,
                        <Button key="2">Operation</Button>, ``
                    ]}
                />
                <MyGraph data={this.state.currentTraceInfo}
                    showTooltips={(x: number, y: number, data: NodeTraceItemResponse, width: number, height: number) => {
                        this.setState({
                            showItemTooltip: data,
                            nodeX: x,
                            nodeY: y,
                            nodeSizeWidth: width,
                            nodeSizeHeight: height
                        });
                    }}

                    hideTooltips={() => {
                        this.setState({
                            showItemTooltip: null,
                        });
                    }}
                />
            </div>
            {
                this.state.showItemTooltip != null ?
                    this.getPopoverInfo()
                    : null
            }


        </Spin>
    }
}

export default withRouter(nodeTraceDisplay);