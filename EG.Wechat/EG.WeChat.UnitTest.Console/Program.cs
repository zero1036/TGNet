using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EG.WeChat.UnitTest.Model;
/*****************************************************
* 目的：单元测试控制台
* 创建人：林子聪
* 创建时间：20141104
* 备注：
* 依赖性：
* 版权：
* 使用本文件时，必须保留本内容的完整性！
*****************************************************/
namespace EG.WeChat.UnitTest.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //AccountControllerUT.Init();
            //AccountControllerUT.ValidationActionTest();
            //AccountControllerUT.SendTemplateMessageTest();
            //EGWechatServiceUT.TestInterface();

            WXResourceCtlUT pUt = new WXResourceCtlUT();
            pUt.ValidationActionTest();
            System.Console.Read();
        }
    }
}
