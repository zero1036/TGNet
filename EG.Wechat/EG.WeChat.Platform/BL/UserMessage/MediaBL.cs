using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EG.WeChat.Model;
using EG.WeChat.Platform.DA;
using EG.WeChat.Platform.Model;
using Senparc.Weixin.MP;
using EG.WeChat.Utility.Tools;

namespace EG.WeChat.Platform.BL
{
    public class MediaBL
    {
        private MediaDA _mediaDA;
        protected MediaDA MediaDA
        {
            get
            {
                if (_mediaDA == null)
                {
                    _mediaDA = new MediaDA();
                }
                return _mediaDA;
            }
        }


        /// <summary>
        /// 添加媒體資源
        /// </summary>
        /// <param name="OpenId"></param>
        /// <param name="Media"></param>
        /// <returns></returns>
        public virtual ResultM NewMedia(string OpenId, string MediaID, UploadMediaFileType Type)
        {
            ResultM result = new ResultM();
            WC_Media model = new WC_Media() { ID = MediaID, OpenID = OpenId, Type = Type.GetHashCode(), Date = DateTime.Now };
            result.Affected = MediaDA.Add(model);
            return result;
        }



        /// <summary>
        /// 最近三天能獲取的媒體資源
        /// </summary>
        public virtual PagingM List(string openId, DateTime? dt,int type, int pageIndex)
        {
            Hashtable model = new Hashtable();
            model.Add("OpenID", string.Format("%{0}%", openId));
            model.Add("Date", dt);
            model.Add("Type", type);
            return MediaDA.GetPageList(model, pageIndex);
        }



        /*      媒體資源下載和管理        */

        /// <summary>
        /// 保存多媒体资源
        /// </summary>
        /// <param name="openId">用户的OpenId</param>
        /// <param name="MediaId">媒体的MediaId</param>
        /// <param name="localId">成为本地资源之后的本地Id</param>
        /// <param name="type">多媒体的类型</param>
        /// <returns></returns>
        public virtual ResultM SaveMediaResource(string openId, string MediaId, UploadMediaFileType Type)
        {
            //结果容器
            ResultM result  = new ResultM();
            int localId     = -1;

            //通过“本地资源”模块，下载资源并获得LocalId
            switch (Type)
            {
                default:
                    throw new Exception("目前不支持此类型。" + Type.ToString());

                case UploadMediaFileType.thumb:
                case UploadMediaFileType.image:
                    {
                        WeChatPictureService service    = new WeChatPictureService("QY");
                        localId                         = service.DownLoadResources(MediaId);
                        EGExceptionResult pResult       = service.GetActionResult();
                        if ( pResult != null  )
                        {
                            result.Message = pResult.Message;
                        }
                    }
                    break;

                //case UploadMediaFileType.video:     //视频类型，微信文档写着不支持下载
                    //break;

                case UploadMediaFileType.voice:
                    {
                        WeChatVoiceService service = new WeChatVoiceService("QY");
                        localId                     = service.DownLoadResources(MediaId);
                        EGExceptionResult pResult   = service.GetActionResult();
                        if (pResult != null)
                        {
                            result.Message = pResult.Message;
                        }
                    }
                    break;

            }

            //记录到数据库
            if (localId >= 0)
            {
                var affectedRows = 
                MediaDA.SaveMediaResource(new WCR_Media_Resource
                {
                    OpenId  = openId,
                    ID      = MediaId,
                    LocalID = localId,
                    Type    = (int)Type,
                    Date    = DateTime.Now.Date,
                    CreatedTime = DateTime.Now,
                });

                result.Affected = affectedRows;
            }

            //返回结果            
            return result;
        }
    }
}
