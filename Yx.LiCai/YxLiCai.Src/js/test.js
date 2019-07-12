/*!
 * @description:test
 * @author:Eilvein
 * @version:V1.0.0
 * @update:2015/5/26
 */

define('test',['jquery','gxdialog','fastclick'],function(require, exports, module) {

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

	$('#ok-bank').on('click', function(){
		$.gxDialog({
			title: '',
			width: 280,
			info: document.getElementById('selectBank')
		});
	});



	$('#alert_html').on('click', function(){
	  $.gxDialog({
	    title: 'gxDialog',
	    info: '<p>Hello World!<p><p>I am <strong style="color:#4a89dc;">gxDialog</strong>!</p>'
	  });
	});





	$('#alert_dom').on('click', function(){
	  $.gxDialog({
	    title: 'gxDialog',
	    info: document.getElementById('the_alert_info')
	  });
	});

	(function(){
	  var _form = $('#the_login');

	  $('#alert_login').on('click', function(){
	    $.gxDialog({
	      title: 'gxDialog',
	      info: _form
	    });
	  });

	  _form.on('submit', function(){
	    $.gxDialog({
	      title: 'gxDialog',
	      info: '正在登录..'
	    });

	    // 功能演示，这里写上你的逻辑代码
	    setTimeout(function(){
	      $.gxDialog({
	        title: 'gxDialog',
	        info: '登录成功（功能演示 ^_^）'
	      });
	    }, 1000);

	    return false;
	  });
	})();
     

	$('#alert_text').on('click', function(){
	  $.gxDialog({
	    title: 'I am title',
	    info: 'Hello World!'
	  });
	});

	$('#confirm').on('click', function(){
	  $.gxDialog({
	    title: 'I am title',
	    info: '现在开始使用 <strong>gxDialog</strong>！',
	    ok: function(){
	    	console.log('ok');
	    },
	    no: function(){
	    	console.log('on');
	    }
	  });
	});

	$('#alert').on('click', function(){
	  $.gxDialog('<p>Hello World!<p><p>I am gxDialog!</p>');
	});


	$('#ios_alert').on('click', function(){
	  $.gxDialog({
	    title: '提示',
	    info: '你好，欢迎使用 gxDialog 对话框插件',
	    ok: function(){},
	    baseClass: 'ios'
	  });
	});

	$('#ios_confirm').on('click', function(){
	  $.gxDialog({
	    title: '“gxDialog”<br>想要给您发送推送通知',
	    info: '“通知”可能包括提现、声音和图标标记。可在“设置”中进行配置。',
	    ok: function(){},
	    okText: '好',
	    no: function(){},
	    noText: '不允许',
	    baseClass: 'ios'
	  });
	});

	$('#ios_rate').on('click', function(){
	  $.gxDialog({
	    title: '评价 gxDialog',
	    info: '感谢您使用 gxDialog，如果您觉得不错，欢迎到 Github 添加 Star。',
	    buttons: [{
	      text: '去评价',
	      callback: function(){
	        location.href = '';
	      }
	    },{
	      text: '稍后提醒',
	      callback: function(){}
	    },{
	      text: '不，谢谢',
	      callback: function(){}
	    }],
	    baseClass: 'ios'
	  });
	});


});

seajs.use('test');
