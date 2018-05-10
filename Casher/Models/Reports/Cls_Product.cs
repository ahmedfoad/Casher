using System;
using System.Collections.Generic;
using System.Linq; using System.Threading.Tasks;
using System.Web;

namespace Casher.Models
{
    public class Cls_Product
    {
        public ClsProduct ClsProduct { get; set; }
        public string ErrorName { get; set; }
        public byte[] BarCodeArr { get; set; }
        public System.Drawing.Image BarCodeImg { get; set; }
    }
}