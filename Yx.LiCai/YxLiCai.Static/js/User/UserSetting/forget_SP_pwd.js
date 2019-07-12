/*!
 *忘记交易密码修改
 *
 */

define('forget_SP_pwd', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库 
    var gxdialog = require("gxdialog");
    var returnUrl = $("#returnUrl").val();
    $.gxDialog.defaults.background = '#000';
    var reg1 = /^[0-9]{6}$/;
    var reg2 = /^.{6}$/;

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
            info: '<div class="pop-box"><h3>修改成功</h3><p>您的交易密码修改成功<p><div class="cd-time">3</div></div>'
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
                    if (returnUrl != "") {
                        window.location.href = returnUrl;
                    }
                    else {
                        window.location.href = "/UserSetting/";
                    }
                };
            }, 1000)
        }
    }
    //start
    //验证码标识
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
                    alertDialog("发送验证码失败");
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

    $('#phoneCode').on('input propertychange', function () {
        var phoneCode = $('#phoneCode').val();
        $('#phoneCode').val(phoneCode);
        if (phoneCode == "") {
            $("#btnOK").addClass("ui-btn-gray");
            $("#btnOK").unbind("click");
        }
    })

    $('#regPassword').on('input propertychange', function () {
        $("#btnOK").unbind("click");
        var regPassword = $('#regPassword').val();
        $('#regPassword').val(regPassword);
        if (regPassword == "" || !reg2.test(regPassword)) {
        }
        else {
            $("#btnOK").removeClass("ui-btn-gray");
            //确定修改
            $("#btnOK").bind("click", function () {

                var phoneCode = $("#phoneCode").val();
                var regPassword = $("#regPassword").val();
                if (phoneCode == "") {
                    alertDialog("请输入验证码"); return;
                }
                if (!reg1.test(phoneCode)) {
                    alertDialog("验证码格式错误，请修改为6位数字"); return;
                }
                if (regPassword == "") {
                    alertDialog("请输入新交易密码"); return;
                }
                if (!reg1.test(regPassword)) {
                    alertDialog("交易密码格式错误，请修改为6位数字"); return;
                }
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/UserSetting/ForgetUserSallpassword",
                    data: { PassWord: regPassword, PhoneCode: phoneCode },
                    success: function (data) {
                        if (data.Status == 0) {
                            event.preventDefault();
                            autoCloseDialog();
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
        $("#btnOK").addClass("ui-btn-gray");

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

seajs.use('forget_SP_pwd');
