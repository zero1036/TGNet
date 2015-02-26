using EG.Utility.DBCommon.dao;
using EG.WeChat.Platform.BL;
using EG.WeChat.Utility.WeiXin.ResponseChain;
using System;

namespace EG.WeChat.Platform.BL
{
    /// <summary>
    /// 分享內容給微信平臺
    /// </summary>
    [InputDataTypeLimit(DataTypes.InputAll)]
    [Description("分享內容給微信平臺")]
    public class CustomHandler_ShareMessage : ICustomHandler
    {

        //---------Members--------

        #region 状态成员

        public IResponseMessage ReadyMessage{ get; set; }
        public IResponseMessage SuccessResponseResult { get; set; }
        public ResponseTextMessage FailResponseResult { get; set; }
        
        #endregion


        //---------Control--------

        #region 构造函数
        /// <summary>
        ///  构造函数
        /// </summary>
        public CustomHandler_ShareMessage()
        {
            ReadyMessage = new ResponseTextMessage("歡迎參與“精彩生活，我愛分享”活動！\n請回復 文本类型或圖片类型、語音类型、視頻类型的内容，進行分享。");
        }
        #endregion

        #region 处理数据

        /// <summary>
        /// 处理数据
        /// </summary>
        public HandlerResult HandlerData(string openId, DataTypes intputType, object rawData)
        {
            try 
	        {
                //根据不同的类型，进行rawData的转换，以及数据的存储处理
                switch (intputType)
	            {
                    case DataTypes.Text:
                        {
                            var data = rawData as String;
                            if (data != null)
                            {
                                TextBL control_Text = TransactionAOP.newInstance(typeof(TextBL)) as TextBL;
                                control_Text.NewText(openId, data);

                                this.SuccessResponseResult = new ResponseTextMessage("您發送的文字內容分享成功！");
                                return HandlerResult.Success;
                            }
                        }
                        break;

                    case DataTypes.Image:
                        {
                            var data = rawData as ImageCan;
                            if (data != null)
                            {
                                MediaBL control_Media = TransactionAOP.newInstance(typeof(MediaBL)) as MediaBL;
                                control_Media.NewMedia(openId, data.MediaID, Senparc.Weixin.MP.UploadMediaFileType.image);

                                this.SuccessResponseResult = new ResponseTextMessage("您發送的圖片內容分享成功！");
                                return HandlerResult.Success;
                            }
                        }
                        break;

                    case DataTypes.Voice:
                        {
                            var data = rawData as VoiceCan;
                            if (data != null)
                            {
                                MediaBL control_Media = TransactionAOP.newInstance(typeof(MediaBL)) as MediaBL;
                                control_Media.NewMedia(openId, data.MediaID, Senparc.Weixin.MP.UploadMediaFileType.voice);

                                this.SuccessResponseResult = new ResponseTextMessage("您發送的語音內容分享成功！");
                                return HandlerResult.Success;
                            }
                        }
                        break;

                    case DataTypes.Video:
                        {
                            var data = rawData as VideoCan;
                            if (data != null)
                            {
                                MediaBL control_Media = TransactionAOP.newInstance(typeof(MediaBL)) as MediaBL;
                                control_Media.NewMedia(openId, data.MediaID, Senparc.Weixin.MP.UploadMediaFileType.video);

                                this.SuccessResponseResult = new ResponseTextMessage("您發送的視頻內容分享成功！");
                                return HandlerResult.Success;
                            }
                        }
                        break;

                    default:
                        {
                            this.FailResponseResult = new ResponseTextMessage("目前未支持此內容類型。\n請重新分享，類型要求為：文本或圖片、語音、視頻。");
                            return HandlerResult.Fail;
                        }
	            }

                //没有合适处理时返回失败
                this.FailResponseResult = new ResponseTextMessage("分享失敗。");
                return HandlerResult.Fail;
	        }
	        catch (Exception ex)
	        {
                this.SuccessResponseResult = new ResponseTextMessage("抱歉，服務故障！");
                Logger.Log4Net.Error("CustomHandler_ShareMessage异常。", ex);
                return HandlerResult.Success;
	        }
        }

        #endregion

    }
}