/*!
 *
 *
 */

define('Index', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库 
    var gxdialog = require("gxdialog");
    //设置密码标识
    var floagWord = false;
    //原始密码标识
    var floagOldPW = false;
    $.gxDialog.defaults.background = '#000';
    //有确认按钮的弹出
    function alertDialog(v) {
        $.gxDialog({
            title: '',
            width: 280,
            closeBtn: false,
            info: '<div class="pop-box"><p class="warning-color">' + v + '</p></div>',
            ok: function () { }
        });
    }
    function autoCloseDialog() {
        $.gxDialog({
            title: '',
            width: 280,
            closeBtn: false,
            info: '<div class="pop-box"><h3>提示信息</h3><p>您还没有进行实名认证，请先认证<p><div class="cd-time">3</div></div>'
        });
        var secondNum = 3;
        loadJump(secondNum);
        function loadJump(num) {
            window.setTimeout(function () {
                num--;
                if (num > 0) {
                    $('.cd-time').html(num);
                    loadJump(num);
                } else {
                    $.gxDialog.close();
                    window.location.href = "/UserSetting/";
                };
            }, 1000)
        }
    }

    window.onload = function () {
        $.ajax({
            type: "get",
            async: "false",
            url: "/UserSetting/GetMessageCount",
            dataType: "json",
            success: function(data) {
                if (data.Status != 0) {
                    $(".icon-tips span").html(data.Status);
                } else {
                    $(".icon-tips").css("display", "none");
                }
            }
        });
    }

    //添加银行卡
    $("#addBink").click(function () {      
        if (IsRealCard == 1) { window.location.href = "/UserSetting/add_bank"; } else {
            event.preventDefault();
            autoCloseDialog();
        }
        
    });
});
seajs.use('Index');
