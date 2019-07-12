/*!
 *修改登录密码
 *
 */

define('user_setting_pwd', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库 
    var gxdialog = require("gxdialog");
    var reg1 = /^[0-9a-zA-Z_/]{6,16}$/;
    var reg2 = /^.{6,16}$/;

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
                    window.location.href = "/UserSetting/";
                };
            }, 1000)
        }
    }
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

    $('#oldPassword').on('input propertychange', function() {
        $("#J_setting_pwd").unbind("click");
        var Opw = $("#oldPassword").val();
        var Npw = $("#regPassword").val();
        if (Opw == "" || !reg2.test(Opw) || Npw == "" || !reg2.test(Npw)) {
            $("#J_setting_pwd").addClass("ui-btn-gray");
            $("#J_setting_pwd").unbind("click");
        } else {
            $("#J_setting_pwd").removeClass("ui-btn-gray");
            // 确认修改密码
            $("#J_setting_pwd").bind("click", function () {
                Opw = $("#oldPassword").val();
                Npw = $("#regPassword").val();
                if (Opw == "") {
                    alertDialog("请输入原始密码");
                    return false;
                }
                if (!reg1.test(Opw)) {
                    alertDialog("原始密码错误，请修改");
                    return false;
                }
                if (Npw == "") {
                    alertDialog("请输入新密码");
                    return false;
                }
                if (!reg1.test(Npw)) {
                    alertDialog("密码格式错误，请修改为6-16位字母、数字、下划线组合");
                    return false;
                }

                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/UserSetting/UpdateUserOldPassWord",
                    data: { OldPassWord: Opw, NewPassWord: Npw },
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
    });

    $('#regPassword').on('input propertychange', function() {
        $("#J_setting_pwd").unbind("click");
        var Opw = $("#oldPassword").val();
        var Npw = $("#regPassword").val();
        if (Opw == "" || !reg2.test(Opw) || Npw == "" || !reg2.test(Npw)) {
            $("#J_setting_pwd").addClass("ui-btn-gray");
        } else {
            $("#J_setting_pwd").removeClass("ui-btn-gray");
            // 确认修改密码
            $("#J_setting_pwd").bind("click", function() {
                Opw = $("#oldPassword").val();
                Npw = $("#regPassword").val();
                if (Opw == "") {
                    alertDialog("请输入原始密码");
                    return false;
                }
                if (!reg1.test(Opw)) {
                    alertDialog("原始密码错误，请修改");
                    return false;
                }
                if (Npw == "") {
                    alertDialog("请输入新密码");
                    return false;
                }
                if (!reg1.test(Npw)) {
                    alertDialog("密码格式错误，请修改为6-16位字母、数字、下划线组合");
                    return false;
                }

                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/UserSetting/UpdateUserOldPassWord",
                    data: { OldPassWord: Opw, NewPassWord: Npw },
                    success: function(data) {
                        if (data.Status == 0) {
                            event.preventDefault();
                            autoCloseDialog();
                            return true;
                        } else {
                            alertDialog(data.Message);
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
        $("#J_setting_pwd").addClass("ui-btn-gray");

        $("#oldPassword").val("");

        $("#regPassword").val("");
    });

});
seajs.use('user_setting_pwd');
