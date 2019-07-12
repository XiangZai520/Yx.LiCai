/*!
 * @description:mani.js
 * @author:Eilvein
 * @version:V1.0.0
 * @update:2015/5/26
 */

define('yixiu/main',['jquery','gxdialog','fastclick'],function(require, exports, module) {

	var $ = jQuery = require("jquery");//jquery库
	var FastClick = require("fastclick");
	var gxdialog = require("gxdialog");

	//FastClick.attach(document.body);

	// 设置弹出层背景颜色
	$.gxDialog.defaults.background = '#000';

	//通用错误弹出调用
	warningDialog("密码输入错误，请重新输入");


	//有确认按钮的弹出
	function warningDialog (v) {
		$.gxDialog({
			title: '',
			width: 280,
			closeBtn: false,
			info: '<div class="pop-box"><p class="warning-color">'+v+'</p></div>',
			ok: function(){}
		});
	}


	// 登录修改确认弹出
	$('#J_setting_pwd').on('click', function(event) {
		event.preventDefault();
		autoCloseDialog('修改成功','xxxxxxx');
	});
	// 取现修改确认弹出
	$('#J_setting_ok').on('click', function(event) {
		event.preventDefault();
		autoCloseDialog('修改成功', 'xxxxxxx');
	});
	// 3秒倒计时弹出
	function autoCloseDialog (t,v) {
		$.gxDialog({
			title: '',
			width: 280,
			closeBtn: false,
			info: '<div class="pop-box"><h3>'+t+'</h3><p>'+v+'<p><div class="cd-time">3</div></div>'
		});
		var secondNum = 3;
		loadJump(secondNum);
		function loadJump (num) {
			window.setTimeout(function () {
				num--;
				if (num > 0) {
					$('.cd-time').html(num);
					loadJump(num);
				} else{
					$.gxDialog.close();
				};
			}, 1000)
		}
	}


	// 密码不同弹出
	$('#findPassword').on('click', function(event) {
		event.preventDefault();
		$.gxDialog({
			title: '',
			width: 280,
			closeBtn: false,
			info: '<div class="pop-box"><p class="warning-color">取现密码不能与登录密码相同</p></div>'
		});
		window.setTimeout(function () {
			$.gxDialog.close();
		}, 3000)
	});



});

seajs.use('yixiu/main');
