/*!
 * @description:product
 * @author:Leo
 * @version:V1.0.0
 * @update:2015/6/10
 */

define('yixiu', ['jquery', 'gxdialog', 'fastclick', 'swipe', 'gxtabs'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库
    var FastClick = require("fastclick");
    var gxdialog = require("gxdialog");
    var Swipe = require('swipe');
    var gxtabs = require('gxtabs');

    var MonthId = $("#id_1").val();
    var Quarter3Id = $("#id_2").val();
    var Quarter6Id = $("#id_3").val();
    var YearId = $("#id_4").val();

    var EndTime1 = $("#itemType_1").find(".enableDate").val();
    var EndTime2 = $("#itemType_2").find(".enableDate").val();
    var EndTime3 = $("#itemType_3").find(".enableDate").val();
    var EndTime4 = $("#itemType_4").find(".enableDate").val();
    var NowTime = new Date();

    var et1 = new Date(EndTime1).getTime();
    var et2 = new Date(EndTime2).getTime();
    var et3 = new Date(EndTime3).getTime();
    var et4 = new Date(EndTime4).getTime();
    var nt = NowTime.getTime();
    var isRealCard = $("#IsRealCard").val();

    // 月份点击Tabs
    $('.J_gxtabs').gxTabs({
        tabList: ".J_gxtabs_nav>ul>li",            //标题列表
        tabContent: ".J_gxtabs_container .J_gxtabs_item",  //内容列表
        tabOn: "active",                		// 菜单划过的类名
        action: "click",                      // click || mouseover
        afterChange: function (tabId) {
            if (typeof ($("#purchasedAmount")) == "undefined")
                return;
            if (tabId == "0") {
                //3个月
                $("#purchasedAmount").val($("#q3_MemberSum").val());
                $("#yiledBase").html($("#q3_YieldBase").val() + "<em>%</em>");
                if (et2 > nt) {
                    $(".product-btn").html("");
                    $(".product-btn").append('<div class="next-time">距下期开售时间：<span class="t_h"></span>:<span class="t_m"></span>:<span class="t_s"></span></div><a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                }

                if (Quarter3Id == null || Quarter3Id == 0 || Quarter3Id=="") {
                    $(".product-btn").html("");
                    $(".product-btn").append('<a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                }

            }
            else {
                //6个月产品
                $("#purchasedAmount").val($("#q6_MemberSum").val());
                $("#yiledBase").html($("#q6_YieldBase").val() + "<em>%</em>");
                if (et3 > nt) {
                    $(".product-btn").html("");
                    $(".product-btn").append('<div class="next-time">距下期开售时间：<span class="t_h"></span>:<span class="t_m"></span>:<span class="t_s"></span></div><a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                }

                if (Quarter6Id == null || Quarter6Id == 0 || Quarter6Id == "") {
                    $(".product-btn").html("");
                    $(".product-btn").append('<a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                }
            }
        }
    });

    //用户当前浏览页面 产品id和类别
    function getProductIDAndCategory() {
        var categoryName = $(".current").children().html(),
          category = 1,
          id = "";
        if (categoryName == "月月盈") {
            id = $("#id_" + category).val();
        }
        else if (categoryName == "季季享") {
            category = parseInt($(".active").html()) == 3 ? 2 : 3;
            id = $("#id_" + category).val();
        }
        else if (categoryName == "年年丰") {
            category = 4;
        }

        return { id: id, category: category };
    }

    /*!
     * 移动端选项卡
     * @class touchTabs
     *
     */
    touchTabs();
    function touchTabs() {
        var elTabs = document.getElementById('tab-box');
        var pagination = document.getElementById('tab-pagination').getElementsByTagName('li');

        window.tabs = new Swipe(elTabs, {
            callback: function (index) {
                setTab(pagination[index]);
            }
        });

        for (var i = 0; i < pagination.length; i++) {
            var _el = pagination[i];
            _el.setAttribute('data-tab', i);
            _el.onclick = function (e) {
                e.preventDefault();
                setTab(this);
                tabs.slide(parseInt(this.getAttribute('data-tab'), 10), 300);
                if (parseInt(this.getAttribute('data-tab')) == 0) {
                    if (et1 > nt) {
                        $(".product-btn").html("");
                        $(".product-btn").append('<div class="next-time">距下期开售时间：<span class="t_h"></span>:<span class="t_m"></span>:<span class="t_s"></span></div><a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                    }

                    if (MonthId == null || MonthId == 0 || MonthId == "") {
                        $(".product-btn").html("");
                        $(".product-btn").append('<a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                    }
                }
                else if (parseInt(this.getAttribute('data-tab')) == 1) {
                    if (et2 > nt) {
                        $(".product-btn").html("");
                        $(".product-btn").append('<div class="next-time">距下期开售时间：<span class="t_h"></span>:<span class="t_m"></span>:<span class="t_s"></span></div><a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                    }

                    if (Quarter3Id == null || Quarter3Id == 0 || Quarter3Id == "") {
                        $(".product-btn").html("");
                        $(".product-btn").append('<a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                    }

                }
                else if (parseInt(this.getAttribute('data-tab')) == 2) {
                    if (et3 > nt) {
                        $(".product-btn").html("");
                        $(".product-btn").append('<div class="next-time">距下期开售时间：<span class="t_h"></span>:<span class="t_m"></span>:<span class="t_s"></span></div><a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                    }
                    if (Quarter6Id == null || Quarter6Id == 0 || Quarter6Id == "") {
                        $(".product-btn").html("");
                        $(".product-btn").append('<a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                    }
                }
                else if (parseInt(this.getAttribute('data-tab')) == 3) {
                    if (et4 > nt) {
                        $(".product-btn").html("");
                        $(".product-btn").append('<div class="next-time">距下期开售时间：<span class="t_h"></span>:<span class="t_m"></span>:<span class="t_s"></span></div><a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                    }
                    if (YearId == null || YearId == 0 || YearId == "") {
                        $(".product-btn").html("");
                        $(".product-btn").append('<a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                    }
                }
                else {
                    $(".product-btn").html("");
                    if (isRealCard == 0) {
                        $(".product-btn").append('<a class="ui-btn" style="cursor:pointer" href="/UserSetting/uc_setting_status">立即买入</a>');
                    }
                    else {
                        $(".product-btn").append('<a class="ui-btn buy" style="cursor:pointer">立即买入</a>');
                    }
                }
            };
        }

        function setTab(elem) {
            for (var i = 0; i < pagination.length; i++) {
                pagination[i].className = pagination[i].className.replace('current', ' ');
            }
            elem.className += 'current';
        }
    }

    //首页 产品详情 跳转到 Prduct/Index 
    //初始化页面，显示选中的产品
    (function () {
        var pID = $("#proID").val(); //Home/Index传值： 产品id
        if (pID == "" || pID == null)
            return;

        if (pID == 1) {
            //月月赢
            tabs.slide(0);
            if(et1>nt){
                $(".product-btn").html("");
                $(".product-btn").append('<div class="next-time">距下期开售时间：<span class="t_h"></span>:<span class="t_m"></span>:<span class="t_s"></span></div><a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
            }
            else {
                $(".product-btn").html("");
                $(".product-btn").append('<a class="ui-btn buy" onclick="buyNow()" href="javascript:void(0)" style="cursor:pointer">立即买入</a>');
            }

            if (MonthId == null || MonthId == 0 || MonthId == "") {
                $(".product-btn").html("");
                $(".product-btn").append('<a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
            }
        }
        else if (pID == 2 || pID == 3) {
            //季季享
            tabs.slide(1);
            $(".nav-season").children().removeClass("active");
            $(".J_gxtabs_container").children().hide();
            if (pID == 2) {
                //3个月
                $(".nav-season").children("li").eq(0).addClass("active");
                if(et2>nt){
                    $(".product-btn").html("");
                    $(".product-btn").append('<div class="next-time">距下期开售时间：<span class="t_h"></span>:<span class="t_m"></span>:<span class="t_s"></span></div><a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                }
                else {
                    $(".product-btn").html("");
                    $(".product-btn").append('<a class="ui-btn buy" onclick="buyNow()" href="javascript:void(0)" style="cursor:pointer">立即买入</a>');
                }

                if (Quarter3Id == null || Quarter3Id == 0 || Quarter3Id == "") {
                    $(".product-btn").html("");
                    $(".product-btn").append('<a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                }
            }
            else if (pID == 3) {
                //6个月
                $(".nav-season").children("li").eq(1).addClass("active");
                if(et3>nt){
                    $(".product-btn").html("");
                    $(".product-btn").append('<div class="next-time">距下期开售时间：<span class="t_h"></span>:<span class="t_m"></span>:<span class="t_s"></span></div><a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                }
                else {
                    $(".product-btn").html("");
                    $(".product-btn").append('<a class="ui-btn buy" onclick="buyNow()" href="javascript:void(0)" style="cursor:pointer">立即买入</a>');
                }

                if (Quarter6Id == null || Quarter6Id == 0 || Quarter6Id == "") {
                    $(".product-btn").html("");
                    $(".product-btn").append('<a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
                }
            }
            quarterSelected(pID)
        }
        else if (pID == 4) {
            //年年丰        
            tabs.slide(2);
            if(et4>nt){
                $(".product-btn").html("");
                $(".product-btn").append('<div class="next-time">距下期开售时间：<span class="t_h"></span>:<span class="t_m"></span>:<span class="t_s"></span></div><a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
            }
            else {
                $(".product-btn").html("");
                $(".product-btn").append('<a class="ui-btn buy" onclick="buyNow()" href="javascript:void(0)" style="cursor:pointer">立即买入</a>');
            }

            if (YearId == null || YearId == 0 || YearId == "") {
                $(".product-btn").html("");
                $(".product-btn").append('<a class="ui-btn ui-btn-gray" href="javascript:;">已售罄</a>');
            }
        }
    })();
    //季季享 3个月/6个月选中事件
    function quarterSelected(id) {
        if (id == 2) { //3个月
            $(".J_gxtabs_container").children("div").eq(0).show();
        }
        else if (id == 3) { //6个月          
            $(".J_gxtabs_container").children("div").eq(1).show();
        }
    }

    //买入按钮
    $(".buy").on("click", function (e) {
        var product = getProductIDAndCategory(),
            id = product.id,
            category = product.category;

        window.top.location.href = "/buy/buyproduct?ptype=" + category;
    });

    function buyNow() {
        var product = getProductIDAndCategory(),
            id = product.id,
            category = product.category;

        window.top.location.href = "/buy/buyproduct?ptype=" + category;
    }

    $('#amountBymonth').on('input propertychange', function () {
        //searchProductClassbyName();
        var amountBymonth = $('#amountBymonth').val();
        amountBymonth = amountBymonth.replace(/[^\d.]/g, "");
        //必须保证第一个为数字而不是.
        amountBymonth = amountBymonth.replace(/^\./g, "");
        //保证只有出现一个.而没有多个.
        amountBymonth = amountBymonth.replace(/\.{2,}/g, ".");
        //保证.只出现一次，而不能出现两次以上
        amountBymonth = amountBymonth.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");

        amountBymonth = amountBymonth.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');

        $('#amountBymonth').val(amountBymonth);

    });

    $('#amountByquarter').on('input propertychange', function () {
        //searchProductClassbyName();
        var amountByquarter = $('#amountByquarter').val();
        amountByquarter = amountByquarter.replace(/[^\d.]/g, "");
        //必须保证第一个为数字而不是.
        amountByquarter = amountByquarter.replace(/^\./g, "");
        //保证只有出现一个.而没有多个.
        amountByquarter = amountByquarter.replace(/\.{2,}/g, ".");
        //保证.只出现一次，而不能出现两次以上
        amountByquarter = amountByquarter.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");

        amountByquarter = amountByquarter.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');

        $('#amountByquarter').val(amountByquarter);

    });

    $('#amountByyear').on('input propertychange', function () {
        //searchProductClassbyName();
        var amountByyear = $('#amountByyear').val();
        amountByyear = amountByyear.replace(/[^\d.]/g, "");
        //必须保证第一个为数字而不是.
        amountByyear = amountByyear.replace(/^\./g, "");
        //保证只有出现一个.而没有多个.
        amountByyear = amountByyear.replace(/\.{2,}/g, ".");
        //保证.只出现一次，而不能出现两次以上
        amountByyear = amountByyear.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");

        amountByyear = amountByyear.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');

        $('#amountByyear').val(amountByyear);

    });

});

seajs.use('yixiu');
