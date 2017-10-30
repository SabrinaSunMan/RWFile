using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RWFile.Model.Partials
{
    [MetadataType(typeof(BIN_REACHED_MetaData))]
    public partial class BIN_REACHED
    {
        public string Bin_Reached_ID { get; set; }

        [DisplayName("X座標")]
        public decimal X { get; set; }

        [DisplayName("Y座標")]
        public decimal Y { get; set; }

        [DisplayName("DUT")]
        public int DUT { get; set; }

        [DisplayName("結果")]
        public string Result { get; set; }

        public int bin1 { get; set; }

        public int bin2 { get; set; }

        public string FK_WT_ID { get; set; }


        public class BIN_REACHED_MetaData
        {
            public string Bin_Reached_ID { get; set; }

            [DisplayName("X座標")]
            public decimal X { get; set; }

            [DisplayName("Y座標")]
            public decimal Y { get; set; }

            [DisplayName("DUT")]
            public int DUT { get; set; }

            [DisplayName("結果")]
            public string Result { get; set; }
             
            public int bin1 { get; set; }
             
            public int bin2 { get; set; }

            public string FK_WT_ID { get; set; }
        }
    }
}
