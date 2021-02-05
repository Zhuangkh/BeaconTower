import { Drawer } from "antd";
import React, { FC, useState, useEffect } from "react"
import MethodGraph from "./methodGraph"
import { GetMethodInfoByEventID } from "../../../../api/resource/methods"

import "./index.less"

interface indexProps {
    eventID: string | null;
    onClose: () => void;
}

const index: FC<indexProps> = (props) => {
    if (props.eventID == null) { return null; }

    const fetch = async () => {
        const data = await GetMethodInfoByEventID(props.eventID as string);
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
        <MethodGraph />

    </Drawer>
}
export default index;