using RWFile.LocalDB;
using System;

namespace RWFile.Service
{
    public class PowerService
    { 
        public Power Create_Power(string PowerData, WT wt_info)
        {
            Power power = new Power();
            power.Power_ID = Guid.NewGuid().ToString().ToUpper();
            power.FK_WT_ID = wt_info.WT_ID;
            string[] tempwords = PowerData.Split(' ');

            power.Pin_name = tempwords[0].ToString();
            //power.mA = Convert.ToDecimal(tempwords[1].ToString()); 
            power.mA = tempwords[1].ToString();
            return power;
        }
    }
}
