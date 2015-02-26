using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP;

namespace EG.WeChat.Platform.Model
{
    public class WC_Media
    {
        [Column(IsPrimaryKey = true)]
        public DateTime Date { get; set; }

        [Column(IsPrimaryKey = true)]
        public string ID { get; set; }

        public string OpenID { get; set; }

        //public UploadMediaFileType Type { get; set; }

        public int Type { get; set; }

    }



    public class WCR_Media_Resource
    {
        [Column(IsPrimaryKey = true)]
        public DateTime Date { get; set; }

        [Column(IsPrimaryKey = true)]
        public string ID { get; set; }

        public string OpenId { get; set; }

        public int Type { get; set; }

        public DateTime CreatedTime { get; set; }

        public int LocalID { get; set; }

    }


}
