/*!
 *添加银行卡
 *
 */

define('add_bank', ['jquery', 'gxdialog', 'jcookie'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库
    var gxdialog = require("gxdialog");
    var jcookie = require("jcookie");
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
    function autoCloseDialog(v) {
        $.gxDialog({
            title: '',
            width: 280,
            closeBtn: false,
            info: '<div class="pop-box"><h3>提示信息</h3><p>' + v + '<p><div class="cd-time">3</div></div>'
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
                };
            }, 1000)
        }
    }
    var flag = false;
    var InterValObj; //timer变量，控制时间
    var count = 60; //间隔函数，1秒执行
    var curCount;//当前剩余秒数
    var ecount = 60;
    var ecurCount;
    var eInterValObj;
    //timer处理函数
    function SetRemainTime() {
        if (curCount == 1) {
            window.clearInterval(InterValObj);//停止计时器
            $("#repOld").hide();
            $("#repNew").show();
            $("#repNew").html("重新发送");
            $("#TimeMove").html("60");
        }
        else {
            curCount--;
            $("#TimeMove").html(curCount);

        }
    }

    ////////////////////////////////////////////////////////////
    $("#repNewMsg").click(function () {
        sendMessage();
        //firstLoad();
    });
    //发送手机验证码
    function sendMessage() {
        var BankName = $("#BankName").html().replace(/[ ]/g, "");
        var BankCard = $("#BankCard").val().replace(/[ ]/g, "");
        var Phone = $("#Phone").val().replace(/[ ]/g, "");
        if (BankName == "") { alertDialog("请选择开户银行"); return; }
        if (BankCard == "") { alertDialog("请输入银行卡号"); return; }
        if (!luhmCheck(BankCard)) { alertDialog("银行卡输入不正确"); return; }
        if (Phone == "") { alertDialog("请输入手机号"); return; }
        if (!isMobel(Phone)) { alertDialog("手机号输入不正确"); return; }
        $.ajax({
            type: "POST",
            async: false,
            url: "/UserSetting/SendBoundBankCard",
            data: { BankName: BankName, BankCard: BankCard, Phone: Phone },
            success: function (data) {
                if (data.Status == 1) {
                    flag = true;
                    $("#repOld").show();
                    $("#repNew").hide();
                    curCount = count;
                    InterValObj = window.setInterval(SetRemainTime, 1000); //启动计时器，1秒执行一次
                } else {
                    flag = false; event.preventDefault();
                    autoCloseDialog(data.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });
    }

    $("#ok-bank").click(function () {
        var BankName = $("#BankName").html().replace(/[ ]/g, "");
        var BankCard = $("#BankCard").val().replace(/[ ]/g, "");
        var Phone = $("#Phone").val().replace(/[ ]/g, "");
        if (BankName == "") { alertDialog("请选择开户银行"); return; }
        var reg = /^(\d{15,19})$/g;
        if (BankCard == "") { alertDialog("请输入银行卡号"); return; }
        if (!reg.test(BankCard)) { alertDialog("银行卡输入不正确"); return; }
        if (Phone == "") { alertDialog("请输入手机号"); return; }
        if (!isMobel(Phone)) { alertDialog("手机号输入不正确"); return; }
        var phoneCode = $("#phoneCode").val().replace(/[ ]/g, "");
        if (phoneCode == "") { alertDialog("请输入手机验证码"); return; }
        //if (!flag) { alertDialog("手机验证码输入不正确"); return; }
        $.ajax({
            type: "POST",
            async: false,
            url: "/UserSetting/ConfimBoundBankCard",
            data: { BankName: BankName, BankCard: BankCard, Phone: Phone, validatecode: phoneCode },
            success: function (data) {
                if (data.Status == 1) { window.location.href = "/UserSetting/"; } else { alertDialog(data.Message); }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });
    });
    function isMobel(value) {
        if (/^13\d{9}$/g.test(value) || (/^14[0-9]\d{8}$/g.test(value)) || (/^15[0-9]\d{8}$/g.test(value)) || (/^18[0-9]\d{8}$/g.test(value)) || (/^17[0-9]\d{8}$/g.test(value)))
        { return true; }
        else
        { return false; }
    }
    //银行卡选择
    var $bankSelect = $('.J_selectBank'),
		$bankInfor = $('#J_bankInfor');
    $bankInfor.on('click', function () {
        $.gxDialog({
            title: '',
            width: 280,
            closeBtn: false,
            info: document.getElementById('selectBank')
        });
    });

    $bankSelect.on('click', 'li', function(event) {
        event.preventDefault();
        var _this = $(this),
            bankLogo = _this.find('img').attr('src'),
            bankName = _this.find('p>b').html();
        $bankInfor.find('img').show().attr('src', bankLogo);
        $bankInfor.find('span').show().text(bankName);
        $bankInfor.find('b').hide();
        _this.addClass('checked');
        _this.siblings('li').removeClass('checked');
        $.gxDialog.close();
    });

    /**
验证是否为银行卡
*/
    function luhmCheck(bankno) {
        var lastNum = bankno.substr(bankno.length - 1, 1);//取出最后一位（与luhm进行比较）

        var first15Num = bankno.substr(0, bankno.length - 1);//前15或18位
        var newArr = new Array();
        for (var i = first15Num.length - 1; i > -1; i--) {    //前15或18位倒序存进数组
            newArr.push(first15Num.substr(i, 1));
        }
        var arrJiShu = new Array();  //奇数位*2的积 <9
        var arrJiShu2 = new Array(); //奇数位*2的积 >9

        var arrOuShu = new Array();  //偶数位数组
        for (var j = 0; j < newArr.length; j++) {
            if ((j + 1) % 2 == 1) {//奇数位
                if (parseInt(newArr[j]) * 2 < 9)
                    arrJiShu.push(parseInt(newArr[j]) * 2);
                else
                    arrJiShu2.push(parseInt(newArr[j]) * 2);
            }
            else //偶数位
                arrOuShu.push(newArr[j]);
        }

        var jishu_child1 = new Array();//奇数位*2 >9 的分割之后的数组个位数
        var jishu_child2 = new Array();//奇数位*2 >9 的分割之后的数组十位数
        for (var h = 0; h < arrJiShu2.length; h++) {
            jishu_child1.push(parseInt(arrJiShu2[h]) % 10);
            jishu_child2.push(parseInt(arrJiShu2[h]) / 10);
        }

        var sumJiShu = 0; //奇数位*2 < 9 的数组之和
        var sumOuShu = 0; //偶数位数组之和
        var sumJiShuChild1 = 0; //奇数位*2 >9 的分割之后的数组个位数之和
        var sumJiShuChild2 = 0; //奇数位*2 >9 的分割之后的数组十位数之和
        var sumTotal = 0;
        for (var m = 0; m < arrJiShu.length; m++) {
            sumJiShu = sumJiShu + parseInt(arrJiShu[m]);
        }

        for (var n = 0; n < arrOuShu.length; n++) {
            sumOuShu = sumOuShu + parseInt(arrOuShu[n]);
        }

        for (var p = 0; p < jishu_child1.length; p++) {
            sumJiShuChild1 = sumJiShuChild1 + parseInt(jishu_child1[p]);
            sumJiShuChild2 = sumJiShuChild2 + parseInt(jishu_child2[p]);
        }
        //计算总和
        sumTotal = parseInt(sumJiShu) + parseInt(sumOuShu) + parseInt(sumJiShuChild1) + parseInt(sumJiShuChild2);

        //计算Luhm值
        var k = parseInt(sumTotal) % 10 == 0 ? 10 : parseInt(sumTotal) % 10;
        var luhm = 10 - k;
        if (lastNum == luhm) {
            return true;
        }
        else {
            return false;
        }
    }

});
seajs.use('add_bank');
