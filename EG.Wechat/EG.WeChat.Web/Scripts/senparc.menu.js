var senparc = {};
var maxSubMenuCount = 5;
var menuState;
senparc.menu = {
    token: '',
    init: function () {
        menuState = $('#menuState');

        $('#buttonDetails').hide();
        $('#menuEditor').hide();

        $("#buttonDetails_type").change(senparc.menu.typeChanged);

        $(':input[id^=menu_button]').click(function () {
            $('#buttonDetails').show();
            var idPrefix = $(this).attr('data-root')
                            ? ('menu_button' + $(this).attr('data-root'))
                            : ('menu_button' + $(this).attr('data-j') + '_sub_button' + $(this).attr('data-i'));

            var keyId = idPrefix + "_key";
            var nameId = idPrefix + "_name";
            var typeId = idPrefix + "_type";
            var urlId = idPrefix + "_url";
            var ClickThenResponseId = idPrefix + "_ClickThenResponse";

            var txtDetailsKey = $('#buttonDetails_key');
            var txtDetailsName = $('#buttonDetails_name');
            var ddlDetailsType = $('#buttonDetails_type');
            var txtDetailsUrl = $('#buttonDetails_url');
            var txtDetailsClickThenResponse = $('#buttonDetails_ClickThenResponse');

            var hiddenButtonKey = $('#' + keyId);
            var hiddenButtonType = $('#' + typeId);
            var hiddenButtonUrl = $('#' + urlId);
            var hiddenClickThenResponse = $('#' + ClickThenResponseId);

            txtDetailsKey.val(hiddenButtonKey.val());
            txtDetailsName.val($('#' + nameId).val());
            ddlDetailsType.val(hiddenButtonType.val());
            txtDetailsUrl.val(hiddenButtonUrl.val());
            txtDetailsClickThenResponse.val(hiddenClickThenResponse.val());

            var regex = new RegExp("^JumpToResponseChain.+");
            if (hiddenButtonType.val() == "click" && hiddenClickThenResponse.val().toString().match(regex) != null) {
                txtDetailsClickThenResponse.val((hiddenClickThenResponse.val().replace("JumpToResponseChain[", "")
                                                                      .replace("]", "")));
                ddlDetailsType.val("ClickThenResponse");
            }
            senparc.menu.LoadedChangeType();

            txtDetailsKey.unbind('blur').blur(function () {
                hiddenButtonKey.val($(this).val());
            });
            ddlDetailsType.unbind('blur').blur(function () {
                var ret = $(this).val();
                if (ret == "ClickThenResponse") {
                    hiddenButtonType.val("click");
                }
                else {
                    hiddenButtonType.val($(this).val());
                }
            });
            txtDetailsUrl.unbind('blur').blur(function () {
                hiddenButtonUrl.val($(this).val());
            });
            txtDetailsClickThenResponse.unbind('blur').blur(function () {
                //hiddenClickThenResponse.val($(this).val());
                hiddenButtonKey.val("JumpToResponseChain[" + $(this).val() + "]");
            });
        });

        $('#menuLogin_Submit').click(function () {
            $.getJSON('/WXMenu/GetToken?t=' + Math.random(), { appId: $('#menuLogin_AppId').val(), appSecret: $('#menuLogin_AppSecret').val() },
                function (json) {
                    if (json.access_token) {
                        senparc.menu.setToken(json.access_token);
                    } else {
                        alert(json.error || '執行過程有錯誤，請檢查！');
                    }
                });
        });

        $('#menuLogin_SubmitOldToken').click(function () {
            senparc.menu.setToken($('#menuLogin_OldToken').val());
        });

        $('#btnGetMenu').click(function () {
            menuState.html('獲取菜單中...');
            $.getJSON('/WXMenu/GetMenu?t=' + Math.random(), { token: senparc.menu.token }, function (json) {
                if (json.menu) {
                    $(':input[id^=menu_button]:not([id$=_type])').val('');
                    $('#buttonDetails:input').val('');

                    var buttons = json.menu.button;
                    //此处i与j和页面中反转
                    for (var i = 0; i < buttons.length; i++) {
                        var button = buttons[i];
                        $('#menu_button' + i + '_name').val(button.name);
                        $('#menu_button' + i + '_key').val(button.key);
                        $('#menu_button' + i + '_type').val(button.type || 'click');
                        $('#menu_button' + i + '_url').val(button.url);
                        $('#menu_button' + i + '_ClickThenResponse').val(button.key);

                        if (button.sub_button && button.sub_button.length > 0) {
                            //二级菜单
                            for (var j = 0; j < button.sub_button.length; j++) {
                                var subButton = button.sub_button[j];
                                var idPrefix = '#menu_button' + i + '_sub_button' + j;
                                $(idPrefix + "_name").val(subButton.name);
                                $(idPrefix + "_type").val(subButton.type || 'click');
                                $(idPrefix + "_key").val(subButton.key);
                                $(idPrefix + "_url").val(subButton.url);
                                $(idPrefix + "_ClickThenResponse").val(subButton.key);
                            }
                        } else {
                            //底部菜单
                            //...
                        }
                    }
                    menuState.html('已完成');
                } else {
                    menuState.html(json.error || '執行過程有錯誤，請檢查！');
                }
            });
        });

        $('#btnDeleteMenu').click(function () {
            if (!confirm('確定要刪除菜單嗎？此操作無法撤銷！')) {
                return;
            }

            menuState.html('刪除菜單中...');
            $.getJSON('/WXMenu/DeleteMenu?t=' + Math.random(), { token: senparc.menu.token }, function (json) {
                if (json.Success) {
                    menuState.html('刪除成功，如果是誤刪，並且介面上有最新的功能表狀態，可以立即點擊【更新到伺服器】按鈕。');
                } else {
                    menuState.html(json.Message);
                }
            });
        });

        $('#submitMenu').click(function () {
            if (!confirm('確定要提交嗎？此操作無法撤銷！')) {
                return;
            }

            menuState.html('上傳中...');

            $('#form_Menu').ajaxSubmit({
                dataType: 'json',
                success: function (json) {
                    if (json.Successed) {
                        menuState.html('上傳成功');
                    } else {
                        menuState.html(json.Message);
                    }
                }
            });
        });
    },

    LoadedChangeType: function () {
        var val = $('#buttonDetails_type').val();
        var Type = val.toUpperCase();

        var regex = new RegExp("^JumpToResponseChain.+");
        if (Type == "CLICK" && $("#buttonDetails_key").val().toString().match(regex) != null) {
            Type = "CLICKTHENRESPONSE";
            $('#buttonDetails_type').val(Type);
        }

        senparc.menu.typeChanged();
    },

    typeChanged: function () {
        var val = $('#buttonDetails_type').val();
        var Type = val.toUpperCase();

        switch (Type) {
            case "VIEW":
                $('#buttonDetails_key_area').hide(100);
                $('#buttonDetails_ClickThenResponse_area').hide(100);
                $('#buttonDetails_url_area').show(100);
                break;

            case "CLICKTHENRESPONSE":
                $('#buttonDetails_key_area').hide(100);
                $('#buttonDetails_url_area').hide(100);
                $('#buttonDetails_ClickThenResponse_area').show(100);
                break;

            default:
                $('#buttonDetails_url_area').hide(100);
                $('#buttonDetails_ClickThenResponse_area').hide(100);
                $('#buttonDetails_key_area').show(100);
                break;
        }
    },
    setToken: function (token) {
        senparc.menu.token = token;
        $('#tokenStr').val(token);
        $('#menuEditor').show();
        $('#menuLogin').hide();
    }
};