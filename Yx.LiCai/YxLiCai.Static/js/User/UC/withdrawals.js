/*!
 *
 *
 */

define('withdrawals', ['jquery', 'gxdialog'], function (require, exports, module) {

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

    $("#fetch_pwd_setting").on('click', function () {
        var raisemoney = $("#moneyNum").val();
        window.location.href = "/UserSetting/fetch_pwd_setting?returnUrl=/Member/Withdrawals?buyMoney=" + raisemoney;
    })


    $("#forget_SPFist_pwd").on('click', function () {
        var raisemoney = $("#moneyNum").val();
        window.location.href = "/UserSetting/forget_SPFist_pwd?returnUrl=/Member/Withdrawals?buyMoney=" + raisemoney;
    })

    $('#moneyNum').on('input propertychange', function () {
        //searchProductClassbyName();
        $("#regBtn").unbind("click");
        var WithdrawalsCount = $("#WithdrawalsCount").val();
        var moneyNum = $('#moneyNum').val();
        moneyNum = moneyNum.replace(/[^\d.]/g, "");
        //必须保证第一个为数字而不是.
        moneyNum = moneyNum.replace(/^\./g, "");
        //保证只有出现一个.而没有多个.
        moneyNum = moneyNum.replace(/\.{2,}/g, ".");
        //保证.只出现一次，而不能出现两次以上
        moneyNum = moneyNum.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
        //保证是小数点后两位
        moneyNum = moneyNum.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');

        $('#moneyNum').val(moneyNum);

        if (moneyNum == "" || parseFloat(moneyNum) <= 0 || parseInt(WithdrawalsCount)<=0) { 
            $("#regBtn").addClass("ui-btn-gray");
            $("#regBtn").unbind("click");
        }
        else { 
            
            $("#regBtn").removeClass("ui-btn-gray");

            $('#regBtn').on('click', function () {
                $("#sallpwd").val("");
                var smoney = $("#smoney").val();//可提现金额
                var moneyNum = $("#moneyNum").val();//用户想提现金额
                if (moneyNum.replace(/[ ]/g, "") == "") { //alertDialog("请输入提现金额"); 
                    return false;
                }
                if (parseFloat(smoney) < parseFloat(moneyNum)) { alertDialog("提现金额不足，请修改"); return false; }
                $("#sallmoney").html("<span>" + moneyNum + "</span>元");
                $.gxDialog({
                    title: '',
                    width: 280,
                    info: document.getElementById('J_buyPwd')
                });
                $("#sallpwd").focus();
            });
        }
    });
 
    $(document).ready(function () { 
        $("#regBtn").addClass("ui-btn-gray");

        var moneyNum = $('#moneyNum').val();
        if (moneyNum != "" || parseFloat(moneyNum) > 0) { 
            $("#regBtn").removeClass("ui-btn-gray");

            $('#regBtn').on('click', function () {
                var smoney = $("#smoney").val();//可提现金额
                var moneyNum = $("#moneyNum").val();//用户想提现金额
                if (moneyNum.replace(/[ ]/g, "") == "") { //alertDialog("请输入提现金额"); 
                    return false;
                }
                if (parseFloat(smoney) < parseFloat(moneyNum)) { alertDialog("提现金额不足，请修改"); return false; }
                $("#sallmoney").html("提现:<span>" + moneyNum + "</span>元");
                $.gxDialog({
                    title: '',
                    width: 280,
                    info: document.getElementById('J_buyPwd')
                });
                $("#sallpwd").focus();
            });
        }

        //提现
        $("#okbuy").click(function () {
            $("#okbuy").hide();
            $.gxDialog.close();
            $("#okbuy").show();
            var smoney = $("#smoney").val();//可提现金额
            var moneyNum = $("#moneyNum").val();//用户想提现金额
            if (moneyNum.replace(/[ ]/g, "") == "") { alertDialog("请输入提现金额"); return false; }
            if (parseFloat(moneyNum) < 0.01) { alertDialog("提现金额需大于0.01元"); return false; }
            if (parseFloat(smoney) < parseFloat(moneyNum)) { alertDialog("提现金额不足，请修改"); return false; }
            if (parseFloat(moneyNum) > 50000) {
                alertDialog("每天最多提现金额为50000元，请修改"); return false;
            }
            var sallpwd = $("#sallpwd").val();
            if (sallpwd.replace(/[ ]/g, "") == "") { alertDialog("请输入交易密码"); return false; }
            $.ajax({
                type: "POST",
                async: false,
                url: "/Member/UserWithdrawals",
                data: { money: moneyNum, sallpwd: sallpwd },
                success: function (data) {
                    if (data.IsSuccess) { 
                        window.location.href = "/Member/WithdrawalsSuccess?money=" + moneyNum;
                    } else { alertDialog(data.Message); return false; }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                },
            });
        });

        $("#WithdrawalUrl").on("click", function () {
            var IsRealCard = $("#IsRealCard").val();
            var IsBindBank = $("#IsBindBank").val();
            if (IsRealCard == 0) {
                $.gxDialog({
                    title: "请先进行身份认证",
                    info: "",
                    buttons: [{
                        text: '确认',
                        callback: function () {
                            window.top.location = "/UserSetting/uc_setting_status?returnUrl=/Member/Withdrawals";
                        }
                    }],
                    baseClass: 'ios'
                });
            }
            //if (IsRealCard!=0&& IsBindBank == 0) {
            //    $.gxDialog({
            //        title: "请先绑定银行卡",
            //        info: "",
            //        buttons: [{
            //            text: '确认',
            //            callback: function () {
            //                window.top.location = "/UserSetting/add_bank?returnUrl=/Member/Withdrawals";
            //            }
            //        }],
            //        baseClass: 'ios'
            //    });
            //}
            if (IsRealCard != 0 && IsBindBank == 0) {
                $.gxDialog({
                    title: "首次投资后收益可提现",
                    info: "",
                    buttons: [{
                        text: '确认',
                        callback: function () {
                            window.top.location = "/Member/Index";
                        }
                    }],
                    baseClass: 'ios'
                });
            }
            if (IsRealCard != 0 && IsBindBank != 0) {
                window.location.href = "/Member/Withdrawals";
            }

        })
    }); 
});

seajs.use('withdrawals');
