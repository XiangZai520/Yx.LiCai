define("regs",["jquery","gxdialog","fastclick"],function(e,n,t){function o(e){a.gxDialog({title:"",width:280,closeBtn:!1,info:'<div class="pop-box"><p class="warning-color">'+e+"</p></div>",ok:function(){}})}function r(e){return/^13\d{9}$/g.test(e)||/^14[0-9]\d{8}$/g.test(e)||/^15[0-9]\d{8}$/g.test(e)||/^18[0-9]\d{8}$/g.test(e)||/^17[0-9]\d{8}$/g.test(e)?!0:!1}function i(e){for(var n=0,t=0;t<e.length;t++){var o=e.charCodeAt(t);o>=1&&126>=o||o>=65376&&65439>=o?n++:n+=2}return n}{var a=jQuery=e("jquery");e("fastclick"),e("gxdialog")}a.gxDialog.defaults.background="#000";var s=0,u=!1;a("#number").blur(function(){s=a("#number").val().replace(/[ ]/g,""),u=r(s)?!0:!1}),a("#btnNextok").click(function(){var e=a("#number").val();if(e=null||""==e,!u)return void o("手机号输入错误，请修改");var n=a("#number").val().trim().replace(/[ ]/g,""),t=a("#inviteCode").val();a.ajax({type:"POST",async:!1,url:"/Login/CheckUserIsMember",data:{Phone:n},success:function(e){if(0==e.Status)window.location.href="/Login/Login?phone="+e.Message;else{if(1==e.Status)return o(e.Message),!1;if(2!=e.Status)return o(e.Message),!1;window.location.href=""!=t?"/Login/Registered?phone="+e.Message+"&inviteCode="+t:"/Login/Registered?phone="+e.Message}},error:function(e,n,t){}})}),a("#number").keyup(function(){var e=i(a("#number").val().replace(/[ ]/g,""));11==e?(a("#btnNext").hide(),a("#btnNextok").show()):(a("#btnNext").show(),a("#btnNextok").hide())})}),seajs.use("regs");