using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWFile.Model.ViewModel
{
    public class DegCViewModel
    {
        [DisplayName("X座標")]
        public string X { get; set; }

        [DisplayName("Y座標")]
        public string Y { get; set; }
         
        public string DUT { get; set; }

        [DisplayName("溫度")]
        public string degC { get; set; }
    }
}
