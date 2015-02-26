using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using EG.WeChat.Platform.Model;
/*****************************************************
* 目的：視頻轉換長事務隊列處理
* 创建人：林子聪
* 创建时间：20150204
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.BL
{
    public class MediaConverterQueue
    {
        /// <summary>
        /// 單例
        /// </summary>
        public static MediaConverterQueue singlelon = new MediaConverterQueue();
        /// <summary>
        /// 轉換事務委託隊列
        /// </summary>
        private Queue<MediaConverter> ActionQueue { get; set; }
        /// <summary>
        /// 加入等待轉換隊列
        /// </summary>
        /// <param name="pAction"></param>
        public void AddQueue(MediaConverter pAction)
        {
            lock (this)
            {
                if (ActionQueue == null)
                    ActionQueue = new Queue<MediaConverter>();

                ActionQueue.Enqueue(pAction);
                //Console.WriteLine(string.Format("action隊列數量{0}", ActionQueue.Count));
            }
        }
        /// <summary>
        /// 執行轉換，并返回最後一位成員結果
        /// </summary>
        /// <returns></returns>
        public List<WXVideoResultJson> ExecuteActions()
        {
            List<WXVideoResultJson> pListOut = null;
            lock (this)
            {
                if (ActionQueue == null || ActionQueue.Count == 0)
                    return pListOut;


                int i = -1;
                while (i != 0)
                {
                    MediaConverter mc = ActionQueue.Dequeue();
                    pListOut = mc.Dlg(mc.Request, mc.lcName, mc.lcClassify);

                    i = ActionQueue.Count;
                }
            }
            return pListOut;
        }
    }
    /// <summary>
    /// 轉換實體
    /// </summary>
    public class MediaConverter
    {
        public string lcName
        { get; set; }
        public string lcClassify
        { get; set; }
        public HttpRequestBase Request
        { get; set; }
        public DlgMediaResourceConvert Dlg
        { get; set; }
    }
    /// <summary>
    /// 轉換事務委託
    /// </summary>
    /// <param name="pRequest"></param>
    /// <param name="lcName"></param>
    /// <param name="lcClassify"></param>
    /// <returns></returns>
    public delegate List<WXVideoResultJson> DlgMediaResourceConvert(HttpRequestBase pRequest, string lcName, string lcClassify);
}
