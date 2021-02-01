
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