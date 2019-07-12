/*!

 *加息券页面js
 *平扬
 */

define('add_interest', ['jquery', 'gxdialog'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库
    var gxdialog = require("gxdialog");
    $.gxDialog.defaults.background = '#000';

    //有确认按钮的弹出
    function alertDialog(v) {
        $.gxDialog({
            title: '',
            width: 280,
            closeBtn: false,
            info: '<div class="pop-box"><p class="warning-color">' + v + '</p></div>',
            ok: function () { }
        });
    }

    $("input[name='ckbox']").click(function () { 
        var id = $(this).val();//加息券id
        var val = parseFloat($(this).attr("num"))*100;//加息券面额
        //计算加息总额度
        var all_int = parseFloat($("#interest").val())*100;
        if ($(this).is(':checked')) {
            all_int = all_int + val;
        } else {
            all_int = all_int - val;
        } 
        if (all_int>200)
        {
            alertDialog("加息额度不能超过2%");
            return false;
        }
        $("#interest").val(all_int/100)
        $("#use_itt").text(all_int/100 + "%");//选中的加息券总额 
        //设置选中的加息券ids
        var add_interest = ""; 
        $("input[name='ckbox']").each(function () {
            if ($(this).is(':checked')) {
                var i = $(this).val();//加息券id
                if (add_interest == "") {

                    add_interest = i
                }
                else {
                    add_interest = add_interest + ':' + i;
                } 
            } 
        });
        $("#add_interest").val(add_interest);

    });
    $("#ok").click(function () {
        var ids = $("#add_interest").val();
        window.location.href = "/Buy/Buy_product?ptype=" + $("#ptype").val() + "&addinterests=" + $("#add_interest").val() + "&addrate=" + $("#interest").val() + "&buyMoney=" + $("#buyMoney").val() + "&ids=" + ids + "&buytype=" + $("#buytype").val();
    });
    $(document).ready(function() {
        var ids = $("#add_interest").val();
        var total = 0;
        if (ids != null && ids !== "") {
            var idsArr = ids.split(":");
            for (var i = 0; i < idsArr.length; i++) {
                var thisId = "#" + idsArr[i];
                $(thisId).attr('checked', 'checked');
                var val = parseFloat($(thisId).attr("num")) * 100;//加息券面额
                total += val;
            }
        }
        $("#interest").val(total / 100);
        $("#use_itt").html(total / 100 + "%");
    });

});

seajs.use('add_interest');
