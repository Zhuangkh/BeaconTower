
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
    TraceID: string;
    NodeID: string
    Type: NodeType;
    BeginTimeStamp: string
    BeginTime: string;
    EndTimeStamp: string | null;
    EndTime: string | null;
    UseMS: number | null;
    PreviousNodeID: string;
    Path: string;
    QueryString: string;
    BeginCustomData: any;
    EndCustomData: any;
    NextNode: Array<NodeTraceItemResponse>;
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