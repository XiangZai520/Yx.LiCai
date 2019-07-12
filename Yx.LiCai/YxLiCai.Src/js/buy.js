/*!
 * @description:buy
 *
 */

define('buy',['jquery','gxdialog','fastclick'],function(require, exports, module) {

	var $ = jQuery = require("jquery");//jquery库
	var FastClick = require("fastclick");
	var gxdialog = require("gxdialog");

	//FastClick.attach(document.body);


	$.gxDialog.defaults.background = '#000';



	$('#ok-buy').on('click', function(){
		$.gxDialog({
			title: '',
			width: 280,
			info: document.getElementById('J_buyPwd')
		});
	});

	$('#ok-pwd').on('click', function(){
		$.gxDialog({
			title: '',
			width: 280,
			info: document.getElementById('J_buyPwd')
		});
	});

	$('#ok-bank').on('click', function(){
		$.gxDialog({
			title: '',
			width: 280,
			info: document.getElementById('selectBank')
		});
	});


	$('#dealPassword').on('click', function(){
		$.gxDialog({
			title: '',
			width: 280,
			info: document.getElementById('J_buyPwd')
		});
	});

	$('#ok-get').on('click', function(){
		$.gxDialog({
			title: '',
			width: 280,
			info: document.getElementById('J_get')
		});
	});


	//银行卡选择
	var $bankSelect = $('.J_selectBank'),
		$bankInfor = $('#J_bankInfor');
	$bankInfor.on('click', function(){
		$.gxDialog({
			title: '',
			width: 280,
			closeBtn: false,
			info: document.getElementById('selectBank')
		});
	});

	$bankSelect.on('click', 'li', function(event) {
		event.preventDefault();
		var _this = $(this),
			bankLogo = _this.find('img').attr('src'),
			bankName = _this.find('p>b').html();
		$bankInfor.find('img').show().attr('src', bankLogo);
		$bankInfor.find('span').show().text(bankName)
		_this.addClass('checked');
		_this.siblings('li').removeClass('checked');
		$.gxDialog.close();
	});

	// 6个密码框
	var _box = $('.box-pwd');
	_box.find('input').each(function(idx, el) {
		var _self = $(this);
		_self.not('input[name=p1]').attr('disabled', 'disabled');
		_self.keyup(function(event) {
			if (el.name == "p1") {
				if (el.value.length == 0) {
					_box.find('input[name=p1]').attr('type', 'number');
				} else if(el.value.length == 1){
					_box.find('input[name=p1]').attr('type', 'password');
					_box.find('input[name=p2]').attr('disabled', false).focus();
				};
			} else if (el.name == "p2") {
				if (el.value.length == 0) {
					_box.find('input[name=p2]').attr({
						disabled: 'disabled',
						type: 'number'
					});
					_box.find('input[name=p1]').focus();
				} else if(el.value.length == 1){
					_box.find('input[name=p3]').attr('disabled', false).focus();
					_box.find('input[name=p2]').attr('type', 'password');
				};
			} else if (el.name == "p3") {
				if (el.value.length == 0) {
					_box.find('input[name=p3]').attr({
						disabled: 'disabled',
						type: 'number'
					});;
					_box.find('input[name=p2]').focus();
				} else if(el.value.length == 1){
					_box.find('input[name=p4]').attr('disabled', false).focus();
					_box.find('input[name=p3]').attr('type', 'password');
				};
			} else if (el.name == "p4") {
				if (el.value.length == 0) {
					_box.find('input[name=p4]').attr({
						disabled: 'disabled',
						type: 'number'
					});
					_box.find('input[name=p3]').focus();
				} else if(el.value.length == 1){
					_box.find('input[name=p5]').attr('disabled', false).focus();
					_box.find('input[name=p4]').attr('type', 'password');
				};
			} else if (el.name == "p5") {
				if(el.value.length == 0){
					_box.find('input[name=p5]').attr({
						disabled: 'disabled',
						type: 'number'
					});
					_box.find('input[name=p4]').focus();
				} else if (el.value.length == 1) {
					_box.find('input[name=p6]').attr('disabled', false).focus();
					_box.find('input[name=p5]').attr('type', 'password');
				};
			} else if (el.name == "p6") {
				if(el.value.length == 0){
					_box.find('input[name=p6]').attr({
						disabled: 'disabled',
						type: 'number'
					});
					_box.find('input[name=p5]').focus();
				} else if (el.value.length == 1) {
					_box.find('input[name=p6]').attr('type', 'password');
				};
			};
		});

	});





});

seajs.use('buy');
