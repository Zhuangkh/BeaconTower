

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