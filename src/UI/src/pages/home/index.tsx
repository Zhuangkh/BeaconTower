import { Button, Descriptions, message, PageHeader } from "antd";
import React, { FC, useEffect, useState } from "react"
import { IsSuccess, NormalErrorTips, Response } from "../../api/common";
import { GetAllDBAliasName } from "../../api/resource/system"
import DBInstanceItem from "./dbinstanceitem"

import "./index.less"


interface HomeProps {

}

const Home: FC<HomeProps> = (props) => {

    const [aliasList, setAliasList] = useState<Array<string>>([]);

    const fetchData = async () => {
        let data: Response<Array<string>> = await GetAllDBAliasName();
        if (IsSuccess(data)) {
            setAliasList(data.data as Array<string>);
        }
        else {
            NormalErrorTips(data);
        }
    }


    useEffect(() => {
        fetchData();
    }, [])

    return <PageHeader
        ghost={false}
        title="系统概要"
        subTitle="当前系统数据库级别的情况概要"
    >
        {aliasList.map((item, index) => {
            return <DBInstanceItem aliasName={item} key={index} id={index} />
        })}
    </PageHeader>
}
export default Home;