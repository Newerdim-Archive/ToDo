const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const {CleanWebpackPlugin} = require('clean-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');

const outputPath = path.resolve(__dirname, 'dist');

module.exports = {
    mode: 'production',
    entry: './src/index.ts',
    output: {
        filename: '[name].bundle.js',
        path: outputPath,
    },
    module: {
        rules: [
            {
                test: /\.ya?ml$/,
                type: 'json',
                use: [
                    {loader: 'yaml-loader'}
                ]
            },
            {
                test: /\.css$/,
                use: [
                    {loader: 'style-loader'},
                    {loader: 'css-loader'},
                ]
            }
        ]
    },
    plugins: [
        new CleanWebpackPlugin(),
        new CopyWebpackPlugin({
            patterns: [
                {
                    from: require.resolve('swagger-ui/dist/oauth2-redirect.html'),
                    to: './'
                }
            ]
        }),
        new HtmlWebpackPlugin({
            template: './src/index.html'
        })
    ],
};