define('countdownproduct', ['jquery', 'gxdialog'], function (require, exports, module) {
    var $ = jQuery = require("jquery");//jquery库 
    
    var InterValObj1;
    var InterValObj2;
    var InterValObj3;
    var InterValObj4;

    var t_d = ".t_d";
    var t_h = ".t_h";
    var t_m = ".t_m";
    var t_s = ".t_s";
    var NowTimeValue = localStorage.getItem("NowTimeValue");
    var nt = new Date(NowTimeValue).getTime();

    function GetRTime1() {//月
        var t = parseInt(et1) - parseInt(nt);
        if (t > 0) {

            var d = Math.floor(t / (1000 * 60 * 60 * 24)); //天 
            var h = Math.floor(t / (1000 * 60 * 60)) % 24; //小时 
            var m = Math.floor(t / (1000 * 60)) % 60; //分钟 
            var s = Math.floor(t / 1000) % 60; //秒 

            $(t_d).html(d);
            $(t_h).html(h);
            $(t_m).html(m);
            $(t_s).html(s);

            nt = parseInt(nt + 1000);
            var newTime = new Date(nt);
            localStorage["NowTimeValue"] = newTime;
        }
        else {
            window.clearInterval(InterValObj1);
            //window.location.href = "/Product/Index/1";
            if (totalTouchTab==0) {
                $(".product-btn").html("<a class='ui-btn' href='/Buy/BuyProduct?ptype=1'>立即买入</a>");
            }
        }
    }

    function GetRTime2() {//季 3个月
        var t = parseInt(et2) - parseInt(nt);
        if (t > 0) {
            var d = Math.floor(t / (1000 * 60 * 60 * 24)); //天 
            var h = Math.floor(t / (1000 * 60 * 60)) % 24; //小时 
            var m = Math.floor(t / (1000 * 60)) % 60; //分钟 
            var s = Math.floor(t / 1000) % 60; //秒 

            $(t_d).html(d);
            $(t_h).html(h);
            $(t_m).html(m);
            $(t_s).html(s);

            nt = parseInt(nt + 1000);
            var newTime = new Date(nt);
            localStorage["NowTimeValue"] = newTime;
        }
        else {
            window.clearInterval(InterValObj2);
            //window.location.href = "/Product/Index/2";
            if (totalTouchTab == 1 && totalJgxtab==0) {
                $(".product-btn").html("<a class='ui-btn' href='/Buy/BuyProduct?ptype=2'>立即买入</a>");
            }
        }
    }

    function GetRTime3() {//季 6个月
        var t = parseInt(et3) - parseInt(nt);
        if (t > 0) {
            var d = Math.floor(t / (1000 * 60 * 60 * 24)); //天 
            var h = Math.floor(t / (1000 * 60 * 60)) % 24; //小时 
            var m = Math.floor(t / (1000 * 60)) % 60; //分钟 
            var s = Math.floor(t / 1000) % 60; //秒 

            $(t_d).html(d);
            $(t_h).html(h);
            $(t_m).html(m);
            $(t_s).html(s);

            nt = parseInt(nt + 1000);
            var newTime = new Date(nt);
            localStorage["NowTimeValue"] = newTime;
        }
        else {
            window.clearInterval(InterValObj3);
            //window.location.href = "/Product/Index/3";
            if (totalTouchTab == 1 && totalJgxtab == 1) {
                $(".product-btn").html("<a class='ui-btn' href='/Buy/BuyProduct?ptype=3'>立即买入</a>");
            }
        }
    }

    function GetRTime4() {//年
        var t = parseInt(et4) - parseInt(nt);
        if (t > 0) {
            var d = Math.floor(t / (1000 * 60 * 60 * 24)); //天 
            var h = Math.floor(t / (1000 * 60 * 60)) % 24; //小时 
            var m = Math.floor(t / (1000 * 60)) % 60; //分钟 
            var s = Math.floor(t / 1000) % 60; //秒 

            $(t_d).html(d);
            $(t_h).html(h);
            $(t_m).html(m);
            $(t_s).html(s);

            nt = parseInt(nt + 1000);
            var newTime = new Date(nt);
            localStorage["NowTimeValue"] = newTime;
        }
        else {
            window.clearInterval(InterValObj4);
            //window.location.href = "/Product/Index/4";
            if (totalTouchTab == 2) {
                $(".product-btn").html("<a class='ui-btn' href='/Buy/BuyProduct?ptype=4'>立即买入</a>");
            }
        }
    }

    if (EndTime1 == "" || et1 <= nt) {//月
        window.clearInterval(InterValObj1);
    }
    else {
        InterValObj1 = window.setInterval(GetRTime1, 1000);
    }

    if (EndTime2 == "" || et2 <= nt) {//季3
        window.clearInterval(InterValObj2);
    }
    else {
        InterValObj2 = window.setInterval(GetRTime2, 1000);
    }

    if (EndTime3 == "" || et3 <= nt) {//季6
        window.clearInterval(InterValObj3);
    }
    else {
        InterValObj3 = window.setInterval(GetRTime3, 1000);
    }

    if (EndTime4 == "" || et4 <= nt) {//年
        window.clearInterval(InterValObj4);
    }
    else {
        InterValObj4 = window.setInterval(GetRTime4, 1000);
    }

});

seajs.use('countdownproduct');