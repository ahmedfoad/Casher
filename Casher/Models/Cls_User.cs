using System;
using System.Collections.Generic;
using System.Linq; using System.Threading.Tasks;
using System.Web;

namespace Casher.Models
{
    public class Cls_User
    {
        public User User { get; set; }
        public List<Cls_View> Cls_Views { get; set; }

    }
}