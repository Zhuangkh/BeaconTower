import { Drawer } from "antd";
import React, { FC, useState, useEffect } from "react"
import { LogInfoItemResponse } from "../../../../../api/model/loginfo";
import { GetLogInfoByMethodEventID } from "../../../../../api/resource/logInfo";
import "./index.less"


interface indexProps {
    show: boolean;
    traceID: string;
    methodEventID: string;
}

const index: FC<indexProps> = (props) => {
    if (!props.show) {
        return null;
    }
    const [data, setData] = useState<Array<LogInfoItemResponse>>([]);

    const fetch = async () => {
        let data = await GetLogInfoByMethodEventID(props.traceID, props.methodEventID);
        setData(data.data!);
    }

    useEffect(() => {
        fetch();
    }, []);


    return <Drawer>
        
    </Drawer>
}


export default index;