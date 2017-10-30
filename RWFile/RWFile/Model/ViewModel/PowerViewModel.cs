using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWFile.Model.ViewModel
{
    public class PowerViewModel
    {
        [DisplayName("X座標")]
        public string X { get; set; }

        [DisplayName("Y座標")]
        public string Y { get; set; }

        public string DUT { get; set; }
         
        public string PinName { get; set; }

        [DisplayName("電流")]
        public string ma { get; set; }
    }
}
