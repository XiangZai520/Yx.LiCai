/*!
 *设置交易密码
 *
 */

define('fetch_pwd_setting', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库 
    var gxdialog = require("gxdialog");
    var reg1 = /^[0-9]{6}$/;
    var reg2 = /^.{6}$/;
    var returnUrl = $("#returnUrl").val();

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
            info: '<div class="pop-box"><h3>设置成功</h3><p>您的交易密码设置成功<p><div class="cd-time">3</div></div>'
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

    $('#fetchPassword').on('input propertychange', function () {
        $("#btnOK").unbind("click");
        var fetchPassword= $('#fetchPassword').val();
        if (fetchPassword == "" || !reg2.test(fetchPassword)) {
            $("#btnOK").addClass("ui-btn-gray");
        }
        else {
            $("#btnOK").removeClass("ui-btn-gray");
            // 确认设置交易密码
            $("#btnOK").click(function () {       
                var Npw = $("#fetchPassword").val();    
                if (Npw == "") { alertDialog("请输入交易密码"); return; }
                if (!reg1.test(Npw)) {
                    alertDialog("新交易密码格式错误，请修改为6位数字"); return;
                }
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/UserSetting/SetUserSallpassword",
                    data: { PassWord: Npw },
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
seajs.use('fetch_pwd_setting');
