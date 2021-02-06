import G6, { TreeGraph, TreeGraphData } from "@antv/g6";
import { Drawer } from "antd";
import React, { FC, useState, useEffect } from "react"
import { MethodInfoResponse } from "../../../../../api/model/methods";
import { NodeTraceItemResponse, NodeType } from "../../../../../api/model/nodes";


import "./index.less"
const webapiSvg = require("../../../../../assets/webapi.svg")
const consoleSvg = require("../../../../../assets/console.svg")
const methodSvg = require("../../../../../assets/method.svg")
const manSvg = require("../../../../../assets/man.svg")

interface indexProps {
    item: NodeTraceItemResponse;
    data: Array<MethodInfoResponse>;
    showNodeTooltips: (x: number, y: number, currentWidth: number, currentHeight: number) => void;
    showMethodTooltips: (x: number, y: number, data: MethodInfoResponse, currentWidth: number, currentHeight: number) => void;
    hideTooltips?: () => void;
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
            const root = graph.findDataById("nodeTraceNode");
            if (root == null) { return; }
            if (code === 'unfold') {
                setCollapsed(root, false);
            } else if (code === 'fold') {
                setCollapsed(root, true);
            }
            graph.layout();
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

const index: FC<indexProps> = (props) => {
    if (props.data == null || props.data.length == 0) {
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
            if (model.id == "nodeTraceNode") {
                props.showNodeTooltips(x, y, 32 * graph.getZoom(), 32 * graph.getZoom());
            }
            else {
                var target = findItemObj(props.data, model.id);
                if (target == null) { return; }
                props.showMethodTooltips(x, y, target, 32 * graph.getZoom(), 32 * graph.getZoom());
            }
        }

    }
    const nodeMouseLevel = () => {

    }
    const findItemObj = (dataList: Array<MethodInfoResponse>, key: string): MethodInfoResponse | null => {
        for (let index = 0; index < dataList.length; index++) {
            const data = dataList[index];
            if (data.key == key) {
                return data;
            }
            var res = findItemObj(data.children, key);
            if (res != null) {
                return res;
            }
        }
        return null;
    }

    const pushChildData = (target: TreeGraphData, data: Array<MethodInfoResponse>) => {
        nodeCount++;
        for (let index = 0; index < data.length; index++) {
            const item = data[index];
            let thisLoop = {
                id: `${item.key}`,
                size: [32, 32],
                type: "image",
                img: methodSvg.default,
                label: item.methodName,
                collapsed: false,
                children: []
            };
            item.switchCollapsedState = () => {
                const subData = graph.findDataById(item.key);
                if (subData != null) {
                    subData.collapsed = !subData.collapsed;
                    graph.refreshLayout();
                }
            }
            target.children?.push(thisLoop);
            pushChildData(thisLoop, item.children);
        }
    }


    useEffect(() => {
        let nodeImg = webapiSvg.default;
        switch (props.item.type) {
            case NodeType.WebServer:
                nodeImg = webapiSvg.default;
                break;
            case NodeType.ConsoleApp:
                nodeImg = consoleSvg.default;
                break;
        }
        let data: TreeGraphData = {
            id: 'nodeTraceNode', size: [32, 32], type: "image",
            img: nodeImg,
            label: props.item.nodeID,
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
    }, []);

    return <div className="method-trace-graph" ref={g6Ref} style={{ height: "100%", width: "100%" }} />
}
export default index;