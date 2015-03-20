//控制器——微信组织机构控制器
var WXOrgController = angular.module('WXOrgController', ['WXService', 'WXDirective']);
//微信用户分组管理页
WXOrgController.controller('OrgCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
    var _intRowCountInPage = 10;
    //用户列表中checkbox控件的name
    var _CheckBoxName = "ids";
    var urlGroup = "/WXOrganization/GetWXGroups";
    var urlCreateGroup = "/WXOrganization/CreateWXGroup";
    var urlUSer = "/WXOrganization/QueryUserTable";
    var urlChangeGroup = '/WXOrganization/ChangeGroup';
    //微信用户分组
    $scope.groups = null;
    $scope.users = null;
    //当前查询条件集合
    $scope.queryItems = null;
    //当前页面索引
    $scope.curPageIndex = null;
    //目标选中分组ID
    $scope.curGroupId = null;
    //目标选中用户OpenID
    $scope.curOpenID = null;
    //加载微信用户
    $http.post(urlUSer).success(function (data) {
        try {
            var pArray = new Array();
            pArray.push(null);//openid
            pArray.push(null);//nickname
            pArray.push(null);//groupid
            pArray.push(null);//country
            pArray.push(null);//province
            pArray.push(null);//city
            pArray.push(null);//sex

            //赋值当前查询条件集合
            $scope.queryItems = pArray;

            if (data.IsSuccess !== undefined && data.IsSuccess !== null && data.IsSuccess == false) {
                alert(data.Message);
            }
            else {
                $scope.users = data.Users;
                //$scope.$apply();
                LoadTablePages(1, data.RecordsCount);
            }
        }
        catch (ex) { }
    });
    //加载微信分组
    $http.post(urlGroup).success(function (response) {
        $scope.groups = response;
    });
    //
    $scope.ChangeCurUser = function (user, element) {
        $scope.curOpenID = user.openid;
        //弹出发送消息对话框
        $scope.OpenForm(element);
    }
    //切换列表索引頁
    $scope.ChangeTbPage = function (pageIndex) {
        $scope.Query(pageIndex);
    }
    //点击分组信息事件
    $scope.QueryByGroupID = function (groupid) {
        var pArray = new Array();
        pArray.push(null);//openid
        pArray.push(null);//nickname
        pArray.push(groupid);//groupid
        pArray.push(null);//country
        pArray.push(null);//province
        pArray.push(null);//city
        pArray.push(null);//sex

        //赋值当前查询条件集合
        $scope.queryItems = pArray;
        //默认显示第一页
        $scope.Query(1);
    }
    //通过用户输入查询
    $scope.QueryByInput = function () {
        var openid = $("#tbOpenID").val();
        var nickname = $("#tbNickName").val();

        var pArray = new Array();
        if (openid !== undefined && openid !== null && openid != "") {
            pArray.push(openid);
        } else {
            pArray.push(null);
        }//openid
        if (nickname !== undefined && nickname !== null && nickname != "") {
            pArray.push(nickname);
        } else {
            pArray.push(null);
        }//nickname
        pArray.push(null);//groupid
        pArray.push(null);//country
        pArray.push(null);//province
        pArray.push(null);//city
        pArray.push(null);//sex

        //赋值当前查询条件集合
        $scope.queryItems = pArray;
        //请求前，激活按钮等待状态
        var $btn = $("#ui_btnLoading_query").button('loading');
        //默认显示第一页
        $scope.Query(1);
        //重置按钮
        $btn.button('reset')
    }
    //切换选中分组
    $scope.ChangeSelectGroup = function (groupid) {
        $scope.curGroupId = groupid;

        //var groupid = $("#ui_SelectGroupID").val();
        //获取选中openid数组
        var selectArray = new Array();
        selectArray = GetSelectValueFromCheckBox(_CheckBoxName);
        if (selectArray == undefined || selectArray.length == 0) {
            alert("請選擇目標移動分組用戶");
            return;
        }
        //选中openid数组转换为json数据
        var postData = $.toJSON(selectArray);

        $.ajax({
            url: urlChangeGroup,
            data: { "ListOpenID": postData, "GroupID": groupid },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.IsSuccess == true) {
                    alert("操作成功");
                    window.location.reload();
                }
                else
                    alert(data.Message);
            }
        });
    }
    //同步微信用户及分组
    $scope.ReloadUser = function () {
        //请求前，激活按钮等待状态
        var $btn = $("#ui_btnLoading_reloaduser").button('loading');
        //执行post请求
        $.post("/WXOrganization/ReLoadWXUser", null, function (data2) {
            if (data2.IsSuccess !== undefined && data2.IsSuccess !== null && !data2.IsSuccess)
                alert(data2.Message)
            else
                alert('完成同步');
            //重置按钮
            $btn.button('reset')
            //刷新表格table
            window.location.reload();
        })
    }
    //发送模板消息
    $scope.SendMessage = function () {
        //请求前，激活按钮等待状态
        var $btn = $("#ui_btnLoading").button('loading');
        //执行post请求
        $.post("/TemplateMessage/MessageSend", $("#id_form1").serialize(), function (data2) {
            if (data2.IsSuccess !== undefined && data2.IsSuccess !== null && !data2.IsSuccess)
                alert(data2.Message)
            else
                alert('發送成功');
            //重置按钮
            $btn.button('reset')
        })
    }
    //创建微信用户分组
    $scope.CreateGroup = function () {
        var txt = $("#ui_txt").val();
        if (txt == undefined || txt == null || txt == "") {
            alert("請輸入分組名稱");
            return;
        }
        if (txt.length > 8) {
            alert("分組名稱不能多于8个字符，請重新輸入");
            return;
        }
        var postData = { "groupname": txt };
        //请求前，激活按钮等待状态
        var $btn = $("#ui_btnLoading_creategroup").button('loading');
        //执行post请求
        $.post("/WXOrganization/CreateWXGroup", postData, function (data2) {
            if (data2.IsSuccess !== undefined && data2.IsSuccess !== null && !data2.IsSuccess) {
                alert(data2.Message);
            } else {
                alert("新建成功");
                $scope.groups = data2;
                $scope.$apply();
            }
            //重置按钮
            $btn.button('reset')
        })
    }
    //创建分页
    function LoadTablePages(PageIndex, RecordCount) {
        try {
            var pageCountArr = new Array();
            if (RecordCount > _intRowCountInPage) {
                var pageCount = RecordCount / _intRowCountInPage;
                for (var i = 1; i <= pageCount + 1; i++) {
                    pageCountArr.push(i);
                }
            }
            else {
                pageCountArr.push(1);
            }
            //根据消息数量，计算显示页数，并设置当前索引
            $scope.pageCount = pageCountArr;
            //$scope.$apply();
            $scope.curPageIndex = PageIndex;
        }
        catch (ex) { }
    }
    //查询
    $scope.Query = function (pPageIndex) {
        if ($scope.queryItems == undefined || $scope.queryItems == null)
            return;

        //序列化数据
        var postData = $.toJSON($scope.queryItems);
        var param = { "PageIndex": pPageIndex, "RowCountInPage": _intRowCountInPage, "filterString": postData };

        $http.post(urlUSer, param).success(function (data) {
            if (data.IsSuccess !== undefined && data.IsSuccess !== null && data.IsSuccess == false) {
                alert(data.Message);
            }
            else {
                $scope.users = data.Users;
                LoadTablePages(pPageIndex, data.RecordsCount);
            }
        });

        //$.ajax({
        //    url: urlUSer,
        //    data: param,
        //    type: "post",
        //    dataType: "json",
        //    success: function (data) {
        //        if (data.IsSuccess !== undefined && data.IsSuccess !== null && data.IsSuccess == false) {
        //            alert(data.Message);
        //        }
        //        else {
        //            $scope.users = data.Users;
        //            //$scope.$apply();
        //            LoadTablePages(pPageIndex, data.RecordsCount);
        //        }
        //    }
        //});
    }
    //弹出对话框
    $scope.OpenForm = function (element) {
        if (element !== undefined && element !== null && element != '') {
            $("#" + element).modal({
                backdrop: false
            });
        }
    }
}]);
//微信企业号基础配置
WXOrgController.controller('QyCfgCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
    $scope.alinkx = $.LV == undefined || $.LV == "V2" ? "/QYConfig/App" : "#QYConfig/App";

    var urlQyConfig = '/QYConfig/Get';
    $scope.qyconfigm = 0;
    $scope.qyconfigs = null;
    $http.post(urlQyConfig).success(function (data) {
        try {
            if (data.IsSuccess !== undefined && data.IsSuccess !== null && data.IsSuccess == false) {
                alert(data.Message);
            }
            else {
                $scope.qyconfigs = data;
            }
        }
        catch (ex) { }
    });
    $scope.SelCurQyapp = function (curQyapp) {
        $scope.qyconfigm = 1;
        instance.qyapp = curQyapp;
    }
}]);
//微信企业应用配置
WXOrgController.controller('QyappCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
    $scope.qyconfigm = 1;
    $scope.qyapp = instance.qyapp;
    $scope.navg = function () {
        $scope.qyconfigm = 2
    }
}]);
//微信企业菜单配置
WXOrgController.controller('QyappmenuCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
    $scope.isedited = false;
    $scope.menus = null;
    $scope.menuModel = null;
    $scope.qyapp = instance.qyapp;
    var urlQyappmenu = '/QYConfig/GetQYAppMenu?agentid=' + $scope.qyapp.agentid;
    $http.post(urlQyappmenu).success(function (data) {
        try {
            if (data.IsSuccess !== undefined && data.IsSuccess !== null && data.IsSuccess == false) {
                alert(data.Message);
            }
            else {
                $scope.menus = data;
            }
        }
        catch (ex) { }
    });
    $scope.StartEdit = function () {
        var $oi = $("#nestableX .dd-handle a");
        $oi.toggleClass("invisible");

        var paction = ("ontouchend" in document) ? "touchstart" : "mousedown";
        if (!$oi.hasClass("invisible")) {
            $scope.isedited = true;
            $oi.on(paction, function (e) {
                e.stopPropagation();
            });
            $("#nestableX .dd-handle input").on(paction, function (e) {
                e.stopPropagation();
            });

            $("#nestableX .dd-handle select").on(paction, function (e) {
                e.stopPropagation();
            });
        }
        else { $scope.isedited = false; }
    }
    $scope.ResetM = function () {

        $http.post(urlQyappmenu).success(function (data) {
            try {
                if (data.IsSuccess !== undefined && data.IsSuccess !== null && data.IsSuccess == false) {
                    alert(data.Message);
                }
                else {
                    $scope.menus = data;
                }
            }
            catch (ex) { }
        });
    }
    $scope.EditBtn = function (target) {
        //console.log($(".div-editbtn", $(target).closest(".dd-handle")));
        $(".div-editbtn", $(target).closest(".dd-handle")).toggle();
    }
    $scope.HighlightM = function () {
        $(".dd-handle").toggleClass("dd-blue");
    }
    $scope.AddM = function () {
        var b = { "name": "" };
        $scope.menus.menu.button.push(b);
    };
    $scope.SaveM = function () {
        //console.log($scope.menuModel);

        try {
            var menu = null;
            var button = new Array();
            var getBtn = function (name) {
                var a1 = $.grep($scope.menus.menu.button, function (value) {
                    return value.name == name;
                });
                if (a1.length == 0) {
                    for (var i = 0, btn; btn = $scope.menus.menu.button[i++];) {
                        a1 = a1.length == 0 && btn.sub_button != undefined ? $.grep(btn.sub_button, function (value) {
                            return value.name == name;
                        }) : a1;
                    }
                }
                return a1[0];
            }
            var showError = function (err) {
                alert(err);
            };

            if ($scope.menuModel != undefined && $scope.menuModel != null) {
                for (var i = 0, pb; pb = $scope.menuModel[i++];) {
                    if (pb.children != undefined) {
                        var sub_button = new Array();
                        for (var j = 0, cb; cb = pb.children[j++];) {
                            //var pchdbtn = getBtn(cb.id);
                            //console.log(cb.$scope.btn2);
                            var pchdbtn = cb.$scope.btn2;

                            if (pchdbtn.name == undefined || pchdbtn.name == null || pchdbtn.name == "" || pchdbtn.name.length > 8) {
                                showError("请务必填写二級菜單名称，並且名稱不能多於4個漢字或8個字母，請檢查！");
                                return;
                            }
                            if (pchdbtn.type == undefined || pchdbtn.type == null || pchdbtn.type == "") {
                                showError("请务必選擇二級菜單類型，請檢查！");
                                return;
                            } if ((pchdbtn.key == "" && pchdbtn.url == "") || (pchdbtn.key == null && purl == "") || (pchdbtn.key == "" && pchdbtn.url == null)) {
                                showError("请务必選擇二級菜單類型对应返回值，請檢查！");
                                return;
                            }

                            var pname = pchdbtn.name;
                            var ptype = pchdbtn.type;
                            var pkey = pchdbtn.key != undefined ? pchdbtn.key : null;
                            var purl = pchdbtn.url != undefined ? pchdbtn.url : null;


                            pchdbtn = pkey != null ? {
                                'name': pname,
                                'type': ptype,
                                "key": pkey
                            } : {
                                'name': pname,
                                'type': ptype,
                                "url": purl
                            };

                            sub_button.push(pchdbtn);
                        }
                        var pnewbtn = {
                            'name': pb.id,
                            'sub_button': sub_button
                        };
                        button.push(pnewbtn);
                    }
                    else {
                        var pnewbtn = {
                            'name': pb.id,
                        };
                        button.push(pnewbtn);
                    }
                }
                menu = { 'button': button };
                //console.log(menu);

                //$scope.menus.menu = menu;
            }
            else {
                menu = $scope.menus.menu;
            }
            var postData = $.toJSON(menu);
            var param = { "agentid": $scope.qyapp.agentid, "svl": postData };
            $.post("/QYConfig/SetQYAppMenu", param, function (data2) {
                if (data2.IsSuccess !== undefined && data2.IsSuccess !== null && !data2.IsSuccess) {
                    alert(data2.Message);
                } else {
                    alert("新建成功");
                }
                //重置按钮
                //$btn.button('reset')
            })
        }
        catch (e) { alert(e); }
    };
    $scope.ConvertMType = function (type) {
        if (type == "click")
            return "点击跳转事件";
        else if ("view")
            return "跳转URL";
        else if ("scancode_push")
            return "扫码推事件URL";
        else if ("scancode_waitmsg")
            return "扫码推事件且弹出“消息接收中”提示框";
        else if ("pic_sysphoto")
            return "弹出系统拍照发图";
        else if ("pic_photo_or_album")
            return "弹出拍照或者相册发图";
        else if ("pic_weixin")
            return "弹出微信相册发图器";
        else if ("location_select")
            return "弹出地理位置选择器";
        else
            return "";
    };
}]);





