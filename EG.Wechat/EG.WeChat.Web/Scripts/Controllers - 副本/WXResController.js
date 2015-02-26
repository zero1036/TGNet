
var WXResController = angular.module("WXResController", ['ngRoute', 'WXService', 'WXDirective']);
//分頁組件空間器
WXResController.controller('PageCtrl', ['$scope', '$http', 'instance', '$routeParams', function ($scope, $http, instance, $routeParams) {
    var _intRowCountInPage = 8;
    $scope.load = function () {
        $scope.QueryX();
    }
    $scope.$on('QueryPage', function (e, pageIndex) {
        $scope.ChangeTbPage(pageIndex);
    });
    $scope.ChangeTbPage = function (pageIndex) {
        $scope.Query(pageIndex);
    }
    $scope.Query = function (pPageIndex) {
        var param = null;
        if (instance.QItem.QueryItems !== null) {
            var obj = instance.QItem.QueryItems;
            //序列化数据
            var postData = $.toJSON(obj);
            param = { "PageIndex": pPageIndex, "RowCountInPage": _intRowCountInPage, "filterString": postData };
        }
        $http.post(instance.QItem.Url, param).success(function (data) {
            if (data.IsSuccess !== undefined && data.IsSuccess !== null && data.IsSuccess == false) {
                alert(data.Message);
            }
            else {
                instance.QItem.Results = data.ListJson;
                LoadTablePages(pPageIndex, data.RecordsCount);
                $scope.RefreshList();
            }
        });
    }
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
}]);
//图片资源列表Controller
WXResController.controller('PictureListCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
    var urlLP = "/WXResource/LoadWXPictures";
    //$http.post(urlLP).success(function (response) {
    //    $scope.pictures = response.ListJson;
    //    instance.pictures = response.ListJson;
    //});
    $scope.$on('CallChildLoadRes', function (e, pType) {
        if (pType != "image")
            return;
        $scope.QueryX();
    });
    $scope.QueryX = function () {
        var pArray = new Array();
        pArray.push("");//keyWord
        pArray.push(null);//bLose
        pArray.push(1);//iSourceType
        instance.QItem = {
            Url: urlLP,
            QueryItems: pArray,
            Results: null
        };
        $scope.$broadcast('QueryPage', 1);
    }
    $scope.RefreshList = function () {
        $scope.pictures = instance.QItem.Results;
        instance.QItem.Results = null;
    }
    $scope.pictures = null;// = pPicsData;
    //设置选中资源，并返回到父页（只有在子页为IFrame时有效）
    $scope.SetSelection = function (picture, mediaID) {
        //执行父级
        $scope.$emit('SetSelectionForPicture', picture);
        //由于图文编辑页面仍然未修改，因此还是需要兼容旧的方法
        try {
            if (parent.selectPicture !== null && parent.selectPicture !== undefined) {
                parent.selectPicture(picture.RPath, mediaID);
            }
        } catch (exception) {
        }
    }
}]);
//图文资源列表Controller
WXResController.controller('ArticleListCtrl', ['$scope', '$http', 'instance', 'ResourceService', function ($scope, $http, instance, ResourceService) {
    var urlLA = "/WXResource/LoadWXArticles";
    //$http.post(urlLA).success(function (response) {
    //    $scope.Articles = response.ListArticles;
    //    instance.Articles = response.ListArticles;
    //});
    $scope.$on('CallChildLoadRes', function (e, pType) {
        if (pType != "mpnews")
            return;
        $scope.QueryX();
    });
    $scope.QueryX = function () {
        var pArray = new Array();
        pArray.push("");//keyWord
        pArray.push(null);//bLose
        pArray.push(1);//iSourceType
        instance.QItem = {
            Url: urlLA,
            QueryItems: pArray,
            Results: null
        };
        $scope.$broadcast('QueryPage', 1);
    }
    $scope.RefreshList = function () {
        $scope.Articles = instance.QItem.Results;
        instance.QItem.Results = null;
    }
    $scope.parentSym = "ALC";
    $scope.EditMode = 0;
    $scope.Articles = null;// = pPicsData;
    ////设置选中资源，并返回到父页（只有在子页为IFrame时有效）
    $scope.SetSelection = function (Article) {
        //执行父级
        $scope.$emit('SetSelectionForArticle', Article);
    }
    //编辑图文消息，将选中图文设定到单例共享服务
    $scope.Edit = function (Article) {
        $scope.EditMode = 1;
        instance.article = Article;
    }
    $scope.Delete = function (PTargetEditIndex, Article) {

        //如果段落剩余一条，不允许删除
        if ($scope.Articles.length < 1)
            return;
        $scope.Articles.splice(PTargetEditIndex, 1);
        ResourceService.Delete(Article.lcId, "News");
    }
    //方法二：设置on监听子控制器中的值
    $scope.$on('EndEdit', function (e, Model) {
        $scope.EditMode = 0;
    });
}]);
//視頻资源列表Controller
WXResController.controller('VideoListCtrl', ['$scope', '$http', 'instance', 'ResourceService', function ($scope, $http, instance, ResourceService) {
    var urlLv = "/WXResource/LoadWXVideos";
    $scope.videos = null;
    $scope.urlVpl = "/WXResource/VideoUpLoad";
    $scope.ipLcName = "";
    $scope.ipLcClassify = "";
    //$scope.video = null;
    $scope.$on('CallChildLoadRes', function (e, pType) {
        if (pType != "video")
            return;
        $scope.QueryX();
    });
    $scope.QueryX = function () {
        var pArray = new Array();
        pArray.push("");//keyWord
        pArray.push(null);//bLose
        pArray.push(1);//iSourceType
        instance.QItem = {
            Url: urlLv,
            QueryItems: pArray,
            Results: null
        };
        $scope.$broadcast('QueryPage', 1);
    }
    $scope.RefreshList = function () {
        $scope.videos = instance.QItem.Results;
        instance.QItem.Results = null;
    }
    //$http.post(url).success(function (response) {
    //    $scope.videos = response.ListJson;
    //    instance.videos = response.ListJson;
    //});
    $scope.UpLoadVideo = function (element) {
        if (element !== undefined && element !== null && element != '') {
            $('#' + element).modal({
                backdrop: false
            });
        }
    }
    $scope.Play = function (video) {
        $scope.video = video;
        //$scope.$broadcast('recall', video);
    };
    $scope.DeleteVideo = function (PTargetEditIndex, Video) {
        if ($scope.videos.length < 1)
            return;
        $scope.videos.splice(PTargetEditIndex, 1);
        ResourceService.Delete(Video.lcId, "Video");
    }
    $scope.SetSelection = function (video) {
        //执行父级
        $scope.$emit('SetSelectionForVideo', video);
    }
}]);
//音頻資源列表Controller
WXResController.controller('VoiceListCtrl', ['$scope', '$http', 'instance', 'ResourceService', function ($scope, $http, instance, ResourceService) {
    var urlLVi = "/WXResource/LoadWXVoices";
    $scope.voices = null;
    $scope.urlVopl = "/WXResource/VoiceUpLoad";
    $scope.ipLcName = "";
    $scope.ipLcClassify = "";
    //$http.post(url).success(function (response) {
    //    $scope.voices = response.ListJson;
    //    instance.voices = response.ListJson;
    //});
    $scope.$on('CallChildLoadRes', function (e, pType) {
        if (pType != "voice")
            return;
        $scope.QueryX();
    });
    $scope.QueryX = function () {
        var pArray = new Array();
        pArray.push("");//keyWord
        pArray.push(null);//bLose
        pArray.push(1);//iSourceType
        instance.QItem = {
            Url: urlLVi,
            QueryItems: pArray,
            Results: null
        };
        $scope.$broadcast('QueryPage', 1);
    }
    $scope.RefreshList = function () {
        $scope.voices = instance.QItem.Results;
        instance.QItem.Results = null;
    }
    $scope.UpLoadVoice = function (element) {
        if (element !== undefined && element !== null && element != '') {
            $('#' + element).modal({
                backdrop: false
            });
        }
    }
    $scope.Play = function (voice) {
        $scope.voice = voice;
        //$scope.$broadcast('recall', voice);
        $scope.SetSelection(voice);
    };
    $scope.DeleteVoice = function (PTargetEditIndex, Voice) {
        if ($scope.voices.length < 1)
            return;
        $scope.voices.splice(PTargetEditIndex, 1);
        ResourceService.Delete(Voice.lcId, "Voice");
    }
    $scope.SetSelection = function (voice) {
        //执行父级
        $scope.$emit('SetSelectionForVoice', voice);
    }

}]);
//图文段落预览Controller
WXResController.controller('NewsModelCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
    $scope.curNewsModel = null;
    $scope.CurMediaIDX = null;
    $scope.showContent = 0;
    $scope.childCtrlSym = "";
    $scope.curDate = function () {
        var myDate = new Date();
        return myDate.getFullYear() + "年" + (myDate.getMonth() + 1) + "月" + myDate.getDate() + "日   ";
    }
    $scope.DisplayMode = function (pMode, pMediaID) {
        //
        if ($scope.EditMode == 1) {
            if (pMode == "A")
                return true;
            else
                return false;
        }
        else {
            if ($scope.showContent == 0) {
                return pMode == "A";
            }
            else {
                if (pMode == "B" && $scope.CurMediaIDX == pMediaID) {
                    return true;
                }
                else if (pMode == "A" && $scope.CurMediaIDX != pMediaID) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
    }
    $scope.SelectNewsModel = function (mediaId, newsModel, pIndex) {
        if ($scope.EditMode == 1)
            return;
        if ($scope.showContent == 1) {
            $scope.showContent = 0;
        } else {
            $scope.showContent = 1;
            $scope.CurMediaIDX = mediaId;
            $scope.curNewsModel = newsModel;
            var parentCtrlSym = $scope.parentSym;
            if (parentCtrlSym !== null && parentCtrlSym !== undefined && parentCtrlSym == "XC")
                $("#sm" + mediaId + parentCtrlSym).html(newsModel.content);
            else
                $("#sm" + mediaId).html(newsModel.content);
        }
    }
}]);
//图文资源编辑Controller
WXResController.controller('ArticleEditCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
    $("#summernote").summernote({
        height: 250,
    });
    //var pUrl = window.location.href;
    //var isCreated = pUrl.substr(pUrl.length - 1, 1);

    //$scope.isCreated = $routeParams.IsCreated;
    //$scope.isCreated = isCreated;
    if (instance.article === null || instance.article === undefined) {
        var pNewArticle = Object.create(ArticlesModel);
        var pNews = Object.create(NewsModel);
        pNewArticle.ListNews = new Array();
        pNewArticle.ListNews.push(pNews);
        $scope.article = pNewArticle;
    }
    else {
        $scope.article = instance.article;
        $scope.media_id = instance.article.media_id;
    }
    $scope.curIndex = null;
    $scope.curNewsModel = null;
    $scope.SelectNewsModelWhenEdit = function (newsModel, index) {
        $scope.curIndex = index;
        $scope.curNewsModel = newsModel;
    }
    $scope.AddNewsModel = function () {
        if ($scope.article.ListNews.length == 10) {
            alert("最多只可以加入10组段落!");
            return;
        }
        var pNew = Object.create(NewsModel);
        $scope.article.ListNews.push(pNew);
    }
    $scope.DeleteNewsModel = function (PTargetEditIndex) {
        if ($scope.curIndex < 1)
            return;
        //如果段落剩余一条，不允许删除
        if ($scope.article.ListNews.length == 1)
            return;
        //目标段落移出数组
        $scope.article.ListNews.splice(PTargetEditIndex, 1);

        $scope.SelectNewsModel(null, $scope.article.ListNews[0], 1);
    }
    $scope.ContentAsHTML = function () {
        var sHTML = $('#summernote').code();
        alert(sHTML);
    }
    $scope.EndEdit = function () {
        //响应父级监听
        $scope.$emit('EndEdit', 1);
    }
    $scope.SaveArticle = function () {
        try {
            if ($scope.curIndex === null || $scope.curIndex === undefined) {
                alert("當前沒有修改哦！");
                return;
            }
            if ($scope.article.lcClassify.indexOf("<") >= 0 || $scope.article.lcClassify.indexOf(">") >= 0) {
                alert("資源分類不允許<>符號！");
                return;
            }
            if ($scope.article.lcName.indexOf("<") >= 0 || $scope.article.lcName.indexOf(">") >= 0) {
                alert("資源名稱不允許<>符號！");
                return;
            }

            var sHTML = $('#summernote').code();
            $scope.article.ListNews[$scope.curIndex - 1].content = sHTML;

            var err = CheckInputForArticle();
            if (err == false)
                return;

            //请求前，激活按钮等待状态
            var $btn = $("#ui_btnLoading_SaveA").button('loading');
            var postData = $.toJSON($scope.article.ListNews);
            postData = postData.replace(/</g, "&lt;");
            postData = postData.replace(/>/g, "&gt;");
            $.ajax({
                url: '/WXResource/ArticleUpLoad',
                data: { "id": $scope.article.lcId, "name": $scope.article.lcName, "cla": $scope.article.lcClassify, "ListNews": postData },
                type: "post",
                dataType: "json",
                success: function (data) {
                    if (data.IsSuccess !== undefined && data.IsSuccess !== null && data.IsSuccess == false)
                        alert(data.Message);
                    else
                        alert("保存成功！");
                    //重置按钮
                    $btn.button('reset')
                }
            });
        }
        catch (ex) {
            alert("資源上傳異常，異常信息：" + ex.message);
        }
    }
    $scope.$on('SetSelectionForPicture', function (e, SelectModel) {
        $scope.curNewsModel.thumb_media_id = SelectModel.media_id;
        $scope.curNewsModel.RPath = SelectModel.RPath;
    });
    $scope.$watch('curNewsModel', function (newVal, oldVal, scope) {
        if (newVal !== oldVal) {
            var sHTML = $('#summernote').code();
            if (oldVal !== undefined && oldVal !== null) {
                if (oldVal.content != sHTML) {
                    oldVal.content = sHTML;
                }
            }

            if (newVal !== undefined && newVal !== null) {
                $('#summernote').code(newVal.content);
            }
        }

    });
    $scope.DisplayMode = function (pMode, pMediaID) {
        return pMode == 'A';
    }
    //保存前檢查輸入
    function CheckInputForArticle() {
        //获取Array数组长度
        var arrlength = $scope.article.ListNews.length;
        //遍历数组并读出对象
        for (var i = 0; i < arrlength; ++i) {
            var pNews = $scope.article.ListNews[i];
            if (pNews.title == "") {
                alert("第" + (i + 1) + "段落缺少標題，請填寫");
                return false;
            }
            else if (pNews.thumb_media_id == "") {
                alert("第" + (i + 1) + "段落缺少配圖，請選擇");
                return false;
            }
            else if (pNews.content == "") {
                alert("第" + (i + 1) + "段落缺少正文，請填寫");
                return false;
            }

        }
        return true;
    }
}]);
//視頻播放Controller
WXResController.controller('VideoPlayCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
    //****VideoJS設置父級事件響應******
    //$scope.$on('recall', function (e, video) {
    //    var mp = vjs("example_video_1");
    //    mp.src({ src: video.RPath, type: "video/mp4" });
    //    mp.play();
    //});
}]);
//音頻播放Controller
WXResController.controller('VoicePlayCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
}]);
//群发消息Controller
WXResController.controller('GroupSendCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
    var urlGroup = "/WXOrganization/GetWXGroups";
    //GroupSendCtrl控制器标识，用于子控制器获取父级控制表起
    $scope.parentSym = "XC";
    $scope.sendType = 1;
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
        $scope.curMediaID = SelectModel.media_id;
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
        else {
            if ($scope.curMediaID == undefined || $scope.curMediaID == null || $scope.curMediaID == "") {
                alert("請選擇發送內容");
                return;
            }
        }

        if ($scope.sendType != 2)
            $scope.curGroupId = "";
        //请求前，激活按钮等待状态
        var $btn = $("#ui_btnLoading_Cre1").button('loading');
        //创建群发消息，加入待审核队伍
        $.ajax({
            url: '/TemplateMessage/CreateGsMessage',
            data: { "mediaid": $scope.curMediaID, "sendtype": $scope.sendType, "sendtarget": $scope.curGroupId, "textcontent": $scope.text, "msgtype": $scope.msgType },
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.IsSuccess == false)
                    alert(data.Message);
                else
                    alert("创建成功");
                //重置按钮
                $btn.button('reset')
            }
        });
    }
    //选中资源，并弹出资源选择对话框
    $scope.SelectRes = function (pType, element) {
        if (element !== undefined && element !== null && element != '') {
            $('#' + element).modal({
                backdrop: false
            });
        }
        $scope.$broadcast('CallChildLoadRes', pType);
        $scope.msgType = pType;
    }
}]);
//资源管理Controller
WXResController.controller('ResourceManageCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
    $scope.article = null;
    $scope.picture = null;
    $scope.voice = null;
    $scope.video = null;
    $scope.text = null;
    $scope.curMediaID = null;
    $scope.curLcId = null;
    $scope.msgType = "text";
    $scope.curPageIndex = 1;
    var urlP = "/WXResource/LoadWXPictures";
    var urlA = "/WXResource/LoadWXArticles";
    var urlV = "/WXResource/LoadWXVideos";
    var urlVO = "/WXResource/LoadWXVoices";
    var urlCS = "/TemplateMessage/CSMessageSend";
    $http.post(urlP).success(function (response) {
        $scope.pictures = response.ListJson;
    });
    $http.post(urlA).success(function (response) {
        $scope.Articles = response.ListJson;
    });
    $http.post(urlVO).success(function (response) {
        $scope.voices = response.ListJson;
    });
    $http.post(urlV).success(function (response) {
        $scope.videos = response.ListJson;
    });
    $scope.showNewsModelFirstPic = function (Article) {
        return Article.ListNews[0].RPath;
    }
    $scope.SelectPicture = function (Picture) {
        $scope.picture = Picture;
        $scope.curMediaID = Picture.media_id;
        $scope.curLcId = Picture.lcId;
    }
    $scope.SelectVoice = function (Voice) {
        $scope.voice = Voice;
        $scope.curMediaID = Voice.media_id;
        $scope.curLcId = Voice.lcId;
    }
    $scope.SelectVideo = function (Video) {
        $scope.video = Video;
        $scope.curMediaID = Video.media_id;
        $scope.curLcId = Video.lcId;
    }
    $scope.SelectArticle = function (Article) {
        $scope.article = Article;
        $scope.curMediaID = Article.media_id;
        $scope.curLcId = Article.lcId;
    }
    $scope.SelectRes = function (pType, element) {
        $scope.msgType = pType;
        $(".ui_a_items").tooltip({ html: true });
    }
    $scope.SendServiceMessage = function () {
        var $btn = $("#ui_btnLoading_CSSend").button('loading');
        //发送群发消息post请求——以用户OpenID为范围发送
        $.ajax({
            url: urlCS,
            data: { "openId": $scope.curOpenID, "msgtype": $scope.msgType, "lcId": $scope.curLcId, "mediaId": $scope.curMediaID, "pTextContent": $scope.text },
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

    }
}]);
//群发消息Controller
WXResController.controller('GroupSendReviewCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
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
    var urlGs = "/TemplateMessage/GetGsMessage";
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
        //绑定模型
        if (CurGs.ContentType == 'text') {
            $scope.text = CurGs.SContent;
        }
        else if (CurGs.ContentType == 'image') {
            //angular.forEach(instance.pictures, function (item) {
            //    if (item.media_id == CurGs.SContent) {
            //        $scope.picture = item;
            //    }
            //});
            $scope.picture = CurGs.ResultJson;
        }
        else if (CurGs.ContentType == 'voice') {
            //angular.forEach(instance.voices, function (item) {
            //    if (item.media_id == CurGs.SContent) {
            //        $scope.voice = item;
            //    }
            //});
            $scope.voice = CurGs.ResultJson;
        }
        else if (CurGs.ContentType == 'video') {
            //angular.forEach(instance.videos, function (item) {
            //    if (item.media_id == CurGs.SContent) {
            //        $scope.video = item;
            //    }
            //});
            $scope.video = CurGs.ResultJson;
        }
        else if (CurGs.ContentType == 'mpnews') {
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
        if ($scope.CurGSMessage.SType == 1) {
            //发送群发消息post请求——以用户OpenID为范围发送
            $.ajax({
                url: '/TemplateMessage/GroupSendingByOpenID',
                data: { "messageid": $scope.CurGSMessage.ID, "mediaid": $scope.curMediaID, "textcontent": $scope.text, "msgtype": $scope.CurCType, "sex": sex },
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
        }
        else {
            //发送群发消息post请求——以分组ID为范围发送
            $.ajax({
                url: '/TemplateMessage/GroupSendingByGroupID',
                data: { "messageid": $scope.CurGSMessage.ID, "mediaid": $scope.curMediaID, "textcontent": $scope.text, "msgtype": $scope.CurCType, "groupid": $scope.CurGSMessage.STarget },
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
        }
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
            url: '/TemplateMessage/GetGsMessageByFilter',
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


//过滤器——截取数组，用于table分页
WXResController.filter('filterSubArray', function () {
    return function (items, pPageIndex) {
        try {
            //每页行数
            var pResultLength = 15;
            var iCount = items.length;
            var iStartIndex = pResultLength * (pPageIndex - 1);

            var newItems = new Array();

            for (var i = 1; i <= pResultLength; i++) {
                if (iStartIndex <= iCount - 1) {
                    var citem = items[iStartIndex];
                    newItems.push(citem);
                }
                iStartIndex += 1;
            }
            return newItems;
        }
        catch (ex) { }
    }
});
//过滤器——过滤集合索引等于目标值
WXResController.filter('filterT1', function () {
    return function (items, inx) {
        var newItems = new Array();
        var i = 0;
        angular.forEach(items, function (item) {
            if (i == inx) {
                newItems.push(item);
            }
            i += 1;
        });
        return newItems;
    }
});
//过滤器——过滤集合索引不等于目标值
WXResController.filter('filterT2', function () {
    return function (items, inx) {
        var newItems = new Array();
        var i = 0;
        angular.forEach(items, function (item) {
            if (i != inx) {
                newItems.push(item);
            }
            i += 1;
        });
        return newItems;
    }
});
//过滤器——过滤集合索引等于目标值
WXResController.filter('filterMediaID', function () {
    return function (items, mediaID) {
        var newItems = new Array();
        angular.forEach(items, function (item) {
            if (item.media_id == mediaID) {
                newItems.push(item);
            }
        });
        return newItems;
    }
});

//检查用户上传文件
function lastname(filepath, tp) {
    //获取欲上传的文件路径
    //var filepath = document.getElementById("file1").value;
    //为了避免转义反斜杠出问题，这里将对其进行转换
    var re = /(\\+)/g;
    var filename = filepath.replace(re, "#");
    //对路径字符串进行剪切截取
    var one = filename.split("#");
    //获取数组中最后一个，即文件名
    var two = one[one.length - 1];
    //再对文件名进行截取，以取得后缀名
    var three = two.split(".");
    //获取截取的最后一个字符串，即为后缀名
    var last = three[three.length - 1];
    //添加需要判断的后缀名类型
    //var tp = "jpg,gif,bmp,png,JPG,GIF,BMP,PNG";
    //返回符合条件的后缀名在字符串中的位置
    var rs = tp.indexOf(last);
    //如果返回的结果大于或等于0，说明包含允许上传的文件类型
    if (rs >= 0) {
        return true;
    } else {
        alert("您选择的上传文件不是有效的图片文件！");
        return false;
    }
}

function CreateArticleModel(mediaID) {
    var pModel = Object.create(ArticlesModel);
    pModel.media_id = mediaID;
    pModel.ListNews = null;
    return pModel;
}
//转换消息内容类型
function ConvertContentType(ContentType) {
    if (ContentType == "text") {
        return "文本";
    }
    else if (ContentType == "image") {
        return "圖片";
    }
    else if (ContentType == "voice") {
        return "音頻";
    }
    else if (ContentType == "video") {
        return "視頻";
    }
    else if (ContentType == "mpnews") {
        return "圖文";
    }
    return "";
}
//转换发送状态
function ConvertSState(pState) {
    if (pState == 1) {
        return "待審核";
    }
    else if (pState == 3) {
        return "已發送";
    }
    return "";
}
//创建模型
function CreatePicModel(path, mediaID, FileName) {
    var pPics = Object.create(PictureModel);
    pPics.media_id = mediaID;
    pPics.FileName = FileName;
    pPics.RPath = path;
    return pPics;
}
//微信图片资源模型
var NewsModel = {
    author: "",
    content: "",
    content_source_url: "",
    digest: "",
    thumb_media_id: "",
    title: "",
    RPath: "",
};
//微信图片资源模型
var ArticlesModel = {
    lcId: 0,
    lcName: "",
    lcClassify: "",
    media_id: "",
    ListNews: null,
};
//微信图片资源模型
var PictureModel = {
    media_id: "",
    FileName: "",
    RPath: "",
};

