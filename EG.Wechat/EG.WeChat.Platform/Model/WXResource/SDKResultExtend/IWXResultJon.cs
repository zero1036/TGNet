﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*****************************************************
* 目的：SDK微信资源生成结果Json擴展接口
* 创建人：林子聪
* 创建时间：20141129
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.Platform.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWXResultJon1
    {
        /// <summary>
        /// 創建時間
        /// </summary>
        long created_at
        { get; set; }
        /// <summary>
        /// 微信回馈Media ID
        /// </summary>
        string media_id
        {
            get;
            set;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWXResultJon2<T>
    {
        /// <summary>
        /// Senparc SDK接口要求ResultJson
        /// </summary>
        T UploadResultJson
        {
            get;
            set;
        }
    }
}
