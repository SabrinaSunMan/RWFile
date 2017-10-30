
using RWFile.Model;
using RWFile.LocalDB;
using RWFile.Repositories;
using RWFile.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RWFile.Model.ViewModel;

namespace RWFile
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        private readonly BaseService _Baseservice;

        public Form1()
        {
            InitializeComponent();
            var unitOfWork = new EFUnitOfWork();
            _Baseservice = new BaseService(unitOfWork);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //防呆，資料庫為一次性產物，若有資料則不給予重複匯資料
            List<WT> getAllWT = _Baseservice.GetAllWT().ToList(); 
            metroButton1.Enabled = getAllWT.Count == 0 ? true : false;

            //未顯示資料列不給予匯出Excel
            metroButton4.Enabled = getAllWT.Count == 0 ? true : false; 
        } 

        private void metroButton1_Click(object sender, EventArgs e)
        {
            List<KeyWord> KeyWordTitle = new List<KeyWord>();
            KeyWordTitle.Add(new KeyWord() { FiledName = "TESTFLOW START:" });
            KeyWordTitle.Add(new KeyWord() { FiledName = "BIN REACHED:" });
            KeyWordTitle.Add(new KeyWord() { FiledName = "Targettemp=105:USL=112:LSL=98" }); //溫度
            KeyWordTitle.Add(new KeyWord() { FiledName = "pin_name=" });//電流   testsuite_name=powershort_CP2_post
            KeyWordTitle.Add(new KeyWord() { FiledName = "TESTFLOW END:" });

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int counter = 0; //紀錄行數
                int CountKeyWordTitle = 0;
                string line;
                string nowString = "";
                StreamReader file = new StreamReader(openFileDialog1.FileName);
                try
                {
                    PackageModel Package = new PackageModel();
                    while ((line = file.ReadLine()) != null)
                    {
                        nowString = line;

                        if (!string.IsNullOrEmpty(line)) //不是空值才開始做事
                        {
                            if (CountKeyWordTitle == KeyWordTitle.Count - 1) //當 關鍵字已搜尋完時,初始化並將 Package 存入DB
                            {
                                //搜尋資料請到 /Bin/Database1.mdf 
                                string now = "你需要恢復原廠設定!";
                                _Baseservice.Add(Package);
                                _Baseservice.Save();
                                CountKeyWordTitle = 0;
                                Package = new PackageModel();
                            }
                            else
                            {
                                if (nowString.Contains(KeyWordTitle[CountKeyWordTitle + 1].FiledName))
                                {
                                    CountKeyWordTitle++;
                                }
                                Regex r = new Regex(KeyWordTitle[CountKeyWordTitle].FiledName,
                                RegexOptions.IgnoreCase);

                                if (r.Match(nowString).Success)
                                {
                                    Package = _Baseservice.Package(KeyWordTitle[CountKeyWordTitle],
                                    nowString, Package);
                                }
                            }
                        }
                        counter++;
                    }
                    file.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message + "Line : " + counter);
                    file.Close();
                }
            }
            MessageBox.Show("Done!");
        }

        /// <summary>
        /// 取得 溫度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton2_Click(object sender, EventArgs e)
        {
            //select Bin.X,Bin.Y,Bin.DUT,deg.degC
            //from BIN_REACHED Bin
            //left join Deg_c deg
            //on deg.FK_WT_ID = Bin.FK_WT_ID
            List<BIN_REACHED> getAllBIN_REACHED = _Baseservice.GetAllBIN_REACHED().ToList();
            List<Deg_c> getAllDeg_c = _Baseservice.GetAllDeg_c().ToList();
            
            var result = from Bin in getAllBIN_REACHED
                         join deg
             in getAllDeg_c on Bin.FK_WT_ID equals deg.FK_WT_ID
                      select new { Bin.X, Bin.Y, Bin.DUT, deg.degC };
            List<DegCViewModel> degCList = new List<DegCViewModel>();
            foreach (var DegC in result)
            {
                degCList.Add(new DegCViewModel() { X = DegC.X.ToString(), Y = DegC.Y.ToString(),
                    degC = DegC.degC.ToString(), DUT = DegC.DUT.ToString()
                });
            } 
            metroGrid1.DataSource = degCList;
            metroLabel2.Text = degCList.Count.ToString();
            //ChooseShowModel(DisplayModel.WFInfo); 
        }

        /// <summary>
        /// 取得 電力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroButton3_Click(object sender, EventArgs e)
        { 
            List<BIN_REACHED> getAllBIN_REACHED = _Baseservice.GetAllBIN_REACHED().ToList();
            List<Power> getAllPower = _Baseservice.GetAllPower().ToList();

            var result = from Bin in getAllBIN_REACHED
                         join power
             in getAllPower on Bin.FK_WT_ID equals power.FK_WT_ID
                         select new { Bin.X, Bin.Y, Bin.DUT, power.Pin_name,power.mA };
            List<PowerViewModel> PowerList = new List<PowerViewModel>();
            foreach (var power in result)
            {
                PowerList.Add(new PowerViewModel()
                {
                    X = power.X.ToString(),
                    Y = power.Y.ToString(),
                    DUT = power.DUT.ToString(),
                    PinName = power.Pin_name,
                    ma = power.mA
                });
            }
            metroGrid1.DataSource = PowerList;
            metroLabel2.Text = PowerList.Count.ToString();
        }

        private void copyAlltoClipboard()
        {
            metroGrid1.SelectAll();
            DataObject dataObj = metroGrid1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void ChooseShowModel(DisplayModel showModel)
        {
            switch(showModel)
            {
                case DisplayModel.WFInfo:
                    List<WT> getAllWT = _Baseservice.GetAllWT().ToList(); 
                    metroGrid1.DataSource = getAllWT;
                    metroGrid1.Columns["WT_ID"].Visible = false;   //不需要顯示
                    return;
                case DisplayModel.BIN_REACHEDInfo:
                    List<BIN_REACHED> getAllBIN_REACHED = _Baseservice.GetAllBIN_REACHED().ToList();
                    metroGrid1.DataSource = getAllBIN_REACHED;
                    metroGrid1.Columns["Bin_Reached_ID"].Visible = false;   //不需要顯示
                    metroGrid1.Columns["FK_WT_ID"].Visible = false;         //不需要顯示
                    return;
                case DisplayModel.Deg_cInfo:
                    List<Deg_c> getAllDeg_c = _Baseservice.GetAllDeg_c().ToList();
                    metroGrid1.DataSource = getAllDeg_c;
                    metroGrid1.Columns["Bin_Reached_ID"].Visible = false;   //不需要顯示
                    metroGrid1.Columns["FK_WT_ID"].Visible = false;         //不需要顯示
                    return;
                case DisplayModel.PowerInfo:
                    List<Power> getAllPower = _Baseservice.GetAllPower().ToList();
                    metroGrid1.DataSource = getAllPower;
                    metroGrid1.Columns["Bin_Reached_ID"].Visible = false;   //不需要顯示
                    metroGrid1.Columns["FK_WT_ID"].Visible = false;         //不需要顯示
                    return;
                default:

                    return;
            }
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            copyAlltoClipboard();
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }
         
    }
}
