/*!
 *
 *
 */

define('uc_setting_status', ['jquery', 'gxdialog', 'spin'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库
    var gxdialog = require("gxdialog");
    var Spinner = require("spin");
    var returnUrl = $("#returnUrl").val();
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

    function autoCloseDialog() {
        $.gxDialog({
            title: '',
            width: 280,
            closeBtn: false,
            info: '<div class="pop-box"><h3>认证成功</h3><p>实名认证成功，即刻开始e休理财<p><div class="cd-time">3</div></div>'
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
    //start

    var Fname = false;
    var FCard = false;

    $("#ok-bank").click(function () {

        var manCode = $("#IDCard").val().replace(/[ ]/g, "");
        if ($("#Name").val().replace(/[ ]/g, "") == "") { alertDialog("请输入姓名"); return; }
        if ($("#Name").val().length > 15) { alertDialog("姓名填写错误，请修改为15字内"); return; }
        if (manCode == "") { alertDialog("请输入身份证号码"); return; }


        var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
        if (!reg.test(manCode)) {
            alertDialog("身份证号码输入错误，请重新输入"); return;
        }
        len = manCode.length;
        if (len == 15) {
            year = "19" + manCode.substr(6, 2);
            if (year > 2015 || year < 1900) {
                alertDialog("身份证号码输入错误，请重新输入"); return;
            }
        } else if (len == 18) {
            year = manCode.substr(6, 4);
            if (year > 2015 || year < 1900) {
                alertDialog("身份证号码输入错误，请重新输入"); return;
            }
        }
        if (!checkIdcard(manCode)) { alertDialog("身份证号码输入错误，请重新输入"); return; }
        var opts = {
        };
        var target = document.getElementById('preview');
        var spinner = new Spinner(opts).spin(target);
        $.ajax({
            type: "POST",
            async: false,
            url: "/UserSetting/UpdateIsRealCard",
            data: { IDcard: manCode, Username: $("#Name").val().replace(/[ ]/g, "") },
            success: function (data) {
                spinner.stop();
                if (data.Status == 1) {
                    autoCloseDialog();
                } else {
                    alertDialog(data.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                spinner.stop();
                alertDialog("姓名与身份证号码不符，请重新输入");
            },
        });
    });

    /*
     *方法一
     */
    //身份证号合法性验证 
    //支持15位和18位身份证号
    //支持地址编码、出生日期、校验位验证
    function IdentityCodeValid(code) {
        var city = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江 ", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北 ", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏 ", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外 " };
        var tip = "";
        var pass = true;

        if (!code || !/^\d{6}(18|19|20)?\d{2}(0[1-9]|1[12])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$/i.test(code)) {
            tip = "身份证号格式错误";
            pass = false;
        }

        else if (!city[code.substr(0, 2)]) {
            tip = "地址编码错误";
            pass = false;
        }
        else {
            //18位身份证需要验证最后一位校验位
            if (code.length == 18) {
                code = code.split('');
                //∑(ai×Wi)(mod 11)
                //加权因子
                var factor = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2];
                //校验位
                var parity = [1, 0, 'X', 9, 8, 7, 6, 5, 4, 3, 2];
                var sum = 0;
                var ai = 0;
                var wi = 0;
                for (var i = 0; i < 17; i++) {
                    ai = code[i];
                    wi = factor[i];
                    sum += ai * wi;
                }
                var last = parity[sum % 11];
                if (parity[sum % 11] != code[17]) {
                    tip = "校验位错误";
                    pass = false;
                }
            }
        }
        if (!pass) alertDialog(tip);
        return pass;
    }
    /*
     *方法二验证
     */
    function checkIdcard(num) {
        num = num.toUpperCase();
        //身份证号码为15位或者18位，15位时全为数字，18位前17位为数字，最后一位是校验位，可能为数字或字符X。
        if (!(/(^\d{15}$)|(^\d{17}([0-9]|X)$)/.test(num))) {
            //alertDialog('身份证号码输入错误，请重新输入');
            return false;
        }
        //校验位按照ISO 7064:1983.MOD 11-2的规定生成，X可以认为是数字10。
        //下面分别分析出生日期和校验位
        var len, re;
        len = num.length;

        if (len == 15) {
            year = "19" + num.substr(6, 2);
            if (year > 2015 || year < 1900) {
                //alertDialog('身份证号码输入错误，请重新输入');           
                return false;
            }
        } else if (len == 18) {
            year = num.substr(6, 4);
            if (year > 2015 || year < 1900) {
                //alertDialog('身份证号码输入错误，请重新输入');
                return false;
            }
        }

        if (len == 15) {
            re = new RegExp(/^(\d{6})(\d{2})(\d{2})(\d{2})(\d{3})$/);
            var arrSplit = num.match(re);

            //检查生日日期是否正确
            var dtmBirth = new Date('19' + arrSplit[2] + '/' + arrSplit[3] + '/' + arrSplit[4]);
            var bGoodDay;
            bGoodDay = (dtmBirth.getYear() == Number(arrSplit[2])) && ((dtmBirth.getMonth() + 1) == Number(arrSplit[3])) && (dtmBirth.getDate() == Number(arrSplit[4]));
            if (!bGoodDay) {
                //alertDialog('身份证号码输入错误，请重新输入');
                return false;
            }
            else {
                //将15位身份证转成18位
                //校验位按照ISO 7064:1983.MOD 11-2的规定生成，X可以认为是数字10。
                var arrInt = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2);
                var arrCh = new Array('1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2');
                var nTemp = 0, i;
                num = num.substr(0, 6) + '19' + num.substr(6, num.length - 6);
                for (i = 0; i < 17; i++) {
                    nTemp += num.substr(i, 1) * arrInt[i];
                }
                num += arrCh[nTemp % 11];
                return true;
            }
        }
        if (len == 18) {
            re = new RegExp(/^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})([0-9]|X)$/);
            var arrSplit = num.match(re);

            //检查生日日期是否正确
            var dtmBirth = new Date(arrSplit[2] + "/" + arrSplit[3] + "/" + arrSplit[4]);
            var bGoodDay;
            bGoodDay = (dtmBirth.getFullYear() == Number(arrSplit[2])) && ((dtmBirth.getMonth() + 1) == Number(arrSplit[3])) && (dtmBirth.getDate() == Number(arrSplit[4]));
            if (!bGoodDay) {
                //alertDialog('身份证号码输入错误，请重新输入');
                return false;
            }
            else {
                //检验18位身份证的校验码是否正确。
                //校验位按照ISO 7064:1983.MOD 11-2的规定生成，X可以认为是数字10。
                var valnum;
                var arrInt = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2);
                var arrCh = new Array('1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2');
                var nTemp = 0, i;
                for (i = 0; i < 17; i++) {
                    nTemp += num.substr(i, 1) * arrInt[i];
                }
                valnum = arrCh[nTemp % 11];
                if (valnum != num.substr(17, 1)) {
                    //alertDialog('身份证号码输入错误，请重新输入');
                    return false;
                }
                return true;
            }
        }
        return false;
    }

    /*方法二*/
    //end
});

seajs.use('uc_setting_status');
