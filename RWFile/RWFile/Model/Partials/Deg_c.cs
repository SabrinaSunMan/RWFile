using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RWFile.Model.Partials
{
    [MetadataType(typeof(Deg_c_MetaData))]
    public partial class Deg_c
    {
        public string DegC_ID { get; set; }

        [DisplayName("溫度")]
        public Nullable<decimal> degC { get; set; }

        public string FK_WT_ID { get; set; }


        public class Deg_c_MetaData
        {
            public string DegC_ID { get; set; }

            [DisplayName("溫度")]
            public Nullable<decimal> degC { get; set; }

            public string FK_WT_ID { get; set; }
        }
    }
}
