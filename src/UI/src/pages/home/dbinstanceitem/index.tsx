import { Badge, Descriptions, Tooltip } from "antd";
import React, { FC, useState, useEffect } from "react"
import { GetDBBlockItemsCount, GetDBFolderName, GetDBFolderPath, GetDBSliceItemsCount, GetDBState, GetDBTraceItemsCount } from "../../../api/resource/system";
import "./index.less"

const DescriptionsItem = Descriptions.Item;

interface DBInstanceItemProps {
    aliasName: string;
    id: any;
}

const DBInstanceItem: FC<DBInstanceItemProps> = (props) => {
    const [dbSliceItemsCount, setDBSliceItemsCount] = useState<number>(0);
    const [dbBlockItemsCount, setDBBlockItemsCount] = useState<number>(0);
    const [dbTraceItemCount, setDBTraceItemCount] = useState<number>(0);
    const [dbFolderPath, setDBFolderPath] = useState<string>("");
    const [dbFolderName, setDBFolderName] = useState<string>("");
    const [runningState, setRunningState] = useState<boolean>(false);

    const fetchData = async () => {
        var getDBSliceItemsCount = GetDBSliceItemsCount(props.aliasName);
        var getDBBlockItemsCount = GetDBBlockItemsCount(props.aliasName);
        var getDBTraceItemCount = GetDBTraceItemsCount(props.aliasName);
        var getDBFolderPath = GetDBFolderPath(props.aliasName);
        var getDBFolderName = GetDBFolderName(props.aliasName);
        var runningState = GetDBState(props.aliasName);
        await Promise.all([getDBSliceItemsCount, getDBBlockItemsCount, getDBTraceItemCount, getDBFolderPath, getDBFolderName]);
        setDBSliceItemsCount((await getDBSliceItemsCount).data as number);
        setDBBlockItemsCount((await getDBBlockItemsCount).data as number);
        setDBTraceItemCount((await getDBTraceItemCount).data as number);
        setDBFolderPath((await getDBFolderPath).data as string);
        setDBFolderName((await getDBFolderName).data as string);
        setRunningState((await runningState).data as boolean);
    }

    useEffect(() => {
        fetchData();
    }, []);

    return <Descriptions
        className="db-instance-item"
        key={props.id}
        bordered
        title={<>?????????:{props.aliasName}</>}
        size="small"
    >
        <DescriptionsItem label="????????????">{dbBlockItemsCount}</DescriptionsItem>
        <DescriptionsItem label="????????????">{dbSliceItemsCount}</DescriptionsItem>
        <DescriptionsItem label="???????????????">{dbTraceItemCount}</DescriptionsItem>
        <DescriptionsItem label="????????????">{runningState?<Badge status="processing" text="?????????" />:<Badge status="error" text="?????????" />}</DescriptionsItem>
        <DescriptionsItem label="???????????????" >
            <Tooltip title={dbFolderPath}>
                <div className="root-folder-path">{dbFolderPath}</div>
            </Tooltip>
        </DescriptionsItem>
        <DescriptionsItem label="???????????????">{dbFolderName}</DescriptionsItem>
    </Descriptions>
}

export default DBInstanceItem;