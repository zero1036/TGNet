/* 尝试在这个JS文件，建立一些通用的JS函数。
   供多个模块都可以共享使用。
*/

/* 定时执行指定的function。(目前不提供取消功能)
   @timeout         毫秒数
   @targetFunction  目标方法  function()无参数类型
*/
function TimeoutToDo(timeout, targetFunction) {
    window.setTimeout(targetFunction, timeout);
}

/* 隐藏指定的Html元素
   @obj         目标对象，请使用document.getElementById获得
   @ShowOrHide  true为显示;false为隐藏
*/
function ShowOrHideElement(obj, ShowOrHide) {
    obj.style.visibility = ShowOrHide ? "visible" : "hidden";
}



function SubForm(url, json) {
    var newname = "N" + Math.random();
    var tempForm = document.createElement("form");
    tempForm.id = "tempForm1";
    tempForm.method = "post";
    tempForm.action = url;
    tempForm.target = newname;
    tempForm.target = "_self";
    for (var i in json) {
        var hideInput = document.createElement("input");
        hideInput.type = "hidden";
        hideInput.name = json[i]["name"];
        hideInput.value = json[i]["value"];
        tempForm.appendChild(hideInput);
    }
    document.body.appendChild(tempForm);
    tempForm.submit();
}
//输入验证
function CheckInput() {
    var num = $('.TextInput');
    for (i = 0; i < num.length; i++) {
        if (num.eq(i).attr("value") == "") {
            num.eq(i).focus();
            return "請填寫必要信息";
        }
    }
    return "";
}
//显示灰色 jQuery 遮罩层 
function showBg() {
    var bh = $("body").height();
    var bw = $("body").width();
    $("#fullbg").css({
        height: bh,
        width: bw,
        display: "block"
    });
    $("#dialog").show();
}
//关闭灰色 jQuery 遮罩 
function closeBg() {
    $("#fullbg,#dialog").hide();
}
//微信图文段落模型
var NewsModel = {
    author: "",
    content: "",
    content_source_url: "",
    digest: "",
    thumb_media_id: "",
    title: "",
};
//等待遮罩效果对象
var CommonMaskUI = {
    StartMask: function (h) {
        var bh = $("body").height();
        var bw = $("body").width();
        $("#fullbg").css({
            height: bh,
            width: bw,
            display: "block"
        });
        $("#dialog").show();
    },
    EndMask: function () {
        $("#fullbg,#dialog").hide();
    }
}
//获取选中checkbox值
function GetSelectValueFromCheckBox(checkboxName) {
    var pResult = "";
    var selectOpenid_array = new Array();

    var check_array = document.getElementsByName(checkboxName);
    for (var i = 0; i < check_array.length; i++) {
        if (check_array[i].checked == true) {
            pResult = check_array[i].value;
            selectOpenid_array.push(pResult);
        }
    }
    return selectOpenid_array;
}
//当图片查找错误时，定一张错误图片
function PicNofind() {
    var img = event.srcElement;
    img.src = "/Images/common/faq.png";
    img.onerror = null; //控制不要一直跳动
}
function setCookie(name, value) {
    var Days = 30;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}
function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");

    if (arr = document.cookie.match(reg))

        return unescape(arr[2]);
    else
        return null;
}