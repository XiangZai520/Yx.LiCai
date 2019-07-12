/*!
 * @description:yixiu
 * @author:Eilvein
 * @version:V1.0.0
 * @update:2015/5/30
 */

define('Plus_income', ['jquery', 'gxdialog', 'fastclick', 'swipe', 'gxtabs'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库
    var FastClick = require("fastclick");
    var gxdialog = require("gxdialog");
    var Swipe = require('swipe');
    var gxtabs = require('gxtabs');

    //FastClick.attach(document.body);


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
            }
            else {

                //6个月产品
                $("#purchasedAmount").val($("#q6_MemberSum").val());
                $("#yiledBase").html($("#q6_YieldBase").val() + "<em>%</em>");
            }
        }
    });

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
            };
        }

        function setTab(elem) {
            for (var i = 0; i < pagination.length; i++) {
                pagination[i].className = pagination[i].className.replace('current', ' ');
            }
            elem.className += 'current';
        }
    }


    $(".loadmore a").on("click", function () {
        var thiselement = $(this);
        var ProductType = $(this).attr("id");
        ProductType = ProductType.replace("loadmore_", "");
        var CurrentPage = parseInt($("#CurrentPage_" + ProductType).val());
        CurrentPage = CurrentPage + 1;
        $.ajax({
            type: "POST",
            url: "/Member/GetData_ajax",
            data: { CurrentPage: CurrentPage, ProductType: ProductType },
            async: false,
            success: function (data) {
                if (data != null) {
                    if (data.IsLastPage == "1") {
                        thiselement.css("display", "none");
                    }
                    var strhtml = data.Content;
                    $("#List_" + ProductType).append(strhtml);
                    $("#CurrentPage_" + ProductType).val(CurrentPage);
                }
            }
        })
    })
});


seajs.use('Plus_income');
