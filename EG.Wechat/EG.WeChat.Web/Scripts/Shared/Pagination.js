/*
分页脚本  Jasonyiu  20141127
注意：一个页面只能存在一个分页页面。

pagingType 分页类型 0：没有复选框，1：有复选框
*/

var pagingType = 1;

var OperationData;

//字符串转换成Json对象
function StringToJson(str) {
    //在Firefox，chrome，opera，safari，ie9，ie8等高级浏览器直接可以用JSON对象的stringify()和parse()方法。
    //var tableData = JSON.parse(str);
    //ie8(兼容模式),ie7和ie6用eval()将字符串转为JSON对象，
    var tableData = eval("(" + str + ")");
    return tableData
}


//提交表单，获取分页数据
function GetPagingList() {
    $.post($("form").attr("action"), $("form").serializeArray(), function (data) {
        var tableData = StringToJson(data.JsonData);
        var head = "";
        var body = "";
        var foot = "";
        if (data.TotalCount == "0") {
            head = GetTHead(tableData[0]);
            foot = GetTFoot(data.PageIndex, data.TotalPages, data.TotalCount);
        } else {
            head = GetTHead(tableData[0]);
            body = GetTBody(tableData);
            foot = GetTFoot(data.PageIndex, data.TotalPages, data.TotalCount);
        }
        $("#table tbody").html(head);
        $("#table tbody").append(body);
        $("#pager").html(foot);
        Operation();
    });
}


//生成表头
function GetTHead(obj, type) {
    var strhtml = new StringBuilder();
    strhtml.Append("<tr>");
    if (pagingType == 1) {
        strhtml.Append("<th width='25'><input type='checkbox' class='checkboxCtrl' onclick='SelectAll(this)' /></th>");
    }
    $.each(obj, function (n, m) {
        strhtml.AppendFormat("<th>{0}</th>", n);
    });
    strhtml.Append("</tr>");
    return strhtml.ToString();
}

//生成表身
function GetTBody(obj) {
    var strhtml = new StringBuilder();
    $.each(obj, function (n, m) {
        strhtml.Append("<tr>");
        if (pagingType == 1) {
            strhtml.Append("<td width='25'><input type='checkbox' class='checkboxCtrl'></td>");
        }
        $.each(m, function (i, j) {
            strhtml.AppendFormat("<td>{0}</td>", j);
        });
        strhtml.Append("</tr>");
    });
    return strhtml.ToString();
}

//生成表脚
function GetTFoot(PageIndex, TotalPages, TotalCount) {
    var strhtml = new StringBuilder();
    strhtml.AppendFormat("<a onclick='GoPage({0})'>首页</a> <a onclick='BackPage()' >上一页</a> ", 1);
    strhtml.AppendFormat("<span>当前 <input type ='text' id='PageIndex' name ='PageIndex' value ='{0}' style='width: 30px; text-align: center;' onkeypress = 'EnterPress(event)'/> 页 </span>", PageIndex);
    strhtml.AppendFormat("<a onclick='NextPage()'>下一页</a> <a onclick='GoPage({0})'>尾页</a> ", TotalPages);
    strhtml.AppendFormat("<span>共 {0} 条，{1} 页</span>", TotalCount, TotalPages);
    $("#TotalPages").val(TotalPages);
    $("#TotalCount").val(TotalCount);
    return strhtml.ToString();
}

//表头全选按钮功能
function SelectAll(obj) {
    var tablebody = $(obj).parent().parent().parent();
    tablebody.find("input:checkbox").attr("checked", obj.checked);
}

//鼠标经过样式
function tableMouseOver() {
    $(".table tr").mouseover(function () {
        $(this).css({ background: "#CDDAEB" });
        $(this).children('td').each(function (index, ele) {
            $(ele).css({ color: "#1D1E21" });
        });
    }).mouseout(function () {
        $(this).css({ background: "#FFF" });
        $(this).children('td').each(function (index, ele) {
            $(ele).css({ color: "#909090" });
        });
    });
}

//下一页
function NextPage() {
    var pageNumber = Number($("#PageIndex").val());
    var TotalPages = Number($("#TotalPages").val());
    if ((pageNumber + 1) <= TotalPages) {
        $("#PageIndex").val(pageNumber + 1)
        GetPagingList();
    }
}

//上一页
function BackPage() {
    var pageNumber = Number($("#PageIndex").val());
    if ((pageNumber - 1) >= 1) {
        $("#PageIndex").val(pageNumber - 1)
        GetPagingList();
    }
}

//到自定页
function GoPage(number) {
    $("#PageIndex").val(number)
    GetPagingList();
}

//Enter键跳转
function EnterPress(e) {
    var e = e || window.event;
    if (e.keyCode == 13) {
        GetPagingList();
    }
}


//在表单后面追加操作面板
function Operation() {
    if (OperationData != null) {
        $("#table tbody tr").has("th").first().append("<th>Operation</th>");
        $.each($("#table tbody tr").has("td"), function (n, m) {
            var strhtml = new StringBuilder();
            strhtml.Append("<td>");
            $.each(OperationData, function (i, j) {
                strhtml.AppendFormat(" <a onclick='{0}' >{1}</a> ", j.OnCheck, j.Name);
            });
            strhtml.Append("</td>");
            $(m).append(strhtml.ToString());
        });
    }
}



