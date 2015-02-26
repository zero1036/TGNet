using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.WeChat.Model
{
    public class T_Group
    {
        [Column(IsPrimaryKey = true)]
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int State { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedTime { get; set; }

    }
}
