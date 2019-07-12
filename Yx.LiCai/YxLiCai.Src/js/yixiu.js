/*!
 * @description:yixiu
 * @author:Eilvein
 * @version:V1.0.0
 * @update:2015/5/30
 */

define('yixiu',['jquery','gxdialog','fastclick','swipe','gxtabs'],function(require, exports, module) {

	var $ = jQuery = require("jquery");//jquery库
	var FastClick = require("fastclick");
	var gxdialog = require("gxdialog");
	var Swipe = require('swipe');
	var gxtabs = require('gxtabs');

    //FastClick.attach(document.body);

	// 月份点击Tabs
	$('.J_gxtabs').gxTabs({
	    tabList:".J_gxtabs_nav>ul>li",            //标题列表
	    tabContent:".J_gxtabs_container .J_gxtabs_item",  //内容列表
	    tabOn:"active",                		// 菜单划过的类名
	    action:"click",                      // click || mouseover
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

    //买入按钮
	$(".buy").bind("click", function (e) {
	    var product = getProductIDAndCategory();            
            category = product.category;

	    window.top.location.href = "/buy/buyproduct?ptype=" + category;
	});

    //详情按钮
	//$(".productdetail").bind("click", function (e) {
	//    var product = getProductIDAndCategory(),
    //        id = product.id,
    //        category = product.category
	//    window.top.location.href = "/Product/Index/" + category;
	//});

    // 产品卡字体颜色
	var $pCard = $('.item-box');
	$pCard.each(function (idx, el) {
	    var $progress = $pCard.eq(idx).find('.progress-setting'),
			$textColor = $pCard.eq(idx).find('.item-conts'),
			progressNum = $progress.data('num');
	    colorChange();
	    function colorChange() {
	        if (progressNum <= 19) {
	        } else if (progressNum <= 27) {
	            $textColor.find('.pt-2').css('color', '#FFF');
	        } else if (progressNum <= 45) {
	            $textColor.find('p').css('color', '#FFF');
	        } else if (progressNum <= 60) {
	            $textColor.find('p').css('color', '#FFF');
	        } else if (progressNum <= 68) {
	            $textColor.find('p').css('color', '#FFF');
	            $textColor.find('span').css('color', '#FFF');
	        } else {
	            $textColor.find('p').css('color', '#FFF');
	            $textColor.find('span').css('color', '#FFF');
	            $textColor.find('h3').css('color', '#FFF');
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
	        callback: function(index) {
	            setTab(pagination[index]);
	        }
	    });

	    for (var i = 0; i < pagination.length; i++) {
	        var _el = pagination[i];
	        _el.setAttribute('data-tab', i);
	        _el.onclick = function(e) {
	            e.preventDefault();
	            setTab(this);
	            tabs.slide(parseInt(this.getAttribute('data-tab'),10),300);
	        };
	    }

	    function setTab(elem) {
	        for (var i = 0; i < pagination.length; i++) {
	            pagination[i].className = pagination[i].className.replace('current',' ');
	        }
	        elem.className += 'current';
	    }
    }
});

seajs.use('yixiu');
