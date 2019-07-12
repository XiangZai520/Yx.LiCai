/*!
 *忘记交易密码跳转页面
 *
 */

define('forget_SPFist_pwd', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库 
    var gxdialog = require("gxdialog");
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
                    window.location.href = "/UserSetting/";
                };
            }, 1000)
        }
    }
    //start
    function isCardNo(card) {
        // 身份证号码为15位或者18位，15位时全为数字，18位前17位为数字，最后一位是校验位，可能为数字或字符X  
        var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
        if (reg.test(card) === false) {
            return false;
        }
    }
    $("#manCode").keyup(function () {
        var num = strlen($("#manCode").val().replace(/[ ]/g, ""));
        if (num == 15 || num == 18) { $("#btnNext").hide(); $("#btnNextok").show(); } else { $("#btnNext").show(); $("#btnNextok").hide(); }
    });
    function strlen(str) {
        var len = 0;
        for (var i = 0; i < str.length; i++) {
            var c = str.charCodeAt(i);
            //单字节加1 
            if ((c >= 0x0001 && c <= 0x007e) || (0xff60 <= c && c <= 0xff9f)) {
                len++;
            }
            else {
                len += 2;
            }
        }
        return len;
    }
    //确定修改
    $("#btnNextok").click(function () {
        var manCode = $("#manCode").val().replace(/[ ]/g, "");
        var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
        if (manCode == "") { alertDialog("请输入身份证"); }
        if (!reg.test(manCode)) {
            alertDialog("身份证号码错误，请修改");
            return;
        }
        len = manCode.length;
        if (len == 15) {
            year = "19" + manCode.substr(6, 2);
            if (year > 2015 || year < 1900) {
                alertDialog("身份证号码错误，请修改"); return;
            }
        } else if (len == 18) {
            year = manCode.substr(6, 4);
            if (year > 2015 || year < 1900) {
                alertDialog('身份证号码错误，请修改'); return;
            }
        }

        $.ajax({
            type: "POST",
            async: false,
            url: "/UserSetting/CheckIDCardTrue",
            data: { IdCard: manCode },
            success: function (data) {
                if (data.Status == 1) {

                    if (returnUrl != "") {
                        window.location.href ="/UserSetting/forget_SP_pwd?returnUrl="+ returnUrl;
                    }
                    else {
                        window.location.href = "/UserSetting/forget_SP_pwd";
                    }

                } else { alertDialog(data.Message); return false; }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });
    });
    //end
});

seajs.use('forget_SPFist_pwd');
