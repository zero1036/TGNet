using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EG.WeChat.Platform.DA;
/*****************************************************
* 目的：微信资源表DA
* 创建人：林子聪
* 创建时间：20141208
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.DA
{
    public class WXResourceDA : WXResAOP
    {
        #region 私有成员
        /// <summary>
        /// 数据访问接口
        /// </summary>
        private ADOTemplateX template = new ADOTemplateX();

        public static readonly string FIELD_NAME_LCID = "LCID";
        public static readonly string FIELD_NAME_LCNAME = "LCNAME";
        public static readonly string FIELD_NAME_LCCLASSIFY = "LCCLASSIFY";
        public static readonly string FIELD_NAME_MEDIA_ID = "media_id";
        public static readonly string FIELD_NAME_MEDIA_TYPE = "media_type";
        public static readonly string FIELD_NAME_CONTENT = "content";
        public static readonly string FIELD_NAME_CTIME = "ctime";
        public static readonly string FIELD_NAME_LOSE = "lose";
        public static readonly string FIELD_NAME_SOURCE_TYPE = "source_type";

        private static readonly string m_tableName_WC_RESOURCE = "WC_RESOURCEX";
        private static readonly string m_proceName_GetRESOURCE = "select * from WC_RESOURCEX";
        private static readonly string m_proceName_UpdateRESOURCE = "PRO_WC_RESOURCEX_UPDATE";
        private static readonly string m_proceName_DeleteRESOURCE = "PRO_WC_RESOURCEX_DEL";
        #endregion

        #region 成员属性
        /// <summary>
        /// 本地编号
        /// </summary>
        public int LCId { get; set; }
        /// <summary>
        /// 本地名称
        /// </summary>
        public string LCName { get; set; }
        /// <summary>
        /// 本地分类
        /// </summary>
        public string LCClassify { get; set; }
        /// <summary>
        /// 微信编号
        /// </summary>
        public string Media_Id { get; set; }
        /// <summary>
        /// 微信类型：text、image、voice、video、mpnews
        /// </summary>
        public string Media_Type { get; set; }
        /// <summary>
        /// 序列化内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime CTime { get; set; }
        /// <summary>
        /// 1:失效、0：生效
        /// </summary>
        public bool Lose { get; set; }
        /// <summary>
        /// 1:本地數據；2：微信接收數據
        /// </summary>
        public int? Source_Type { get; set; }
        #endregion

        #region 公有
        /// <summary>
        /// 通过资源类型获取所拥有资源
        /// </summary>
        /// <param name="media_Type"></param>
        /// <returns></returns>
        public DataTable GetWXResources(string media_Type)
        {
            string strsql = string.Format("select * from {0} where {1}=@PMediaType  order by {2} desc", m_tableName_WC_RESOURCE, FIELD_NAME_MEDIA_TYPE, FIELD_NAME_CTIME);
            //
            return template.Query(strsql, new string[] { "@PMediaType" }, new object[] { media_Type }, null);
        }
        /// <summary>
        /// 保存单个资源
        /// </summary>
        /// <param name="lcid"></param>
        /// <param name="lcname"></param>
        /// <param name="lcclassify"></param>
        /// <param name="media_Id"></param>
        /// <param name="media_Type"></param>
        /// <param name="content"></param>
        /// <param name="iCreateTime"></param>
        /// <param name="iSourceType"></param>
        /// <returns></returns>
        public override int? SaveResource(int? lcid, string lcname, string lcclassify, string media_Id, string media_Type, string content, DateTime iCreateTime, int iSourceType)
        {
            DataSet result = template.ExecuteX(m_proceName_UpdateRESOURCE, new string[] { "@Plcid", "@Plcname", "@Plcclassify", "@Pmediaid", "@Pmediatype", "@Pcontent", "@Pctime", "@Psourcetype" }, new object[] { lcid, lcname, lcclassify, media_Id, media_Type, content, iCreateTime, iSourceType }, null, CommandType.StoredProcedure);
            object obj = result.Tables[0].Rows[0][0];
            if (obj != null)
                return Convert.ToInt32(obj);
            return null;
        }
        /// <summary>
        /// 刪除資源
        /// </summary>
        /// <param name="lcid"></param>
        /// <returns></returns>
        public override int DeleteResource(int lcid)
        {
            return template.Execute(m_proceName_DeleteRESOURCE, new string[] { "@Plcid" }, new object[] { lcid }, null, CommandType.StoredProcedure);
        }
        #endregion

    }
}
