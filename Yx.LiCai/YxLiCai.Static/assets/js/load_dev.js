define("load",["jquery"],function(e,s,o){var a=jQuery=e("jquery"),c=(a("#header"),a("#footer"));c.load("/dist/footer.html"),a(".bbb").hide(),a(".aaa").on("click",".ccc",function(e){e.preventDefault();var s=a(this);s.hasClass("active")?(console.log("111"),s.css("color","#333333"),s.siblings(".bbb").hide(),s.removeClass("active")):(s.siblings(".bbb").show(),s.css("color","red"),s.addClass("active"),console.log("222"))}),a(".aaa").on("click","span",function(e){e.preventDefault();var s=a(this),o=s.parent().siblings(".ccc");o.hasClass("active")?(o.css("color","#333333").removeClass("active"),s.parent().hide(),console.log("333")):console.log("444")})}),seajs.use("load");