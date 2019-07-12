/*!
 *忘记密码跳页
 *
 */

define('forget_pwd', ['jquery', 'gxdialog'], function (require, exports, module) {

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
    //start
    //验证码标识
    var floagCode = false;
    var InterValObj; //timer变量，控制时间
    var count = 60; //间隔函数，1秒执行
    var curCount;//当前剩余秒数
    if (dianhua != "")
    { sendMessage(); }
    $("#repNewMsg").click(function () {
        sendMessage();
    });

    //发送手机验证码
    function sendMessage() {
        $("#repOld").show();
        $("#repNew").hide();
        curCount = count;
        InterValObj = window.setInterval(SetRemainTime, 1000); //启动计时器，1秒执行一次

        $.ajax({
            type: "POST",
            async: false,
            url: "/Login/SendValidateCode",
            data: { account: dianhua },
            success: function (data) {
                if (!data) {
                    alertDialog("发送失败");
                } 
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });
    }
    //timer处理函数
    function SetRemainTime() {
        if (curCount == 1) {
            window.clearInterval(InterValObj);//停止计时器
            $("#repOld").hide();
            $("#repNew").show();
            $("#TimeMove").html("60");
        }
        else {
            curCount--;
            $("#TimeMove").html(curCount);
        }
    }
    $("#phoneCode").blur(function () {
        if ($("#phoneCode").val() == "") {
            $("#phoneCode").attr("placeholder", "机器人才不会输验证码");
            floagCode = false;
        } else {
            $.ajax({
                type: "POST",
                async: false,
                url: "/Login/CheckUserCode",
                data: { Code: $("#phoneCode").val().replace(/[ ]/g, ""), Phone: dianhua },
                success: function (data) {
                    if (data == "true") {
                        floagCode = true;
                    } else { floagCode = false; }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                },
            });
        }
    });
    //下一步按钮
    $("#btnNextok").click(function () {
        if ($("#phoneCode").val() == "") { alertDialog("请输入验证码"); return; }
        if (!floagCode) { alertDialog("手机验证码不正确或者已经过期"); return; }
        window.location.href = "/Login/setting_pwd?phone=" + dianhua;
    });
    $("#phoneCode").keyup(function () {
        var num = strlen($("#phoneCode").val().replace(/[ ]/g, ""));
        if (num == 6) { $("#btnNext").hide(); $("#btnNextok").show(); } else { $("#btnNext").show(); $("#btnNextok").hide(); }
    });
    function strlen(str) {
        var len = 0;
        for (var i = 0; i < str.length; i++) {
            var c = str.charCodeAt(i);
            //单字节加1 
            if ((c >= 0x0001 && c <= 0x007e) || (0xff60 <= c && c <= 0xff9f)) {
                len++;
            }
            else {
                len += 2;
            }
        }
        return len;
    }
    //end
});

seajs.use('forget_pwd');
