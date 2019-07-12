define('uc/show', ['jquery', 'fastclick'], function (require, exports, module) {

    var $ = jQuery = require("jquery");//jquery库
    var FastClick = require("fastclick");

    //FastClick.attach(document.body);

    // 常见问题
    $('.uc-matter-list').click(function () {
        var _this = $(this);
        if (_this.hasClass('hover')) {
            _this.children('.uc-matter-menu').hide();
            _this.removeClass('hover');
        } else{
            _this.children('.uc-matter-menu').show();
            _this.addClass('hover');
        };
    });


});

seajs.use('uc/show');
