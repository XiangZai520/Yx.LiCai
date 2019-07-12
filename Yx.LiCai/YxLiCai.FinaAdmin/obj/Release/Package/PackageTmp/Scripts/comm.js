/*!
 *祥仔共用类别
 *
 */

/**
 * 键盘监听事件，只能输入数字、小数点、小数点后两位
 * @param obj
 * @param e
 */
function onlyNum(e) {
    if (!isNumber(e.value)) {
        if ((e.value + '').lastIndexOf('.') == ((e.value + '').length - 1) && ((e.value + '').length - (e.value + '').replace('.', '').length) == 1) {
            return false;
        }
        e.value = '';
        return false;
    }
    if ((e.value + '').length > 10) {
        e.value = (e.value + '').substring(0, 10);
        return false;
    }
    if ((e.value + '').indexOf('.') > 0 && ((e.value + '').length - 1 - (e.value + '').indexOf('.')) > 2) {
        e.value = (e.value + '').substring(0, (e.value + '').indexOf('.') + 3);
        return false;
    }
}
/** 
 * 判断是否是数字
 *  
 * @param str 
 * @returns 
 */
function isNumber(s) {
    var regu = "^[0-9]+$";
    var re = new RegExp(regu);
    if (s.search(re) != -1) {
        return true;
    }
    else {
        return false;
    }
};