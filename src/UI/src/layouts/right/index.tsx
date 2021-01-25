import React, { FC, useState } from "react"
import "./index.less"
import { Layout } from 'antd';
import { Route, Switch } from "react-router-dom";
import Home from "../../pages/home"
import Node from "../../pages/node"
const { Header, Content, Footer } = Layout;

interface RightContentProps {
    collapsed: boolean;
}
const collapsedSize = 80;
const uncollapsedSize = 200;
const RightContent: FC<RightContentProps> = (props: RightContentProps) => {

    return <Layout className="right-content" style={{ marginLeft: (props.collapsed ? collapsedSize : uncollapsedSize) }}>
        <Content className="content">
            <div>
                <Switch>
                    <Route path="/node">
                        <Node />
                    </Route>
                    <Route path="/">
                        <Home />
                    </Route>
                </Switch>
            </div>
        </Content>
        <Footer className="fotter">BenLampson ©2021 Created by Ant</Footer>
    </Layout>
}


export default RightContent;