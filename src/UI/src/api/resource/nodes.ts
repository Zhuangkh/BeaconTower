import { Instance, Response } from "../common";

const controllerPrefix="nodetrace";

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