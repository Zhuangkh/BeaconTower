import { ConfigProvider } from 'antd';
import React, { Component } from 'react';
import zhCN from 'antd/lib/locale/zh_CN';
import 'antd/dist/antd.less';
import { HashRouter as Router, Route } from 'react-router-dom';
import MainLayouts from './layouts';
import ReactDOM from 'react-dom';

export default class App extends Component<any, any>{
    render() {
        return <ConfigProvider locale={zhCN}>
            <Router>
                <Route exact path="/" component={MainLayouts} />
            </Router>
        </ConfigProvider>
    }
}


ReactDOM.render(<App />,
    document.getElementById("app")
);
