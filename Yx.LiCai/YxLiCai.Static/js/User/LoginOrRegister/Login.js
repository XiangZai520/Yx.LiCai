/*!
 *
 *
 */

define('login', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库
    var gxdialog = require("gxdialog");
    $.gxDialog.defaults.background = '#000';
    var reg1 = /^[0-9a-zA-Z_/]{6,16}$/;
    var reg2 = /^[0-9]{4}$/;
    var reg3 = /^.{6,16}$/;
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

    $('#randomCode').on('input propertychange', function () {
        $("#regBtn").unbind("click");
        var logPassword = $('#logPassword').val().replace(/[ ]/g, "");
        var randomCode = $("#randomCode").val().replace(/[ ]/g, "");
        if (randomCode == "" || !reg2.test(randomCode) || logPassword == "" || !reg3.test(logPassword)) {
            $("#regBtn").addClass("ui-btn-gray");
            $("#regBtn").unbind("click");
        } else {
            $("#regBtn").removeClass("ui-btn-gray");

            //登录
            $("#regBtn").bind("click", function () {
                var randomCode = $("#randomCode").val().replace(/[ ]/g, "");
                var logPassword = $('#logPassword').val();
                if (randomCode == "") {
                    alertDialog("请输入验证码");
                    return false;
                }

                if (!reg2.test(randomCode)) {
                    alertDialog("验证码输入错误，请修改");
                    return false;
                }

                if (logPassword == "") {
                    alertDialog("请输入登录密码");
                    return false;
                }

                if (!reg1.test(logPassword)) {
                    alertDialog("密码输入格式错误，请修改");
                    return false;
                }

                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/Login/CheckLogin",
                    data: { password: logPassword, Phone: dianhua, RandomCode: randomCode },
                    success: function (data) {
                        if (data.Status == 0) {
                            window.location.href = "/Member/";
                            return true;
                        } else {
                            alertDialog(data.Message);
                            return false;
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    }
                });
            });

        }
    });

    $('#logPassword').on('input propertychange', function () {
        $("#regBtn").unbind("click");
        var logPassword = $('#logPassword').val().replace(/[ ]/g, "");
        var randomCode = $("#randomCode").val().replace(/[ ]/g, "");
        if (logPassword == "" || !reg3.test(logPassword) || randomCode == "" || !reg2.test(randomCode)) {
            $("#regBtn").addClass("ui-btn-gray");
        } else {
            $("#regBtn").removeClass("ui-btn-gray");

            //登录
            $("#regBtn").bind("click", function () {
                var randomCode = $("#randomCode").val().replace(/[ ]/g, "");
                var logPassword = $('#logPassword').val();
                if (randomCode == "") {
                    alertDialog("请输入验证码");
                    return false;
                }

                if (!reg2.test(randomCode)) {
                    alertDialog("验证码输入错误，请修改");
                    return false;
                }

                if (logPassword == "") {
                    alertDialog("请输入登录密码");
                    return false;
                }

                if (!reg1.test(logPassword)) {
                    alertDialog("密码输入格式错误，请修改");
                    return false;
                }

                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/Login/CheckLogin",
                    data: { password: logPassword, Phone: dianhua, RandomCode: randomCode },
                    success: function (data) {
                        if (data.Status == 0) {
                            window.location.href = "/Member/";
                            return true;
                        } else {
                            alertDialog(data.Message);
                            return false;
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    },
                });
            });

        }
    });

    $(document).ready(function () {
        $("#regBtn").addClass("ui-btn-gray");

        $("#randomCode").val("");

        $("#logPassword").val("");

        $("img.captcha").click(function () {
            $(this).attr("src", "/Login/CaptchaImage?a=" + new Date().getTime());
        });
    })
});

seajs.use('login');
