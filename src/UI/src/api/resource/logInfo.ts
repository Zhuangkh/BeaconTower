import { Instance, PageResponse, Response } from "../common";
import { LogInfoItemResponse } from "../model/loginfo"
const controllerPrefix = "loginfo";


export const GetLogInfoByMethodEventID = async (traceID: string, methodEventID: string): Promise<Response<Array<LogInfoItemResponse>>> => {
    let res: any;
    await Instance.get(`${controllerPrefix}/items/traceid(${traceID})?methodEventID=${methodEventID}`).then((data: any) => {
        res = data;
    });
    return res;
}
