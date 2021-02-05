import { Drawer } from "antd";
import React, { FC, useState, useEffect } from "react"
import MethodGraph from "./methodGraph"
import { GetMethodInfoByEventID } from "../../../../api/resource/methods"
import { NodeTraceItemResponse } from "../../../../api/model/nodes";

import "./index.less"
import { MethodInfoResponse } from "../../../../api/model/methods";
import { ResponseCode } from "../../../../api/common";

interface indexProps {
    eventID: string | null;
    onClose: () => void;
    item: NodeTraceItemResponse | null;
}

const index: FC<indexProps> = (props) => {
    if (props.eventID == null) { return null; }
    const [data, setData] = useState<Array<MethodInfoResponse>>([]);

    const fetch = async () => {
        const res = await GetMethodInfoByEventID(props.item?.traceID as string, props.eventID as string);
        if (res.code == ResponseCode.Success) {
            setData(res.data!);
        }

    }

    useEffect(() => {
        fetch();
    }, [])

    return <Drawer
        visible={true}
        title={"函数追踪详情"}
        onClose={() => { props.onClose() }}
        width="100vw"
    >
        <MethodGraph item={props.item!} data={data} />

    </Drawer>
}
export default index;