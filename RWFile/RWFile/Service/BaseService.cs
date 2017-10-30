using RWFile.LocalDB;
using RWFile.Model;
using RWFile.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RWFile.Service
{
    public class BaseService
    {
        Bin_ReachedService bin_service = new Bin_ReachedService();
        Deg_cService Deg_service = new Deg_cService();
        PowerService power_service = new PowerService();

        private readonly IDBRepositories<WT> _WT;
        private readonly IDBRepositories<BIN_REACHED> _Bin_Reached;
        private readonly IDBRepositories<Deg_c> _Deg_c;     //溫度
        private readonly IDBRepositories<Power> _Power;     //電力
        private readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _WT = new DBRepositories<WT>(unitOfWork);
            _Bin_Reached = new DBRepositories<BIN_REACHED>(unitOfWork);
            _Deg_c = new DBRepositories<Deg_c>(unitOfWork);
            _Power = new DBRepositories<Power>(unitOfWork);
        }
         
        public PackageModel Package(KeyWord KeyWordTitle,string ReadLineStr, PackageModel package)
        { 
            if(KeyWordTitle.FiledName == "TESTFLOW START:")
            {
                package.WT_Info = new WT();
                package.WT_Info.WT_ID = Guid.NewGuid().ToString().ToUpper();
                package.WT_Info.SDate = Convert.ToDateTime(NoneSplit(ReadLineStr, KeyWordTitle.FiledName));

            }
            if(KeyWordTitle.FiledName== "BIN REACHED:")
            {
                ReadLineStr = NoneSplit(ReadLineStr, KeyWordTitle.FiledName);
               
                if (package.Bin_Info == null)
                { package.Bin_Info = new List<BIN_REACHED>(); }
                     
                package.Bin_Info.Add(bin_service.Create_Bin_Reached(ReadLineStr, package.WT_Info));
            }
            if (KeyWordTitle.FiledName == "Targettemp=105:USL=112:LSL=98")
            {
                
                int startInt = ReadLineStr.IndexOf(KeyWordTitle.FiledName)+ KeyWordTitle.FiledName.Length;
                //取溫度 
                int readyGetInt = ReadLineStr.Length - startInt;
                package.Degc_Info = Deg_service.Create_Deg_c(ReadLineStr.Substring(startInt, readyGetInt).Replace("degC","").Trim(), package.WT_Info);
            }

            if (KeyWordTitle.FiledName == "pin_name=")
            { 
                int startInt = ReadLineStr.IndexOf(KeyWordTitle.FiledName) + KeyWordTitle.FiledName.Length;
                //取電力  
                int readyGetInt = ReadLineStr.Length - startInt;
                if (package.PoserInfo == null)
                { package.PoserInfo = new List<Power>(); }
                package.PoserInfo.Add(power_service.Create_Power(ReadLineStr.Substring(startInt, readyGetInt).Replace(" mA", "").Trim(), package.WT_Info));
            }

            if (KeyWordTitle.FiledName == "TESTFLOW END:")
            {
                string [] TempEnd = NoneSplit(ReadLineStr, KeyWordTitle.FiledName).Split(' ');

                package.WT_Info.EDate = Convert.ToDateTime(TempEnd[1]+" "+ TempEnd[2]);
            }
            return package;
        }
          
        private string NoneSplit(string OriginalStr,string ReplaceStr)
        {
            return OriginalStr.Replace(ReplaceStr, "").Replace("*","");
        }

        public IEnumerable<WT> GetAllWT()
        { 
            return _WT.GetAll();
        }

        public IEnumerable<BIN_REACHED> GetAllBIN_REACHED()
        {
            return _Bin_Reached.GetAll();
        }

        public IEnumerable<Deg_c> GetAllDeg_c()
        {
            return _Deg_c.GetAll();
        }

        public IEnumerable<Power> GetAllPower()
        {
            return _Power.GetAll();
        }
         
        public void Add(PackageModel PackageInfo)
        {
            PackageInfo.WT_Info.CreateTime_ = DateTime.Now;
            _WT.Create(PackageInfo.WT_Info);
            //_WT.Commit(); //

            var Binreached = PackageInfo.Bin_Info;
            foreach (var BinItem in Binreached)
            {
                _Bin_Reached.Create(BinItem);
                //_Bin_Reached.Commit();//
            }

            _Deg_c.Create(PackageInfo.Degc_Info);
            //_Deg_c.Commit();//

            var power = PackageInfo.PoserInfo;
            foreach (var powerItem in power)
            {
                _Power.Create(powerItem);
                //_Power.Commit();//
            } 
            
        } 

        public void Save()
        {
            _unitOfWork.Save();
        }
    }
}
