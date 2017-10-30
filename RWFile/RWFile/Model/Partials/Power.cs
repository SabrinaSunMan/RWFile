using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RWFile.Model.Partials
{
    [MetadataType(typeof(Power_MetaData))]
    public partial class Power
    {
        public string Power_ID { get; set; }

        [DisplayName("PinName")]
        public string Pin_name { get; set; }

        [DisplayName("電力")]
        public string mA { get; set; }

        public string FK_WT_ID { get; set; }

        public class Power_MetaData
        {
            public string Power_ID { get; set; }

            [DisplayName("PinName")]
            public string Pin_name { get; set; }

            [DisplayName("電力")]
            public string mA { get; set; }
            
            public string FK_WT_ID { get; set; }
        }
    }
}
