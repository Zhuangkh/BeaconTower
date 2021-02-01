import G6, { TreeGraph, TreeGraphData } from '@antv/g6'
import React, { FC, useState, useEffect } from "react"
import { NodeTraceItemResponse } from '../../../../api/model/nodes'
import "./index.less"

const webapiSvg = require("../../../../assets/webapi.svg")
const methodSvg = require("../../../../assets/method.svg")
const manSvg = require("../../../../assets/man.svg")


interface MyGraphProps {
    data: NodeTraceItemResponse | null;
}
const initOpt = {
    width: 600,
    height: 400,
    maxZoom: 3,
    minZoom: 1,
    layout: {
        type: "compactBox",
        direction: 'LR',
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
const index: FC<MyGraphProps> = (props) => {
    if (props.data == null) {
        return null;
    }
    const g6Ref = React.useRef<any>()
    let graph: TreeGraph;

    const nodeMouseEnter = (evt: any) => {
        const { item } = evt
        const model = item.getModel();
        const point = graph.getCanvasByPoint(model.x, model.y);
        if (point != undefined) {
            const y = point.y - model.size[1] * graph.getZoom() / 4;
            const x = point.x - model.size[0] / 4;
        }
    }
    const nodeMouseLevel = () => {
        // setShowTooltip(false);
    }
    const nodeClick = (item: any) => {
        console.log(item);
    }

    const pushChildData = (target: TreeGraphData, data: NodeTraceItemResponse) => {
        let thisLoop = {
            id: `${data.nodeID}`,
            size: [32, 32],
            type: "image",
            img: manSvg.default,
            label: data.nodeID,
            collapsed: false,
            children: []
        };
        target.children?.push(thisLoop); 
        for (let index = 0; index < data.nextNode.length; index++) {
            const element = data.nextNode[index];
            pushChildData(thisLoop, element);
        }
    }

    useEffect(() => {
        let data: TreeGraphData = {
            id: 'userNode', size: [32, 32], type: "image",
            img: manSvg.default,
            label: "用户",
            collapsed: false,
            children: []
        };
        if (props.data != null) {
            pushChildData(data, props.data);
        }

        graph = new G6.TreeGraph({
            container: g6Ref.current as HTMLElement,
            ...initOpt
        });
        graph.data(data);
        graph.render();

        graph.changeSize(
            (g6Ref as any).current.scrollWidth,
            (g6Ref as any).current.scrollHeight
        );
        graph.fitCenter();
        graph.on('node:mouseenter', nodeMouseEnter);
        graph.on('node:mouseleave', nodeMouseLevel);
        graph.on('node:click', nodeClick);
    }, []);

    return <div id="graph" ref={g6Ref} style={{ height: "100vh", width: "100%" }} />

}

export default index;

