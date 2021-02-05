import { Drawer } from "antd";
import React, { FC, useState, useEffect } from "react"
import { MethodInfoResponse } from "../../../../../api/model/methods";
import { NodeTraceItemResponse } from "../../../../../api/model/nodes";

import "./index.less"

interface indexProps {
    item: NodeTraceItemResponse;
    data: Array<MethodInfoResponse>;
}

const index: FC<indexProps> = (props) => {

    return <></>
}
export default index;