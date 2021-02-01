import { Button, Table } from "antd";
import React, { FC, useState, useEffect } from "react"
import { ResponseCode } from "../../../../../api/common";
import { GetTraceIDListByNodeAndPathAlias } from "../../../../../api/resource/nodes";
import "./index.less"

interface indexProps {
    nodeAlias: string;
    pathAlias: string | null;
}

const pathTableColumns = [{
    title: "TraceID",
    width: "500px",
    dataIndex: "traceID"
}, {
    title: "操作",
    render: (item: string) => {
        return <Button type="primary" shape="round">查看trace详情</Button>
    }
}];

const pageSize = 4;
const index: FC<indexProps> = (props) => {
    if (props.pathAlias == null) { return <></> }

    const [data, setData] = useState<Array<any>>([]);
    const [pathPageIndex, setPathPageIndex] = useState<number>(1);
    const [totalData, setTotalData] = useState<number>(0);


    const fetch = async (currentIndex: number) => {
        if (props.pathAlias == null) return;
        let res = await GetTraceIDListByNodeAndPathAlias(props.nodeAlias, props.pathAlias, pageSize, currentIndex);
        if (res.code == ResponseCode.Success && res.data != undefined && res.data != null) {
            let resData = res.data;
            resData.forEach((element: string) => {
                data.push({
                    key: element,
                    traceID: element
                });
            });
            setData(res.data);
            setTotalData(res.total);
        }
    }
    useEffect(() => {
        // fetch(0);
    }, []);

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