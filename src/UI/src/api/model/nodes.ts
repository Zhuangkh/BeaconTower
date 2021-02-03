
export interface NodeIDMapSummaryInfo {
    key: string;
    aliasName: string;
    orignalIDLength: number;
    orignalID: string;
    traceCount: string;
}

export interface PathMapSummaryInfo {
    key: string;
    aliasName: string;
    orignalPathLength: number;
    orignalPath: string;
    nodeAliasName: string;
    traceItemCount: string;
}

export interface NodeTraceItemResponse {
    key:string;
    traceID: string;
    nodeID: string;
    type: NodeType;
    beginTimeStamp: string;
    beginTime: string;
    endTimeStamp: string | null;
    endTime: string | null;
    useMS: number | null;
    previousNodeID: string;
    path: string;
    queryString: string;
    beginCustomData: any;
    endCustomData: any;
    nextNode: Array<NodeTraceItemResponse>;
}

export enum NodeType {
    WebServer = 0,
    MqConsumer = 1,
    Gateway = 2,
    AuthCentral = 3,
    ConsoleApp = 4,
    ClientApp = 5,
    Unset = 0xff,
}

export const GetNodeTypeStr = (type: NodeType): string => {
    switch (type) {
        case NodeType.WebServer:
            return "Web服务";
        case NodeType.MqConsumer:
            return "MQ消费者";
        case NodeType.Gateway:
            return "网关";
        case NodeType.AuthCentral:
            return "权限中心";
        case NodeType.ConsoleApp:
            return "控制台应用";
        case NodeType.ClientApp:
            return "客户端应用";
        case NodeType.Unset:
        default:
            return "未知";
    }
}