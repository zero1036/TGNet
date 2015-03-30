//控制器——微信组织机构控制器
var WXOrgController = angular.module('WXOrgController', ['WXService', 'WXDirective', 'ui.bootstrap']);
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
    $scope.urlModel = "http://testwechat.cloudapp.net/qy";
    $scope.qyconfigm = 1;
    $scope.qyapp = instance.qyapp;
    var urlSave = "/QYConfig/SetQYAppConfig";
    var urlQyappconfig = '/QYConfig/GetQYAppConfig?aid=' + $scope.qyapp.agentid;
    $scope.qyappconfig = null;
    $http.post(urlQyappconfig).success(function (data) {
        try {
            if (data.IsSuccess !== undefined && data.IsSuccess !== null && data.IsSuccess == false) {
                alert(data.Message);
            }
            else {
                $scope.qyappconfig = data;
            }
        }
        catch (ex) { }
    });
    $scope.navg = function () {
        $scope.qyconfigm = 2
    }
    $scope.setConfig = function () {
        $("#div-editappconfig").toggle();
    };
    $scope.SaveQYApp = function () {
        var param = { "aid": $scope.qyapp.agentid, "token": $scope.qyappconfig.token, "aeskey": $scope.qyappconfig.aeskey };
        $.post(urlSave, param, function (data2) {
            if (data2.IsSuccess !== undefined && data2.IsSuccess !== null && !data2.IsSuccess) {
                alert(data2.Message);
            } else {
                alert("保存成功");
            }
            ////重置按钮
            //$btn.button('reset')
        })
    };
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
        if ($oi.length > 0 && !$oi.hasClass("invisible")) {
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
    $scope.DeleteBtn = function (btn) {
        if ($scope.menus.menu.button == undefined || $scope.menus.menu.button == null || $scope.menus.menu.button < 1)
            return;
        var s = $.inArray(btn, $scope.menus.menu.button);
        if (s != -1) {
            $scope.menus.menu.button.splice(s, 1);
            return;
        }
        else {
            for (var i = 0, b; b = $scope.menus.menu.button[i++];) {
                if (b.sub_button == undefined || b.sub_button == null || b.length < 1) continue;
                s = $.inArray(btn, b.sub_button);
                if (s != -1) {
                    b.sub_button.splice(s, 1);
                    return;
                }
            }
        }
    };
    $scope.HighlightM = function () {
        $(".dd-handle").toggleClass("dd-blue");
    }
    $scope.AddM = function () {
        //var $dd = $($scope.menus.menu.button);
        //var $nb = $dd.eq($scope.menus.menu.button.length - 1);
        //if ($nb.length > 0 && $nb[0].name == "") {
        //    alert("發現未設置名稱的菜單");
        //    return;
        //}

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
            var checkexitem = function (pt, pk, pn) {
                if (pt != "key" && exkeys != null) {
                    var index = $.inArray(pk, exkeys);
                    if (index == -1)
                        exkeys.push(pn);
                    else
                        return "不允許重複菜單键值，請檢查！";
                }

                if (exnames != null) {
                    var index = $.inArray(pn, exnames);
                    if (index == -1)
                        exnames.push(pn);
                    else
                        return "不允許重複菜單名稱，請檢查！";
                }
                return "";
            };

            var exkeys = new Array();
            var exnames = new Array();
            if ($scope.menuModel != undefined && $scope.menuModel != null) {
                for (var i = 0, pb; pb = $scope.menuModel[i++];) {
                    var pbbtn = pb.$scope.btn2 == undefined ? pb.$scope.btn1 : pb.$scope.btn2;
                    var pbname = pbbtn.name;
                    if (pb.children != undefined) {
                        var sub_button = new Array();
                        for (var j = 0, cb; cb = pb.children[j++];) {
                            //var pchdbtn = getBtn(cb.id);
                            //console.log(cb.$scope.btn2);
                            var pchdbtn = cb.$scope.btn2 == undefined ? cb.$scope.btn1 : cb.$scope.btn2;

                            if (pchdbtn == undefined) {
                                showError("请务必填写二級菜單名称，並且名稱不能多於4個漢字或8個字母，請檢查！");
                                return;
                            }
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
                            //检查重复菜单名称及键值
                            var err = checkexitem(ptype, pkey, pname);
                            if (err != "") {
                                showError(err);
                                return;
                            }

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
                            'name': pbname,
                            'sub_button': sub_button
                        };
                        button.push(pnewbtn);
                    }
                    else {
                        var pnewbtn = {
                            'name': pbname,
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
            //请求前，激活按钮等待状态
            var $btn = $("#btn-savemenu").button('loading');
            $.post("/QYConfig/SetQYAppMenu", param, function (data2) {
                if (data2.IsSuccess !== undefined && data2.IsSuccess !== null && !data2.IsSuccess) {
                    alert(data2.Message);
                } else {
                    alert("新建成功");
                }
                //重置按钮
                $btn.button('reset')
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
//企业号创建发送消息
WXOrgController.controller('QYMessageSendCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
    var urlQyConfig = '/QYConfig/Get';
    $scope.editAdded = false;
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
//企业应用创建发送消息
WXOrgController.controller('QYMsCreateCtrl', ['$scope', '$http', 'instance', '$modal', function ($scope, $http, instance, $modal) {
    var urlGroup = "/WXOrganization/GetWXGroups";
    //GroupSendCtrl控制器标识，用于子控制器获取父级控制表起
    $scope.parentSym = "XC";
    $scope.sendTypeText = "全部用戶";
    $scope.curGroupId = null;
    $scope.curGroupText = "";
    $scope.article = null;
    $scope.picture = null;
    $scope.voice = null;
    $scope.video = null;
    $scope.text = null;
    $scope.curMediaID = null;
    $scope.msgType = "text";
    $scope.isSecrect = false;
    $scope.sendTargetText = '';
    $scope.sendTargetList = [];
    $scope.msgItem = {};
    $scope.mTargetTxt = '';
    $scope.dTargetTxt = '';
    $scope.tTargetTxt = '';


    $scope.ChangeSelectGroup = function (id, name) {
        $scope.curGroupId = id;
        $scope.curGroupText = name;
    }
    //加载微信分组
    $http.post(urlGroup).success(function (response) {
        if (response !== undefined && response !== null) {
            $scope.groups = response;
            $scope.curGroupId = response[0].id;
            $scope.curGroupText = response[0].name;
        }
    });
    //方法一：通过add方法，从单例Service入边获取选中图文
    $scope.add = function () {
        $scope.article = instance.article;
    };
    //方法二：设置on监听子控制器中的值
    $scope.$on('SetSelectionForVoice', function (e, SelectModel) {
        $scope.voice = SelectModel;
        $scope.curMediaID = SelectModel.media_id;
        $scope.childCtrlSym = "XC";
    });
    //方法二：设置on监听子控制器中的值
    $scope.$on('SetSelectionForVideo', function (e, SelectModel) {
        $scope.video = SelectModel;
        $scope.curMediaID = SelectModel.media_id;
        $scope.childCtrlSym = "XC";
    });
    //方法二：设置on监听子控制器中的值
    $scope.$on('SetSelectionForArticle', function (e, SelectModel) {
        $scope.article = SelectModel;
        $scope.curMediaID = SelectModel.lcId;
    });
    //方法二：设置on监听子控制器中的值
    $scope.$on('SetSelectionForPicture', function (e, SelectModel) {
        $scope.picture = SelectModel;
        $scope.curMediaID = SelectModel.media_id;
    });
    //创建群发消息，加入待审核队伍
    $scope.CreateGroupMessage = function () {
        var textContext;
        if ($scope.msgType == "text") {
            if ($scope.text == undefined || $scope.text == null || $scope.text == "") {
                alert("請輸入發送內容");
                return;
            } else {
                if ($scope.text.indexOf("<") >= 0 || $scope.text.indexOf(">") >= 0) {
                    alert("不允許HTML標籤哦");
                    return;
                }
            }
            //textContext = $("#RichTxtForGroup").val();
        }
        else if ($scope.msgType == "image" || $scope.msgType == "voice" || $scope.msgType == "video" || $scope.msgType == "file") {
            if ($scope.curMediaID == undefined || $scope.curMediaID == null || $scope.curMediaID == "") {
                alert("微信端素材已失效，請重新上傳素材，或重新選擇發送內容！");
                return;
            }
        }
        else if ($scope.msgType == "news" || $scope.msgType == "mpnews") {
            if ($scope.article.lcId == undefined || $scope.article.lcId == null) {
                alert("素材已被移除或正在修改中，請重新選擇發送內容！");
                return;
            }
            if ($scope.article.byLink) {
                $scope.msgType = "news";
            }
            else {
                $scope.msgType = "mpnews";
            }
        }


        //请求前，激活按钮等待状态
        //var $btn = $("#ui_btnLoading_Cre1").button('loading');

        $scope.msgItem.MediaId = $scope.curMediaID;
        $scope.msgItem.MsgType = $scope.msgType;
        $scope.msgItem.Content = $scope.text;
        $scope.msgItem.AgentId = instance.qyapp.agentid;
        $scope.msgItem.safe = $scope.isSecrect ? 1 : 0;
        if ($scope.sendTargetText != '' && $scope.sendTargetText != undefined)
            $scope.msgItem.ToTarget = '{"touser":"' + $scope.mTargetTxt + '","toparty":"' + $scope.dTargetTxt + '","totag":"' + $scope.tTargetTxt + '"}';
        else
            $scope.msgItem.ToTarget = '{"touser":"@all","toparty":"","totag":""}';


        //创建群发消息，加入待审核队伍
        $.ajax({
            url: '/QYTempMessage/CreateGsMessage',
            //data: { "mediaid": $scope.curMediaID, "sendtype": $scope.sendType, "sendtarget": $scope.curGroupId, "textcontent": $scope.text, "msgtype": $scope.msgType,'wx_type':2,"agendId":1,"safe":$scope.isSecrect?1:0 },
            data: $scope.msgItem,
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.IsSuccess == false)
                    alert(data.Message);
                else
                    alert("创建成功");
                //重置按钮
                //$btn.button('reset')
            }
        });
    }
    //选中资源，并弹出资源选择对话框
    $scope.SelectRes = function (pType, element) {
        //if (pType == 'text') {
        //    $scope.isText = true;
        //}
        //else {
        //    $scope.isText = false;
        //}

        if (element !== undefined && element !== null && element != '') {
            $('#' + element).modal({
                backdrop: false
            });
        }
        $scope.$broadcast('CallChildLoadRes', pType);
        $scope.msgType = pType;
    }


    //打开选择发送对象modal
    //调用了 bootstrap ui angular
    $scope.openSendTargetModal = function (size) {
        var modalInstance = $modal.open({
            templateUrl: '/Scripts/Views/Page/QYSendTargetModal.html',
            controller: 'sendModalCtrl',
            size: size,
            resolve: {
                sendTargetList: function () {
                    return $scope.sendTargetList;
                },

            }
        });

        modalInstance.result.then(function (sendTargetList) {
            $scope.sendTargetList = sendTargetList;

            if ($scope.sendTargetList != undefined && $scope.sendTargetList.length > 0)
                $scope.getSendTargetNames($scope.sendTargetList);
            else
                $scope.sendTargetText = '';
        }, function () {
            //$log.info('Modal dismissed at: ' + new Date());
        });
    }

    //显示发送对象名字
    $scope.getSendTargetNames = function (sendTargetList) {
        $scope.sendTargetText = '';
        $scope.mTargetTxt = '';
        $scope.dTargetTxt = '';
        $scope.tTargetTxt = '';
        for (var i = 0; i < sendTargetList.length; i++) {
            $scope.sendTargetText += sendTargetList[i].Name + '|';
            if (sendTargetList[i].type == '1') {
                $scope.mTargetTxt += sendTargetList[i].id + '|';
                continue;
            }
            if (sendTargetList[i].type == '2') {
                $scope.dTargetTxt += sendTargetList[i].id + '|';
                continue;
            }
            if (sendTargetList[i].type == '3') {
                $scope.tTargetTxt += sendTargetList[i].id + '|';
                continue;
            }
        }
        $scope.sendTargetText = $scope.sendTargetText.substring(0, $scope.sendTargetText.length - 1);
        $scope.mTargetTxt = $scope.mTargetTxt.substring(0, $scope.mTargetTxt.length - 1);
        $scope.dTargetTxt = $scope.dTargetTxt.substring(0, $scope.dTargetTxt.length - 1);
        $scope.tTargetTxt = $scope.tTargetTxt.substring(0, $scope.tTargetTxt.length - 1);
    }

    $scope.getSecrectFlag = function () {
        if ($scope.isSecrect) {
            $scope.msgItem.Safe = 0;
            $scope.isSecrect = false;
        }
        else {
            $scope.isSecrect = true;
            $scope.msgItem.Safe = 1;
        }

    }
}]);


//企业应用创建功能发送消息
WXOrgController.controller('QYFuncMsCreateCtrl', ['$scope', '$http', 'instance', '$modal', function ($scope, $http, instance, $modal) {
    var urlGroup = "/WXOrganization/GetWXGroups";
    //GroupSendCtrl控制器标识，用于子控制器获取父级控制表起
    $scope.parentSym = "XC";
    $scope.sendTypeText = "全部用戶";
    $scope.curGroupId = null;
    $scope.curGroupText = "";
    $scope.article = null;
    $scope.picture = null;
    $scope.voice = null;
    $scope.video = null;
    $scope.text = null;
    $scope.curMediaID = null;
    $scope.msgType = "vote";
    $scope.isSecrect = false;
    $scope.sendTargetText = '';
    $scope.sendTargetList = [];
    $scope.msgItem = {};
    $scope.mTargetTxt = '';
    $scope.dTargetTxt = '';
    $scope.tTargetTxt = '';

    //获取投票列表
    $http.post('/VoteManage/GetVoteList').success(function (response) {
        $scope.voteList = response;
    });

    $scope.getVoteItem = function (item) {
        $scope.voteTitle = item.Title;
        $scope.voteID = item.ID;
    }


    //$scope.ChangeSelectGroup = function (id, name) {
    //    $scope.curGroupId = id;
    //    $scope.curGroupText = name;
    //}
    ////加载微信分组
    //$http.post(urlGroup).success(function (response) {
    //    if (response !== undefined && response !== null) {
    //        $scope.groups = response;
    //        $scope.curGroupId = response[0].id;
    //        $scope.curGroupText = response[0].name;
    //    }
    //});
    ////方法一：通过add方法，从单例Service入边获取选中图文
    //$scope.add = function () {
    //    $scope.article = instance.article;
    //};
    ////方法二：设置on监听子控制器中的值
    //$scope.$on('SetSelectionForVoice', function (e, SelectModel) {
    //    $scope.voice = SelectModel;
    //    $scope.curMediaID = SelectModel.media_id;
    //    $scope.childCtrlSym = "XC";
    //});
    ////方法二：设置on监听子控制器中的值
    //$scope.$on('SetSelectionForVideo', function (e, SelectModel) {
    //    $scope.video = SelectModel;
    //    $scope.curMediaID = SelectModel.media_id;
    //    $scope.childCtrlSym = "XC";
    //});
    ////方法二：设置on监听子控制器中的值
    //$scope.$on('SetSelectionForArticle', function (e, SelectModel) {
    //    $scope.article = SelectModel;
    //    $scope.curMediaID = SelectModel.lcId;
    //});
    ////方法二：设置on监听子控制器中的值
    //$scope.$on('SetSelectionForPicture', function (e, SelectModel) {
    //    $scope.picture = SelectModel;
    //    $scope.curMediaID = SelectModel.media_id;
    //});
    //创建群发消息，加入待审核队伍
    $scope.CreateGroupMessage = function () {
        var textContext;
        //if ($scope.msgType == "text") {
        //    if ($scope.text == undefined || $scope.text == null || $scope.text == "") {
        //        alert("請輸入發送內容");
        //        return;
        //    } else {
        //        if ($scope.text.indexOf("<") >= 0 || $scope.text.indexOf(">") >= 0) {
        //            alert("不允許HTML標籤哦");
        //            return;
        //        }
        //    }
            //textContext = $("#RichTxtForGroup").val();
        //}
        //else {
        //    if ($scope.curMediaID == undefined || $scope.curMediaID == null || $scope.curMediaID == "") {
        //        alert("微信端素材已失效，請重新上傳素材，或重新選擇發送內容！");
        //        return;
        //    }
        //    $scope.msgItem.Safe = 0;
        //}


        //请求前，激活按钮等待状态
        //var $btn = $("#ui_btnLoading_Cre1").button('loading');

        $scope.msgItem.MediaId = $scope.voteID;
        $scope.msgItem.MsgType = $scope.msgType;
        $scope.msgItem.Content ="vote";
        $scope.msgItem.AgentId = instance.qyapp.agentid;
        //$scope.msgItem.safe = $scope.isSecrect ? 1 : 0;
        if ($scope.sendTargetText != '' && $scope.sendTargetText != undefined)
            $scope.msgItem.ToTarget = '{"touser":"' + $scope.mTargetTxt + '","toparty":"' + $scope.dTargetTxt + '","totag":"' + $scope.tTargetTxt + '"}';
        else
            $scope.msgItem.ToTarget = '{"touser":"@all","toparty":"","totag":""}';


        //创建群发消息，加入待审核队伍
        $.ajax({
            url: '/QYTempMessage/CreateGsMessage',
            //data: { "mediaid": $scope.curMediaID, "sendtype": $scope.sendType, "sendtarget": $scope.curGroupId, "textcontent": $scope.text, "msgtype": $scope.msgType,'wx_type':2,"agendId":1,"safe":$scope.isSecrect?1:0 },
            data: $scope.msgItem,
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.IsSuccess == false)
                    alert(data.Message);
                else
                    alert("创建成功");
                //重置按钮
                //$btn.button('reset')
            }
        });
    }
    //选中资源，并弹出资源选择对话框
    //$scope.SelectRes = function (pType, element) {
    //    if (pType == 'text') {
    //        $scope.isText = true;
    //    }
    //    else {
    //        $scope.isText = false;
    //    }

    //    if (element !== undefined && element !== null && element != '') {
    //        $('#' + element).modal({
    //            backdrop: false
    //        });
    //    }
    //    $scope.$broadcast('CallChildLoadRes', pType);
    //    $scope.msgType = pType;
    //}


    //打开选择发送对象modal
    $scope.openSendTargetModal = function (size) {
        var modalInstance = $modal.open({
            templateUrl: '/Scripts/Views/Page/QYSendTargetModal.html',
            controller: 'sendModalCtrl',
            size: size,
            resolve: {
                sendTargetList: function () {
                    return $scope.sendTargetList;
                },

            }
        });

        modalInstance.result.then(function (sendTargetList) {
            $scope.sendTargetList = sendTargetList;

            if ($scope.sendTargetList != undefined && $scope.sendTargetList.length > 0)
                $scope.getSendTargetNames($scope.sendTargetList);
            else
                $scope.sendTargetText = '';
        }, function () {
            //$log.info('Modal dismissed at: ' + new Date());
        });
    }

    //显示发送对象名字
    $scope.getSendTargetNames = function (sendTargetList) {
        $scope.sendTargetText = '';
        $scope.mTargetTxt = '';
        $scope.dTargetTxt = '';
        $scope.tTargetTxt = '';
        for (var i = 0; i < sendTargetList.length; i++) {
            $scope.sendTargetText += sendTargetList[i].Name + '|';
            if (sendTargetList[i].type == '1') {
                $scope.mTargetTxt += sendTargetList[i].id + '|';
                continue;
            }
            if (sendTargetList[i].type == '2') {
                $scope.dTargetTxt += sendTargetList[i].id + '|';
                continue;
            }
            if (sendTargetList[i].type == '3') {
                $scope.tTargetTxt += sendTargetList[i].id + '|';
                continue;
            }
        }
        $scope.sendTargetText = $scope.sendTargetText.substring(0, $scope.sendTargetText.length - 1);
        $scope.mTargetTxt = $scope.mTargetTxt.substring(0, $scope.mTargetTxt.length - 1);
        $scope.dTargetTxt = $scope.dTargetTxt.substring(0, $scope.dTargetTxt.length - 1);
        $scope.tTargetTxt = $scope.tTargetTxt.substring(0, $scope.tTargetTxt.length - 1);
    }

    //$scope.getSecrectFlag = function () {
    //    if ($scope.isSecrect) {
    //        $scope.msgItem.Safe = 0;
    //        $scope.isSecrect = false;
    //    }
    //    else {
    //        $scope.isSecrect = true;
    //        $scope.msgItem.Safe = 1;
    //    }

    //}
}]);


//企业消息审核
WXOrgController.controller('QYMsReviewCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
    //微信分组集合
    $scope.groups = null;
    ////发送类型
    //$scope.sendType = 1;
    //消息集合
    $scope.GSMessages = null;
    //当前选中组
    $scope.CurGroupId = null;
    //当前选中消息
    $scope.CurGSMessage = null;
    //当前选中内容类型
    $scope.CurCType = null;
    //当前选中消息的内容
    $scope.CurContent = null;
    //当前选中消息的mediaID
    $scope.curMediaID = null;
    //根据消息数量，计算显示页数
    $scope.pageCount = null;
    //当前页面索引
    $scope.curPageIndex = null;


    $scope.text = null;
    $scope.article = null;
    $scope.picture = null;
    $scope.video = null;
    $scope.voice = null;
    //
    //var urlGroup = "/WXOrganization/GetWXGroups";
    var urlGs = "/QYMessage/GetQYMs";
    ////加载微信分组
    //$http.post(urlGroup).success(function (response) {
    //    $scope.groups = response;
    //});
    //加载群发消息
    $http.post(urlGs).success(function (response) {
        $scope.GSMessages = response;

        //創建分頁
        LoadTablePages(response);
    });
    //选中消息变更
    $scope.ChangeCur = function (CurGs) {
        $scope.CurGSMessage = CurGs;
        $scope.CurCType = CurGs.ContentType;

        //绑定当前curMediaID
        $scope.curMediaID = CurGs.SContent;
        if (CurGs.ResultJson == null || CurGs.ResultJson == undefined) {
            alert("素材已被修改或移除，請重新創建消息！");
        }
        //绑定模型
        if (CurGs.ContentType == 'text') {
            $scope.text = CurGs.SContent;
        }
        else if (CurGs.ContentType == 'image') {
            $scope.picture = CurGs.ResultJson;
        }
        else if (CurGs.ContentType == 'voice') {
            $scope.voice = CurGs.ResultJson;
        }
        else if (CurGs.ContentType == 'video') {
            $scope.video = CurGs.ResultJson;
        }
        else if (CurGs.ContentType == 'news') {
            $scope.article = CurGs.ResultJson;
        }
        else if (CurGs.ContentType == 'mpnews') {
            $scope.article = CurGs.ResultJson;
        }
        else if (CurGs.ContentType == 'news') {
            $scope.article = CurGs.ResultJson;
        }
        //切换当前行时，关闭发送按钮
        $scope.displayBtn = 0;
    }
    //是否显示发送按钮
    $scope.displayBtn = 0;
    //发送群发消息
    $scope.SendGroupMessage = function () {
        var sex = "";
        //var
        var textContext;
        var GetKeyW = function () {
            if ($scope.CurCType == "text")
                return $scope.text;
            else if ($scope.CurCType == "image" || $scope.CurCType == "voice" || $scope.CurCType == "video" || $scope.CurCType == "file") {
                return $scope.curMediaID
            }
            else {
                return $scope.article.lcId;
            }
        }

        if ($scope.CurCType == "text") {
            if ($scope.text == undefined || $scope.text == null || $scope.text == "") {
                alert("缺少發送內容");
                return;
            }
        }
        else {
            if ($scope.curMediaID == undefined || $scope.curMediaID == null || $scope.curMediaID == "") {
                alert("請選擇發送內容");
                return;
            }
        }
        if ($scope.CurGSMessage.SState != 1) {
            alert("消息已發送，請重新選擇！");
            return;
        }

        //请求前，激活按钮等待状态
        var $btn = $("#ui_btnLoading").button('loading');

        //发送群发消息post请求——以用户OpenID为范围发送
        $.ajax({
            url: '/QYMessage/QYMsSend',
            data: {
                "messageid": $scope.CurGSMessage.ID,
                "keyW": GetKeyW(),
                "msgtype": $scope.CurCType,
                "agentId": 47,
                "toUser": "@all",
                "toParty": "@all",
                "toTag": "@all",
                "safe": $scope.CurGSMessage.safe
            },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.IsSuccess == false)
                    alert(data.Message);
                else
                    alert("发送成功");
                //重置按钮
                $btn.button('reset')
            }
        });

        //最後更新顯示
        angular.forEach($scope.GSMessages, function (item) {
            if (item.ID == $scope.CurGSMessage.ID) {
                item.SState = 3;
                item.SStateX = '已發送';
            }
        });
    }
    //切换列表索引頁
    $scope.ChangeTbPage = function (pageIndex) {
        $scope.curPageIndex = pageIndex;
    }
    //消息类型下拉框默认标题
    $scope.SQCType_Title = "所有類型";
    //消息类型下拉框切换选项
    $scope.ChangeContentType = function (type) {
        $scope.SQCType = type;
        var pValue = ConvertContentType(type);
        if (pValue == "")
            $scope.SQCType_Title = "所有類型";
        else
            $scope.SQCType_Title = pValue;
    }
    //發送狀態下拉框默认标题
    $scope.SQSState_Title = "所有狀態"
    //發送狀態下拉框切换选项
    $scope.ChangeSState = function (state) {
        $scope.SQSState = state;
        var pValue = ConvertSState(state);
        if (pValue == "")
            $scope.SQSState_Title = "所有狀態";
        else
            $scope.SQSState_Title = pValue;
    }
    //查询选项——发送状态
    $scope.SQSState = null;
    //查询选项——内容类型
    $scope.SQCType = null;
    //查询
    $scope.Query = function () {
        var pArray = new Array();

        pArray.push(null);//ID
        pArray.push(null);//userid
        pArray.push(null);//mtime
        pArray.push(null);//stime
        pArray.push(null);//stype
        pArray.push(null);//starget
        if ($scope.SQCType !== undefined && $scope.SQCType !== null && $scope.SQCType != "") {
            pArray.push($scope.SQCType);
        } else {
            pArray.push(null);
        }//contenttype
        pArray.push(null);//scontent
        if ($scope.SQSState !== undefined && $scope.SQSState !== null) {
            pArray.push($scope.SQSState);
        } else {
            pArray.push(null);
        }//SState
        //
        var postData = $.toJSON(pArray);

        $.ajax({
            url: '/QYMessage/GetQYMsByFilter',
            data: { "filterString": postData },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.IsSuccess !== undefined && data.IsSuccess !== null && data.IsSuccess == false) {
                    alert(data.Message);
                }
                else {
                    $scope.GSMessages = data;
                    $scope.$apply();
                    LoadTablePages(data);
                }
                ////重置按钮
                //$btn.button('reset')
            }
        });
    }
    //取消发送
    $scope.CancelSend = function () {
        $scope.displayBtn = 0
    }


    //创建分页
    function LoadTablePages(response) {
        try {
            var pageCountArr = new Array();
            if (response.length > 15) {
                var pageCount = response.length / 15;
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
            $scope.curPageIndex = 1;
        }
        catch (ex) { }
    }
}]);

//选择发送对象modal的页面ctrl
WXOrgController.controller('sendModalCtrl', ['$scope', '$http', 'instance', 'memberService', '$modalInstance', 'sendTargetList', function ($scope, $http, instance, memberService, $modalInstance, sendTargetList) {
    $scope.urlMenu = '/QYDepart/GetDepartMenu';

    $scope.sendTargetList = [];
    $scope.sendTargetItem = {};
    $scope.sendTargetText = '';

    //清除所有对象选择
    $scope.removeAllTarget = function () {
        $scope.sendTargetText = '';
        $scope.sendTargetList = [];
        for (var i = 0; i < $scope.memberList.length; i++) {
            if ($scope.memberList[i].IsChecked) {
                $scope.memberList[i].IsChecked = false;
            }
        }
    }

    //var departTree;
    //var setting = {
    //    view: {
    //        dblClickExpand: false,
    //        showLine: false,
    //        expandSpeed: "fast"
    //        //($.browser.msie && parseInt($.browser.version) <= 6) ? "" : 
    //    },
    //    data: {
    //        key: {
    //            name: "Name"
    //        },
    //        simpleData: {
    //            enable: true,
    //            idKey: "DepartmentID",
    //            pIdKey: "ParentDepartmentID",
    //            rootPId: ""
    //        }
    //    },
    //    callback: {
    //        onClick: function (event, treeId, treeNode, clickFlag) {
    //            $scope.$apply(function () {
    //                $scope.getMemberList(treeNode.DepPKID);
    //            });
    //        }
    //    }
    //};
    //var url = $scope.urlMenu;
    //$http.post(url, null).success(function (data) {
    //    // 如果返回数据不为空，加载"业务模块"目录
    //    if (data != null) {
    //        // 将返回的数据赋给zTree
    //        departTree = $.fn.zTree.init($("#treedepart"), setting, data);
    //        var rootNode = departTree.getNodesByFilter(function (node) { return node.level == 0 }, true);
    //        $scope.getMemberList(rootNode.DepPKID);
    //        //zTree = $.fn.zTree.getZTreeObj(uiId);
    //        //if (zTree) {
    //        //    // 默认展开所有节点
    //        //    zTree.expandAll(true);
    //        //}
    //    }
    //}).error(function () {
    //});


    //var departSelTree;
    //var node;
    //var setting2 = {
    //    check: {
    //        enable: true,
    //        chkboxType: { "Y": "", "N": "" }
    //    },
    //    view: {
    //        dblClickExpand: false,
    //        showLine: false,
    //        expandSpeed: "fast"
    //        //($.browser.msie && parseInt($.browser.version) <= 6) ? "" : 
    //    },
    //    data: {
    //        key: {
    //            name: "Name"
    //        },
    //        simpleData: {
    //            enable: true,
    //            idKey: "DepartmentID",
    //            pIdKey: "ParentDepartmentID",
    //            rootPId: ""
    //        }
    //    },
    //    callback: {
    //        beforeClick: function beforeClick(treeId, treeNode) {
    //            departSelTree = $.fn.zTree.getZTreeObj("selDepartMenu");
    //            departSelTree.checkNode(treeNode, !treeNode.checked, null, true);
    //            return false;
    //        },
    //        onCheck: function onCheck(e, treeId, treeNode) {
    //            $scope.$apply(function () {
    //                //departSelTree = $.fn.zTree.getZTreeObj(attrs.id);
    //                //nodes = departSelTree.getCheckedNodes(true);
    //                if (treeNode.checked) {
    //                    $scope.sendTargetItem = {};
    //                    $scope.sendTargetItem.type = '2';
    //                    $scope.sendTargetItem.id = treeNode.DepartmentID;
    //                    $scope.sendTargetItem.Name = treeNode.Name;
    //                    $scope.sendTargetList.push($scope.sendTargetItem);

    //                }
    //                else {
    //                    for (var i = 0; i < $scope.sendTargetList.length; i++) {
    //                        if ($scope.sendTargetList[i].type == '2' && $scope.sendTargetList[i].id == treeNode.DepartmentID) {
    //                            $scope.sendTargetList.splice(i, 1);
    //                            i--;
    //                            break;
    //                        }
    //                    }
    //                }
    //                $scope.getSendTargetNames($scope.sendTargetList);
    //            });
    //        }
    //    }
    //};
    //var url = $scope.urlMenu;
    //$http.post(url, null).success(function (data) {
    //    // 如果返回数据不为空，加载"业务模块"目录
    //    if (data != null) {
    //        // 将返回的数据赋给zTree
    //        departSelTree = $.fn.zTree.init($("#selDepartMenu"), setting2, data);

    //        for (var i = 0; i < $scope.sendTargetList.length; i++) {
    //            if ($scope.sendTargetList[i].type != '2')
    //                continue;
    //            node = departSelTree.getNodeByParam("DepartmentID", $scope.sendTargetList[i].id);
    //            if (node != null || node != undefined) {
    //                node.checked = true;
    //            }
    //        }
    //        //zTree = $.fn.zTree.getZTreeObj(uiId);
    //        //if (zTree) {
    //        //    // 默认展开所有节点
    //        //    zTree.expandAll(true);
    //        //}
    //    }
    //}).error(function () {
    //});

    //獲取成員列表
    $scope.getMemberList = function (depPKID) {
        memberService.getMemberList(depPKID).success(function (data) {
            $scope.memberList = data;
            for (var i = 0; i < $scope.sendTargetList.length; i++) {
                if ($scope.sendTargetList[i].type != '1')
                    continue;
                for (var j = 0; j < $scope.memberList.length; j++) {
                    if ($scope.memberList[j].Name == $scope.sendTargetList[i].Name && $scope.memberList[j].UserId == $scope.sendTargetList[i].id) {
                        $scope.memberList[j].IsChecked = true;
                        break;
                    }
                }
            }

        }).error(function () {
            alert('獲取成員失敗');
        });
    }


    //成员checkbox事件
    $scope.memberToggleChecked = function (item) {
        if (!item.IsChecked) {
            $scope.sendTargetItem = {};
            item.IsChecked = true;
            $scope.sendTargetItem.type = '1';
            $scope.sendTargetItem.id = item.UserId;
            $scope.sendTargetItem.Name = item.Name;
            $scope.sendTargetList.push($scope.sendTargetItem);

        }
        else {
            item.IsChecked = false;
            for (var i = 0; i < $scope.sendTargetList.length; i++) {
                if ($scope.sendTargetList[i].type == '1' && $scope.sendTargetList[i].id == item.UserId) {
                    $scope.sendTargetList.splice(i, 1);
                    i--;
                    break;
                }
            }
        }

        $scope.getSendTargetNames($scope.sendTargetList);

    }

    //显示发送对象名字
    $scope.getSendTargetNames = function (sendTargetList) {
        $scope.sendTargetText = '';
        for (var i = 0; i < sendTargetList.length; i++) {
            $scope.sendTargetText += sendTargetList[i].Name + '|';
        }
        $scope.sendTargetText = $scope.sendTargetText.substring(0, $scope.sendTargetText.length - 1);
    }

    //将发送对象传回父作用域
    $scope.sendTargetOK = function () {
        $modalInstance.close($scope.sendTargetList);
    }

    //关闭modal框
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    }

    //获取从父ctrl传来的对象
    if (sendTargetList != undefined && sendTargetList != null) {
        $scope.sendTargetList = sendTargetList;
        $scope.getSendTargetNames($scope.sendTargetList);
    }
}
]);




