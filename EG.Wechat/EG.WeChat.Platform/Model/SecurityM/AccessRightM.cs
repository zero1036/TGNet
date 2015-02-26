using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.WeChat.Model
{
    public class TR_Group_Right
    {
        [Column(IsPrimaryKey = true)]
        public int GroupID { get; set; }

        [Column(IsPrimaryKey = true)]
        public string Controller { get; set; }

        public string ControllerName { get; set; }

        public string ControllerD { get; set; }

        [Column(IsPrimaryKey = true)]
        public string Action { get; set; }

        public string ActionName { get; set; }

        public string ActionD { get; set; }

    }
}
