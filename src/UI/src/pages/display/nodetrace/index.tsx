import G6 from '@antv/g6'
import React, { FC, useState, useEffect } from "react"
import { useParams } from "react-router-dom"

import "./index.less"


interface NodeTraceDisplayProps {

}
const data = {
    nodes: [
        { id: 'node0', size: 50 },
        { id: 'node1', size: 130 },
    ],
    edges: [
        { source: 'node0', target: 'node1' },
    ],
};
const NodeTraceDisplay: FC<NodeTraceDisplayProps> = (props) => {

    const g6Ref = React.useRef<any>()
    let { nodeAlias } = useParams<any>();
    useEffect(() => {
        // 实例化 Minimap 

        // 实例化 Graph
        let graph = new G6.Graph({            
            container: g6Ref.current as HTMLElement,
            width: 600,
            height: 400, 
            modes: {
                default: ['drag-canvas', 'zoom-canvas']
            },
            defaultNode: {
                type: 'circle',
                labelCfg: {
                    style: {
                        fill: '#000000A6',
                        fontSize: 10
                    }
                },
                style: {
                    stroke: '#72CC4A',
                    width: 150
                }
            },
            defaultEdge: {
                type: 'line'
            },
            layout: {
                type: 'force',
                preventOverlap: true,
                linkDistance: (d: any) => {
                    if (d.source.id === 'node0') {
                        return 100;
                    }
                    return 30;
                },
            },
            nodeStateStyles: {
                hover: {
                    stroke: 'red',
                    lineWidth: 3
                }
            },
            edgeStateStyles: {
                hover: {
                    stroke: 'blue',
                    lineWidth: 3
                }
            }
        })

        graph.data(data)

        graph.render();
        graph.changeSize(
            (g6Ref as any).current.scrollWidth,
            (g6Ref as any).current.scrollHeight
        );
        

        graph.on('node:mouseenter', (evt: any) => {
            graph.setItemState(evt.item, 'hover', true)
        })

        graph.on('node:mouseleave', (evt: any) => {
            graph.setItemState(evt.item, 'hover', false)
        })

        graph.on('edge:mouseenter', (evt: any) => {
            graph.setItemState(evt.item, 'hover', true)
        })

        graph.on('edge:mouseleave', (evt: any) => {
            graph.setItemState(evt.item, 'hover', false)
        })
    }, []);

    return <div id="graph" ref={g6Ref} style={{ height: "100vh", width: "100%" }}></div>
}

export default NodeTraceDisplay;
