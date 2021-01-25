import { Descriptions } from "antd";
import React, { FC, useState, useEffect } from "react"
import { GetDBBlockItemsCount, GetDBFolderName, GetDBFolderPath, GetDBSliceItemsCount, GetDBTraceItemsCount } from "../../../api/resource/system";


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

    const fetchData = async () => {
        var getDBSliceItemsCount = GetDBSliceItemsCount(props.aliasName);
        var getDBBlockItemsCount = GetDBBlockItemsCount(props.aliasName);
        var getDBTraceItemCount = GetDBTraceItemsCount(props.aliasName);
        var getDBFolderPath = GetDBFolderPath(props.aliasName);
        var getDBFolderName = GetDBFolderName(props.aliasName);
        await Promise.all([getDBSliceItemsCount, getDBBlockItemsCount, getDBTraceItemCount, getDBFolderPath, getDBFolderName]);
        setDBSliceItemsCount((await getDBSliceItemsCount).data as number);
        setDBBlockItemsCount((await getDBBlockItemsCount).data as number);
        setDBTraceItemCount((await getDBTraceItemCount).data as number);
        setDBFolderPath((await getDBFolderPath).data as string);
        setDBFolderName((await getDBFolderName).data as string);
    }

    useEffect(() => {
        fetchData();
    }, []);

    return <Descriptions
        key={props.id}
        bordered
        title={props.aliasName}
        size="small"
    >
        <DescriptionsItem label="Product">{dbSliceItemsCount}</DescriptionsItem>
        <DescriptionsItem label="Billing">{dbBlockItemsCount}</DescriptionsItem>
        <DescriptionsItem label="Billing">{dbTraceItemCount}</DescriptionsItem>
        <DescriptionsItem label="Billing">{dbFolderPath}</DescriptionsItem>
        <DescriptionsItem label="Billing">{dbFolderName}</DescriptionsItem>
        <DescriptionsItem label="Config Info">
            Data disk type: MongoDB
<br />
Database version: 3.4
<br />
Package: dds.mongo.mid
<br />
Storage space: 10 GB
<br />
Replication factor: 3
<br />
Region: East China 1<br />
        </DescriptionsItem>
    </Descriptions>
}

export default DBInstanceItem;