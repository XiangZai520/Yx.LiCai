/*!
 *
 *
 */

define('regs', ['jquery', 'gxdialog', 'fastclick'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库
    var FastClick = require("fastclick");
    var gxdialog = require("gxdialog");

    //FastClick.attach(document.body);

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
    //start

    //电话
    var dianhua = 0;
    //验证码标识
    var floag = false;
    //$(document).ready(function () {
    $("#number").blur(function () {
        dianhua = $("#number").val().replace(/[ ]/g, ""); //获得用户填写的号码值 赋值给变量dianhua        
        if (!isMobel(dianhua)) { floag = false; } else { floag = true; }
    });
    //});
    //下一步按钮事件
    $("#btnNextok").click(function () {
        if (!floag) { alertDialog("手机格式不正确！或者没有输入，请重新输入"); return; }
        var phoneNum = $("#number").val().trim().replace(/[ ]/g, "");
        $.ajax({
            type: "POST",
            async: false,
            url: "/Login/CheckUserIsMember",
            data: { Phone: phoneNum },
            success: function (data) {
                if (data) {
                    window.location.href = "/Login/Login?phone=" + phoneNum;
                }
                else {
                    window.location.href = "/Login/Registered?phone=" + phoneNum;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });
    });
    //下一步按钮失效情况
    $("#number").keyup(function () {       
        var num = strlen($("#number").val().replace(/[ ]/g, ""));      
        //if (num == 11) {$("#btnNext").removeAttr("class").attr("class", "ui-btn"); } else { $("#btnNext").removeAttr("class").attr("class", "ui-btn ui-btn-gray"); }
        if (num == 11) { $("#btnNext").hide(); $("#btnNextok").show(); } else { $("#btnNext").show(); $("#btnNextok").hide(); }
    });

    /** 
    * 验证手机号 
    *  
    * @param value 
    * @return 
    */
    function isMobel(value) {
        if (/^13\d{9}$/g.test(value) || (/^14[0-9]\d{8}$/g.test(value)) || (/^15[0-9]\d{8}$/g.test(value)) || (/^18[0-9]\d{8}$/g.test(value)) || (/^17[0-9]\d{8}$/g.test(value)))
        { return true; }
        else
        { return false; }
    }
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

    //end
});

seajs.use('regs');
