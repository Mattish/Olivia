var path = require('path');
var CopyWebpackPlugin = require('copy-webpack-plugin');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var webpack = require("webpack");
const fs = require('fs');
var config = {
    entry: {
        //index: "./src/app.ts",
        taketwo: "./src/taketwo/taketwo.ts"
    },
    output: {
        path: path.join(__dirname, "build"),
        filename: "[name].js"
    },
    resolve: {
        extensions: ['', '.ts', '.js'],
        root: path.resolve('./src')
    },
    module: {
        loaders: [
            { test: /\.ts$/, loader: 'ts-loader', exclude: /node_modules/ },
            { test: /\.html$/, loader: "html-loader?attrs=false", exclude: '/node_modules/' },
            { test: /\.css$/, loader: ExtractTextPlugin.extract('style-loader', 'css-loader?-url') },
            { test: /\.less$/, loader: ExtractTextPlugin.extract('style-loader', 'css-loader?-url!less-loader') },
        ]
    },
    plugins: [
        new CopyWebpackPlugin([
            { from: './src/assets', to: './assets' },
            // { from: './libs/bootstrap/fonts', to: './fonts' }
        ]),
        new ExtractTextPlugin('[name].css')
    ]
};
var files = fs.readdirSync(path.join(__dirname, "src/pages"))
files.forEach(function (file) {
    if (path.extname(file) === '.ejs') {
        var inputFile = path.join(__dirname, "src/pages", file);
        var baseName = path.basename(file, '.template.ejs');
        var outFile = baseName + '.html';
        config.plugins.push(new HtmlWebpackPlugin({
            template: inputFile,
            filename: outFile,
            chunks: [baseName]
        }));
        console.log("Adding %s, Outputting to %s", inputFile, outFile);
    }
});
module.exports = config;