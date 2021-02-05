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
    onCreated: (nodeCount: number) => void;
}

const setCollapsed = (target: TreeGraphData, state: boolean) => {
    if (target.id != "userNode") {
        target.collapsed = state;
    }

    if (target.children != null && target.children != undefined) {
        for (let index = 0; index < target.children.length; index++) {
            const element = target.children[index];
            setCollapsed(element, state);
        }
    }
}

const initOpt = {
    width: 600,
    height: 400,
    maxZoom: 3,
    minZoom: 0.3,
    plugins: [new G6.ToolBar({
        getContent: () => {
            return `
            <ul>
              <li code='fold' title='折叠所有'><svg viewBox="64 64 896 896" focusable="false" data-icon="menu-fold" width="28px" height="28px" fill="currentColor" aria-hidden="true"><path d="M408 442h480c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8H408c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8zm-8 204c0 4.4 3.6 8 8 8h480c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8H408c-4.4 0-8 3.6-8 8v56zm504-486H120c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h784c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8zm0 632H120c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h784c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8zM115.4 518.9L271.7 642c5.8 4.6 14.4.5 14.4-6.9V388.9c0-7.4-8.5-11.5-14.4-6.9L115.4 505.1a8.74 8.74 0 000 13.8z"></path></svg></li>
              <li code='unfold' title='展开所有'><svg viewBox="64 64 896 896" focusable="false" data-icon="menu-unfold" width="28px" height="28px" fill="currentColor" aria-hidden="true"><path d="M408 442h480c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8H408c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8zm-8 204c0 4.4 3.6 8 8 8h480c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8H408c-4.4 0-8 3.6-8 8v56zm504-486H120c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h784c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8zm0 632H120c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h784c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8zM142.4 642.1L298.7 519a8.84 8.84 0 000-13.9L142.4 381.9c-5.8-4.6-14.4-.5-14.4 6.9v246.3a8.9 8.9 0 0014.4 7z"></path></svg></li>
            </ul>
          `
        },
        handleClick: (code: any, graph: TreeGraph) => {
            const root = graph.findDataById("userNode");
            if (root == null) { return; }
            if (code === 'unfold') {
                setCollapsed(root, false);
            } else if (code === 'fold') {
                setCollapsed(root, true);
            }
            graph.refreshLayout();
        }
    })],
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
    let nodeCount: number = 0;
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
        nodeCount++;
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
        data.switchCollapsedState = () => {
            const subData = graph.findDataById(data.key);
            if (subData != null) {
                subData.collapsed = !subData.collapsed;
                graph.refreshLayout();
            }
        }
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
        props.onCreated(nodeCount);
    }, []);

    return <div id="graph" className="my-graph" ref={g6Ref} style={{ height: "100vh", width: "100%" }} />

}

export default index;

