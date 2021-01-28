import G6, { Graph, TreeGraph, TreeGraphData } from '@antv/g6'
import { Button, Tooltip } from 'antd'
import React, { FC, useState, useEffect, Component } from "react"
import { useParams } from "react-router-dom"
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
        getId: function getId(d: any) {
            return d.id;
        },
        getHeight: function getHeight() {
            return 16;
        },
        getWidth: function getWidth() {
            return 16;
        },
        getVGap: function getVGap() {
            return 80;
        },
        getHGap: function getHGap() {
            return 20;
        },
    },
    modes: {
        default: [
            {
                type: 'collapse-expand',
                shouldBegin: (e: any) => {

                    if (e.item && e.item.getModel().id === 'userNode') return false;
                    return true;
                },
            },
            'drag-canvas',
            'zoom-canvas',
        ],
    },
}

interface NodeTraceDisplayProps {

}
interface NodeTraceDisplayState {
    showTooltip: boolean;
    tooltipY: number;
    tooltipX: number;
}
export default class NodeTraceDisplay extends Component<NodeTraceDisplayProps, NodeTraceDisplayState>{
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
    state = {
        showTooltip: false,
        tooltipY: 0,
        tooltipX: 0,
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
    }
    render() {
        return <>
            <div style={{ position: "fixed", zIndex: 1000 }}>
                <Button onClick={() => {
                    if (this.graph == null) return;
                    this.graph.addChild({
                        id: (this.index.toString()), size: [32, 32], type: "image",
                        img: manSvg.default,
                        label: "用户",
                    }, "userNode");

                    this.index++;
                }}>添加</Button>
            </div>
            <div id="graph" ref={this.g6Ref} style={{ height: "100vh", width: "100%" }}>
                <Tooltip title={"asdasdasd"}
                    visible={this.state.showTooltip}
                    getPopupContainer={() => document.getElementById("graph") as HTMLElement}
                >
                    <div style={{ position: "fixed", left: this.state.tooltipX, top: this.state.tooltipY, cursor: "default" }}>　</div>
                </Tooltip>
            </div>
        </>
    }
}

// const NodeTraceDisplay: FC<NodeTraceDisplayProps> = (props) => {

//     const g6Ref = React.useRef<any>()
//     var graph: Graph;
//     let { nodeAlias } = useParams<any>();
//     const [showTooltip, setShowTooltip] = useState<boolean>(false);
//     const [tooltipX, setTooltipX] = useState<number>(0);
//     const [tooltipY, setTooltipY] = useState<number>(0);

//     const nodeMouseEnter = (evt: any) => {
//         const { item } = evt
//         const model = item.getModel();
//         const point = graph.getCanvasByPoint(model.x, model.y);
//         const y = point.y - model.size[1] * graph.getZoom() / 4;
//         const x = point.x - model.size[0]  / 4;
//         setTooltipX(x);
//         setTooltipY(y);
//         setShowTooltip(true);
//     }
//     const nodeMouseLevel = () => {
//         setShowTooltip(false);
//     }
//     const nodeClick = (item: any) => {
//         console.log(item);
//     }

//     useEffect(() => {
//         graph = new G6.Graph({
//             container: g6Ref.current as HTMLElement,
//             ...initOpt
//         });
//         graph.render();
//         graph.changeSize(
//             (g6Ref as any).current.scrollWidth,
//             (g6Ref as any).current.scrollHeight
//         );
//         graph.fitCenter();
//         graph.on('node:mouseenter', nodeMouseEnter);
//         graph.on('node:mouseleave', nodeMouseLevel);
//         graph.on('node:click', nodeClick); 
//     }, []);

//     return <>
//         <div style={{position:"fixed",zIndex:1000}}>
//         <Button onClick={() => {
//             graph.addItem("node", {
//                 id: index++, size: [32, 32], x: 64*index, y: 64, type: "image",
//                 img: manSvg.default,
//                 label: "用户", 
//             }, false);
//         }}>添加</Button>
//         </div>
//         <div id="graph" ref={g6Ref} style={{ height: "100vh", width: "100%" }}>
//             <Tooltip title={"asdasdasd"}
//                 visible={showTooltip}
//                 getPopupContainer={() => document.getElementById("graph") as HTMLElement}
//             >
//                 <div style={{ position: "fixed", left: tooltipX, top: tooltipY, cursor: "default"}}>　</div>
//             </Tooltip>
//         </div>

//     </>
// }

// export default NodeTraceDisplay;

