/**
 *
 *   demo:
 *
 *   <div class="loadMore" data-next-url="/" data-addition="" data-selector="" data-classname-lock="">
 *       <a href="">更多礼包</a>
 *   </div>
 *
 */
define('loadmore', ['jquery'], function(require, exports, module) {

    var $ = jQuery = require("jquery");

    $.fn.loadMore = function(config) {
        var defaults = {
            nextUrl:null,      // 下一次的连接
            additionData:null,  // 附加数据
            classnameLock:"",   // 加载时锁状态的class
            success:null,      // 成功之后手动处理
            error:null,      // 失败之后手动处理
            selector:""        // 子节点选择器
        };

        var options = $.extend(defaults, config);
        return this.each(function() {

            var entry = $(this);
            var _lock = false;
            var selector = options.nextUrl || entry.attr("data-selector");
            var nextUrl = options.nextUrl || entry.attr("data-nextUrl");
            var addition = options.additionData || entry.attr("data-addition");
            var btn = selector ? entry.find(selector) : entry;
            var classnameLock = options.classnameLock || entry.attr('data-lockClassname');

            var onError = options.error || function(){
                alert("加载完毕，已没有更多数据");
            };

            var onSuccess = options.success || function(response){
                if(!response.code){
                    var data = response.data;
                    var html = data.html;
                    $(html).insertBefore(entry);
                    if(data.nextUrl){
                        entry.attr('data-nextUrl',data.nextUrl);
                        nextUrl = data.nextUrl;
                    }else{
                        entry.attr('data-nextUrl',"");
                        entry.remove();
                    }
                    return true;
                }
                if(response.code){
                    if(response.msg){
                        alert(response.msg);
                    }else{
                        alert("加载完毕，已没有更多数据");
                    }
                    return false;
                }
                return true;
            } || options.success;

            function lock(){
                _lock = true;
                entry.addClass(classnameLock);
            }
            function unlock(){
                _lock = false;
                entry.removeClass(classnameLock);
            }
            function islock(){
                return _lock ;
            }

            entry.click(function(){

                if(islock()){
                    return false;
                }

                if(!nextUrl){
                    entry.hide();
                    return false;
                }

                lock();
                $.ajax({
                    url: nextUrl ,
                    type: 'get',
                    dataType:window.DEBUG_TEST_DATA ? 'json':'jsonp',
                    timeout:8000,
                    data: {
                        addition:addition
                    },
                    error:function(){
                        onError.apply(this,arguments);
                        unlock();
                    },
                    success:function(response){
                        onSuccess.apply(this,arguments);
                        unlock();
                    }
                });

                return false;
            });
        });
    };


    ////////////////////////////////////////////////////////////
    if (typeof module != "undefined" && module.exports) {
        module.exports = $;
    }

});
