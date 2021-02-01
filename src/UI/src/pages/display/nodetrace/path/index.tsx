import { Button, Modal, Table } from "antd";
import React, { FC, useEffect, useState } from "react"
import { ResponseCode } from "../../../../api/common";
import { PathMapSummaryInfo } from "../../../../api/model/nodes";
import { GetNodeAllPathInfo, GetNodePathItemCount } from "../../../../api/resource/nodes";
import "./index.less"

interface indexProps {
    show: boolean;
    onOk?: () => void;
    onCancel?: () => void;
    nodeAlias: string;
}
const columns = [{
    title: "路径",
    width: "300px",
    dataIndex: "orignalPath"
}, {
    title: "已追踪",
    width: "150px",
    dataIndex: "traceItemCount"
}, {
    title: "操作",
    render: (item: any) => {
        return <Button>操作</Button>
    }
}];
const pageSize = 4;
const index: FC<indexProps> = (props) => {
    if (!props.show) {
        return null;
    }
    const [data, setData] = useState<Array<PathMapSummaryInfo>>([]);
    const [pageIndex, setPageIndex] = useState<number>(1);
    const [totalData, setTotalData] = useState<number>(0);

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
            setTotalData(res.total);
            getAllCount(res.data);
        }
    }
    useEffect(() => {
        fetch(0);
    }, []);
    return <Modal
        maskClosable={false}
        className="node-path"
        title="Path列表"
        width="80vw"
        visible={true}
        onOk={() => {
            if (props.onOk != undefined) {
                props.onOk();
            }
        }}
        onCancel={() => {
            if (props.onCancel != undefined) {
                props.onCancel();
            }
        }}>
        <Table size="small" bordered columns={columns} dataSource={data}
            pagination={{
                current: pageIndex,
                total: totalData,
                pageSize: pageSize,
                hideOnSinglePage: true,
                onChange: (page) => {
                    fetch(page);
                    setPageIndex(page);
                }
            }
            }
        />
    </Modal>
}

export default index;