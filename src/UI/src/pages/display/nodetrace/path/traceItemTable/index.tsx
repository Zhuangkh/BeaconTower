import { Button, Table } from "antd";
import React, { FC, useState, useEffect } from "react"
import { ResponseCode } from "../../../../../api/common";
import { PathMapSummaryInfo } from "../../../../../api/model/nodes";
import { GetNodeAllPathInfo, GetNodePathItemCount } from "../../../../../api/resource/nodes";
import "./index.less"

interface indexProps {
    nodeAlias: string;
}

const pathTableColumns = [{
    title: "TraceID",
    width: "500px",
    dataIndex: "traceID"
}, {
    title: "操作",
    render: (item: any) => {
        return <Button type="primary" shape="round">查看trace详情</Button>
    }
}];

const pageSize = 4;
const index: FC<indexProps> = (props) => {


    const [data, setData] = useState<Array<PathMapSummaryInfo>>([]);
    const [pathPageIndex, setPathPageIndex] = useState<number>(1);
    const [pathTotalData, setPathTotalData] = useState<number>(0);
    const [currentSelectedPath, setCurrentSelectedPath] = useState<PathMapSummaryInfo | null>(null);

    const getAllCount = async (tempData: Array<PathMapSummaryInfo>) => {
        let allJob = [];
        for (let index = 0; index < tempData.length; index++) {
            const element = tempData[index];
            var load = async () => {
                let countRes = await GetNodePathItemCount(props.nodeAlias, element.aliasName);
                if (countRes.data != undefined && countRes.data != null) {
                    element.traceItemCount = countRes.data.toString();
                }
            }
            allJob.push(load());
        }
        await Promise.all(allJob);
        setData([...tempData]);
    }
    const fetch = async (currentIndex: number) => {
        let res = await GetNodeAllPathInfo(props.nodeAlias, pageSize, currentIndex);
        if (res.code == ResponseCode.Success && res.data != undefined && res.data != null) {
            for (let index = 0; index < res.data.length; index++) {
                const element = res.data[index];
                element.key = element.aliasName;
                element.traceItemCount = "获取中..";
            }
            setData(res.data);
            setPathTotalData(res.total);
            getAllCount(res.data);
        }
    }
    useEffect(() => {
        // fetch(0);
    }, []);

    return <Table size="small" bordered columns={pathTableColumns} dataSource={data}
        pagination={{
            current: pathPageIndex,
            total: pathTotalData,
            pageSize: pageSize,
            hideOnSinglePage: true,
            onChange: (page) => {
                fetch(page);
                setPathPageIndex(page);
            }
        }}
    />
}


export default index;