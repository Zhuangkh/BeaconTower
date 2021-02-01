import G6, { TreeGraph, TreeGraphData } from '@antv/g6'
import { Button, PageHeader, Spin, Tooltip } from 'antd'
import React, { Component } from "react"
import { RouteComponentProps, withRouter } from "react-router-dom"
import { NodeIDMapSummaryInfo } from '../../../api/model/nodes'
import { GetNodeSummaryInfo } from '../../../api/resource/nodes'
import PathModal from "./path"
import "./index.less"

const webapiSvg = require("../../../assets/webapi.svg")
const methodSvg = require("../../../assets/method.svg")
const manSvg = require("../../../assets/man.svg")

const initOpt = {
    width: 600,
    height: 400,
    maxZoom: 3,
    minZoom: 1,
    layout: {
        type: "compactBox",
        direction: 'TB',
    },
    modes: {
        default: [
            {
                type: 'collapse-expand',
                shouldBegin: (e: any) => {
                    if (e.item && e.item.getModel().id === 'userNode1') return false;
                    return true;
                },
            },
            'drag-canvas',
            'zoom-canvas',
        ],
    },
}

interface NodeTraceDisplayProps extends RouteComponentProps {

}
interface NodeTraceDisplayState {
    showTooltip: boolean;
    tooltipY: number;
    tooltipX: number;
    nodeInfo?: NodeIDMapSummaryInfo;
    loading: boolean;
    showPathModel: boolean;
}

class nodeTraceDisplay extends Component<NodeTraceDisplayProps, NodeTraceDisplayState>{
    g6Ref: any = React.createRef();
    graph: TreeGraph | null = null;
    index: number = 2;
    data: TreeGraphData = {
        id: 'userNode', size: [32, 32], type: "image",
        img: manSvg.default,
        label: "用户",
        collapsed: true,
        children: []
    };
    constructor(props: NodeTraceDisplayProps) {
        super(props);
        this.state = {
            showTooltip: false,
            tooltipY: 0,
            tooltipX: 0,
            loading: true,
            nodeInfo: undefined,
            showPathModel: false
        }
    }
    fetchData = async () => {
        let res = await GetNodeSummaryInfo((this.props.match.params as any).nodeAlias);
        this.setState({
            nodeInfo: res.data,
            loading: false
        });
    }
    nodeMouseLevel = () => {
        if (this.graph == null) { return; }
        this.setState({
            showTooltip: false
        });
    }
    nodeMouseEnter = (evt: any) => {
        if (this.graph == null) { return; }
        const { item } = evt;
        const model = item.getModel();
        const point = this.graph.getCanvasByPoint(model.x, model.y);
        const y = point.y - model.size[1] * this.graph.getZoom() / 4;
        const x = point.x - model.size[0] / 4;
        this.setState({
            tooltipX: x,
            tooltipY: y,
            showTooltip: true
        });

    }
    componentDidMount() {
        this.graph = new G6.TreeGraph({
            container: this.g6Ref.current as HTMLElement,
            ...initOpt
        });
        this.graph.data(this.data);
        this.graph.render();
        this.graph.changeSize(
            (this.g6Ref as any).current.scrollWidth,
            (this.g6Ref as any).current.scrollHeight
        );
        this.graph.fitCenter();
        this.graph.on('node:mouseenter', this.nodeMouseEnter);
        this.graph.on('node:mouseleave', this.nodeMouseLevel);
        this.fetchData();
    }
    render() {
        return <Spin spinning={this.state.loading} tip="加载中...">
            <PathModal show={this.state.showPathModel}
                onOk={() => { this.setState({ showPathModel: false }) }}
                onCancel={() => { this.setState({ showPathModel: false }) }}
                nodeAlias={(this.props.match.params as any).nodeAlias}
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
                {/* <div style={{ position: "fixed", zIndex: 1000 }}>
                <Button onClick={() => {
                    if (this.graph == null) return;
                    this.graph.addChild({
                        id: (this.index.toString()), size: [32, 32], type: "image",
                        img: manSvg.default,
                        label: "用户",
                    }, "userNode");

                    this.index++;
                }}>添加</Button>
            </div> */}
                <div id="graph" className="graph" ref={this.g6Ref} >
                    <Tooltip title={"asdasdasd"}
                        visible={this.state.showTooltip}
                        getPopupContainer={() => document.getElementById("graph") as HTMLElement}
                    >
                        <div style={{ position: "fixed", left: this.state.tooltipX, top: this.state.tooltipY, cursor: "default" }}>　</div>
                    </Tooltip>
                </div>
            </div>
        </Spin>
    }
}

export default withRouter(nodeTraceDisplay);