using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace EG.WeChat.Platform.Model
{

    public class WC_Text
    {
   


        public string OpenID { get; set; }

        public DateTime Date { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// 经度值
        /// </summary>
        public float? Lng { get; set; }

        /// <summary>
        /// 纬度值
        /// </summary>
        public float? Lat { get; set; }

    }



}
