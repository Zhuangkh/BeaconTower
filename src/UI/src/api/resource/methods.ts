import { Instance, PageResponse, Response } from "../common";
import { MethodInfoResponse } from "../model/methods"
const controllerPrefix = "methodtrace";

export const GetMethodInfoByEventID = async (traceID: string, eventID: string): Promise<Response<Array<MethodInfoResponse>>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/items/traceid(${traceID})?eventID=${eventID}`).then((data: any) => {
        res = data;
    });
    return res;
}
