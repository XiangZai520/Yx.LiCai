/*!
 *用户忘记密码进来页面
 *
 */

define('setting_pwd', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库 
    var gxdialog = require("gxdialog");

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
                    window.location.href = "/Login/RegisteredOrLogin";
                };
            }, 1000)
        }
    }
    //用户忘记密码标识
    var floagWord = false;
    $(document).ready(function (e) {
        //显示明文密码
        $("#ShowPasswerd").mousedown(function () {
            var temp = $("#regPassword").val();
            $("#pwspn").html("<input type='text' class='input-txt reg-pwd' id='regPassword' name='regPassword' placeholder='请输入6-16位字母、数字、下划线'   value='" + temp + "' />");
        })
        $("#ShowPasswerd").mouseup(function () {
            var temp = $("#regPassword").val();
            $("#pwspn").html("<input type='password' class='input-txt reg-pwd' id='regPassword' name='regPassword' placeholder='请输入6-16位字母、数字、下划线' value='" + temp + "' />");
            //$("#regPassword").bind("blur", Checkpassword);
            var pw = $("#regPassword").val().replace(/[ ]/g, "");
            var reg = /^[0-9a-zA-Z_/]{6,16}$/;
            if (reg.test(pw)) {
                floagWord = true;
            } else { floagWord = false; }
            $("#regPassword").bind("blur", Checkpassword);
        })
        //用户注册
        $("#nextBtn").click(function () {
            if ($("#regPassword").val().replace(/[ ]/g, "") == "") { alertDialog("请输入密码"); return; }
            if (!floagWord)
            { alertDialog("6-16位字母、数字、下划线！"); return; }
            $.ajax({
                type: "POST",
                async: false,
                url: "/Login/UpdateUserPassWord",
                data: { password: $("#regPassword").val().replace(/[ ]/g, ""), Phone: dianhua },
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
        //忘记密码填写验证
        $("#regPassword").blur(function () {
            Checkpassword();

        });
    });
    function Checkpassword() {
        var pw = $("#regPassword").val().replace(/[ ]/g, "");
        var reg = /^[0-9a-zA-Z_/]{6,16}$/;
        if (reg.test(pw)) {
            floagWord = true;
        } else { floagWord = false; }
    }
});
seajs.use('setting_pwd');
