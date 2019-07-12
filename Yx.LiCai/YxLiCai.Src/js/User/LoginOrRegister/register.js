/*!
 *
 *
 */

define('reg', ['jquery', 'gxdialog'], function (require, exports, module) {

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
    //验证码标识
    var floagCode = false;
    //注册车时密码标识
    var floagWord = false;

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
    /*
    //显示明文密码
    $("#ShowPasswerd").mousedown(function () {
        var temp = $("#regPassword").val();
        $("#pwspn").html("<input type='text' class='input-txt reg-pwd' id='regPassword' name='regPassword' placeholder='请输入6-16位字母、数字、下划线'  value='" + temp + "' />");
    })
    $("#ShowPasswerd").mouseup(function () {
        var temp = $("#regPassword").val();
        $("#pwspn").html("<input type='password' class='input-txt reg-pwd' id='regPassword' name='regPassword' placeholder='请输入6-16位字母、数字、下划线' value='" + temp + "' />");       
        var pw = $("#regPassword").val().replace(/[ ]/g, "");
        var reg = /^[0-9a-zA-Z_/]{6,16}$/;
        if (reg.test(pw)) {
            floagWord = true;
        } else { floagWord = false; }

        $("#regPassword").bind("blur", Checkpassword);
    })
    */


    //用户注册
    $("#regBtn").click(function () {
        var code = $("#phoneCode").val().replace(/[ ]/g, "");
        if (code == "") { alertDialog("请输入验证码！"); return; }
        if (!floagCode) {
            alertDialog("验证码输入不正确或者过期！"); return;
        }
        if ($("#regPassword").val().replace(/[ ]/g, "") == "") { alertDialog("请输入密码"); return; }
        if (!floagWord)
        { alertDialog("建议使用6-16位密码数字、字母、下划线！"); return; }
        $.ajax({
            type: "POST",
            async: false,
            url: "/Login/AddUser",
            data: { password: $("#regPassword").val().replace(/[ ]/g, ""), Phone: dianhua },
            success: function (data) {
                if (data) { window.location.href = "/Member/"; } else { alertDialog("该手机已经注册过"); }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });
    });
    //注册时密码填写验证
    $("#regPassword").blur(function () {
        Checkpassword();
    });
    function Checkpassword() {
        var pw = $("#regPassword").val().replace(/[ ]/g, "");
        if (pcheckPassword(pw)) {
            floagWord = true;
        } else { floagWord = false; }
    }

    //});
    $("#phoneCode").focus(function () {
        if ($("#phoneCode").val() == "机器人才不会输验证码") {
            $("#phoneCode").val("");
        }
    });

    $("#phoneCode").blur(function () {
        if ($("#phoneCode").val().replace(/[ ]/g, "") == "") {
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
    ///检测密码（）
    function pcheckPassword(str) {
        var reg = /^[0-9a-zA-Z_/]{6,16}$/;
        if (!reg.test(str)) {
            return false;
        } else {
            return true;
        }
    };
    // 显隐密码
    $('.show-pwd').on('click', function (e) {
        e.preventDefault();
        var _this = $(this);
        var isActive = _this.hasClass('yes-pwd');
        if (isActive) {
            _this.removeClass('yes-pwd');
            _this.parent('dd').children('input').attr('type', 'password');
        } else {
            _this.addClass('yes-pwd');
            _this.parent('dd').children('input').attr('type', 'text');
        };
    });
    
});

seajs.use('reg');

