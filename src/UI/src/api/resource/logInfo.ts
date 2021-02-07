import { Instance, PageResponse, Response } from "../common";
import { LogInfoItemResponse } from "../model/loginfo"
const controllerPrefix = "loginfo";


export const GetLogInfoByMethodEventID = async (traceID: string
    , methodEventID: string | null
    , eventID: string | null
): Promise<Response<Array<LogInfoItemResponse>>> => {
    let res: any;
    let query = "?1=1";
    if (methodEventID != null) {
        query += `&methodEventID=${methodEventID}`;
    }
    if (eventID != null) {
        query += `&eventID=${eventID}`;
    }
    await Instance.get(`${controllerPrefix}/items/traceid(${traceID})${query}`).then((data: any) => {
        res = data;
    });
    return res;
}
