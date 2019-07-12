/*!
 *忘记交易密码修改
 *
 */

define('forget_SP_pwd', ['jquery', 'gxdialog'], function (require, exports, module) {

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
    function autoCloseDialog() {
        $.gxDialog({
            title: '',
            width: 280,
            closeBtn: false,
            info: '<div class="pop-box"><h3>修改成功</h3><p>一休哥提示，您的密码已修改成功<p><div class="cd-time">3</div></div>'
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
    //start
    //验证码标识
    var floagCode = false;
    var InterValObj; //timer变量，控制时间
    var count = 60; //间隔函数，1秒执行
    var curCount;//当前剩余秒数      
    if (userPhone != "")
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
            data: { account: userPhone },
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
                data: { Code: $("#phoneCode").val().replace(/[ ]/g, ""), Phone: userPhone },
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
    //确定修改
    $("#btnOK").click(function () {
        if ($("#phoneCode").val() == "") { alertDialog("请输入验证码"); return; }
        if (!floagCode) { alertDialog("验证码输入不正确或者过期！"); return; }
        var pw = $("#regPassword").val().replace(/[ ]/g, "");
        var pw = $("#regPassword").val().replace(/[ ]/g, "");
        var reg = /^[0-9]{6}$/;
        if (!reg.test(pw)) {
            alertDialog("密码格式有误，请重新输入"); return;
        }
        $.ajax({
            type: "POST",
            async: false,
            url: "/UserSetting/UpdateUserSallpassword",
            data: { PassWord: pw },
            success: function (data) {
                if (data) {
                    event.preventDefault();
                    autoCloseDialog();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });
    });
    //end
});

seajs.use('forget_SP_pwd');
