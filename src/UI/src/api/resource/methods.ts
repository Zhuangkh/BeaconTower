import { Instance, PageResponse, Response } from "../common";
import { MethodInfoResponse } from "../model/methods"
const controllerPrefix = "methodtrace";

export const GetMethodInfoByEventID = async (eventID:string): Promise<Response<string>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/alias`).then((data: any) => {
        res = data;
    });
    return res;
}
