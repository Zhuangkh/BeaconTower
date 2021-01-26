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
        title={<>数据库:{props.aliasName}</>}
        size="small"
    >
        <DescriptionsItem label="分块总数">{dbBlockItemsCount}</DescriptionsItem>
        <DescriptionsItem label="切片总数">{dbSliceItemsCount}</DescriptionsItem>
        <DescriptionsItem label="存储数据量">{dbTraceItemCount}</DescriptionsItem>
        <DescriptionsItem label="运行状态">{runningState?<Badge status="processing" text="运行中" />:<Badge status="error" text="已停止" />}</DescriptionsItem>
        <DescriptionsItem label="文件根目录" >
            <Tooltip title={dbFolderPath}>
                <div className="root-folder-path">{dbFolderPath}</div>
            </Tooltip>
        </DescriptionsItem>
        <DescriptionsItem label="对应文件夹">{dbFolderName}</DescriptionsItem>
    </Descriptions>
}

export default DBInstanceItem;