define("test",["jquery","gxdialog","fastclick"],function(o,i,t){{var n=jQuery=o("jquery");o("fastclick"),o("gxdialog")}n.gxDialog.defaults.background="#000",n("#ok-buy").on("click",function(){n.gxDialog({title:"",width:280,info:document.getElementById("J_buyPwd")})}),n("#ok-bank").on("click",function(){n.gxDialog({title:"",width:280,info:document.getElementById("selectBank")})}),n("#alert_html").on("click",function(){n.gxDialog({title:"gxDialog",info:'<p>Hello World!<p><p>I am <strong style="color:#4a89dc;">gxDialog</strong>!</p>'})}),n("#alert_dom").on("click",function(){n.gxDialog({title:"gxDialog",info:document.getElementById("the_alert_info")})}),function(){var o=n("#the_login");n("#alert_login").on("click",function(){n.gxDialog({title:"gxDialog",info:o})}),o.on("submit",function(){return n.gxDialog({title:"gxDialog",info:"正在登录.."}),setTimeout(function(){n.gxDialog({title:"gxDialog",info:"登录成功（功能演示 ^_^）"})},1e3),!1})}(),n("#alert_text").on("click",function(){n.gxDialog({title:"I am title",info:"Hello World!"})}),n("#confirm").on("click",function(){n.gxDialog({title:"I am title",info:"现在开始使用 <strong>gxDialog</strong>",ok:function(){console.log("ok")},no:function(){console.log("on")}})}),n("#alert").on("click",function(){n.gxDialog("<p>Hello World!<p><p>I am gxDialog!</p>")}),n("#ios_alert").on("click",function(){n.gxDialog({title:"提示",info:"你好，欢迎使用 gxDialog 对话框插件",ok:function(){},baseClass:"ios"})}),n("#ios_confirm").on("click",function(){n.gxDialog({title:"“gxDialog”<br>想要给您发送推送通知",info:"“通知”可能包括提现、声音和图标标记。可在“设置”中进行配置。",ok:function(){},okText:"好",no:function(){},noText:"不允许",baseClass:"ios"})}),n("#ios_rate").on("click",function(){n.gxDialog({title:"评价 gxDialog",info:"感谢您使用 gxDialog，如果您觉得不错，欢迎到 Github 添加 Star。",buttons:[{text:"去评价",callback:function(){location.href=""}},{text:"稍后提醒",callback:function(){}},{text:"不，谢谢",callback:function(){}}],baseClass:"ios"})})}),seajs.use("test");