/*!
 * @description:more.js
 * @author:Eilvein
 * @version:V1.0.0
 * @update:2015/5/26
 */

define('more',['jquery','fastclick','loadmore'],function(require, exports, module) {

	var $ = jQuery = require("jquery");//jquery库
	var FastClick = require("fastclick");
	var loadmore = require("loadmore");

	//FastClick.attach(document.body);


    $(".loadmore").loadMore({});



});

seajs.use('more');
