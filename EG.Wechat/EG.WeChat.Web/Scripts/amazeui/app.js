(function ($) {
    'use strict';

})(jQuery);

//滑动至底部
function ScrollToBottom() {
    var $w = $(window);
    //$w.smoothScroll({ position: $(document).height() - $w.height() });
    $w.smoothScroll({ position: $(document).height()  });
}

//生成分区段落
function loadAccordion2(uiIndex, pData) {
    var template = Handlebars.compile('{{>accordion}}'),
    data = {
        accordionData: pData
    },
    html = template(data.accordionData);

    $('#div' + uiIndex).append(html);
}

//初始化自动轮播图片
function loadSlider2(uiIndex, pData) {
    var template = Handlebars.compile('{{>slider}}'),
    data = {
        accordionData: pData
    }
    html = template(data.accordionData);

    $('#div' + uiIndex).append(html);
}

//初始化底部导航条
function loadNavbar() {

    var template = Handlebars.compile('{{>navbar}}'),
   data = {
       accordionData: {
           "theme": "one",
           "options": {
               "cols": "4",
               "iconpos": "top"
           },
           "content": [
             {
                 "title": "立即領取",
                 "link": "tel:123456789",
                 "icon": "download",
                 "dataApi": "ScrollToBottom()"
             },
             {
                 "title": "一鍵分享",
                 "link": "",
                 "icon": "share-square-o",
                 "dataApi": "data-am-navbar-share"
             },
             {
                 "title": "二維碼",
                 "link": "",
                 "icon": "qrcode",
                 "dataApi": "data-am-navbar-qrcode = Images/qrcode.jpg"
             },
             {
                 "title": "關於我們",
                 "link": "http://webchat.cloudapp.net/",
                 "icon": "user-md",
                 "dataApi": ""
             }
           ]
       }
   },
   html = template(data.accordionData);

    $('body').append(html);
}
//初始化所有AUI模块
function loadUI() {
    $.each(['slider', 'menu', 'gallery', 'footer', 'navbar', 'accordion'], function (i, m) {
        var module = $.AMUI[m];
        module && module.init && module.init();
    })
}




/******************类定义**********************/
var CardInfo = {
    Tile: '',
    CardImgPath: '',
    Content1: '',
    Content2: '',
    Content3: '',
    makeSound: function () {
        alert("ok");
    }
};
