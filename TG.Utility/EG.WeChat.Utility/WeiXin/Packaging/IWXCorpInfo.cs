using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*****************************************************
* 目的：企业号基础设置适配接口
* 创建人：林子聪
* 创建时间：20150313
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Utility.WeiXin
{
    public interface IWXCorpInfo
    {
        int aid { get; }

        string token { get; set; }

        string aeskey { get; set; }

        string aname { get; set; }

        string round_logo_url { get; set; }
    }
}
