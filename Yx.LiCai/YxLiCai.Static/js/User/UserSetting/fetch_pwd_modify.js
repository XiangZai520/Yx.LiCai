/*!
 *
 *修改交易密码
 */

define('fetch_pwd_modify', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库 
    var gxdialog = require("gxdialog");
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
            info: '<div class="pop-box"><h3>修改成功</h3><p>您的交易密码已修改成功<p><div class="cd-time">3</div></div>'
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

    $('#oldPassword').on('input propertychange', function () {
        $("#J_setting_ok").unbind("click");
        var Opw = $("#oldPassword").val();
        var Npw = $("#fetchPassword").val();
        if (Opw == "" || !reg1.test(Opw) || Npw == "" || !reg1.test(Npw)) {
            $("#J_setting_ok").addClass("ui-btn-gray");
            $("#J_setting_ok").unbind("click");
        } else {
            $("#J_setting_ok").removeClass("ui-btn-gray");

            // 确认修改交易密码
            $("#J_setting_ok").bind("click", function () {

                if (Opw == "") {
                    alertDialog("请输入原交易密码");
                    return false;
                }
                if (!reg1.test(Opw)) {
                    alertDialog("原交易密码错误，请修改");
                    return false;
                }
                if (Npw == "") {
                    alertDialog("请输入新交易密码");
                    return false;
                }
                if (!reg1.test(Npw)) {
                    alertDialog("新交易密码格式错误，请修改为6位数字");
                    return false;
                }

                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/UserSetting/UpdateUserSallpassword",
                    data: { OldPassWord: Opw, NewPassWord: Npw },
                    success: function (result) {
                        if (result.Status == 0) {
                            event.preventDefault();
                            autoCloseDialog();
                            return true;
                        } else {
                            alertDialog(result.Message);
                            return false;
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    }
                });

            });
        }
    });

    $('#fetchPassword').on('input propertychange', function() {
        $("#J_setting_ok").unbind("click");
        var Opw = $("#oldPassword").val();
        var Npw = $("#fetchPassword").val();
        if (Opw == "" || !reg1.test(Opw) || Npw == "" || !reg1.test(Npw)) {
            $("#J_setting_ok").addClass("ui-btn-gray");
        } else {
            $("#J_setting_ok").removeClass("ui-btn-gray");

            // 确认修改交易密码
            $("#J_setting_ok").bind("click", function() {

                if (Opw == "") {
                    alertDialog("请输入原交易密码");
                    return false;
                }
                if (!reg1.test(Opw)) {
                    alertDialog("原交易密码错误，请修改");
                    return false;
                }
                if (Npw == "") {
                    alertDialog("请输入新交易密码");
                    return false;
                }
                if (!reg1.test(Npw)) {
                    alertDialog("新交易密码格式错误，请修改为6位数字");
                    return false;
                }

                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/UserSetting/UpdateUserSallpassword",
                    data: { OldPassWord: Opw, NewPassWord: Npw },
                    success: function(result) {
                        if (result.Status == 0) {
                            event.preventDefault();
                            autoCloseDialog();
                            return true;
                        } else {
                            alertDialog(result.Message);
                            return false;
                        }

                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                    }
                });

            });
        }
    });

    $(document).ready(function () {
        $("#J_setting_ok").addClass("ui-btn-gray");

        $("#oldPassword").val("");

        $("#fetchPassword").val("");
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
            _this.parent('dd').children('input').attr('type', 'tel');
        };
    });
});
seajs.use('fetch_pwd_modify');
