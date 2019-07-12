/*!
 *用户忘记密码进来页面
 *
 */

define('setting_pwd', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库 
    var gxdialog = require("gxdialog");

    $.gxDialog.defaults.background = '#000';

    var reg1 = /^[0-9a-zA-Z_/]{6,16}$/;
    var reg2 = /^.{6,16}$/;
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
            info: '<div class="pop-box"><h3>修改成功</h3><p>您的密码已修改成功<p><div class="cd-time">3</div></div>'
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
                    window.location.href = "/Login/RegisteredOrLogin";
                };
            }, 1000)
        }
    }

    $('#regPassword').on('input propertychange', function () {
        $("#nextBtn").unbind("click");
        var regPassword = $('#regPassword').val();
        if (regPassword == "" || !reg2.test(regPassword)) {
            $("#nextBtn").addClass("ui-btn-gray");
        }
        else {
            $("#nextBtn").removeClass("ui-btn-gray");
            //用户注册
            $("#nextBtn").bind("click", function () {
                var regPassword = $("#regPassword").val();

                if (regPassword == "") {
                    alertDialog("请输入密码");
                    return false;
                }
                if (!reg1.test(regPassword)) {
                    alertDialog("密码格式错误，请修改为6-16位字母、数字、下划线组合"); return false;
                }
                if (Code == "") {
                    alertDialog("手机验证码不正确！"); return false;
                }
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/Login/UpdateUserPassWord",
                    data: { Code: Code, password: regPassword, Phone: dianhua },
                    success: function (data) {
                        if (data.Status == 0) {
                            event.preventDefault();
                            autoCloseDialog();
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
    })


    $(document).ready(function () {
        $("#nextBtn").addClass("ui-btn-gray");

        $("#regPassword").val("");
    })

    // 显隐密码
    $('.show-pwd').on('click', function (e) {
        e.preventDefault();
        var _this = $(this);
        var isActive = _this.hasClass('yes-pwd');
        if (isActive) {
            _this.removeClass('yes-pwd');
            //_this.parent('dd').children('input').attr('type', 'password');
            $("#regPassword").attr('type', 'password');
        } else {
            _this.addClass('yes-pwd');
            //_this.parent('dd').children('input').attr('type', 'tel');
            $("#regPassword").attr('type', 'tel');
        };
    });

});
seajs.use('setting_pwd');
