/*!
 * @description:product
 * @author:Leo
 * @version:V1.0.0
 * @update:2015/6/10
 */

define('caculate', ['jquery'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库

    //init
    var account_month = $("#account-month"),
        account_quarter = $("#account-quarter"),
        account_year = $("#account-year");

    //计算按钮event
    $(".income-account .ui-btn").bind("click", function () {
        var id = $(this).attr("id"),       
            top = $(this).parent("div").parent("div"),
            amount = top.find(".account-money").eq(0).val(), //买入金额
            duration = 0, //购买期限
            rate = parseFloat(top.find(".income-num").html().split('：')[1]), //利率
            expectedProfit = 0; //预期收益

        if (id == "year") { //年年丰
            expectedProfit = amount * 0.13;           
        }
        else {
            switch (id) {
                case "month": duration = top.find(".account-money").eq(1).val(); break;
                case "quarter": duration = top.find(".account-word-s span b").html() * 30; break;
            }

            if (!parseInt(amount) || !parseInt(duration) || parseInt(amount) <= 0 || parseInt(duration) <= 0)
                return;

            //计算预期收益
            // 季季享预期收益 = 投资金额 * 年化利率 / 365 * 投资天数   
            if (id == "month") {
                expectedProfit = countMonthExpectedProfit(amount, rate, duration);
            }
            else {
                expectedProfit = commonExpectedProfit(amount, rate, duration);
            }
        }

        top.find(".account-income").html("<h2>预期收益:</h2><b>" + expectedProfit.toFixed(2) + "</b>");
    });

    //计算月月赢预期收益
    function countMonthExpectedProfit(amount, rate, duration) {
        var expectedProfit = 0;

        if (duration > 300) {
            expectedProfit = amount * (12 / 100) * (duration - 300) / 365; //超过300天的部分

            //小于等于300天的部分
            expectedProfit += amount * (10 * ((7 + 11.5) / 100) / 2) * 30 / 365;
        }
        else {
            var stages = Math.floor((duration - 1) / 30), //多少期
                f_duration = duration - stages * 30,
                f_rate = 7 + 0.5 * stages;

            expectedProfit = commonExpectedProfit(amount, f_rate, f_duration);

            for (var i = 0; i < stages; i++) {
                var s_rate = 7 + i * 0.5,
                    s_duration = 30;

                expectedProfit += commonExpectedProfit(amount, s_rate, s_duration);
            }
        }

        return expectedProfit;
    }

    //计算预期收益
    // amount投资金额；rate年化利率； duration天数
    function commonExpectedProfit(amount, rate, duration) {
        var _currentRate = rate > 12 ? 12 : rate;
        return amount * (_currentRate / 100) / 365 * duration;
    }

    //输入框事件
    //购买金额输入框
    $('.invest-amount').on('input propertychange', function () {
        var moneyNum = $(this).val();
        moneyNum = moneyNum.replace(/[^\d.]/g, "");
        //必须保证第一个为数字而不是.
        moneyNum = moneyNum.replace(/^\./g, "");
        //保证只有出现一个.而没有多个.
        moneyNum = moneyNum.replace(/\.{2,}/g, ".");
        //保证.只出现一次，而不能出现两次以上
        moneyNum = moneyNum.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
        //保证是小数点后两位
        moneyNum = moneyNum.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');

        $(this).val(moneyNum);
    });

    //购买金额不小于100
    $('.invest-amount').on('blur', function () {
        var minAmount = 100,
            investAmount = $(this).val();
        investAmount = investAmount >= 100 ? investAmount : minAmount;
        $(this).val(investAmount);
    });
    //月月赢更改投资期限event
    $(".month-duration").bind("blur", function (e) {
        var duration = $(this).val(), //投资天数
            rate = 7; //起始利率7%
        if (!parseInt(duration) || duration <= 0)
            return;

        if (parseInt(duration) > 300) {
            rate = 12;
        }
        else {
            //每30天利率涨0.5%
            rate = rate + (Math.floor((duration - 1) / 30) * 0.5);
        }
        // set value
        account_month.find(".income-num").html("收益率：" + rate + "%+<span>0%</span>");
    });
});

seajs.use('caculate');
