define('yxClick', ['jquery', 'fastclick'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库
    var FastClick = require("fastclick");

    $(".leaf").click(function () {
        $(this).hide();
    });
    $(".ui-inviteBtn").click(function () {
        $(".leaf").show();
    });
});

seajs.use('yxClick');
