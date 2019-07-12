/*!
 *消息中心js
 *平扬
 */

define('message', ['jquery', 'gxdialog'], function(require, exports, module) {

    var $ = jQuery = require("jquery"); //jquery库 
    var gxdialog = require("gxdialog");

    $.gxDialog.defaults.background = '#000';
    //
    $(document).ready(function(){
        //$(".J_dope").on('click', function(event) {
        $(document).on("click", ".J_dope", function () {
            event.preventDefault();
            var msg = $(this).find('h2');
            var id = msg.attr('id');
            $("#top_div").html(msg.html());
            $(this).find('i.hover').removeClass('hover');
            $.gxDialog({
                title: '',
                width: 280,
                info: $('.dope-pup')
            });
            $.ajax({
                type: "POST",
                url: "/usersetting/read_message",
                data: { id: id },
                async: false,
                success: function(data) {
                }
            });
        });

        $(".loadmore a").on("click", function () {
            var thiselement = $(this);
            var CurrentPage = parseInt($("#CurrentPage").val());
            CurrentPage = CurrentPage + 1;
            $.ajax({
                type: "POST",
                url: "/usersetting/get_message",
                data: { page: CurrentPage },
                async: false,
                success: function(data) {
                    if (data != null) {
                        if (data.IsLastPage == "1") {
                            thiselement.css("display", "none");
                        }
                        var strhtml = data.Content;
                        $("#List").append(strhtml);
                        $("#CurrentPage").val(CurrentPage);
                    }
                }
            });
        });

    });


    
});
seajs.use('message');
