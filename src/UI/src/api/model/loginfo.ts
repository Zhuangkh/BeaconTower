

export interface LogInfoItemResponse {
    key: string;
    level: LogLevel;
    eventID: string;
    methodEventID: string;
    traceID: string;
    message: string;
    methodID: string;
    methodName: string;
    fileName: string;
    lineNumber: number;
    timeStamp: string;
    timeInfo: string;
    customData: any;
}

export enum LogLevel {
    Trace,
    Debug,
    Info,
    Warning,
    Error,
    Panic
}

export const GetLogLevelStr = (level: LogLevel): string => {
    switch (level) {
        case LogLevel.Trace:
            return "追踪信息";
        case LogLevel.Info:
            return "提示信息";
        case LogLevel.Debug:
            return "调试信息";
        case LogLevel.Warning:
            return "警告信息";
        case LogLevel.Error:
            return "错误信息";
        case LogLevel.Panic:
            return "可能会引起崩溃";
    }
}