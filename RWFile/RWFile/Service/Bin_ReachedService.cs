using RWFile.LocalDB;
using System;

namespace RWFile.Service
{
    public class Bin_ReachedService 
    { 
        public BIN_REACHED Create_Bin_Reached(string REACHEDData, WT wt_info)
        { 
            BIN_REACHED bin = new BIN_REACHED();
            bin.Bin_Reached_ID = Guid.NewGuid().ToString().ToUpper();
            bin.FK_WT_ID = wt_info.WT_ID;
            string[] tempwords = REACHEDData.Split(' ');
            //第一格是座標,第二格式DUT數,第三格F/P結果,第四格Bin數字,第五格Bin2數字
            string[] tempXY = tempwords[1].Replace("W", "").Split(',');
            //第一格是X,第二格是Y
            bin.X = Convert.ToDecimal(tempXY[0].ToString());
            bin.Y = Convert.ToDecimal(tempXY[1].ToString());

            bin.DUT = Convert.ToInt16(tempwords[2].ToString());
            bin.Result = tempwords[3].ToString();
            bin.bin1 = Convert.ToInt16(tempwords[4].ToString());
            bin.bin1 = Convert.ToInt16(tempwords[5].ToString());
              
            return bin;
        } 
         
    }
}
