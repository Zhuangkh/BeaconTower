import G6, { Graph } from '@antv/g6'
import { Button, Tooltip } from 'antd'
import React, { FC, useState, useEffect } from "react"
import { useParams } from "react-router-dom"
import "./index.less"

interface MyGraphProps {

}
const initOpt = {
    width: 600,
    height: 400,
    maxZoom: 3,
    minZoom: 1,
    modes: {
        default: ['drag-canvas', 'zoom-canvas']
    },
}
const index: FC<MyGraphProps> = (props) => {

    const g6Ref = React.useRef<any>()
    let graph: Graph;

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

    useEffect(() => {
        graph = new G6.Graph({
            container: g6Ref.current as HTMLElement,
            ...initOpt
        });
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

