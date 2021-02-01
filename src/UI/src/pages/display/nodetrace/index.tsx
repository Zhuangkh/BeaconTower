import { Button, PageHeader, Spin, Tooltip } from 'antd'
import React, { Component } from "react"
import { RouteComponentProps, withRouter } from "react-router-dom"
import { NodeIDMapSummaryInfo, NodeTraceItemResponse } from '../../../api/model/nodes'
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
            showItemTooltip: null
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
                    showTooltips={(x: number, y: number, data: NodeTraceItemResponse) => {
                        this.setState({
                            showItemTooltip: data,
                            nodeX: x,
                            nodeY: y
                        });
                        console.log(`x:${x} y:${y}`)
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
                    <Tooltip visible={true} getPopupContainer={() => document.getElementById("toolTipsDiv") as HTMLElement} title={this.state.showItemTooltip.nodeID}>
                        <div id="toolTipsDiv" style={{ position: "fixed", left: this.state.nodeX, top: this.state.nodeY }} >　</div>
                    </Tooltip>
                    : null
            }
        </Spin>
    }
}

export default withRouter(nodeTraceDisplay);