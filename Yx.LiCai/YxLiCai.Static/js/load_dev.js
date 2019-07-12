/*!
 * @description:load header&&footer
 * @author:Eilvein
 * @version:V1.0.0
 * @update:2015/5/26
 */

define('load',['jquery'],function(require, exports, module) {

	var $ = jQuery = require("jquery");//jquery库

	var $header = $('#header'),
		$footer = $('#footer');
	// $header.load("/dist/header.html");
	$footer.load("/dist/footer.html");

	$('.bbb').hide();
	$('.aaa').on('click', '.ccc', function(event) {
		event.preventDefault();
		var _this = $(this);
		if (_this.hasClass('active')) {
			console.log("111");
			_this.css('color', '#333333');
			_this.siblings('.bbb').hide();
			_this.removeClass('active');
		} else{
			_this.siblings(".bbb").show();
			_this.css('color', 'red');
			_this.addClass('active');
			console.log("222")
		};
	});

	$('.aaa').on('click', 'span', function(event) {
		event.preventDefault();
		var _this = $(this);
		var _curr = _this.parent().siblings(".ccc");
		if (_curr.hasClass('active')) {
			_curr.css('color', '#333333').removeClass('active');
			_this.parent().hide();
			console.log("333")
		} else{
			console.log("444")
		};
	});

});

seajs.use('load');
