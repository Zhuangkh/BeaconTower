import { Drawer } from "antd";
import React, { FC, useState, useEffect } from "react"
import MethodGraph from "./methodGraph"

import "./index.less"

interface indexProps {

}

const index: FC<indexProps> = (props) => {

    return <Drawer
        visible={true}
        title={"函数追踪详情"}
    >
        <MethodGraph/>

    </Drawer>
}
export default index;