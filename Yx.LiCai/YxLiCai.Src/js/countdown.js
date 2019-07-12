﻿define('countdown', ['jquery', 'gxdialog'], function (require, exports, module) {
    var $ = jQuery = require("jquery");//jquery库 

    var EndTime1 = $("#itemType_1").find(".enableDate").val();
    var EndTime2 = $("#itemType_2").find(".enableDate").val();
    var EndTime3 = $("#itemType_3").find(".enableDate").val();
    var EndTime4 = $("#itemType_4").find(".enableDate").val();

    function GetRTime1() {//月
        var EndTime = new Date(EndTime1);
        var NowTime = new Date();
        var t = EndTime.getTime() - NowTime.getTime();
        var h = Math.floor(t / 1000 / 60 / 60 % 24);
        var m = Math.floor(t / 1000 / 60 % 60);
        var s = Math.floor(t / 1000 % 60);

        var t_h = "#itemType_1 .t_h";
        var t_m = "#itemType_1 .t_m";
        var t_s = "#itemType_1 .t_s";

        $(t_h).html(h);
        $(t_m).html(m);
        $(t_s).html(s);

    }

    function GetRTime2() {//季 3个月
        var EndTime = new Date(EndTime2);
        var NowTime = new Date();
        var t = EndTime.getTime() - NowTime.getTime();
        var h = Math.floor(t / 1000 / 60 / 60 % 24);
        var m = Math.floor(t / 1000 / 60 % 60);
        var s = Math.floor(t / 1000 % 60);

        var t_h = "#itemType_2 .t_h";
        var t_m = "#itemType_2 .t_m";
        var t_s = "#itemType_2 .t_s";

        $(t_h).html(h);
        $(t_m).html(m);
        $(t_s).html(s);

    }

    function GetRTime3() {//季 6个月
        var EndTime = new Date(EndTime3);
        var NowTime = new Date();
        var t = EndTime.getTime() - NowTime.getTime();
        var h = Math.floor(t / 1000 / 60 / 60 % 24);
        var m = Math.floor(t / 1000 / 60 % 60);
        var s = Math.floor(t / 1000 % 60);

        var t_h = "#itemType_3 .t_h";
        var t_m = "#itemType_3 .t_m";
        var t_s = "#itemType_3 .t_s";

        $(t_h).html(h);
        $(t_m).html(m);
        $(t_s).html(s);

    }

    function GetRTime4() {//年
        var EndTime = new Date(EndTime4);
        var NowTime = new Date();
        var t = EndTime.getTime() - NowTime.getTime();
        var h = Math.floor(t / 1000 / 60 / 60 % 24);
        var m = Math.floor(t / 1000 / 60 % 60);
        var s = Math.floor(t / 1000 % 60);

        var t_h = "#itemType_4 .t_h";
        var t_m = "#itemType_4 .t_m";
        var t_s = "#itemType_4 .t_s";

        $(t_h).html(h);
        $(t_m).html(m);
        $(t_s).html(s);

    }

    window.setInterval(GetRTime1, 0);
    window.setInterval(GetRTime2, 0);
    window.setInterval(GetRTime3, 0);
    window.setInterval(GetRTime4, 0);



});

seajs.use('countdown');