
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