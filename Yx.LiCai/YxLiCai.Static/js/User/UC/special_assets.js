/*!
 *特享金列表页js
 *平扬
 */

define('special_assets', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库 
    var gxdialog = require("gxdialog");
    $.gxDialog.defaults.background = '#000';

    $(".loadmore a").on("click", function () {
        var thiselement = $(this); 
        var CurrentPage = parseInt($("#CurrentPage").val());
        CurrentPage = CurrentPage + 1;
        $.ajax({
            type: "POST",
            url: "/SpecialAssets/GetUserSpecialAssets",
            data: { page: CurrentPage },
            async: false,
            success: function (data) {
                if (data != null) {
                    if (data.IsLastPage == "1") {
                        thiselement.css("display", "none");
                    }
                    var strhtml = data.Content;
                    $("#List").append(strhtml);
                    $("#CurrentPage").val(CurrentPage);
                }
            }
        })
    });
    $("#WithdrawalUrl_s").on("click", function () {
        var IsRealCard = $("#IsRealCard_s").val();
        var IsBindBank = $("#IsBindBank_s").val();
        if (IsRealCard == 0) {
            $.gxDialog({
                title: "请先进行身份认证",
                info: "",
                buttons: [{
                    text: '确认',
                    callback: function () {
                        window.top.location = "/UserSetting/uc_setting_status?returnUrl=/SpecialAssets/Withdrawals";
                    }
                }],
                baseClass: 'ios'
            });
        }
        if (IsRealCard != 0 && IsBindBank == 0) {
            $.gxDialog({
                title: "请先绑定银行卡",
                info: "",
                buttons: [{
                    text: '确认',
                    callback: function () {
                        window.top.location = "/UserSetting/add_bank?returnUrl=/SpecialAssets/Withdrawals";
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
seajs.use('special_assets');
