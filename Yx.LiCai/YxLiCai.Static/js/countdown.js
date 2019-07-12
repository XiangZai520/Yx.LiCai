define('countdown', ['jquery', 'gxdialog'], function (require, exports, module) {
    var $ = jQuery = require("jquery");//jquery库 

    var InterValObj1;
    var InterValObj2;
    var InterValObj3;
    var InterValObj4;

    var TimeValue1 = $("#itemType_1").find(".enableDate").val();
    var TimeValue2 = $("#itemType_2").find(".enableDate").val();
    var TimeValue3 = $("#itemType_3").find(".enableDate").val();
    var TimeValue4 = $("#itemType_4").find(".enableDate").val();
    var NowTimeValue = localStorage.getItem("NowTimeValue");

    var EndTime1 = new Date(TimeValue1);
    var EndTime2 = new Date(TimeValue2);
    var EndTime3 = new Date(TimeValue3);
    var EndTime4 = new Date(TimeValue4);
    var NowTimeTotal = new Date(NowTimeValue);

    function GetRTime1() {//月
        NowTimeValue = localStorage.getItem("NowTimeValue");
        var NowTime = new Date(NowTimeValue);
        var t = parseInt(EndTime1.getTime()) - parseInt(NowTime.getTime());
        if (t > 0) {
            var d = Math.floor(t / (1000 * 60 * 60 * 24)); //天 
            var h = Math.floor(t / (1000 * 60 * 60)) % 24; //小时 
            var m = Math.floor(t / (1000 * 60)) % 60; //分钟 
            var s = Math.floor(t / 1000) % 60; //秒 

            var t_d = "#itemType_1 .t_d";
            var t_h = "#itemType_1 .t_h";
            var t_m = "#itemType_1 .t_m";
            var t_s = "#itemType_1 .t_s";

            $(t_d).html(d);
            $(t_h).html(h);
            $(t_m).html(m);
            $(t_s).html(s);

            NowTime = parseInt(NowTime.getTime() + 1000);

            var newTime = new Date(NowTime);
            //var nowDate = newTime.getFullYear() + "," + parseInt(newTime.getMonth() + 1) + "," + newTime.getDate() + "," + newTime.getHours() + "," + newTime.getMinutes() + "," + newTime.getSeconds();

            //$("#NowTime").val(newTime);
            localStorage["NowTimeValue"] = newTime;
        }
        else {
            window.clearInterval(InterValObj1);
            //window.location.href = "/Home/Index";
            $("#itemType_1").html("<a class='ui-btn' href='/Buy/BuyProduct?ptype=1'>立即买入</a>");
        }

    }

    function GetRTime2() {//季 3个月
        NowTimeValue = localStorage.getItem("NowTimeValue");
        var NowTime = new Date(NowTimeValue);
        var t = parseInt(EndTime2.getTime()) - parseInt(NowTime.getTime());
        if (t > 0) {
            var d = Math.floor(t / (1000 * 60 * 60 * 24)); //天 
            var h = Math.floor(t / (1000 * 60 * 60)) % 24; //小时 
            var m = Math.floor(t / (1000 * 60)) % 60; //分钟 
            var s = Math.floor(t / 1000) % 60; //秒 

            var t_d = "#itemType_2 .t_d";
            var t_h = "#itemType_2 .t_h";
            var t_m = "#itemType_2 .t_m";
            var t_s = "#itemType_2 .t_s";

            $(t_d).html(d);
            $(t_h).html(h);
            $(t_m).html(m);
            $(t_s).html(s);

            NowTime = parseInt(NowTime.getTime() + 1000);
            var newTime = new Date(NowTime);
            //var nowDate = newTime.getFullYear() + "," + parseInt(newTime.getMonth() + 1) + "," + newTime.getDate() + "," + newTime.getHours() + "," + newTime.getMinutes() + "," + newTime.getSeconds();

            //$("#NowTime").val(newTime);
            localStorage["NowTimeValue"] = newTime;
        }
        else {
            window.clearInterval(InterValObj2);
            //window.location.href = "/Home/Index";
            $("#itemType_2").html("<a class='ui-btn' href='/Buy/BuyProduct?ptype=2'>立即买入</a>");
        }
    }

    function GetRTime3() {//季 6个月
        NowTimeValue = localStorage.getItem("NowTimeValue");
        var NowTime = new Date(NowTimeValue);
        var t = parseInt(EndTime3.getTime()) - parseInt(NowTime.getTime());
        if (t > 0) {
            var d = Math.floor(t / (1000 * 60 * 60 * 24)); //天 
            var h = Math.floor(t / (1000 * 60 * 60)) % 24; //小时 
            var m = Math.floor(t / (1000 * 60)) % 60; //分钟 
            var s = Math.floor(t / 1000) % 60; //秒 

            var t_d = "#itemType_3 .t_d";
            var t_h = "#itemType_3 .t_h";
            var t_m = "#itemType_3 .t_m";
            var t_s = "#itemType_3 .t_s";

            $(t_d).html(d);
            $(t_h).html(h);
            $(t_m).html(m);
            $(t_s).html(s);

            NowTime = parseInt(NowTime.getTime() + 1000);
            var newTime = new Date(NowTime);
            //var nowDate = newTime.getFullYear() + "," + parseInt(newTime.getMonth() + 1) + "," + newTime.getDate() + "," + newTime.getHours() + "," + newTime.getMinutes() + "," + newTime.getSeconds();

            //$("#NowTime").val(newTime);
            localStorage["NowTimeValue"] = newTime;
        }
        else {
            window.clearInterval(InterValObj3);
            //window.location.href = "/Home/Index";
            $("#itemType_3").html("<a class='ui-btn' href='/Buy/BuyProduct?ptype=3'>立即买入</a>"); 
        }
    }

    function GetRTime4() {//年
        NowTimeValue = localStorage.getItem("NowTimeValue");
        var NowTime = new Date(NowTimeValue);
        var t = parseInt(EndTime4.getTime()) - parseInt(NowTime.getTime());
        if (t > 0) {
            var d = Math.floor(t / (1000 * 60 * 60 * 24)); //天 
            var h = Math.floor(t / (1000 * 60 * 60)) % 24; //小时 
            var m = Math.floor(t / (1000 * 60)) % 60; //分钟 
            var s = Math.floor(t / 1000) % 60; //秒 

            var t_d = "#itemType_4 .t_d";
            var t_h = "#itemType_4 .t_h";
            var t_m = "#itemType_4 .t_m";
            var t_s = "#itemType_4 .t_s";

            $(t_d).html(d);
            $(t_h).html(h);
            $(t_m).html(m);
            $(t_s).html(s);

            NowTime = parseInt(NowTime.getTime() + 1000);
            var newTime = new Date(NowTime);
            //var nowDate = newTime.getFullYear() + "," + parseInt(newTime.getMonth() + 1) + "," + newTime.getDate() + "," + newTime.getHours() + "," + newTime.getMinutes() + "," + newTime.getSeconds();

            //$("#NowTime").val(newTime);
            localStorage["NowTimeValue"] = newTime;
        }
        else {
            window.clearInterval(InterValObj4);
            //window.location.href = "/Home/Index?v=" + parseInt(100 * Math.random());
            //window.location.href = "/Home/Index";
            $("#itemType_4").html("<a class='ui-btn' href='/Buy/BuyProduct?ptype=4'>立即买入</a>");
        }
    }


    if (TimeValue1 == "" || EndTime1 <= NowTimeTotal) {//月
        window.clearInterval(InterValObj1);
    }
    else {
        InterValObj1 = window.setInterval(GetRTime1, 1000);
    }

    if (TimeValue2 == "" || EndTime2 <= NowTimeTotal) {//季3
        window.clearInterval(InterValObj2);
    }
    else {
        InterValObj2 = window.setInterval(GetRTime2, 1000);
    }

    if (TimeValue3 == "" || EndTime3 <= NowTimeTotal) {//季6
        window.clearInterval(InterValObj3);
    }
    else {
        InterValObj3 = window.setInterval(GetRTime3, 1000);
    }

    if (TimeValue4 == "" || EndTime4 <= NowTimeTotal) {//年
        window.clearInterval(InterValObj4);
    }
    else {
        InterValObj4 = window.setInterval(GetRTime4, 1000);
    }

});

seajs.use('countdown');