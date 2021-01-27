import React, { Component } from "react"
import { Layout } from 'antd';
import "./index.less"
import LeftSider from "./left";
import RightContent from "./right"
import { Route, Switch } from "react-router-dom";
import NodeTraceDisplay from "../pages/display/nodetrace"

interface MainLayoutsState {
    siderCollapsed: boolean;
}
interface MainLayoutsProps {

}

export default class MainLayouts extends Component<MainLayoutsProps, MainLayoutsState>{
    constructor(props: MainLayoutsProps) {
        super(props);
        this.state = {
            siderCollapsed: false
        }
    }


    onSiderCollapsed = (state: boolean) => {
        this.setState({ siderCollapsed: state })
    }

    render() {

        return <Switch>
            <Route path="/display/:nodeAlias">
                <NodeTraceDisplay />
            </Route>
            <Route path="/">
                <Layout >
                    <LeftSider onCollapsed={this.onSiderCollapsed.bind(this)} defaultCollapsed={this.state.siderCollapsed} />
                    <RightContent collapsed={this.state.siderCollapsed} />
                </Layout>
            </Route>
        </Switch>
    }
}