define('yxClick', ['jquery', 'fastclick', 'gxdialog', 'gxtabs'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库
    var FastClick = require("fastclick");
    var gxdialog = require("gxdialog");
    var gxtabs = require('gxtabs');

    //FastClick.attach(document.body);

    $.gxDialog.defaults.background = '#000';

    //
    $('.J_dope').on('click', function (event) {
        event.preventDefault();
        $.gxDialog({
            title: '',
            width: 280,
            info: $('.dope-pup')
        });
    });


    // 收益大对比弹出
    $('.J_diff').on('click', function (event) {
        event.preventDefault();
        $.gxDialog({
            title: '',
            width: 280,
            info: document.getElementById('J_balance')
        });
    });


    //弹出计算器
    $('.counter-tools').on('click', function (event) {
        event.preventDefault();
        $.gxDialog({
            title: "",
            width: 280,
            info: $('.account')
        });
    });

    //计算器
    var flag = 1;
    $('.account-word-s').click(function () {
        if (flag == 1) {
            $(this).children('ul').show();
            $(this).children('i').addClass('hover');
            flag = 2;
        } else if (flag == 2) {
            $(this).children('ul').hide();
            $(this).children('i').removeClass('hover');
            flag = 1;
        }
    });
    $('.account-word-s ul li').click(function () {
        var duration = $(this).find('b').text();
        $('.account-word-s span b').text(duration);
        var rateHtml = "收益率：9%+<span>0%</span>";
        //季季享更改投资期限 -- 更新利率
        if (duration == 3) {
            rateHtml = "收益率：9%+<span>0%</span>";
        }
        else if (duration == 6) {
            rateHtml = "收益率：11%+<span>0%</span>";
        }
        $(this).parents(".acconnt-list").next().html(rateHtml);
    })
    $('#J_account').gxTabs({
        tabList: ".J_gxtabs_nav>li",
        tabContent: ".J_gxtabs_container .J_gxtabs_item",
        tabOn: "hover",
        action: "click", // click || mouseover
        afterChange: function () {

        }
    });



    //使用加息券
    var flag = 1;
    $('.add-money-left').click(function () {
        if (flag == 1) {
            $(this).addClass('hover');
            flag = 2;
        } else if (flag == 2) {
            $(this).removeClass('hover');
            flag = 1;
        }
    });
    // 确认赎回
    $('.atong-left').click(function () {
        if (flag == 1) {
            $(this).addClass('hover');
            flag = 2;
        } else if (flag == 2) {
            $(this).removeClass('hover');
            flag = 1;
        }
    });

    // 提现金额构成
    var flag = 1;
    $('.bring-up li h2').click(function () {
        if (flag == 1) {
            $(this).siblings('div').show();
            $(this).children('p').children('i').addClass('hover');
            flag = 2;
        } else if (flag == 2) {
            $(this).siblings('div').hide();
            $(this).children('p').children('i').removeClass('hover');
            flag = 1;
        }
    });


});

seajs.use('yxClick');
