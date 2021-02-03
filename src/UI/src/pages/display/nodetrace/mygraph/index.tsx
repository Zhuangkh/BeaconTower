import G6, { TreeGraph, TreeGraphData } from '@antv/g6'
import React, { FC, useState, useEffect } from "react"
import { NodeTraceItemResponse, NodeType } from '../../../../api/model/nodes'
import "./index.less"

const webapiSvg = require("../../../../assets/webapi.svg")
const consoleSvg = require("../../../../assets/console.svg")
const methodSvg = require("../../../../assets/method.svg")
const manSvg = require("../../../../assets/man.svg")


interface MyGraphProps {
    data: NodeTraceItemResponse | null;
    showTooltips: (x: number, y: number, data: NodeTraceItemResponse, currentWidth: number, currentHeight: number) => void;
    hideTooltips: () => void;
}
const initOpt = {
    width: 600,
    height: 400,
    maxZoom: 3,
    minZoom: 0.3,
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
            const y = point.y - 32 * graph.getZoom() / 2;
            const x = point.x - 32 * graph.getZoom() / 2;
            if (props.data == null) {
                return;
            }
            var target = findItemObj(props.data, model.id);
            if (target == null) { return; }
            props.showTooltips(x, y, target, 32 * graph.getZoom(), 32 * graph.getZoom());
        }

    }
    const nodeMouseLevel = () => {
        props.hideTooltips();
    }
    const nodeClick = (item: any) => {
        console.log(item);
    }
    const findItemObj = (data: NodeTraceItemResponse, key: string): NodeTraceItemResponse | null => {
        if (data.key == key) {
            return data;
        }
        for (let index = 0; index < data.nextNode.length; index++) {
            const element = data.nextNode[index];
            var res = findItemObj(element, key);
            if (res != null) {
                return res;
            }
        }
        return null;
    }

    const pushChildData = (target: TreeGraphData, data: NodeTraceItemResponse) => {
        let imgType: any = manSvg.default;
        switch (data.type) {
            case NodeType.WebServer:
                imgType = webapiSvg.default;
                break;
            case NodeType.ConsoleApp:
                imgType = consoleSvg.default;
                break;
        }
        let thisLoop = {
            id: `${data.key}`,
            size: [32, 32],
            type: "image",
            img: imgType,
            label: data.nodeID,
            collapsed: true,
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

