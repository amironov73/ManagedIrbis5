const path = require ('path')
const HtmlWebpackPlugin = require ('html-webpack-plugin')
const CopyWebpackPlugin = require ('copy-webpack-plugin')

module.exports = {
    entry: './src/js/app.js',
    plugins: [
        new HtmlWebpackPlugin ({
            template: './src/index.html',
            filename: 'index.html',
            inject: false
        }),

        new CopyWebpackPlugin ({
            patterns: [
                { from: path.resolve (__dirname, "src", "img"), to: "img" },
            ]
        })
    ],
    module: {
        rules: [
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    'css-loader'
                ]
            },
            {
                test: /\.(png|jpe?g|gif|ico)$/i,
                type: "asset/resource"
                // loader: 'file-loader',
                // options: {
                //     name: '[name].[ext]'
                // }
            }
        ]
    },
    output: {
        path: path.resolve (__dirname, 'dist'),
        filename: 'bundle.js'
    },
    devtool: 'source-map'
}
