define("reg",["jquery","gxdialog","fastclick"],function(e,a,t){var d=jQuery=e("jquery"),s=(e("gxdialog"),e("fastclick"));s(document.body),d(".show-pwd").on("click",function(e){e.preventDefault();var a=d(this),t=a.hasClass("yes-pwd");t?(a.removeClass("yes-pwd"),a.parent("dd").children("input").attr("type","password")):(a.addClass("yes-pwd"),a.parent("dd").children("input").attr("type","text"))}),d.gxDialog.defaults.background="#000"}),seajs.use("reg");