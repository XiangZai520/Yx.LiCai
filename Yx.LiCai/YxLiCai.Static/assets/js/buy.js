define("buy",["jquery","gxdialog","fastclick"],function(e,t,n){{var i=jQuery=e("jquery");e("fastclick"),e("gxdialog")}i.gxDialog.defaults.background="#000",i("#dealPassword").on("click",function(){i.gxDialog({title:"",width:280,info:document.getElementById("J_buyPwd")})}),i("#ok-get").on("click",function(){i.gxDialog({title:"",width:280,info:document.getElementById("J_get")})});var a=function(){var e=i(".J_selectBank"),t=i("#J_bankInfor");t.on("click",function(){i.gxDialog({title:"",width:280,closeBtn:!1,info:document.getElementById("selectBank")})}),e.on("click","li",function(e){e.preventDefault();var n=i(this),a=n.find("img").attr("src"),c=n.find("p>b").html();t.find("img").show().attr("src",a),t.find("span").show().text(c),n.addClass("checked"),n.siblings("li").removeClass("checked"),i.gxDialog.close()})};a();var c=function(e){var t=e;t.on("click",function(e){e.preventDefault();var t=i(this),n=t.hasClass("yes-pwd");n?(t.removeClass("yes-pwd"),t.parent("dd").children("input").attr("type","password")):(t.addClass("yes-pwd"),t.parent("dd").children("input").attr("type","tel"))})};c(i(".show-pwd"));var d=function(){var e=i(".J_payWay");i.each(e,function(t,n){var a=i(this),c=a.find("input[type=radio]");a.on("click",function(t){t.preventDefault(),c.attr("checked")||(e.find("input[type=radio]").removeAttr("checked").removeClass("checked"),c.attr("checked","checked").addClass("checked"))})})};d()}),seajs.use("buy");