import { Instance, PageResponse, Response } from "../common";
import { NodeIDMapSummaryInfo, NodeTraceItemResponse, PathMapSummaryInfo } from "../model/nodes"

const controllerPrefix = "nodetrace";

export const GetAliasName = async (): Promise<Response<string>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/alias`).then((data: any) => {
        res = data;
    });
    return res;
}


export const GetState = async (): Promise<Response<boolean>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/state`).then((data: any) => {
        res = data;
    });
    return res;
}


export const GetSliceCount = async (): Promise<Response<number>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/slice/items/count`).then((data: any) => {
        res = data;
    });
    return res;
}


export const GetBlockCount = async (): Promise<Response<number>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/block/items/count`).then((data: any) => {
        res = data;
    });
    return res;
}


export const GetTraceCount = async (): Promise<Response<number>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/trace/items/count`).then((data: any) => {
        res = data;
    });
    return res;
}


export const GetFolderPath = async (): Promise<Response<string>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/folder/path`).then((data: any) => {
        res = data;
    });
    return res;
}


export const GetFolderName = async (): Promise<Response<string>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/folder/name`).then((data: any) => {
        res = data;
    });
    return res;
}


export const GetNodeCount = async (): Promise<Response<number>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/nodes/count`).then((data: any) => {
        res = data;
    });
    return res;
}


export const GetUnhandledItemCount = async (): Promise<Response<number>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/unhandled/items/count`).then((data: any) => {
        res = data;
    });
    return res;
}

export const GetAllNodeList = async (): Promise<Response<Array<NodeIDMapSummaryInfo>>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/nodes`).then((data: any) => {
        res = data;
        for (let index = 0; index < res.data.length; index++) {
            const item = res.data[index];
            item.key = item.orignalID;
            item.traceCount = "获取中..";
        }
    });
    return res;
}


export const GetNodeTraceCount = async (alias: string): Promise<Response<number>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/nodes/alias(${alias})/items/count`).then((data: any) => {
        res = data;
    });
    return res;
}


export const GetNodeSummaryInfo = async (alias: string): Promise<Response<NodeIDMapSummaryInfo>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/nodes/alias(${alias})`).then((data: any) => {
        res = data;
    });
    return res;
}
export const GetNodeTrace = async (traceID: string): Promise<Response<NodeTraceItemResponse>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/nodes/items/traceID(${traceID})`).then((data: any) => {
        res = data;
    });
    return res;
}
export const GetNodeTraceItemSummaryInfo = async (traceID: string): Promise<Response<number>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/nodes/items/traceID(${traceID})/summary`).then((data: any) => {
        res = data;
    });
    return res;
}
export const GetNodeAllPathInfo = async (nodeAlias: string, pageSize: number, pageIndex: number): Promise<PageResponse<Array<PathMapSummaryInfo>>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/nodes/alias(${nodeAlias})/items/path/items?pageSize=${pageSize}&pageIndex=${pageIndex}`).then((data: any) => {
        res = data;
    });
    return res;
}


export const GetNodePathItemCount = async (nodeAlias: string, pathAlias: string): Promise<Response<number>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/nodes/alias(${nodeAlias})/items/path/alias(${pathAlias})/count`).then((data: any) => {
        res = data;
    });
    return res;
}

export const GetTraceIDListByNodeAndPathAlias = async (nodeAlias: string, pathAlias: string, pageSize: number, pageIndex: number): Promise<PageResponse<Array<string>>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/nodes/alias(${nodeAlias})/items/path/alias(${pathAlias})/items/traceID?pageSize=${pageSize}&pageIndex=${pageIndex}`).then((data: any) => {
        res = data;
    });
    return res;
}