define("yx/fixedbar",["jquery","fastclick"],function(n,c,e){var i=jQuery=n("jquery"),a=n("fastclick");a(document.body);var f=i(".yx-fixedBar"),o=i(".yx-back"),r=function(){i(window).scroll(function(){i(this).scrollTop()>100?f.fadeOut():f.fadeIn()})};r();var t=function(){o.on("click",function(n){n.preventDefault(),window.history.back(-1)})};t()}),seajs.use("yx/fixedbar");