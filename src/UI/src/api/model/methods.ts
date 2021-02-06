
export interface MethodInfoResponse {
    key: string;
    traceID: string;
    nodeID: string;
    eventID: string;
    methodEventID: string;
    preMethodEventID: string;
    methodID: string;
    methodName: string;
    fileName: string;
    lineNumber: string;
    beginTimeStamp: string;
    beginTime: string;
    endTimeStamp: string | null;
    endTime: string | null;
    duration: string | null;
    beginCustomData: any;
    endCustomData: any;
    children: Array<MethodInfoResponse>;
    switchCollapsedState: () => void;
}