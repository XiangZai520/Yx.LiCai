/*!
 * @description:buy
 *
 */

// (function($){
$(function(){

	//$.gxDialog.defaults.background = '#000';
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

})
