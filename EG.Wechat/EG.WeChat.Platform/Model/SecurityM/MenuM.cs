using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.WeChat.Model.SecurityM
{
    public class T_Menu
    {
        [Column(IsPrimaryKey = true)]
        public string Code { get; set; }
        public string Name { get; set; }
        public string FatherCode { get; set; }
        public int Sort { get; set; }
        public int State { get; set; }
        public string Controller { get; set; }
        public string Description { get; set; }
        public string Href { get; set; }
    }
}
