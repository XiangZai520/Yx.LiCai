function onlyNum(e,l){return isNumber(e.value)?(e.value+"").length>l?(e.value=(e.value+"").substring(0,l),!1):(e.value+"").indexOf(".")>0&&(e.value+"").length-1-(e.value+"").indexOf(".")>2?(e.value=(e.value+"").substring(0,(e.value+"").indexOf(".")+3),!1):void 0:(e.value+"").lastIndexOf(".")==(e.value+"").length-1&&(e.value+"").length-(e.value+"").replace(".","").length==1?!1:(e.value=e.value.replace(/[^\d]/g,""),!1)}function isNumber(e){var l="^[0-9]+$",u=new RegExp(l);return-1!=e.search(u)?!0:!1}