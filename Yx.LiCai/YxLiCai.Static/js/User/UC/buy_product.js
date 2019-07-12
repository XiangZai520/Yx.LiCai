define('buy', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库
    var gxdialog = require("gxdialog");
    var InterValObj;
    var oid;
    var p_type;
    var b_money;
    var buyCount = 0;
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

    //有确认按钮的弹出
    function alertDialog1(v) {
        $.gxDialog({
            title: '',
            width: 280,
            closeBtn: false,
            info: '<div class="pop-box"><p class="warning-color">' + v + '</p></div>',
            ok: function () { window.location.href = "/UserSetting/uc_setting_status" }
        });
    }


    $("#bindBank").on('click', function () {
        var ptype = $("#ptype").val();
        var isRealCard = $("#isRealCard").val();
        if (isRealCard != "1") { alertDialog1("未进行实名认证，请先认证"); return; }
        else {
            //window.location.href = "/UserSetting/add_bank/"

            var raisemoney = $("#raisemoney").val();
            window.location.href = "/UserSetting/add_bank?returnUrl=/Buy/Buy_product?ptype=" + ptype + "&buyMoney=" + raisemoney + "&buytype=" + buy_type;
        }
    })

    $("#fetch_pwd_setting").on('click', function () {
        var ptype = $("#ptype").val();
        var raisemoney = $("#raisemoney").val();
        buy_type = $(".checked").val();
        window.location.href = "/UserSetting/fetch_pwd_setting?returnUrl=/Buy/Buy_product?ptype=" + ptype + "&buyMoney=" + raisemoney + "&buytype=" + buy_type;
    })

    $("#forget_SPFist_pwd").on('click', function () {
        var ptype = $("#ptype").val();
        var raisemoney = $("#raisemoney").val();
        window.location.href = "/UserSetting/forget_SPFist_pwd?returnUrl=/Buy/Buy_product?ptype=" + ptype + "&buyMoney=" + raisemoney + "&buytype=" + buy_type;
    })

    $('#raisemoney').on('input propertychange', function () {
        //searchProductClassbyName();
        var raisemoney = $('#raisemoney').val();
        raisemoney = raisemoney.replace(/[^\d.]/g, "");
        //必须保证第一个为数字而不是.
        raisemoney = raisemoney.replace(/^\./g, "");
        //保证只有出现一个.而没有多个.
        raisemoney = raisemoney.replace(/\.{2,}/g, ".");
        //保证.只出现一次，而不能出现两次以上
        raisemoney = raisemoney.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");

        raisemoney = raisemoney.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');

        $('#raisemoney').val(raisemoney);

        if (raisemoney == "" || parseFloat(raisemoney) < 100) {
            //$("#buyit").css("background", "#ddd");
            //$("#buyit").css("box-shadow", "0 5px 0 0 #ddd");
            $("#buyit").addClass("ui-btn-gray");
            $("#buyit").unbind("click");
        }
        else {

            //$("#buyit").css("background", "#00b7ee");
            //$("#buyit").css("box-shadow", "0 5px 0 0 #00b7ee");

            $("#buyit").removeClass("ui-btn-gray");

            $('#buyit').on('click', function () {
                $("#sallpwd").val("");
                var raisemoney = $("#raisemoney").val();//投资金额
                var balance = parseFloat($("#balance").val());//余额金额 
                var rbuy = $(".checked").val();
                var AvailableAmount = $("#AvailableAmount").html();

                $("#sallmoney span").html(raisemoney);
                if (isNaN(raisemoney)) { alertDialog("请输入买入金额"); return; }

                if (raisemoney.replace(/[ ]/g, "") == "") { alertDialog("请输入买入金额"); return; }

                raisemoney = parseFloat(raisemoney);

                if (raisemoney < 100) { alertDialog("买入金额需大于100元"); return; }

                if (AvailableAmount < raisemoney) {
                    alertDialog("输入金额不可大于可购买金额"); return;
                }

                if (rbuy == 1) {//使用余额付款           
                    if (balance < raisemoney) {
                        alertDialog("余额不足，建议使用银行卡购买"); return;
                    }
                }
                if (isBindBank == "0") {//未绑定银行卡
                    var bankCardNum = $("#bankCardNum").val();
                    if (bankCardNum == "") {
                        alertDialog("请输入银行卡号"); return;
                    }
                }
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

        $("#buyit").addClass("ui-btn-gray"); 
 
        //买入
        $("#okbuy").click(function () {
            $("#okbuy").hide();
            $.gxDialog.close();
            $("#okbuy").show();
            var raisemoney = $("#raisemoney").val();//投资金额
            var ptype = $("#ptype").val();//购买产品类型
            var addrateids = $("#addrateids").val();//加息券id列表
            var productid = $("#productid").val();//购买产品id
            var sallpwd = $("#sallpwd").val();//密码
            var rbuy = $(".checked").val();
            if (sallpwd.replace(/[ ]/g, "") == "") { alertDialog("请输入交易密码"); return; }
            if (raisemoney.replace(/[ ]/g, "") == "") { alertDialog("请输入买入金额"); return; }
 

            if (rbuy == 1) {//余额买入
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/Buy/BalanceBuyProducts",
                    data: { ptype: ptype, money: raisemoney, ids: addrateids,sallpwd: sallpwd },
                    success: function (data) {
                        if (data.Status == 0) {
                            $("#sallpwd").val("");//
                            window.location.href = "/Buy/BuySuccess?ptype=" + ptype + "&money=" + raisemoney;
                        } else { alertDialog(data.Message); return false; }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    },
                });
            } else {//银行卡购买 
                var bankCardNum = "";
                if (isBindBank == "0") {//未绑定银行卡 
                    bankCardNum = $("#bankCardNum").val();
                    if (bankCardNum == "") {
                        alertDialog("请输入银行卡号"); return;
                    }
                } 
                $.ajax({
                    type: "POST",
                    async: false,
                    url: "/Buy/PayBuyProductsByLLFirst",
                    data: { ptype: ptype, money: raisemoney, ids: addrateids, bankCardNum: bankCardNum, sallpwd: sallpwd },
                    success: function (data) {
                        if (data.Status == 0) {
                            window.location.href = data.Message;
                        } else { alertDialog(data.Message); return false; }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alertDialog("服务器异常"); return;
                    }
                });
            }

        });

        $("#AddInterestCount").click(function () {
            var ptype = $("#ptype").val();
            var raisemoney = $("#raisemoney").val();
            var ids = $("#ids").val();
            buy_type = $(".checked").val();
            window.location.href = "/buy/addinterest?ptype=" + ptype + "&buyMoney=" + raisemoney + "&ids=" + ids + "&buytype=" + buy_type;
        });

        var raisemoney = $('#raisemoney').val();
        if (raisemoney != "") {
            $("#buyit").removeClass("ui-btn-gray");

            $('#buyit').on('click', function () {
                $("#sallpwd").val("");
                var raisemoney = $("#raisemoney").val();//投资金额
                var balance = parseFloat($("#balance").val());//余额金额 
                var rbuy = $(".checked").val();
                var AvailableAmount = $("#AvailableAmount").html();

                $("#sallmoney span").html(raisemoney);
                if (isNaN(raisemoney)) { alertDialog("请输入买入金额"); return; }

                if (raisemoney.replace(/[ ]/g, "") == "") { alertDialog("请输入买入金额"); return; }

                raisemoney = parseFloat(raisemoney);

                if (raisemoney < 100) { alertDialog("买入金额需大于100元"); return; }

                if (AvailableAmount < raisemoney) {
                    alertDialog("输入金额不可大于可购买金额"); return;
                }

                if (rbuy == 1) {//使用余额付款           
                    if (balance < raisemoney) {
                        alertDialog("余额不足，建议使用银行卡购买"); return;
                    }
                }

                if (isBindBank == "0") {//未绑定银行卡
                    var bankCardNum = $("#bankCardNum").val();
                    if (bankCardNum == "") {
                        alertDialog("请输入银行卡号"); return;
                    }
                }

                $.gxDialog({
                    title: '',
                    width: 280,
                    info: document.getElementById('J_buyPwd')
                });
                $("#sallpwd").focus();
            });
        }

        /**
	 * 付款方式选择
	 * [payWay description]
	 * @return {[type]} [description]
	 */
        var payWay = function () {
            var $el = $('.J_payWay');
            $.each($el, function (idx, val) {
                var _this = $(this);
                var inputRadio = _this.find('input[type=radio]');
                _this.on('click', function (e) {
                    e.preventDefault();
                    if (!inputRadio.attr('checked')) {
                        $el.find('input[type=radio]').removeAttr('checked').removeClass('checked');
                        inputRadio.attr('checked', 'checked').addClass('checked');
                    }
                });
            });
        };
        payWay();

        //银行卡选择
        var banksSelect = function() {
            var $bankSelect = $('.J_selectBank'),
                $bankInfor = $('#J_bankInfor');
            // 弹出
            $bankInfor.on('click', function() {
                $.gxDialog({
                    title: '',
                    width: 280,
                    closeBtn: true,
                    info: document.getElementById('selectBank')
                });
            });
        };
        banksSelect();

    });


});

seajs.use('buy');
