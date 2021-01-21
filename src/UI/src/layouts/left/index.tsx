import React, { FC, ReactElement } from "react"
import { Menu, Layout } from 'antd';
import Icon from '@ant-design/icons';
import "./index.less"
import nodeSvg from "../../icons/node.svg";

let { Sider } = Layout;
let MenuItem = Menu.Item;

interface menuItem {
    key: string;
    text: ReactElement;
    icon: ReactElement;
}

const menuItems: Array<menuItem> = [
    {
        key: "home",
        text: <>首页</>,
        icon: <Icon component={nodeSvg} />
    },
];

const LeftSider: FC<any> = (props) => {
    return <Sider
        collapsible={true}
        className="leftSider"
    >
        <div className="logo" />
        <Menu theme="dark" mode="inline" defaultSelectedKeys={['home']}>
            {menuItems.map(item => {
                return <MenuItem key={item.key} icon={item.icon}>
                    {item.text}
                </MenuItem>
            })}
        </Menu>
    </Sider>

}

export default LeftSider;