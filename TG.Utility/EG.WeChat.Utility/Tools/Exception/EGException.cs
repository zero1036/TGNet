using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.Exceptions;
/*****************************************************
* 目的：EG定制异常通用处理
* 创建人：林子聪
* 创建时间：20141119
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.Tools
{
    /// <summary>
    /// EGExceptionOperator
    /// </summary>
    public class EGExceptionOperator
    {
        /// <summary>
        /// 抛异常
        /// </summary>
        /// <typeparam name="Tx"></typeparam>
        /// <param name="strMessage"></param>
        /// <param name="pCode"></param>
        public static void ThrowX<Tx>(string strMessage, EGActionCode pCode)
            where Tx : Exception, new()
        {
            EGExceptionResult pResult = new EGExceptionResult();
            pResult.IsSuccess = false;
            pResult.Message = strMessage;
            pResult.ExCode = ((int)pCode).ToString();

            throw new EGException(strMessage, new Tx(), pResult);
        }
        /// <summary>
        /// 抛异常
        /// </summary>
        /// <typeparam name="Tx"></typeparam>
        /// <param name="strMessage"></param>
        /// <param name="pCode"></param>
        public static void ThrowX<Tx>(string strMessage, string pCode)
            where Tx : Exception, new()
        {
            EGExceptionResult pResult = new EGExceptionResult();
            pResult.IsSuccess = false;
            pResult.Message = strMessage;
            pResult.ExCode = pCode;

            throw new EGException(strMessage, new Tx(), pResult);
        }
        /// <summary>
        /// 转换异常为输出json实体
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static EGExceptionResult ConvertException(Exception ex)
        {
            EGExceptionResult ActionResult = new EGExceptionResult();
            //EG定制异常
            if (ex is EG.WeChat.Utility.Tools.EGException)
            {
                if ((ex as EG.WeChat.Utility.Tools.EGException) != null)
                {
                    ActionResult = (ex as EG.WeChat.Utility.Tools.EGException).JsonResult;
                }
            }
            //Senparc SDK报错异常
            else if (ex is Senparc.Weixin.Exceptions.ErrorJsonResultException)
            {
                if ((ex as Senparc.Weixin.Exceptions.ErrorJsonResultException) != null)
                {
                    ErrorJsonResultException pEx = (ex as Senparc.Weixin.Exceptions.ErrorJsonResultException);
                    ActionResult.Message = pEx.JsonResult.errmsg;
                    ActionResult.IsSuccess = false;
                    var code = Enum.GetName(typeof(Senparc.Weixin.ReturnCode), pEx.JsonResult.errcode);
                    if (code == null)
                        ActionResult.ExCode = "9999";
                    else
                        ActionResult.ExCode = code.ToString();
                }
            }
            //其他异常，未来可在这扩展其他异常
            else
            {
                EGExceptionResult pResult = new EGExceptionResult();
                pResult.ExCode = ((int)EGActionCode.未知错误).ToString();
                pResult.IsSuccess = false;
                pResult.Message = ex.Message;

                ActionResult = pResult;
            }
            return ActionResult;
        }
    }
    /// <summary>
    /// EG定制异常
    /// </summary>
    public class EGException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public EGExceptionResult JsonResult { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        /// <param name="jsonResult"></param>
        public EGException(string message, Exception inner, EGExceptionResult jsonResult)
            : base(message, inner)
        {
            JsonResult = jsonResult;
        }
    }
    /// <summary>
    /// 异常结果输出Json实体
    /// </summary>
    public class EGExceptionResult
    {
        /// <summary>
        /// 
        /// </summary>
        public EGExceptionResult()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pIsSuccess"></param>
        /// <param name="pMessage"></param>
        /// <param name="pExCode"></param>
        public EGExceptionResult(bool pIsSuccess, string pMessage, string pExCode)
        {
            this.IsSuccess = pIsSuccess;
            this.Message = pMessage;
            this.ExCode = pExCode;
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess
        {
            get;
            set;
        }
        /// <summary>
        /// 错误内容
        /// </summary>
        public string Message
        {
            get;
            set;
        }
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ExCode
        {
            get;
            set;
        }
    }
    /// <summary>
    /// EG定制异常枚举代码
    /// </summary>
    public enum EGActionCode
    {
        执行成功 = 0,
        缺少必要参数 = 1001,
        缺少目标数据 = 1002,
        XML交换格式读取失败 = 1003,
        JSON交换格式读取失败 = 1004,
        文件上传失败 = 1005,
        输入参数错误 = 1006,
        数据库表保存错误 = 1007,
        未知错误 = 9999
    }
}
