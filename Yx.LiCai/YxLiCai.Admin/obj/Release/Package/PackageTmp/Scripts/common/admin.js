$(document).ready(function () {
    //头部下拉菜单
    function downlist() {
        $('.J_menu').click(function () {
            $(this).next('.J_menuList').slideToggle("slow");
            if ($(this).find('span').hasClass('hover-down')) {
                $(this).find('span').removeClass().addClass('hover-up');
            } else {
                $(this).find('span').removeClass().addClass('hover-down');
            }
        })

        $('.J_sidebar').css("height", window.screen.height);
    } downlist();
});

function adminglass() {
    this.openwinbox = function(openbox){
        $(openbox).fadeIn(400);
        var obheight = $(openbox).height();
        var popMargTop = 0;
        var popMargLeft = 0;
        if (obheight > 1000)
        {
            popMargTop = 220;
            popMargLeft = $(openbox).width() / 2;
        }
        else
        {
            popMargTop = $(openbox).height() / 2;
            popMargLeft = $(openbox).width() / 2;
        }
        $(openbox).css({
            'margin-top': -popMargTop+20,
            'margin-left': -popMargLeft,
           // 'height': $(openbox).height()
        });
        var bodyheight = document.documentElement.clientHeight || document.body.clientHeight;
        if (bodyheight < $(openbox).height()) {
            if (bodyheight > 300)
            {
                bodyheight = 300;
            }
            $(openbox).css({
               // 'margin-top': 0,
                'height': bodyheight,
                'overflow': "scroll"
                });
        }
        //创建遮罩层
        var boarddiv = '<div class="J_shadeLayer"></div>';
        $('.J_shadeLayer').css("height", document.documentElement.clientHeight || document.body.clientHeight); //遮罩层高度为屏幕高度
        $('body').append(boarddiv);
        $('.J_shadeLayer').fadeIn(300);
    };
    this.closewinbox = function(openbox){
            $(openbox).fadeOut(300);
            $('.J_shadeLayer').fadeOut(400);
            $('.J_shadeLayer').remove();
    };
    this._closewinbox = function (openbox) {
        $(openbox).fadeOut(300);
        $('.J_shadeLayer').fadeOut(400);
        $('.J_shadeLayer').empty();
        $('.J_shadeLayer').remove();
    };
    //日历控件
    this.datepickerbox = function(startid,enddateid){
        $(startid).datepicker({
            dateFormat: 'yy-mm-dd',  //日期格式，自己设置
            yearRange: '2014:2015',//年份范围
            prevText: '前一月',
            nextText: '后一月',
            currentText: ' ',
            monthNames: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
        });
        $(enddateid).datepicker({
            dateFormat: 'yy-mm-dd',  //日期格式，自己设置
            yearRange: '1110:2014',//年份范围
            prevText: '前一月',
            nextText: '后一月',
            currentText: ' ',
            monthNames: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
        });
    };

    //输入框value显示与隐藏
    this.inpFocusBlur = function(inpId){
        var inpval;
        $(inpId).bind("focus", function(){
            inpval = $(this).val();
            $(this).val('');
            $(this).css('color','#666');
        });
        $(inpId).bind("blur", function(){
            if($(this).val() == ''){
                $(this).val(inpval);
                $(this).css('color','#ccc');
            }else{
                $(this).css('color','#666');
            }
        });
    };

    //滑动下拉列表
    this.slidedownList = function(clicklist,showboxlist){
        $(clicklist).click(function(){
            $(this).parent().find(showboxlist).slideToggle('slow');
            if($(this).find('span').hasClass('hover-down')){
                $(this).find('span').removeClass().addClass('hover-up');
            }else{
                $(this).find('span').removeClass().addClass('hover-down');
            }

        });
    };

    //复选框全选/取消
    this.clickCheck = function(id,idbox){
        $(id).click(function(){
            if($(this).is(":checked")){
                $(this).parents(idbox).find('input[name="J_checklist"]').each(function(){
                    $(this).prop("checked", true);
                });
            }else{
                $(this).parents(idbox).find('input[name="J_checklist"]').each(function(){
                    $(this).prop("checked", false);
                });
            }

        });
    };

    this.hoveritem = function(itemid,itembox){
        $(itemid).bind('mouseover', function(e){
                if($(this).find(itembox).length > 0){
                    $(this).find(itembox).fadeIn(300);
                }
        });
        $(itemid).bind('mouseout', function(e){
               $(this).find(itembox).hide();
        });


    };
    this.switchTab = function(idlist,idbox){
        $(idlist).bind('click', function(e){
            //获取相应节点
            var lilist = $(idbox +' .J_nav li'),
                tabct = $(idbox +' .J_tabct'),
                idx = 0;

            for(var i= 0;i<lilist.length;i++){
                if(lilist[i]== e.target) idx=i;
            }
            //事件
            lilist.removeClass('J_cur');
            $(lilist[idx]).addClass('J_cur');
            tabct.addClass('hide');
            $(tabct[idx]).removeClass('hide');
        });
    };
    //分类管理
    this.fladddeleitem = function(){
         $('.add-openbox').delegate('.J_addBtn','click',function(){
            var item = $(this).parent().find('.win-t-item');
            var inp = '<span class="t-item"><input value="" type="text" class="inp J_inp" name=""/><input type="button" name="" value="" class="icondele J_deleBtn"></span>';
            item.append(inp);
         });

        $('.add-openbox').delegate('.J_deleBtn','click',function(){
            $(this).parent().remove();
        });

        $('.add-openbox').delegate('.J_t_text','click',function(){
            var inp ='<input value="" type="text" class="inp J_inp" name=""/>';
            var txt = $(this).html();
            $(this).parent().find('.J_deleBtn').before(inp);
            $(this).parent().find('.J_inp').val(txt);
            $(this).remove();

        });


    }
    //是否显示
    this.ifshow = function(showbtn,hidebtn,ifbox){
        $(showbtn).bind('click',function(){
            $(ifbox).removeClass('hide');
        })
        $(hidebtn).bind('click',function(){
            $(ifbox).addClass('hide');
        })
    }

     //删除节点
    this.deleDome = function(t){
        t.parent().remove();
    };
    //添加节点
    this.addDom = function(itemdombox,itemnode){
        var itembox = $(itemdombox);
        var itemlist = itemnode;
        itembox.append(itemlist);
    }

}

//扩展Date
//new Date().Format("yyyy年MM月dd日");
//new Date().Format("MM/dd/yyyy");
//new Date().Format("yyyyMMdd");
//new Date().Format("yyyy-MM-dd hh:mm:ss");
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}
//提示框
var popNotification = $("#popupNotification").kendoNotification({
    position: {
        pinned: true,
        top: 30,
        right: 80
    },
    autoHideAfter: 5000,
    stacking: "down"
}).data("kendoNotification");

