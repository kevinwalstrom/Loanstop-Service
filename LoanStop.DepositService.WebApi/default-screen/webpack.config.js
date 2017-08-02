var path = require('path');
var webpack = require('webpack');
var ExtractTextPlugin = require('extract-text-webpack-plugin')

var fs = require("fs");

var isProdBuild = process.argv.indexOf('-p') !== -1;

var envPlugin = new webpack.DefinePlugin({
  __DEBUG__: JSON.stringify(!isProdBuild),
  __RELEASE__: JSON.stringify(isProdBuild),
  'process.env.NODE_ENV': isProdBuild ? '"production"' : '"development"'
});

console.log('Copying font files');

module.exports = {
  entry: {
    //app: './src/index.es',
    defaultscreen: './src/default-screen.es'
  },
  output: {
    filename: '[name].js',
    path: path.join(__dirname, 'app/assets'),
    publicPath: 'http://localhost:8080/assets' // Required for webpack-dev-server
  },
  resolve: {
    root: [
      __dirname
    ],
    extensions: ['', '.js', '.es', '.jsx', '.raw.less']
  },
  node: {
    fs: "empty"
  },
  module: {
    loaders: [
      { 
        test: /\.(jsx|es)$/, 
        loader: 'babel',
        include: [path.join(__dirname)],
        query: {
          plugins: ['transform-runtime'],
          presets: ['react', 'es2015', 'stage-0']
        }
      },
      { test: /\.raw\.less$/, loader: 'raw!less'},
      { test: /\.less$/, loader: 'style!css!less', exclude: /(\.raw\.less$)|node_modules/},
      { test: /\.json$/, loader: 'json', exclude: /node_modules/},
      { test: /\.css/, loader: ExtractTextPlugin.extract('style-loader', 'css-loader') }
    ]
  },
  plugins: [
    new ExtractTextPlugin('style.css', { allChunks: true }),
    new webpack.HotModuleReplacementPlugin(), envPlugin
  ]
};
