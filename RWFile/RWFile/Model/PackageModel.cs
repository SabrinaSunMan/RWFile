
using RWFile.LocalDB;
using System.Collections.Generic;

namespace RWFile.Model
{
    public class PackageModel
    {
          
        public WT WT_Info { get; set; }

        public List<BIN_REACHED> Bin_Info { get; set; }

        public Deg_c Degc_Info { get; set; }

        public List<Power> PoserInfo { get; set; }
    }
}
