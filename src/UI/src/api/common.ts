import { message } from "antd";
import axios from "axios"


export interface Response<T> {
    message: string;
    code: ResponseCode;
    data?: T;
}
export interface PageResponse<T> extends Response<T> {
    total: number;
}
export enum ResponseCode {
    Success = 0,
    Error = 1,
    System = 2,
    Logic = 4,
    Parameter = 8,
    Network = 16,
}

export const Instance = axios.create({
    baseURL: "/api/",
    timeout: 1000 * 3
});

Instance.interceptors.response.use(function (response) {
    let res: any = {};
    res.data = response.data.data;
    res.code = response.data.code;
    res.message = response.data.message;
    if (response.data.total != undefined) {
        res["total"] = response.data.total;
    }
    return res;
}, function (error) {
    let res: Response<any> = {
        code: ResponseCode.Error | ResponseCode.Network,
        message: "Network Error!"
    };
    return res;
});

export const NormalErrorTips = (data: Response<any>) => {
    if (IsNetworkError(data)) {
        message.error("网络异常!");
    }
    else {
        message.error(data.message);
    }
}


export const Is = (data: Response<any>, code: ResponseCode): boolean => {
    return (data.code & code) == code;
}
export const IsSuccess = (data: Response<any>): boolean => {
    return data.code === ResponseCode.Success;
}
export const IsNetworkError = (data: Response<any>): boolean => {
    return Is(data, ResponseCode.Error) && Is(data, ResponseCode.Network);
}