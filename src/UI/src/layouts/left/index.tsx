import React, { FC, ReactElement, useState } from "react"
import { Menu, Layout } from 'antd';
import Icon from '@ant-design/icons';
import "./index.less"
import Node from "../../icons/node"
import Home from "../../icons/home"
import { Link } from "react-router-dom";

let { Sider } = Layout;
let MenuItem = Menu.Item;

interface menuItem {
    key: string;
    text: ReactElement;
    link: string;
    icon: ReactElement;
}

const menuItems: Array<menuItem> = [
    {
        key: "home",
        link: "/",
        text: <>首页</>,
        icon: <Icon component={Home} />
    },
    {
        key: "node",
        link: "/node",
        text: <>节点追踪</>,
        icon: <Icon component={Node} />
    },
];

const fullTitle = "Beacon Tower";
const shortTitle = "B.T.";

interface LeftSiderProps {
    onCollapsed: (state: boolean) => void;
    defaultCollapsed: boolean;
}


const LeftSider: FC<LeftSiderProps> = (props: LeftSiderProps) => {

    const [collapsed, setCollapsed] = useState<boolean>(props.defaultCollapsed);

    return <Sider
        theme="light"
        collapsible={true}
        collapsed={collapsed}
        onCollapse={(state: boolean) => { setCollapsed(state); props.onCollapsed(state); }}
        className="leftSider"
    >
        <div className="logo" >
            {collapsed ? shortTitle : fullTitle}
        </div>
        <Menu theme="light" mode="inline" defaultSelectedKeys={['home']}>
            {menuItems.map(item => {
                return <MenuItem key={item.key} icon={item.icon} >
                    <Link to={item.link}>
                        {item.text}
                    </Link>
                </MenuItem>

            })}
        </Menu>
    </Sider>

}

export default LeftSider;