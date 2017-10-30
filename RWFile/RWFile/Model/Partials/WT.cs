using RWFile.LocalDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWFile.Model.Partials
{
    [MetadataType(typeof(WT_MetaData))]
    public partial class WT
    {
        public string WT_ID { get; set; }

        [DisplayName("開始時間")]
        public DateTime SDate { get; set; }

        [DisplayName("結束時間")]
        public DateTime EDate { get; set; }

        [DisplayName("建立時間")]
        public DateTime CreateTime_ { get; set; }

        public class WT_MetaData
        {
            public string WT_ID { get; set; }

            [DisplayName("開始時間")]
            public DateTime SDate { get; set; }

            [DisplayName("結束時間")]
            public DateTime EDate { get; set; }

            [DisplayName("建立時間")]
            public DateTime CreateTime_ { get; set; }
        }
    }
}
