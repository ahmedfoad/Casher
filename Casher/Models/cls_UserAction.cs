using System;
using System.Collections.Generic;
using System.Linq; using System.Threading.Tasks;
using System.Web;

namespace Casher.Models
{
    public class cls_UserAction
    {
        public int ID { get; set; }
        public string EmpName { get; set; }
        public int Viewid { get; set; }
        public DateTime ActionDate { get; set; }
        public string Action { get; set; }
        public string Operation { get; set; }
    }
}