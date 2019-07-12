/*!
 * @description:Register
 * @author:Eilvein
 * @version:V1.0.0
 * @update:2015/5/30
 */

define('reg',['jquery','gxdialog','fastclick'],function(require, exports, module) {

	var $ = jQuery = require("jquery");//jquery库
	var gxdialog = require("gxdialog");
	var FastClick = require("fastclick");

    // debugger;


	FastClick(document.body);


	// 显隐密码
	$('.show-pwd').on('click', function(e) {
		e.preventDefault();
		var _this = $(this);
		var isActive = _this.hasClass('yes-pwd');
		if (isActive) {
			_this.removeClass('yes-pwd');
			_this.parent('dd').children('input').attr('type', 'password');
		} else{
			_this.addClass('yes-pwd');
			_this.parent('dd').children('input').attr('type', 'text');
		};
	});





	// 验证超时提示
	$.gxDialog.defaults.background = '#000';
	//gxOvertime();
	function gxOvertime() {
		$.gxDialog({
			title: '',
			info: '<div class="pop-box"><p>验证码超时，请重新发送<p><div><button type="submit" class="ui-btn">确认</button></div></div>'
		});
	}



});

seajs.use('reg');
