import { Button, PageHeader, Spin } from 'antd'
import React, { Component } from "react"
import { RouteComponentProps, withRouter } from "react-router-dom"
import { NodeIDMapSummaryInfo, NodeTraceItemResponse } from '../../../api/model/nodes'
import { GetNodeSummaryInfo, GetNodeTrace } from '../../../api/resource/nodes'
import MyGraph from "./nodeTraceTreeGraph"
import ItemPopover from "./itemPopover"
import PathModal from "./path"
import MethodDrawer from "./methodDrawer"
import LogInfoDrawer from "./methodDrawer/logInfoDrawer"
import "./index.less"


interface NodeTraceDisplayProps extends RouteComponentProps {

}
interface NodeTraceDisplayState {
    nodeInfo?: NodeIDMapSummaryInfo;
    loading: boolean;
    showPathModel: boolean;
    currentTraceInfo: NodeTraceItemResponse | null;
    showItemTooltip: NodeTraceItemResponse | null;
    nodeCount: number;
    nodeX: number;
    nodeY: number;
    nodeSizeWidth: number;
    nodeSizeHeight: number;
    showLogEventID: string | null;
    eventID: string | null;
    showLogDrawer: boolean;
}

class nodeTraceDisplay extends Component<NodeTraceDisplayProps, NodeTraceDisplayState>{
    index: number = 2;
    constructor(props: NodeTraceDisplayProps) {
        super(props);
        this.state = {
            nodeX: 0,
            nodeY: 0,
            loading: true,
            nodeCount: 0,
            nodeInfo: undefined,
            showPathModel: false,
            currentTraceInfo: null,
            showItemTooltip: null,
            nodeSizeWidth: 32,
            nodeSizeHeight: 32,
            eventID: null,
            showLogEventID: null,
            showLogDrawer: false
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
        return <Spin spinning={this.state.loading} tip="?????????...">
            <PathModal show={this.state.showPathModel}
                onOk={() => { this.setState({ showPathModel: false }) }}
                onCancel={() => { this.setState({ showPathModel: false }) }}
                nodeAlias={(this.props.match.params as any).nodeAlias}
                onSelectTraceID={this.traceIDSelected}
                nodeID={this.state.nodeInfo?.orignalID!}
            />
            <div className={"display"} >
                <PageHeader
                    ghost={false}
                    title={this.state.nodeInfo != undefined ? `${this.state.nodeInfo.orignalID}??????` : ""}
                    subTitle={this.state.currentTraceInfo == null ? "" : `????????????????????????TraceID???:${this.state.currentTraceInfo.traceID}???????????????,???????????????${this.state.nodeCount}???????????????`}
                    className="page-header"
                    extra={[
                        <Button key="3" onClick={() => {
                            this.setState({
                                showPathModel: true
                            })
                        }}>??????Trace??????</Button>,
                        <Button key="2">Operation</Button>, ``
                    ]}
                />
                <MyGraph
                    onCreated={(nodeCount) => {
                        this.setState({
                            nodeCount: nodeCount
                        })
                    }}
                    data={this.state.currentTraceInfo}
                    showTooltips={(x: number, y: number, data: NodeTraceItemResponse, width: number, height: number) => {
                        this.setState({
                            showItemTooltip: data,
                            nodeX: x,
                            nodeY: y,
                            nodeSizeWidth: width,
                            nodeSizeHeight: height
                        });
                    }}
                />
            </div>
            <ItemPopover
                showClose={true}
                onCloseClicked={() => { this.setState({ showItemTooltip: null }) }}
                data={this.state.showItemTooltip}
                nodeX={this.state.nodeX}
                nodeY={this.state.nodeY}
                showNodeLogBtn={true}
                onShowNodeLogClicked={(eventID: string) => { this.setState({ showLogEventID: eventID, showLogDrawer: true }) }}
                nodeSizeHeight={this.state.nodeSizeHeight}
                nodeSizeWidth={this.state.nodeSizeWidth}
                onShowMethodClicked={(eventID) => {
                    this.setState({ eventID: eventID });
                }}
                popoverDivID={"nodeTraceItemPopover"}
            />
            <LogInfoDrawer
                eventID={this.state.showLogEventID}
                show={this.state.showLogDrawer}
                traceID={this.state.showItemTooltip?.traceID!}
                methodEventID={null}
                onColseClicked={() => { this.setState({ showLogEventID: null, showLogDrawer: false }) }}
                nodeInfo={this.state.showItemTooltip!}
                methodInfo={null}
            />
            <MethodDrawer eventID={this.state.eventID} item={this.state.showItemTooltip} onClose={() => {
                this.setState({
                    eventID: null
                })
            }} />

        </Spin>
    }
}

export default withRouter(nodeTraceDisplay);