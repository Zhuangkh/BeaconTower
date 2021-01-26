import { Instance, Response } from "../common";


export const GetAliasName = async (): Promise<Response<string>> => {
    let res: any;
    await Instance.get("nodetrace/alias").then((data: any) => {
        res = data;
    });
    return res;
}


export const GetState = async (): Promise<Response<boolean>> => {
    let res: any;
    await Instance.get("nodetrace/state").then((data: any) => {
        res = data;
    });
    return res;
}


export const GetSliceCount = async (): Promise<Response<number>> => {
    let res: any;
    await Instance.get("nodetrace/slice/items/count").then((data: any) => {
        res = data;
    });
    return res;
}


export const GetBlockCount = async (): Promise<Response<number>> => {
    let res: any;
    await Instance.get("nodetrace/block/items/count").then((data: any) => {
        res = data;
    });
    return res;
}


export const GetTraceCount = async (): Promise<Response<number>> => {
    let res: any;
    await Instance.get("nodetrace/trace/items/count").then((data: any) => {
        res = data;
    });
    return res;
}


export const GetFolderPath = async (): Promise<Response<string>> => {
    let res: any;
    await Instance.get("nodetrace/folder/path").then((data: any) => {
        res = data;
    });
    return res;
}


export const GetFolderName = async (): Promise<Response<string>> => {
    let res: any;
    await Instance.get("nodetrace/folder/name").then((data: any) => {
        res = data;
    });
    return res;
}