const htmlP = require("html-webpack-plugin");
const path = require("path");
const webpack = require("webpack");
module.exports = {
    mode: "development",
    entry: {
        main: "./src/app.tsx",
    },
    output: {
        filename: "app.js",
        path: path.resolve(__dirname, "dist")
    },
    devtool: "eval-source-map",
    devServer: {
        contentBase: './dist',
        host: "0.0.0.0",
        port: 8081,
        contentBase: "./src"
    },
    resolve: {
        extensions: [".ts", ".tsx", ".js"]
    },
    plugins: [
        new htmlP({
            template: path.resolve(__dirname, "./src/index.html")
        })
    ],
    module: {
        rules: [
            {
                test: /\.(css|less)$/,
                use: [
                    { loader: "style-loader" },
                    { loader: "css-loader" },
                    {
                        loader: "less-loader",
                        options: {
                            lessOptions: {
                                javascriptEnabled: true
                            }
                        }
                    }
                ]
            }, {
                test: /\.(png|svg|jpg|gif)$/,
                use: [
                    'file-loader'
                ],

            },
            {
                test: /\.(ts|tsx)$/,
                use: ["ts-loader"]
            },
            {
                test: /\.svg$/,
                use: [
                    {
                        loader: 'babel-loader',
                    },
                    {
                        loader: '@svgr/webpack',
                        options: { babel: false, icon: true, typescript: true, ext: "tsx", },
                    }
                ],
            }
        ]
    }

}