import { Button, Card, Checkbox, Divider, Drawer, List, Space } from "antd";
import React, { FC, useState, useEffect } from "react"
import { GetLogLevelStr, LogInfoItemResponse, LogLevel } from "../../../../../api/model/loginfo";
import { NodeTraceItemResponse } from "../../../../../api/model/nodes";
import { GetLogInfoByMethodEventID } from "../../../../../api/resource/logInfo";
import TraceIcon from "../../../../../icons/trace"
import InfoIcon from "../../../../../icons/info"
import DebugIcon from "../../../../../icons/debug"
import WarningIcon from "../../../../../icons/warning"
import ErrorIcon from "../../../../../icons/error"
import CrashIcon from "../../../../../icons/crash"
import { FileUnknownTwoTone } from "@ant-design/icons"

import "./index.less"
import { MethodInfoResponse } from "../../../../../api/model/methods";


interface indexProps {
    show: boolean;
    traceID: string;
    methodEventID: string | null;
    eventID: string | null;
    nodeInfo: NodeTraceItemResponse;
    methodInfo: MethodInfoResponse | null;
    onColseClicked: () => void;
}

const index: FC<indexProps> = (props) => {
    if (!props.show) {
        return null;
    }
    const [data, setData] = useState<Array<LogInfoItemResponse>>([]);
    const [showTrace, setShowTrace] = useState<boolean>(true);
    const [showDebug, setShowDebug] = useState<boolean>(true);
    const [showInfo, setShowInfo] = useState<boolean>(true);
    const [showWarning, setShowWarning] = useState<boolean>(true);
    const [showError, setShowError] = useState<boolean>(true);
    const [showPainc, setShowPainc] = useState<boolean>(true);

    const fetch = async () => {
        console.log(props)
        let data = await GetLogInfoByMethodEventID(props.traceID, props.methodEventID, props.eventID);
        setData(data.data!);
    }

    useEffect(() => {
        fetch();
    }, []);

    const getDrawerTitle = () => {
        let base = `${props.nodeInfo.nodeID}在${props.nodeInfo.path}上的`;
        if (props.methodInfo != null) {
            base += `函数${props.methodInfo.methodName}的日志输出`;
        }
        else {
            base += "所有函数的日志输出";
        }
        return <>{base}
            <Space >
                <Checkbox checked={showTrace} onChange={(e) => { setShowTrace(e.target.checked); }}><TraceIcon width={"16px"} height={"16px"} />{GetLogLevelStr(LogLevel.Trace)}</Checkbox>
                <Checkbox checked={showDebug} onChange={(e) => { setShowDebug(e.target.checked); }}><DebugIcon width={"16px"} height={"16px"} />{GetLogLevelStr(LogLevel.Debug)}</Checkbox>
                <Checkbox checked={showInfo} onChange={(e) => { setShowInfo(e.target.checked); }}><InfoIcon width={"16px"} height={"16px"} />{GetLogLevelStr(LogLevel.Info)}</Checkbox>
                <Checkbox checked={showWarning} onChange={(e) => { setShowWarning(e.target.checked); }}><WarningIcon width={"16px"} height={"16px"} />{GetLogLevelStr(LogLevel.Warning)}</Checkbox>
                <Checkbox checked={showError} onChange={(e) => { setShowError(e.target.checked); }}><ErrorIcon width={"16px"} height={"16px"} />{GetLogLevelStr(LogLevel.Error)}</Checkbox>
                <Checkbox checked={showPainc} onChange={(e) => { setShowPainc(e.target.checked); }}><CrashIcon width={"16px"} height={"16px"} />{GetLogLevelStr(LogLevel.Panic)}</Checkbox>


            </Space></>;
    }
    return <Drawer
        visible={true}
        title={getDrawerTitle()}
        onClose={() => {
            props.onColseClicked();
        }}
        width="100vw"
    >
        <Divider orientation="left">数据详情</Divider>
        <List
            size="small"
            bordered
            itemLayout="horizontal"
        >
            {data.map(item => {
                let icon = <FileUnknownTwoTone />;                
                switch (item.level) {
                    case LogLevel.Trace:
                        if(!showTrace){return null;}
                        icon = <TraceIcon />;
                        break;
                    case LogLevel.Info:
                        if(!showInfo){return null;}
                        icon = <InfoIcon />;
                        break;
                    case LogLevel.Debug:
                        if(!showDebug){return null;}
                        icon = <DebugIcon />;
                        break;
                    case LogLevel.Warning:
                        if(!showWarning){return null;}
                        icon = <WarningIcon />;
                        break;
                    case LogLevel.Error:
                        if(!showError){return null;}
                        icon = <ErrorIcon />;
                        break;
                    case LogLevel.Panic:
                        if(!showPainc){return null;}
                        icon = <CrashIcon />;
                        break;
                }
                return <List.Item key={item.key}>
                    <List.Item.Meta
                        avatar={icon}
                        title={`输出时间:${item.timeInfo}`}
                        description={`输出位置在文件:${item.fileName}的第${item.lineNumber}行`}
                    />
                    {item.message}
                </List.Item>
            })}

        </List>



    </Drawer>
}


export default index;