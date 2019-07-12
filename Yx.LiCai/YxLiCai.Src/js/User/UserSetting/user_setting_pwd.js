/*!
 *修改登录密码
 *
 */

define('user_setting_pwd', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库 
    var gxdialog = require("gxdialog");
    //设置密码标识
    var floagWord = false;
    //原密码标识
    var floagOldPW = false;
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
    $("#J_setting_pwd").click(function () {
        var Opw = $("#oldPassword").val().replace(/[ ]/g, "");
        var Npw = $("#regPassword").val().replace(/[ ]/g, "");
        if (Opw == "") { alertDialog("原密码没有输入，请输入"); return; } else { $("#MessageShow").empty(); }
        if (!floagOldPW) { alertDialog("原密码输入错误，请重新输入"); return; } else { $("#MessageShow").empty(); }
        if (Npw == "") { alertDialog("设置密码没有输入，请输入"); return; } else { $("#MessageShow").empty(); }
        if (!floagWord) { alertDialog("6-16位字母、数字、下划线！请重新输入"); return; } else { $("#MessageShow").empty(); }
        $.ajax({
            type: "POST",
            async: false,
            url: "/UserSetting/UpdateUserOldPassWord",
            data: { PassWord: Npw },
            success: function (data) {
                if (data) {
                    event.preventDefault();
                    autoCloseDialog();
                } else { alertDialog("修改失败"); }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });

    });
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
        $("#regPassword").bind("blur", passwordONblur);
    })*/
    //设置密码填写验证
    $("#regPassword").blur(function () {
        passwordONblur();
    });
    function passwordONblur() {
        var pw = $("#regPassword").val().replace(/[ ]/g, "");
        var reg = /^[0-9a-zA-Z_/]{6,16}$/;
        if (reg.test(pw)) {
            floagWord = true;
        } else { floagWord = false; }
      
    }

    //原密码检测
    $("#oldPassword").blur(function () {
        var pw = $("#oldPassword").val().replace(/[ ]/g, "");
        if (pw == "") { floagOldPW = false; return; }
        $.ajax({
            type: "POST",
            async: false,
            url: "/UserSetting/CheckUserOldPassWord",
            data: { PassWord: pw },
            success: function (data) {              
                if (data) { floagOldPW = true; $("#MessageShow").empty(); } else { floagOldPW = false; $("#MessageShow").html("原密码输入错误，请重新输入"); }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });
    });
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
seajs.use('user_setting_pwd');
