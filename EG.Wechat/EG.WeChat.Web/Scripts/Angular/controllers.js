
var mainApp = angular.module("mainApp", []);

mainApp.factory('instance', function () {
    return {};
});

mainApp.controller("PictureListCtrl", function ($scope, instance) {
    //$scope.reset = function () {
    Dropzone.options.dropzoneForm = {
        //init: function () {
        //    this.on("complete", function (data) {
        //        //var res = eval('(' + data.xhr.responseText + ')');
        //        var res = JSON.parse(data.xhr.responseText);
        //    });

        //},
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
                    //$("#testbtn").click();
                    //AddJx(data.xhr.responseText)

                    //var      data.FilePath

                    var res = eval('(' + data.xhr.responseText + ')');
                    //AddJx("Images/WallImages/" + res.FileName);
                    //dd("Images/WallImages/" + res.FileName);
                    //$("#targC").attr("value", "Images/WallImages/" + res.FileName);
                    //$("#targC").val("Images/WallImages/" + res.FileName);
                    //$("#testbtn").click();
                    //$scope.newPath = "Images/WallImages/" + res.FileName;
                    //$scope.addPic("Images/WallImages/" + res.FileName);

                    var _array = $scope.pictures;
                    var list2 = CreateModel2("/Images/WallImages/imagepath/" + res.FileName);
                    _array.push(list2);
                    $scope.pictures = _array;
                    $scope.$apply();


                    var msg;
                    if (res.Result) {
                        msg = "恭喜，已成功上传" + res.Count + "张照片！";
                    }
                    else {
                        msg = "上传失败，失败的原因是：" + res.Message;
                    }
                    //$("#message").text(msg);
                    //$("#dialog").dialog("open");
                }

                var res = JSON.parse(data.xhr.responseText);
            });

            //删除图片的事件，当上传的图片为空时，使上传按钮不可用状态
            this.on("removedfile", function () {
                if (this.getAcceptedFiles().length === 0) {
                    $("#submit-all").attr("disabled", true);
                }
            });
        },
        paramName: "file", // The name that will be used to transfer the file
        accept: function (file, done) {
            if (lastname(file.name)) {
                done();
            }
            else {
                done("请上传有效图片文件");
            }
        },
        //添加上传取消和删除预览图片的链接，默认不添加
        addRemoveLinks: true,
        //关闭自动上传功能，默认会true会自动上传
        //也就是添加一张图片向服务器发送一次请求
        autoProcessQueue: false,
        //允许上传多个照片
        uploadMultiple: true,
        //最多支持上传数量
        maxFiles: 4,
        //最大文件大小
        maxFilesize: 2, // MB
        //反馈消息
        dictFileTooBig: "图片太大了，请小于2m",
        dictResponseError: "服务器连接通讯错误哦",
        dictMaxFilesExceeded: "最多一次传输4个文件"

    };
    //}


    var postData = CreateModelArray();
    $scope.pictures = postData;
    $scope.addPic = function (path) {
        var _array = $scope.pictures;
        var list2 = CreateModel2(path);
        _array.push(list2);
        $scope.pictures = _array;
        //$scope.$apply();
    }
    $scope.newPath = "";
    $scope.add = function () {
        var _array = $scope.pictures;
        var list2 = CreateModel2($scope.newPath);
        _array.push(list2);
        $scope.pictures = _array;
    }



});

//app.controller('sideCtrl', function ($scope, instance) {
//    $scope.change = function () {
//        instance.path = $scope.path;
//    };
//});

function dd(path) {
    $scope.
    instance.path = path;
}

//function PictureListCtrl($scope) {
//    var postData = CreateModelArray();
//    $scope.pictures = postData;
//    $scope.addPic = function () {
//        var _array = $scope.pictures;
//        var list2 = CreateModel2();
//        _array.push(list2);
//        $scope.pictures = _array;
//        //$scope.$apply();
//    }
//}

function AddJx(path) {
    var dd = PictureListCtrl();
    dd.addPic(path);
    PictureListCtrl.addPic(path);
}

function lastname(filepath) {
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
    var tp = "jpg,gif,bmp,JPG,GIF,BMP";
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

function CreateModelArray() {

    var model1 = CreateModel();
    //var model2 = CreateModel2();
    //全局模型数据
    var _arrayNews = new Array();
    _arrayNews.push(model1);
    //_arrayNews.push(model2);

    //var postData = $.toJSON(_arrayNews);
    return _arrayNews;
    //_arrayNews.push(pNews);
}

function CreateModelArray2() {

    var model1 = CreateModel();
    var model2 = CreateModel2(path);
    //全局模型数据
    var _arrayNews = new Array();
    _arrayNews.push(model1);
    _arrayNews.push(model2);

    //var postData = $.toJSON(_arrayNews);
    return _arrayNews;
    //_arrayNews.push(pNews);
}

function CreateModel2(path) {
    var pPics = Object.create(PictureModel);
    pPics.name = "Rx100";
    pPics.snippet = "rx100 sony";
    //pPics.path = "/Images/WallImages/imagepath/316422.jpg";
    pPics.path = path
    return pPics;
}

function CreateModel() {
    var pPics = Object.create(PictureModel);
    pPics.name = "Motorola XOOM™ with Wi-Fi";
    pPics.snippet = "The Next, Next Generation tablet.";
    pPics.path = "/Images/WallImages/imagepath/316422.jpg";
    return pPics;
}

//微信图文段落模型
var PictureModel = {
    name: "",
    snippet: "",
    path: "",
};

