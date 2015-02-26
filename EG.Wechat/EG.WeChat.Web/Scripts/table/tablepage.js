jQuery.extend({
    page: function (divId, pagesize) {
        var div = divId;
        var x = "#" + div + " table tr:has(td)";
        var y = "#" + div;
        var z = "#" + div + " table";
        var table = $(x);
        var row = table.length;
        var pageSize = pagesize;
        var allPage = parseInt("" + ((row + pageSize - 1) / pageSize), 10);
        var currentPage = 1;
        $(table).hide();
        var width = $(z).width();
        var max_number = parseInt((width - 130) / 20, 10);
        //alert(max_number);  
        //构建菜单     
        $(y).append("<div id='menu'></div>");
        $(y).css("width", $(z).width() + "px");
        $("#menu").css("width", $(z).width() + "px");
        $("#menu").css("padding", "2px 10px 2px 10px");
        $("#menu").css("position", "absolute");

        showRow("1");
        bar();
        //为菜单添加事件        
        function bar() {
            var left;
            var right;
            if (max_number >= allPage) {
                left = 1;
                right = allPage;
            }
            else {
                var center = parseInt((max_number + 1) / 2, 10);
                if (currentPage <= center) {
                    left = 1;
                    right = max_number;
                }
                else {
                    if (allPage % 2 == 1) {
                        center = center - 1;
                    }
                    var maxRight = currentPage + center - 1;
                    if (maxRight >= allPage) {
                        left = allPage - max_number + 1;
                        right = allPage;
                    }
                    else {
                        if (allPage % 2 == 0) {
                            left = currentPage - center + 1;
                        }
                        else left = currentPage - center;
                        right = left + max_number - 1;
                    }

                }
            }
            $("#menu").empty();
            $("#menu").append("<span id='pre'>pre</span>");      //上一页  
            for (var i = left; i <= right; i++) {
                $("#menu").append("<span id='" + i + "'>" + i + "</span>");
                $("span").css("width", "20px");
            }
            $("#menu").append("<span id='next'>next</span>");   //下一页  
            $("#menu").append("<span id='page_currentPage_allPage'></span>");
            $("#next").css("width", "30px");
            $("#pre").css("width", "30px");
            $("#page_currentPage_allPage").css("border", "solid 1px green");
            $("#page_currentPage_allPage").css("width", "30px");
            $("#page_currentPage_allPage").css("margin", "0px 0px 0px 10px");
            $("#page_currentPage_allPage").css("padding", "0px 2px 0px 2px");
            $("#page_currentPage_allPage").html(currentPage + "<b>/</b>" + allPage);
            $("span:not('#pre','#next','#page_currentPage_allPage')").bind("click", {}, function () { showRow($(this).attr("id")); });
            $("span:not('#page_currentPage_allPage')").bind("mouseover", {}, function () { $(this).css("cursor", "hand"); });
            $("#pre").bind("click", {}, function () { showPre(); });
            $("#next").bind("click", {}, function () { showNext(); });
        }

        function showRow(page) {
            currentPage = page - 0;
            $(table).hide();
            var first = (currentPage - 1) * pageSize;
            var last = pageSize * currentPage;
            if (last > row) last = row;
            for (var i = first; i < last; i++) {
                table.eq(i).show();
            }
            bar();
            var id = "#" + currentPage;
            var cd = "span:not(" + id + ")";
            $(cd).css("color", "#000000");
            $(id).css("color", "red");
            $("#page_currentPage_allPage").html(currentPage + "<b>/</b>" + allPage);
        }

        function showPre() {
            var p;
            if (currentPage - 0 == 1) {
                p = allPage;
            }
            else {
                p = currentPage - 1;
            }
            showRow(p);

        }

        function showNext() {
            var p;
            if (currentPage == allPage) {
                p = 1;
            }
            else {
                p = currentPage + 1;
            }
            showRow(p);
        }
        $("#1").css("color", "red");
    }
});