/* 微信提供WeixinJSBridge浏览器对象，允许通过JS的调用，来实现某些功能。
   接口文档，请参考    http://mp.weixin.qq.com/wiki/index.php  的“Weixin JS接口”节点。
*/


/* 【使用帮助】
   0.在【VS环境】阅读的时候此文件的时候，
     建议右键=>OutLining=>Collapse to Definitions，将代码全部折叠起来。
   1.在需要此实例支持的Page，添加此JS脚本文件的引用，
     比如： <script src="@Url.Content("~/Scripts/WeiXinJS.js")"></script>
   2.直接调用单件实例的方法。
     比如：WeiXinJSInstance.CloseWindow();
*/


/* -------------WeiXinJS类的定义----------------- */
var WeiXinJSClass = function () {

    /* 将目标事件，绑定到Ready事件 */
    this._appendHanderToReadyEvent = function (targetFunction) {
        if (typeof WeixinJSBridge == "undefined") {
            if (document.addEventListener) {
                document.addEventListener('WeixinJSBridgeReady', targetFunction, false);
            } else if (document.attachEvent) {
                document.attachEvent('WeixinJSBridgeReady', targetFunction);
                document.attachEvent('onWeixinJSBridgeReady', targetFunction);
            }
        } else {
            targetFunction();
        };
    };

    /* 显示/隐藏 微信中网页右上角按钮。*/
    this._showOrHideOptionMenu = function (bool_ShowOrHide) {
        //## 根据bool_ShowOrHide，指定是 显示/隐藏 的命令。
        var cmd;
        if (bool_ShowOrHide == true) {
            cmd = 'showOptionMenu';
        }
        else {
            cmd = 'hideOptionMenu';
        }
        
        //##调用命令
        WeixinJSBridge.call(cmd);
    };

    /* 显示/隐藏 微信中网页底部导航栏。 */
    this._showOrHideToolbar = function (bool_ShowOrHide) {
        //## 根据bool_ShowOrHide，指定是 显示/隐藏 的命令。
        var cmd;
        if (bool_ShowOrHide == true) {
            cmd = 'showToolbar';
        }
        else {
            cmd = 'hideToolbar';
        }

        //##调用命令
        WeixinJSBridge.call(cmd);
    };

    /* 关闭浏览器窗体，返回微信客户端会话界面  */
    this._closeWindow = function () {
        if (typeof WeixinJSBridge != "undefined") {
            WeixinJSBridge.invoke('closeWindow', {}, function (res) {
            });
        }
    };

    /* 获取用户网络状态 */
    this._getNetworkType = function (callbackFunction) {
        WeixinJSBridge.invoke('getNetworkType', {},function (e) {
            var result = e.err_msg;   //这个e.err_msg为接口的结果。很好奇，为何名字叫err_msg。

            //##使用正则表达式判断格式，然后拆分数据
            var regex = new RegExp("^network_type:\\w+");
            if (result.match(regex)) {
                callbackFunction(result.split(":")[1]);
            }
            else {
                callbackFunction("unknown");
            }
       });
    }

};
/* ---------------------------------------------*/


/* ----------------绑定到公有方法---------------- */

/* 显示/隐藏 微信中网页右上角按钮。*/
WeiXinJSClass.prototype.ShowOrHideOptionMenu = function (bool_ShowOrHide) {     //@bool_ShowOrHide True为显示,False为隐藏   @return不关注
    var instance = this;
    this._appendHanderToReadyEvent(function () {
        instance._showOrHideOptionMenu(bool_ShowOrHide);
    });
};

/* 显示/隐藏 微信中网页底部导航栏。 */
WeiXinJSClass.prototype.ShowOrHideToolbar   = function (bool_ShowOrHide) {      //@bool_ShowOrHide True为显示,False为隐藏   @return不关注
    var instance = this;
    this._appendHanderToReadyEvent(function () {
        instance._showOrHideToolbar(bool_ShowOrHide);
    });
};

/* 关闭浏览器窗体，返回微信客户端会话界面  */
WeiXinJSClass.prototype.CloseWindow         = function () {                     //@return不关注
    this._closeWindow();
};

/* 获取用户网络状态 */
WeiXinJSClass.prototype.GetNetworkType      = function (callbackFunction) {     //@@callbackFunction 带一个参数的Function;结果将会通过参数传递回去。
                                                                                //结果枚举(string，全部小写)：
                                                                                //wifi    wifi网络
                                                                                //edge    非wifi,包含3G/2G
                                                                                //fail    网络断开连接
                                                                                //wwan   （2g或者3g）
                                                                                //unknown 未知
    var instance = this;
    this._appendHanderToReadyEvent(function () {
        instance._getNetworkType(callbackFunction);
    });

    /* 调用范例：
        var callback = function (result) {
            alert("获取到的网络状态为：\r\n" + result);
        };
        WeiXinJSInstance.GetNetworkType(callback);
    */
};

/* ---------------------------------------------- */


/* ----------------单件模式的对象----------------- */
var WeiXinJSInstance = new WeiXinJSClass();
/* ---------------------------------------------- */

