/*!
 * @description:buy
 *
 */

define('buy',['jquery','gxdialog','fastclick'],function(require, exports, module) {

	var $ = jQuery = require("jquery");//jquery库
	var FastClick = require("fastclick");
	var gxdialog = require("gxdialog");

	//FastClick.attach(document.body);

	// 弹出背景设置
	$.gxDialog.defaults.background = '#000';



	// $('#ok-buy').on('click', function(){
	// 	$.gxDialog({
	// 		title: '',
	// 		width: 280,
	// 		info: document.getElementById('J_buyPwd')
	// 	});
	// });

	// $('#ok-pwd').on('click', function(){
	// 	$.gxDialog({
	// 		title: '',
	// 		width: 280,
	// 		info: document.getElementById('J_buyPwd')
	// 	});
	// });

	// $('#ok-bank').on('click', function(){
	// 	$.gxDialog({
	// 		title: '',
	// 		width: 280,
	// 		info: document.getElementById('selectBank')
	// 	});
	// });


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
	var banksSelect = function () {
		var $bankSelect = $('.J_selectBank'),
			$bankInfor = $('#J_bankInfor');
		// 弹出
		$bankInfor.on('click', function(){
			$.gxDialog({
				title: '',
				width: 280,
				closeBtn: true,
				info: document.getElementById('selectBank')
			});
		});
		//选择
		//$bankSelect.on('click', 'li', function(event) {
		//	event.preventDefault();
		//	var _this = $(this),
		//		bankLogo = _this.find('img').attr('src'),
		//		bankName = _this.find('p>b').html();
		//	$bankInfor.find('img').show().attr('src', bankLogo);
		//	$bankInfor.find('span').show().text(bankName)
		//	_this.addClass('checked');
		//	_this.siblings('li').removeClass('checked');
		//	$.gxDialog.close();
		//});
	}
	banksSelect();

	/**
	 * 6框密码输入，未使用
	 * [sixBoxIn description]
	 * @return {[type]} [description]
	 */
	function sixBoxIn () {
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
	}

	/**
	 * 显隐密码
	 * [showPassword description]
	 * @param  {[type]} obj [description]
	 * @return {[type]}     [description]
	 */
	var showPassword = function (obj) {
		var $el = obj;
		$el.on('click', function (e) {
			e.preventDefault();
			var _this = $(this);
			var isActive = _this.hasClass('yes-pwd');
			if (isActive) {
				_this.removeClass('yes-pwd');
				_this.parent('dd').children('input').attr('type', 'password');
			} else {
				_this.addClass('yes-pwd');
				_this.parent('dd').children('input').attr('type', 'tel');
			};
		});
	};
	showPassword($('.show-pwd'));

	/**
	 * 付款方式选择
	 * [payWay description]
	 * @return {[type]} [description]
	 */
	var payWay = function () {
		var $el = $('.J_payWay');
		$.each($el, function(idx, val) {
			var _this = $(this);
			var inputRadio = _this.find('input[type=radio]');
			_this.on('click', function(e) {
				e.preventDefault();
				if (!inputRadio.attr('checked')) {
					$el.find('input[type=radio]').removeAttr('checked').removeClass('checked');
					inputRadio.attr('checked','checked').addClass('checked');
				}
			});
		});
	};
	payWay();



});

seajs.use('buy');
