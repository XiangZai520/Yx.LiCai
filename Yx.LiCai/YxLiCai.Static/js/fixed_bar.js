/*!
 * @description:FixedBar
 * @author:Eilvein
 * @version:V1.0.0
 * @update:2015/7/8
 */

define('yx/fixedbar',['jquery', 'fastclick'],function(require, exports, module) {

	var $ = jQuery = require("jquery");//jquery库
	var FastClick = require("fastclick");

	// 移动端300ms延迟
	FastClick(document.body);

	var $fixedBar = $('.yx-fixedBar');
	var $back = $('.yx-back');
	/**
	 * 浮动按钮
	 * [fixedBox description]
	 * @param  {[type]}   [description]
	 * @return {[type]}   [description]
	 */
	var fixedBox = function () {
		$(window).scroll(function(){
			if($(this).scrollTop() > 100){
				$fixedBar.fadeOut();
			} else{
				$fixedBar.fadeIn();
			}
		})
	};
	fixedBox();

	/**
	 * 返回上一页
	 * [backEvent description]
	 * @param  {[type]}   [description]
	 * @return {[type]}   [description]
	 */
	var backEvent = function(){
		$back.on('click', function(e){
			e.preventDefault();
			window.history.back(-1);
		})
	};
	backEvent();

});

seajs.use('yx/fixedbar');
