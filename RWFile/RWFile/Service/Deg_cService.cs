using RWFile.LocalDB;
using System;

namespace RWFile.Service
{
    public class Deg_cService
    {  

        public Deg_c Create_Deg_c(string degC, WT wt_info)
        {
            Deg_c deg = new Deg_c();
            deg.DegC_ID = Guid.NewGuid().ToString().ToUpper();
            deg.FK_WT_ID = wt_info.WT_ID;
            deg.degC = Convert.ToDecimal(degC); 
            return deg;
        }
    }
}
