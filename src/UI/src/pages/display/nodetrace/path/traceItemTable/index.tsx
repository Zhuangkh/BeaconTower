import { Button, Table } from "antd";
import React, { FC, useState, useEffect } from "react"
import { ResponseCode } from "../../../../../api/common";
import { GetTraceBeginTime, GetTraceIDListByNodeAndPathAlias } from "../../../../../api/resource/nodes";
import "./index.less"

interface indexProps {
    nodeAlias: string;
    pathAlias: string | null;
    onSelectTraceItem: (traceID: string) => void;
}



const pageSize = 10;
const index: FC<indexProps> = (props) => {
    if (props.pathAlias == null) { return <></> }

    const [data, setData] = useState<Array<any>>([]);
    const [pathPageIndex, setPathPageIndex] = useState<number>(1);
    const [totalData, setTotalData] = useState<number>(0);
    const pathTableColumns = [{
        title: "TraceID",
        width: "500px",
        dataIndex: "traceID"
    }, {
        title: "发生时间",
        width: "200px",
        dataIndex: "dateTime"
    },{
        title: "操作",
        render: (item: any) => {
            return <Button type="primary" shape="round" onClick={() => {
                props.onSelectTraceItem(item.traceID);
            }}>查看trace详情</Button>
        }
    }];

    const fetchTraceDateTimeInfo = async (data: Array<any>) => {
        console.log(data);
        const taskList = [];
        for (let index = 0; index < data.length; index++) {
            console.log(data);
            const element = data[index];
            let loader=async () => {
                let res = GetTraceBeginTime(props.nodeAlias, props.pathAlias as string, element.traceID);
                element.dateTime= (await res).data;
            };
            taskList.push(loader());
        }
        await Promise.all(taskList);
        setData([...data]);
    }

    const fetch = async (currentIndex: number) => {
        if (props.pathAlias == null) return;
        let res = await GetTraceIDListByNodeAndPathAlias(props.nodeAlias, props.pathAlias, pageSize, currentIndex);
        if (res.code == ResponseCode.Success && res.data != undefined && res.data != null) {
            let resData = res.data;
            let currentPageData: any = [];
            resData.forEach((element: string) => {
                currentPageData.push({
                    key: element,
                    traceID: element,
                    dateTime:"获取中.."
                });
            });
            fetchTraceDateTimeInfo(currentPageData);
            setData([...currentPageData]);

            setTotalData(res.total);
        }
    }
    useEffect(() => {
        setData([]);
        setTotalData(0);
        fetch(1);
    }, [props.pathAlias]);

    return <Table size="small" bordered columns={pathTableColumns} dataSource={data}
        pagination={{
            current: pathPageIndex,
            total: totalData,
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