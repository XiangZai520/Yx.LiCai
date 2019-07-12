/*!
 *
 *
 */

define('login', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库
    var gxdialog = require("gxdialog");
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
    //function alertDialog(v) {
    //    $.gxDialog({
    //        title: '',
    //        width: 280,
    //        info: '<div class="pop-box"><p class="warning-color">' + v + '</p></div>'
    //    });
    //}
    //start

    //验证码标识
    var floagCode = false;  
    $(document).ready(function () {      
        $("#randomCode").blur(function () {
            var Code = $("#randomCode").val().replace(/[ ]/g, "");
            $.ajax({
                type: "POST",
                async: false,
                url: "/Login/CheckCode",
                data: { Code: Code },
                success: function (result) {                  
                    if (result.Result) {
                        floagCode = true;
                    } else { floagCode = false; }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                },
            });
        });
        $("img.captcha").click(function () {
            $(this).attr("src", $(this).attr("src") + "?a=" + new Date().getTime());
        });

        //登录
        $("#regBtn").click(function () {
            if ($("#randomCode").val().replace(/[ ]/g, "") == "") { alertDialog("请输入验证码"); return; }
            if (!floagCode) { alertDialog("验证码输入不正确"); return; }
     
            if ($("#logPassword").val().replace(/[ ]/g, "") == "") { alertDialog("请输入密码"); return; }
            $.ajax({
                type: "POST",
                async: false,
                url: "/Login/CheckUserPassWord",
                data: { password: $("#logPassword").val().replace(/[ ]/g, ""), Phone: dianhua },
                success: function (data) {             
                    if (data) {
                        $("#pwds").hide();
                        window.location.href = "/Member/";
                    } else {
                        //$("#pwds").show();
                        alertDialog("密码输入错误，请重新输入");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                },
            });
        });
        $("#forgetPW").click(function () {
            window.location.href = "/Login/forget_pwd?phone=" + dianhua;
        });        
    });

    //end
});

seajs.use('login');
