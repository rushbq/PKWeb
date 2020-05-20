﻿
//Click事件, 觸發搜尋鈕
function triggerSearch(objType) {
    if (objType == '1') {
        $('#btn_Search1').trigger('click');
    } else {
        $('#btn_Search2').trigger('click');
    }
}

$(function () {
    /* 回到頁首 */
    $("#gotop").click(function () {
        jQuery("html,body").animate({
            scrollTop: 0
        }, 300);
    });

    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('#gotop').fadeIn("fast");
        } else {
            $('#gotop').stop().fadeOut("fast");
        }
    });

});

/* 
  [功能名稱] 強化encodeURIComponent
  [引用方式]
  fixedEncodeURIComponent(string)

*/
//function fixedEncodeURIComponent(str) {
//    return encodeURIComponent(str).replace(/[!'()]/g, escape).replace(/\*/g, "%2A");
//}
function fixedEncodeURIComponent(str) {
    return encodeURIComponent(str).replace(/[!'()*]/g, function (c) {
        return '%' + c.charCodeAt(0).toString(16);
    });
}

/* 
  [功能名稱] 日期格式化
  [引用方式]
  var ObjDate = new Date().Format("yyyy_MM_dd_hh_mm_ss_S");

*/
Date.prototype.Format = function (fmt) { //author: meizz
    var o = {
        "M+": this.getMonth() + 1,                 //月份
        "d+": this.getDate(),                    //日
        "h+": this.getHours(),                   //小時
        "m+": this.getMinutes(),                 //分
        "s+": this.getSeconds(),                 //秒
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度
        "S": this.getMilliseconds()             //毫秒
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}


/* 
  [功能名稱] Delay函式
  [引用方式]
  延遲1秒
    delay(function () {
        //....do something

    }, 1000);

*/
var delay = (function () {
    var timer = 0;
    return function (callback, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms);
    };
})();
