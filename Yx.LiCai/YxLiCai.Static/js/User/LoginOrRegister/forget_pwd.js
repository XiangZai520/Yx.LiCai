/*!
 *忘记密码跳页
 *
 */

define('forget_pwd', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库 
    var gxdialog = require("gxdialog");
    $.gxDialog.defaults.background = '#000';
    var reg = /^[0-9]{6}$/;

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
                    alertDialog("发送验证码失败");
                    return;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });
        var length = dian.length;
        var lastphone = dian.substr(parseInt(length - 4));
        var content = "我们已将验证码发送至您****" + lastphone + "的手机，请耐心等待。";
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
        $("#btnNext").unbind("click");
        var phoneCode = $('#phoneCode').val();
        if (phoneCode == "" || !reg.test(phoneCode)) {
            $("#btnNext").addClass("ui-btn-gray");
        }
        else {
            $("#btnNext").removeClass("ui-btn-gray");
            //下一步按钮
            $("#btnNext").bind("click", function () {
                if ($("#phoneCode").val() == "") {
                    alertDialog("请输入验证码"); return false;
                }
                if (!reg.test(phoneCode)) {
                    alertDialog("验证码错误，请修改"); return false;
                }

                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/Login/CheckUserCode",
                    data: { Code: $("#phoneCode").val().replace(/[ ]/g, ""), Phone: dianhua },
                    success: function (data) {
                        if (data.Status == 1) {
                            window.location.href = "/Login/setting_pwd?phone=" + data.Message + "&Code=" + $("#phoneCode").val().replace(/[ ]/g, "");
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
    })

    $(document).ready(function () {
        $("#btnNext").addClass("ui-btn-gray");

        $("#phoneCode").val("");
    })
});

seajs.use('forget_pwd');
