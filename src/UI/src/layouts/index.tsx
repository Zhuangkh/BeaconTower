import React, { Component } from "react"
import { Layout } from 'antd';
import "./index.less"
import LeftSider from "./left";
import RightContent from "./right"

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
        return <Layout >
            <LeftSider onCollapsed={this.onSiderCollapsed.bind(this)} defaultCollapsed={this.state.siderCollapsed} />
            <RightContent collapsed={this.state.siderCollapsed} />
        </Layout>
    }
}