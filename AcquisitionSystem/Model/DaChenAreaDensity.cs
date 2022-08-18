using FilterArithmetic;
using LogHelper;
using Newtonsoft.Json;
using System.Configuration;
using System.Net;
using System.Net.Sockets;

namespace AcquisitionSystem.Model
{
    internal class DaChenAreaDensity
    {
        private string address_ip = ConfigurationManager.AppSettings["AreaIP"];
        private int port = int.Parse(ConfigurationManager.AppSettings["AreaPort"]);

        private int before_scan_num = 0;
        //毫米数据包 --MMFILTERDATA
        private String AreaD_BF = " B6 1D 27 F6 DB 7D F2 3F 01 02 52 01 00 00 00 00 |" +
                                  "4D 4D 46 49 4C 54 45 52 44 41 54 41 00 00 00 00 00 00 00 00 00 00 00 00 | 00 00 00 00 | " +
                                  "F0 94 3D CA | 71 D4 45 0B 58 14 21 40";
        /*
         * 
         * 5370656369616C4261736963496E666F
         B6 1D 27 F6 DB 7D F2 3F   
        01 02 52 01 00 00 00 00 
        53 70 65 63 69 61 6C 42 61 73 69 63 49 6E 66 6F 00 00 00 00 00 00 00 00 00 00 00 00
        26 14 0A 50
        71 D4 45 0B 58 14 21 40
        6f3e74b5
        */
        //SPECIAL_BI 获取状态
        private string AreaFlag = "B6 1D 27 F6 DB 7D F2 3F 01 02 52 01 00 00 00 00 |" +
                                  "53 50 45 43 49 41 4C 5F 42 49 00 00 00 00 00 00 00 00 00 00 00 00 00 00 |00 00 00 00|" +
                                  "36 D9 50 B8| 71 D4 45 0B 58 14 21 40";
        private int ScanNum = 0;//趟号


        //# 发送ATLWEIGHT_BF  分区
        private string Partition = "B6 1D 27 F6 DB 7D F2 3F | 01 02 52 01 00 00 00 00 |" +
                                   "41 54 4C 57 45 49 47 48 54 5F 42 46 00 00 00 00 00 00 00 00 00 00 00 00 | 00 00 00 00 | " +
                                   "E2 4E 61 66 | 71 D4 45 0B 58 14 21 40";


        public string GetmmData()
        {
            AreaModel areahelp1 = new AreaModel();
            // form1.ScanNum = 0;

            Tuple<byte[], int> ArealBFStatus = NetworksStatus(AreaFlag);
            byte[] area_g_infostatus = ArealBFStatus.Item1[(24 + 8 + 8 + 4)..(ArealBFStatus.Item2 - (8 + 4))];

            int CurrentScanNum = BitConverter.ToInt32(area_g_infostatus[0..4]);

            ///////////////////////////////////////////////
            // Tuple<byte[], int> ArealBF1 = NetworksHM(AreaD_BF);
            // byte[] area_g_info1 = ArealBF1.Item1[((24 + 8 + 8))..^((8 + 4))];
            //int Current_ScanNum = BitConverter.ToInt32(area_g_info1[4..(4 + 4)]);
            // Form1.ScanNum = Current_ScanNum;



            if (AcquisitionSystem.ScanNum == CurrentScanNum)
            {
                LogHelper.LogHelper.Instance.WriteLog("趟号重复，过滤的趟数" + CurrentScanNum, LogType.Warning);
                return "";
            }
            else
            {
                //  ScanNum = Current_ScanNum;
                LogHelper.LogHelper.Instance.WriteLog("通过状态获取到的趟号值：" + CurrentScanNum, LogType.Notice);
                Tuple<byte[], int> ArealBF = NetworksHM(AreaD_BF);
                byte[] area_g_info = ArealBF.Item1[((24 + 8 + 8))..^((8 + 4))];
                ScanNum = BitConverter.ToInt32(area_g_info[4..(4 + 4)]);
                AcquisitionSystem.ScanNum = ScanNum;
                LogHelper.LogHelper.Instance.WriteLog("通过毫米数据获取到的趟号值：" + ScanNum, LogType.Notice);
                var datalen = BitConverter.ToInt32(area_g_info[0..4]) / 4 - 1;
                double[] _cur_datahm = new double[datalen];
                byte[] area_g = ArealBF.Item1[(24 + 8 + 8 + 4)..(ArealBF.Item2 - (8 + 4))];
                List<string> tempList = new List<string>();
                for (int i = 0; i < datalen; i++)
                {
                    _cur_datahm[i] = Math.Round(BitConverter.ToSingle(area_g[(i * 4)..(i * 4 + 4)]), 3);
                    tempList.Add(_cur_datahm[i].ToString());
                }
                #region  过滤集合左右0值
                //int flag = 0;
                //for (int i = 0; i < datalen; i++)
                //{
                //    _cur_datahm[i] = Math.Round(BitConverter.ToSingle(area_g[(i * 4)..(i * 4 + 4)]), 3);

                //    if ((_cur_datahm[i] <= 0 || _cur_datahm[i] == 0) && flag == 0)
                //    {
                //    }
                //    else
                //    {
                //        tempList.Add(_cur_datahm[i].ToString());
                //        flag = 1;
                //    }
                //}
                //List<string> tempList2 = new List<string>();
                //for (int i = 0; i < tempList.Count; i++)
                //{

                //    if (double.Parse(tempList[tempList.Count - i-1]) <= 0 && flag == 1)
                //    {
                //        int ddd = tempList.Count - i - 1;
                //    }
                //    else
                //    {
                //        tempList2.Add(tempList[tempList.Count - i-1].ToString());
                //        flag = 2;
                //    }
                //}
                //List<string> tempList3 = new List<string>();

                //for (int i = 0; i < tempList2.Count; i++)
                //{
                //    tempList3.Add(tempList2[tempList2.Count - i - 1]);

                //}

                #endregion
                areahelp1.COMMAND = 3;
                areahelp1.ESNUM = 2;
                areahelp1.CODE = "";
                areahelp1.MSG = "";
                areahelp1.DATATYPE = 1;
                areahelp1.COUPLINGFB = 0;
                // List<string> tempList = new List<string>();
                //tempList.Add(String.Join(",", _cur_datahm));
                areahelp1.DATA = tempList;
                areahelp1.IDNUM = ScanNum;
                areahelp1.KADJNUM = 0.00;
                areahelp1.TARNUM = 0.00;
                areahelp1.SPEUNUM = 0.00;
                areahelp1.SPEDNUM = 0.00;
                areahelp1.CONUNUM = 0.00;
                areahelp1.CONDNUM = 0.00;
                areahelp1.DETYPE = 3;
                areahelp1.PARNUM = 0;
                areahelp1.TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                areahelp1.DeviceNO = "2";
                string json1 = JsonConvert.SerializeObject(areahelp1);
                return json1;
            }
        }

        /// <summary>
        /// 获取测厚仪状态
        /// </summary>
        /// <param name="_cmd"></param>
        /// <returns></returns>
        private Tuple<byte[], int> NetworksStatus(string _cmd)
        {
            try
            {
                _cmd = _cmd.Replace("\r", "").Replace("\n", "").Replace("|", "").Replace(" ", "");
                byte[] _cmd_bytes = BitConverts.HexString2Bytes(_cmd);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = 300;
                EndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(address_ip), port);
                socket.Connect(iPEndPoint);
                socket.Send(_cmd_bytes);
                byte[] _data_r = new byte[4096];
                int len = socket.Receive(_data_r);
                socket.Close();
                return new Tuple<byte[], int>(_data_r, len);
            }
            catch (Exception e)
            {
                LogHelper.LogHelper.Instance.WriteLog("获取状态数据失败" + e.ToString(), LogType.Error);
                throw e;
            }
        }
        /// <summary>
        /// 获取测厚仪数据 
        /// </summary>
        /// <param name="_cmd"></param>
        /// <returns></returns>
        private Tuple<byte[], int> NetworksHM(string _cmd)
        {
            try
            {
                _cmd = _cmd.Replace("\r", "").Replace("\n", "").Replace("|", "").Replace(" ", "");
                byte[] _cmd_bytes = BitConverts.HexString2Bytes(_cmd);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = 300;
                EndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(address_ip), port);
                socket.Connect(iPEndPoint);
                socket.Send(_cmd_bytes);
                byte[] _data_r = new byte[8192];
                int len = socket.Receive(_data_r);
                socket.Close();

                return new Tuple<byte[], int>(_data_r, len);
            }
            catch (Exception e)
            {

                LogHelper.LogHelper.Instance.WriteLog("获取数据失败" + e.ToString(), LogType.Error);
                throw e;
            }
        }



        #region 分区数据
        /// <summary>
        /// 测厚仪分区数据
        /// </summary>
        /// <param name="result"></param>

        public string GetData()
        {
            //CSystemParameter _sys_param = _icp.Resolve<CSystemParameter>();
            //if (result.StopMachineState)
            //{
            //    return;
            //}
            Tuple<byte[], int> ArealBF = Networks(Partition);
            //开始解析
            byte[] area_g_info = ArealBF.Item1[((24 + 8 + 8 + 4))..^((8 + 4))];
            before_scan_num = BitConverter.ToInt32(area_g_info[4..(4 + 4)]);
            //  result.scan_direction = area_g_info[8];

            //采集到相同的趟数过滤
            if (AcquisitionSystem.PartScanNum == before_scan_num)
            {
                LogHelper.LogHelper.Instance.WriteLog("获取分区趟号重复，过滤的趟数" + before_scan_num, LogType.Warning);
                AcquisitionSystem.RepetiStatic = false;
                return "";
            }
            else
            {
                AcquisitionSystem.RepetiStatic = true;
                AcquisitionSystem.PartScanNum = before_scan_num;
            }

            string[] _cur_data = new string[50];
            List<double> _data_r = new List<double>();

            byte[] area_g = ArealBF.Item1[((24 + 8 + 8 + 4) + 25)..(ArealBF.Item2 - (8 + 4))];
            for (int i = 0; i < 50; i++)
            {
                _cur_data[i] = Math.Round(BitConverter.ToSingle(area_g[(i * 4)..(i * 4 + 4)]), 3).ToString();
                _data_r.Add(Math.Round(BitConverter.ToSingle(area_g[(i * 4)..(i * 4 + 4)]), 3));
            }
            _data_r = _data_r.Where(x => x != 1000000).ToList();
            CPKDynamic cpknew = new();
            string cpk = String.Empty;
            string cov = String.Empty;
            string avg = String.Empty;
            if (_data_r.Count<15) {
                cpk = "0";
                cov = "0";
                avg = "0";
            }
            else {
                cpk = cpknew.GetCPK(_data_r, AcquisitionSystem.WeightUPLimit, AcquisitionSystem.WeightDownLimit).ToString();
                cov = cpknew.GetCov(_data_r).ToString();
                avg = cpknew.Avage(_data_r).ToString();
            }
           
            _cur_data = _cur_data.Select(x => x == "1000000" ? "" : x).ToArray();
            string Part_Cur_data = string.Join(",", _cur_data ) + "," + cpk + "," + cov + "," + avg;
            return Part_Cur_data = Part_Cur_data.Trim();
        }

        private Tuple<byte[], int> Networks(string _cmd)
        {
            try
            {
                _cmd = _cmd.Replace("\r", "").Replace("\n", "").Replace("|", "").Replace(" ", "");
                byte[] _cmd_bytes = BitConverts.HexString2Bytes(_cmd);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.SendTimeout = 300;
                EndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(address_ip), port);
                socket.Connect(iPEndPoint);
                socket.Send(_cmd_bytes);
                byte[] _data_r = new byte[4096];
                int len = socket.Receive(_data_r);

                socket.Close();

                return new Tuple<byte[], int>(_data_r, len);
            }
            catch (Exception e)
            {
                LogHelper.LogHelper.Instance.WriteLog("获取分区数据失败" + e.ToString(), LogType.Error);
                throw e;
            }
        }


        #endregion
    }

}
