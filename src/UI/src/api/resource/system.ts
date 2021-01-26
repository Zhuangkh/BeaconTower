import { Instance, Response } from "../common";


export const GetAllDBAliasName = async (): Promise<Response<Array<string>>> => {
    let res: any;
    await Instance.get("system/instance/items/alias").then((data: any) => {
        res = data;
    });
    return res;
}

export const GetDBSliceItemsCount = async (alias: string): Promise<Response<number>> => {
    let res: any;
    await Instance.get(`system/instance/items/alias(${alias})/slice/items/count`).then((data: any) => {
        res = data;
    });
    return res;
}

export const GetDBBlockItemsCount = async (alias: string): Promise<Response<number>> => {
    let res: any;
    await Instance.get(`system/instance/items/alias(${alias})/block/items/count`).then((data: any) => {
        res = data;
    });
    return res;
}

export const GetDBTraceItemsCount = async (alias: string): Promise<Response<number>> => {
    let res: any;
    await Instance.get(`system/instance/items/alias(${alias})/trace/items/count`).then((data: any) => {
        res = data;
    });
    return res;
}

export const GetDBFolderPath = async (alias: string): Promise<Response<string>> => {
    let res: any;
    await Instance.get(`system/instance/items/alias(${alias})/folder/path`).then((data: any) => {
        res = data;
    });
    return res;
}

export const GetDBFolderName = async (alias: string): Promise<Response<string>> => {
    let res: any;
    await Instance.get(`system/instance/items/alias(${alias})/folder/name`).then((data: any) => {
        res = data;
    });
    return res;
}

export const GetDBState = async (alias: string): Promise<Response<boolean>> => {
    let res: any;
    await Instance.get(`system/instance/items/alias(${alias})/state`).then((data: any) => {
        res = data;
    });
    return res;
}