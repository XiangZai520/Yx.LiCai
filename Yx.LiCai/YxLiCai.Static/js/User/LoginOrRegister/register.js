/*!
 *
 *
 */

define('reg', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库 
    var gxdialog = require("gxdialog");
    $.gxDialog.defaults.background = '#000';
    var reg1 = /^[0-9a-zA-Z_/]{6,16}$/;
    var reg2 = /^[0-9]{6}$/;
    var reg3= /^.{6,16}$/;
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

    var inviteCode = $("#inviteCode").val();//邀请码

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
                    alertDialog("发送手机验证码失败");
                    return;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });

        var length = dian.length;
        var lastphone = dian.substr(parseInt(length - 4));

        var content = "验证码已发送至您****" + lastphone + "的手机，请耐心等待。";
        $(".yx-tips").css("display", "");
        $(".yx-tips").html(content);
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

    $('#phoneCode').on('input propertychange', function () {
        var phoneCode = $("#phoneCode").val().replace(/[ ]/g, "");
        $('#phoneCode').val(phoneCode);
        if (phoneCode == "") {
            $("#regBtn").addClass("ui-btn-gray");
            $("#regBtn").unbind("click");
        }
    })

    $('#regPassword').on('input propertychange', function () {
        $("#regBtn").unbind("click");
        var regPassword = $('#regPassword').val().replace(/[ ]/g, "");
        $('#regPassword').val(regPassword);
        if (regPassword == "" || !reg3.test(regPassword)) {
            $("#regBtn").addClass("ui-btn-gray");
        }
        else {
            $("#regBtn").removeClass("ui-btn-gray");

            //用户注册
            $("#regBtn").bind("click", function () {
                var code = $("#phoneCode").val().replace(/[ ]/g, "");
                var regPassword = $('#regPassword').val();
                if (code == "") { alertDialog("请输入短信验证码"); return false; }
                if (!reg2.test(code)) {
                    alertDialog("验证码输入错误，请修改"); return false;
                }
                if (regPassword == "") { alertDialog("请设置登录密码"); return false; }
                if (!reg1.test(regPassword)) {
                    alertDialog("密码格式错误，请修改为6-16位字母、数字、下划线组合"); return false;
                }

                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/Login/RegisterUser",
                    data: { password: regPassword, Phone: dianhua, InviteCode: inviteCode, RandomCode: code },
                    success: function (data) {
                        if (data.Status == 0) {
                            window.location.href = "/Member/";
                            return true;
                        }
                        else {
                            alertDialog(data.Message);
                            return false;
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    }
                });
            });
        }
    })

    $(document).ready(function () {
        $("#regBtn").addClass("ui-btn-gray");

        $("#phoneCode").val("");

        $("#regPassword").val("");

    })


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

