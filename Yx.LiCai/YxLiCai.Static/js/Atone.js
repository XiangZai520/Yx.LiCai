/*!
 * @description:more.js
 * @author:Eilvein
 * @version:V1.0.0
 * @update:2015/5/26
 */

define('Atone', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库
    //var FastClick = require("fastclick");
    //var loadmore = require("loadmore");
    var gxdialog = require("gxdialog");
    var OrderInfoIDS = $("#OrderInfoIDS").val();

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

    ////FastClick.attach(document.body);
    $(function () {
        var flag = $("#flag").val();
        if (flag != undefined) {
            var title = "";
            var info = "";
            var next_url = "";

            if (flag == 1) {
                title = "没有进行实名认证？";
                info = "现在就去实名认证。";
                next_url = "/UserSetting/uc_setting_status/";
            }
            else if (flag == 2) {
                title = "没有绑定银行卡？";
                info = "现在就去绑定银行卡。";
                next_url = "/UserSetting/uc_setting_status/";
            }
            if (flag != 0) {
                $.gxDialog({
                    title: title,
                    info: info,
                    buttons: [{
                        text: '确认',
                        callback: function () {
                            window.top.location = next_url;
                        }
                    }],
                    baseClass: 'ios'
                });
            }
        }
    })

    //$(".loadmore").loadMore({});
    $(".loadmore").on("click", function () {
        var thiselement = $(this);
        var CurrentPage = parseInt($("#CurrentPage").val());
        CurrentPage = CurrentPage + 1;
        $.ajax({
            type: "POST",
            url: "/Atone/GetData_ajax",
            data: { CurrentPage: CurrentPage },
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
    })

    $("#atone").on("click", function () {
        var OrderInfoIDS = "";
        $("input[name='cb']").each(function () {
            if ($(this).is(":checked"))
            {
                OrderInfoIDS = OrderInfoIDS + (OrderInfoIDS == "" ? "" : ",") + $(this).val();
            }
        })
        if (OrderInfoIDS == "") {
            alertDialog("请选择要赎回的订单");
            return false;
        }
        else {
            window.top.location = "/Atone/ConfirmAtone?OrderInfoIDS=" + OrderInfoIDS;
        }
    })

    //$("#ConfirmAtone").on("click", function () {
    //    //验证交易密码
    //    $("#J_buyPwd").show();
    //    return;
    //    var ConfirmUrl = $("#ConfirmUrl").val();
    //    window.top.location = ConfirmUrl;
    //})
    $('#ConfirmAtone').on('click', function () {
        $.gxDialog({
            title: '',
            width: 280,
            info: document.getElementById('J_buyPwd')
        });
    });
    $("#okbuy").on("click", function () {
        var AtoneTotalMoney = $("#AtoneTotalMoney").html();
        var oiids = $("#oiids").val();
        var sallpwd = $("#sallpwd").val();

        if (sallpwd=="") {
            alertDialog("请输入交易密码");
            return false;
        }
        $("#okbuy").hide();
        $.gxDialog.close();
        $("#okbuy").show();
        $.ajax({
            type: "POST",
            url: "/Atone/ExecAtone_ajax",
            data: { AtoneTotalMoney: AtoneTotalMoney, oiids: oiids, sallpwd: sallpwd },
            async: false,
            success: function (data) {
                if (data != null) {
                    if (data.result == 0)
                    {
                        window.top.location = "/Atone/ExecAtone?AtoneTotalMoney=" + data.AtoneTotalMoney;
                    }
                    else if (data.result == 1)
                    {
                        $("#sallpwd").val("")
                        //$.gxDialog({
                        //    title: '提示',
                        //    info: '交易密码不正确'
                        //});
                        alertDialog("交易密码错误，请修改");
                    }
                    else if (data.result == 2) {
                        $("#sallpwd").val("")
                        //$.gxDialog({
                        //    title: '提示',
                        //    info: '赎回失败，请重试'
                        //});
                        alertDialog("赎回失败，请重试");
                    }
                }
            }
        })
    })

    $("#fetch_pwd_setting").on('click', function () {
        window.location.href = "/Atone/ConfirmAtone?OrderInfoIDS=" + OrderInfoIDS;
    })


    $("#forget_SPFist_pwd").on('click', function () {
        window.location.href = "/Atone/ConfirmAtone?OrderInfoIDS=" + OrderInfoIDS;
    })

});

seajs.use('Atone');
