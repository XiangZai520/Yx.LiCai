//模块目录
var base = ""; 
seajs.config({

    // Sea.js 的基础路径
    base: "http://172.16.16.252:8089/js",
    // 别名配置
    alias: {
        'jquery': 'lib/jquery.js',
        'jquery/jquery-validate':"lib/jquery.validate.js",
        'fastclick':"lib/fastclick.js",
        'gxdialog':"dialog/gxdialog.js",
        'swipe':"swipe.js",
        'gxtabs':"gxtabs.js",
        'zepto':"lib/zepto.js",
        'loadmore':"loadmore.js",
        'jcookie': "lib/jquery.cookie.js",
        'spin': "lib/spin.js",
        'jweixin': "lib/jweixin-1.0.0.js"
    },
    // 路径配置
    paths: {
        'mod':base + 'mod'
    }
});

//js获取项目根路径
function getRootPath(){
    //获取当前网址
    var curWwwPath=window.document.location.href;
    //获取主机地址之后的目录
    var pathName=window.document.location.pathname;
    var pos=curWwwPath.indexOf(pathName);
    //获取主机地址
    var localhostPaht=curWwwPath.substring(0,pos);
    //获取带"/"的项目名
    var projectName=pathName.substring(0,pathName.substr(1).indexOf('/')+1);
    return(localhostPaht+projectName);
}
