import { Drawer, List } from "antd";
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
    traceID: string;
    methodEventID: string | null;
    nodeInfo: NodeTraceItemResponse;
    methodInfo: MethodInfoResponse;
    onColseClicked: () => void;
}

const index: FC<indexProps> = (props) => {
    if (props.methodEventID == null) {
        return null;
    }
    const [data, setData] = useState<Array<LogInfoItemResponse>>([]);

    const fetch = async () => {
        let data = await GetLogInfoByMethodEventID(props.traceID, props.methodEventID!);
        setData(data.data!);
    }

    useEffect(() => {
        fetch();
    }, []);


    return <Drawer
        visible={true}
        title={`${props.nodeInfo.nodeID}在${props.nodeInfo.path}上的函数${props.methodInfo.methodName}的日志输出`}
        onClose={() => {
            props.onColseClicked();
        }}
        width="100vw"
    >
        <List
            bordered
            itemLayout="horizontal"
        >
            {data.map(item => {
                let icon = <FileUnknownTwoTone />;
                switch (item.level) {
                    case LogLevel.Trace:
                        icon = <TraceIcon />;
                        break;
                    case LogLevel.Info:
                        icon = <InfoIcon />;
                        break;
                    case LogLevel.Debug:
                        icon = <DebugIcon />;
                        break;
                    case LogLevel.Warning:
                        icon = <WarningIcon />;
                        break;
                    case LogLevel.Error:
                        icon = <ErrorIcon />;
                        break;
                    case LogLevel.Panic:
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