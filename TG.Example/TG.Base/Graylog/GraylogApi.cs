using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TG.Example
{
    public class GraylogApi
    {
        public void SearchKeyword()
        {
            string domain = "http://192.168.1.xx:xxxx";
            string url = string.Format("{0}/search/universal/keyword?query=source%3Aiis_wx_wap%20%26%26%20message%3A'ERROR'&keyword=2%20weeks%20ago%20to%20wednesday&fields=message", domain);
            string auth = "Basic ZGV2OjEyMzQ1Ng==";

            var result = HttpHelper.HttpGet(url, 1000, null, auth);
            Console.WriteLine(result.Data);
        }
    }
}
