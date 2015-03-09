﻿//指令
var WXDirective = angular.module('WXDirective', []);
//自定义标签——发送模板消息HTML
WXDirective.directive("temmessagesend", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/TemMessageSend.html'
    }
});
//自定义标签——系统默认导航ztree控件
WXDirective.directive('tree', function () {
    return {
        require: '?ngModel',
        restrict: 'A',
        link: function ($scope, element, attrs, ngModel) {
            var uiId = 'ui_ztree';
            //var opts = angular.extend({}, $scope.$eval(attrs.nlUploadify));
            var setting = {
                view: {
                    dblClickExpand: false,
                    showLine: false,
                    expandSpeed: ($.browser.msie && parseInt($.browser.version) <= 6) ? "" : "fast"
                },
                data: {
                    key: {
                        name: "resourceName"
                    },
                    simpleData: {
                        enable: true,
                        idKey: "resourceID",
                        pIdKey: "parentID",
                        rootPId: ""
                    }
                },

                callback: {
                    onClick: function (event, treeId, treeNode, clickFlag) {
                        $scope.$apply(function () {
                            ngModel.$setViewValue(treeNode);
                        });
                        //重新设置当前文档的URL，促使进入路由系统
                        window.location.href = "#" + treeNode.accessPath;

                    }
                }
            };
            //
            $.post("/Menu/GetMenu", null, function (data) {
                // 如果返回数据不为空，加载"业务模块"目录
                if (data != null) {
                    // 将返回的数据赋给zTree
                    $.fn.zTree.init(element, setting, data);
                    //              alert(treeObj);
                    zTree = $.fn.zTree.getZTreeObj(uiId);
                    if (zTree) {
                        // 默认展开所有节点
                        zTree.expandAll(true);
                    }
                }
            });
            //$.fn.zTree.init(element, setting, zNodes);
        }
    };
});
//自定义标签——拖放上传圖片控件dropzone
WXDirective.directive('dropzonex', function () {
    return {
        restrict: 'A',
        link: function (scope, el, attrs) {
            el.dropzone({
                //url: attrs.url,
                //maxFilesize: attrs.maxsize,
                //init: function () {
                //    scope.files.push({ file: 'added' }); // here works
                //    this.on('success', function (file, json) {
                //    });
                //    this.on('addedfile', function (file) {
                //        scope.files.push({ file: 'added' }); // here doesn't work
                //    });
                //}
                //初始化
                init: function () {
                    var submitButton = document.querySelector("#submit-all")

                    myDropzone = this; // closure

                    //为上传按钮添加点击事件
                    submitButton.addEventListener("click", function () {
                        //手动上传所有图片
                        myDropzone.processQueue();
                    });

                    //当添加图片后的事件，上传按钮恢复可用
                    this.on("addedfile", function () {
                        $("#submit-all").removeAttr("disabled");
                    });

                    //当上传完成后的事件，接受的数据为JSON格式
                    this.on("complete", function (data) {
                        if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                            //
                            try {
                                var res = eval('(' + data.xhr.responseText + ')');
                                var msg;
                                if (res.IsSuccess) {
                                    var arrlength = res.ListJson.length;
                                    //遍历数组并读出对象
                                    for (var i = 0; i < arrlength; ++i) {
                                        var pNews = res.ListJson[i];
                                        if (pNews !== null && pNews !== undefined)
                                            scope.pictures.push(pNews);
                                    }

                                    scope.$apply();
                                    msg = "恭喜，已成功上传" + res.ListJson.length + "张照片！";
                                }
                                else {
                                    msg = "上传失败，失败的原因是：" + res.Message;
                                }
                                alert(msg);
                            }
                            catch (exception)
                            { }
                            //$("#message").text(msg);
                            //$("#dialog").dialog("open");
                        }
                        //var res = JSON.parse(data.xhr.responseText);
                    });

                    //删除图片的事件，当上传的图片为空时，使上传按钮不可用状态
                    this.on("removedfile", function () {
                        if (this.getAcceptedFiles().length === 0) {
                            $("#submit-all").attr("disabled", true);
                        }
                    });
                },
                //验证
                accept: function (file, done) {
                    if (lastname(file.name, "jpg,gif,bmp,png,JPG,GIF,BMP,PNG")) {
                        done();
                    }
                    else {
                        done("请上传有效图片文件");
                    }
                },
                // The name that will be used to transfer the file
                paramName: "file",
                //添加上传取消和删除预览图片的链接，默认不添加
                addRemoveLinks: true,
                //关闭自动上传功能，默认会true会自动上传
                //也就是添加一张图片向服务器发送一次请求
                autoProcessQueue: false,
                //允许上传多个照片
                uploadMultiple: true,
                //最多支持上传数量
                maxFiles: 2,
                //最大文件大小
                maxFilesize: 2, // MB
                //反馈消息
                dictFileTooBig: "图片太大了，请小于2m",
                dictResponseError: "服务器连接通讯错误哦",
                dictMaxFilesExceeded: "最多一次传输2个文件"
            })
        }
    }
});
//自定义标签——拖放上传視頻控件dropzone
WXDirective.directive('dropzonexvideo', function () {
    return {
        restrict: 'A',
        link: function (scope, el, attrs) {
            el.dropzone({
                url: scope.urlVpl,
                //初始化
                init: function () {
                    var submitButton = document.querySelector("#submit-all")

                    myDropzone = this; // closure

                    //为上传按钮添加点击事件
                    submitButton.addEventListener("click", function () {
                        myDropzone.options.url = scope.urlVpl + "?lcname=" + scope.ipLcName + "&lcclassify=" + scope.ipLcClassify;
                        //手动上传所有文件
                        myDropzone.processQueue();
                    });

                    //当添加文件后的事件，上传按钮恢复可用
                    this.on("addedfile", function () {
                        $("#submit-all").removeAttr("disabled");
                    });

                    //当上传完成后的事件，接受的数据为JSON格式
                    this.on("complete", function (data) {
                        if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                            //
                            try {
                                var res = eval('(' + data.xhr.responseText + ')');
                                var msg;
                                if (res.IsSuccess) {
                                    var arrlength = res.ListJson.length;

                                    if (scope.videos !== null && scope.videos !== undefined) {
                                        //遍历数组并读出对象
                                        for (var i = 0; i < arrlength; ++i) {
                                            var pNews = res.ListJson[i];
                                            if (pNews !== null && pNews !== undefined)
                                                scope.videos.push(pNews);
                                        }
                                        scope.$apply();
                                    }
                                    msg = "恭喜，已成功上传" + res.ListJson.length + "段視頻！";
                                }
                                else {
                                    msg = "上传失败，失败的原因是：" + res.Message;
                                }
                                alert(msg);
                            }
                            catch (exception)
                            { }
                            //$("#message").text(msg);
                            //$("#dialog").dialog("open");
                        }
                        //var res = JSON.parse(data.xhr.responseText);
                    });

                    //删除文件的事件，当上传的文件为空时，使上传按钮不可用状态
                    this.on("removedfile", function () {
                        if (this.getAcceptedFiles().length === 0) {
                            $("#submit-all").attr("disabled", true);
                        }
                    });
                },
                //验证
                accept: function (file, done) {
                    if (lastname(file.name, "mp4,MP4")) {
                        done();
                    }
                    else {
                        done("目前僅支持MP4視頻格式");
                    }
                },
                // The name that will be used to transfer the file
                paramName: "file",
                //添加上传取消和删除预览文件的链接，默认不添加
                addRemoveLinks: true,
                //关闭自动上传功能，默认会true会自动上传
                //也就是添加一张文件向服务器发送一次请求
                autoProcessQueue: false,
                //允许上传多个文件
                uploadMultiple: true,
                //最多支持上传数量
                maxFiles: 1,
                //最大文件大小
                maxFilesize: 10, // MB
                //反馈消息
                dictFileTooBig: "視頻太大了，请小于10m",
                dictResponseError: "服务器连接通讯错误哦",
                dictMaxFilesExceeded: "最多一次传输2个文件"
            })
        }
    }
});
//自定义标签——拖放上传音頻控件dropzone
WXDirective.directive('dropzonexvoice', function () {
    return {
        restrict: 'A',
        link: function (scope, el, attrs) {
            el.dropzone({
                url: scope.urlVopl,
                //初始化
                init: function () {
                    var submitButton = document.querySelector("#submit-all")

                    myDropzone = this; // closure

                    //为上传按钮添加点击事件
                    submitButton.addEventListener("click", function () {
                        myDropzone.options.url = scope.urlVopl + "?lcname=" + scope.ipLcName + "&lcclassify=" + scope.ipLcClassify;
                        //手动上传所有文件
                        myDropzone.processQueue();
                    });

                    //当添加文件后的事件，上传按钮恢复可用
                    this.on("addedfile", function () {
                        $("#submit-all").removeAttr("disabled");
                    });

                    //当上传完成后的事件，接受的数据为JSON格式
                    this.on("complete", function (data) {
                        if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                            //
                            try {
                                var res = eval('(' + data.xhr.responseText + ')');
                                var msg;
                                if (res.IsSuccess) {
                                    var arrlength = res.ListJson.length;

                                    if (scope.voices !== null && scope.voices !== undefined) {
                                        //遍历数组并读出对象
                                        for (var i = 0; i < arrlength; ++i) {
                                            var pNews = res.ListJson[i];
                                            if (pNews !== null && pNews !== undefined)
                                                scope.voices.push(pNews);
                                        }
                                        scope.$apply();
                                    }
                                    msg = "恭喜，已成功上传" + res.ListJson.length + "段音頻！";
                                }
                                else {
                                    msg = "上传失败，失败的原因是：" + res.Message;
                                }
                                alert(msg);
                            }
                            catch (exception)
                            { }
                            //$("#message").text(msg);
                            //$("#dialog").dialog("open");
                        }
                        //var res = JSON.parse(data.xhr.responseText);
                    });

                    //删除文件的事件，当上传的文件为空时，使上传按钮不可用状态
                    this.on("removedfile", function () {
                        if (this.getAcceptedFiles().length === 0) {
                            $("#submit-all").attr("disabled", true);
                        }
                    });
                },
                //验证
                accept: function (file, done) {
                    if (lastname(file.name, "mp3,wma,wav,amr,AMR,WAV,WMA,MP3")) {
                        done();
                    }
                    else {
                        done("目前支持mp3, wma, wav, amr音頻格式");
                    }
                },
                // The name that will be used to transfer the file
                paramName: "file",
                //添加上传取消和删除预览文件的链接，默认不添加
                addRemoveLinks: true,
                //关闭自动上传功能，默认会true会自动上传
                //也就是添加一张文件向服务器发送一次请求
                autoProcessQueue: false,
                //允许上传多个文件
                uploadMultiple: true,
                //最多支持上传数量
                maxFiles: 2,
                //最大文件大小
                maxFilesize: 5, // MB
                //反馈消息
                dictFileTooBig: "音頻大小: 不超过5M,長度 : 不超過60秒",
                dictResponseError: "服务器连接通讯错误哦",
                dictMaxFilesExceeded: "最多一次传输2个文件"
            })
        }
    }
});
//自定义标签——wxarticleedit图文编辑
WXDirective.directive('wxarticleedit', function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/Page/WXArticleEdit.html'
    }
});
//自定义标签——wxarticles图文列表
WXDirective.directive("wxarticles", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/WXArticles.html'
    }
});
//自定义标签——wxarticles图片列表
WXDirective.directive("wxpictures", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/WXPictures.html'
    }
});
//自定义标签——wxvideos視頻列表
WXDirective.directive("wxvideos", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/WXVideos.html'
    }
});
//自定义标签——wxvoices音頻列表
WXDirective.directive("wxvoices", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/WXVoices.html'
    }
});
//自定义标签——wxarticlesconfig图文列表
WXDirective.directive("wxarticlesconfig", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/Page/WXArticlesConfig.html'
    }
});
//自定义标签——wxvideos視頻列表
WXDirective.directive("wxvideosconfig", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/Page/WXVideosConfig.html'
    }
});
//自定义标签——wxvideos音頻列表
WXDirective.directive("wxvoicesconfig", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/Page/WXVoicesConfig.html'
    }
});
//自定义标签——wxarticles图片预览
WXDirective.directive("wxpicturedisplay", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/WXPictureDisplay.html'
    }
});
//自定义标签——wxarticledisplay图文预览
WXDirective.directive("wxarticledisplay", function () {
    return {
        /*restrict的取值范围:
         *E - 表示创建的是一个HTML标签: <custom-tag></custom-tag>
         *A - 为HTML标签创建属性: <div my-directive="exp"> </div>
         *C - 为HTML标签创建类: <div class="my-directive: exp;"></div>
         *M - 创建HTML注释: <!-- directive: my-directive exp -->
        */
        restrict: "E",
        /*replace为true时模块则覆盖标签，比如模块是:<div></div>，
         *则<customTag></customTag>最终解释为<div></div>
        */
        replace: true,
        /*模块，即把<customTag>映射成最终什么样的HTML代码*/
        templateUrl: '/Scripts/Views/WXArticleDisplay.html'
    }
});
//自定义标签——wxvideoplay視頻预览
WXDirective.directive("wxvideoplay", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/WXVideoPlay.html'
    }
});
//自定义标签——wxvoiceplay視頻预览
WXDirective.directive("wxvoiceplay", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/WXVoicePlay.html'
    }
});
//自定义标签——resourcemanage资源管理
WXDirective.directive("resourcemanage", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/ResourceManage.html'
    }
});
//自定义标签——resourcemanage资源管理
WXDirective.directive("message", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/Page/Message.html'
    }
});
//自定义标签——groupsend资源管理
WXDirective.directive("groupsend", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/Page/GroupSend.html'
    }
});
//自定义标签——groupsendreview资源管理
WXDirective.directive("groupsendreview", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/Page/GroupSendReview.html'
    }
});
//自定义标签——pagesuc分頁控件
WXDirective.directive("pagesuc", function () {
    return {
        restrict: "E",
        replace: true,
        templateUrl: '/Scripts/Views/PagesUC.html'
    }
});
