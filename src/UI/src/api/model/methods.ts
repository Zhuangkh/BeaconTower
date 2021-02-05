
export interface MethodInfoResponse {
    Key: string;
    TraceID: string;
    NodeID: string;
    EventID: string;
    MethodEventID: string;
    PreMethodEventID: string;
    MethodID: string;
    MethodName: string;
    FileName: string;
    LineNumber: string;
    BeginTimeStamp: string;
    BeginTime: string;
    EndTimeStamp: string | null;
    EndTime: string | null;
    Duration: string | null;
    BeginCustomData: any;
    EndCustomData: any;
    Children: Array<MethodInfoResponse>;
}