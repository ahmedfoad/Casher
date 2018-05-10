using System;
using System.Collections.Generic;
using System.Linq; using System.Threading.Tasks;
using System.Web;

namespace Casher.Models
{
    public class Cls_View
    {
        public ClsView ClsView { get; set; }
        public bool Role_Enter { get; set; }
        public bool Role_Save { get; set; }
        public bool Role_Edit { get; set; }
        public bool Role_Delete { get; set; }

    }
    public class ClsView
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}