/*!
 *设置交易密码
 *
 */

define('fetch_pwd_setting', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库 
    var gxdialog = require("gxdialog");
    //设置密码标识
    var floagWord = false;
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
    // 确认修改密码
    $("#btnOK").click(function () {       
        var Npw = $("#fetchPassword").val().replace(/[ ]/g, "");    
        if (Npw == "") { $("#MessageShow").html("请输入设置交易密码"); return; } else { $("#MessageShow").empty(); }
        if (!floagWord) { $("#MessageShow").html("设置交易密码为6位数字，请重新输入"); return; } else { $("#MessageShow").empty(); }
        $.ajax({
            type: "POST",
            async: false,
            url: "/UserSetting/UpdateUserSallpassword",
            data: { PassWord: Npw },
            success: function (data) {
                if (data) {
                    event.preventDefault();
                    autoCloseDialog();
                } else { alertDialog("设置交易密码失败"); }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });
    });
    /*
    //显示明文密码
    $("#ShowPasswerd").mousedown(function () {
        var temp = $("#fetchPassword").val();
        $("#pwspn").html("<input type='text' class='input-txt fetch-pwd' id='fetchPassword' name='fetchPassword'   value='" + temp + "' />");
    })
    $("#ShowPasswerd").mouseup(function () {
        var temp = $("#fetchPassword").val();
        $("#pwspn").html("<input type='password' class='input-txt fetch-pwd' id='fetchPassword' name='fetchPassword' value='" + temp + "' />");
        var pw = $("#fetchPassword").val().replace(/[ ]/g, "");
        var reg = /^[0-9]{6}$/;
        if (reg.test(pw)) {
            floagWord = true;
        } else { floagWord = false; }
        $("#fetchPassword").bind("blur", passwordONblur);
    })*/
    //设置密码填写验证
    $("#fetchPassword").blur(function () {
        passwordONblur();
    });
    function passwordONblur() {
        var pw = $("#fetchPassword").val().replace(/[ ]/g, "");
        var reg = /^[0-9]{6}$/;
        if (reg.test(pw)) {
            floagWord = true;
        } else { floagWord = false; }
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
            _this.parent('dd').children('input').attr('type', 'tel');
        };
    });
});
seajs.use('fetch_pwd_setting');
