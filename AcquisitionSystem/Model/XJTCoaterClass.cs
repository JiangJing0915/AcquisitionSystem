using HslCommunication;
using HslCommunication.Profinet.Omron;
using LogHelper;
using System.Configuration;

namespace AcquisitionSystem.Model
{
    internal class XJTCoaterClass
    {
        private string ip_address = "127.0.0.1";

        private int port = 9600;
        private string coater_LineSpeedStand = "5000 | 00 | FF | FF 03 | 00 | 0C 00 | 10 00 | 01 04 | 00 00 | C0 08 00  | A8 | 01 00";

        /// <summary>
        /// 获取涂布机点位信息
        /// </summary>
        /// <param name="_cmd"></param>
        /// <returns></returns>
        #region 新嘉拓涂布机PLC点位
        /*
         实时速度	         D10	 REAL
        设定速度	         D5064	REAL
        实时泵速	         D5030	REAL
        设定泵速	         D6004	REAL
        左刀距实时值	     D6010	REAL
        左刀距设定值	     D6006	REAL
        右刀距实时值	     D6012	REAL
        右刀距设定值	     D6008	REAL
        泵口压力	         D5036	REAL
        涂布压力	         D5032	REAL
        回流压力	         D5034	REAL
        涂布阀打开位置	     D1820	REAL
        涂布阀打开速度	     D1822	REAL
        涂布阀打开加速度	 D1824	REAL
        涂布阀打开减速度	 D1826	REAL
        涂布阀打开所需距离	 D1828	REAL
        涂布阀关闭位置	     D1830	REAL
        涂布阀关闭速度	     D1832	REAL
        涂布阀关闭加速度	 D1834	REAL
        涂布阀关闭减速度	 D1836	REAL
        涂布阀关闭所需距离	 D1838	REAL
        回流阀打开位置	     D1840	REAL
        回流阀打开速度	     D1842	REAL
        回流阀打开加速度	 D1844	REAL
        回流阀打开减速度	 D1846	REAL
        回流阀打开所需距离	 D1848	REAL
        回流阀关闭位置	     D1850	REAL
        回流阀关闭速度	     D1852	REAL
        回流阀关闭加速度	 D1854	REAL
        回流阀关闭减速度	 D1856	REAL
        回流阀关闭所需距离	 D1858	REAL
*/
        #endregion
        private Tuple<double[], int> Networks(string _cmd)
        {
            string AddressIP = ConfigurationManager.AppSettings["CoaterIP"].ToString();

            OmronFinsNet omronFinsNet = new OmronFinsNet(AddressIP, 9600);
            OperateResult connect = omronFinsNet.ConnectServer();
            double[] data_r = new double[4096];
            try
            {
                data_r[0] = omronFinsNet.ReadFloat("D10").Content;
                data_r[1] = omronFinsNet.ReadFloat("D5064").Content;
                data_r[2] = omronFinsNet.ReadFloat("D5030").Content;
                data_r[3] = omronFinsNet.ReadFloat("D6004").Content;
                data_r[4] = omronFinsNet.ReadFloat("D6010").Content;
                data_r[5] = omronFinsNet.ReadFloat("D6006").Content;
                data_r[6] = omronFinsNet.ReadFloat("D6012").Content;
                data_r[7] = omronFinsNet.ReadFloat("D6008").Content;
                data_r[8] = omronFinsNet.ReadFloat("D5036").Content;
                data_r[9] = omronFinsNet.ReadFloat("D5032").Content;
                data_r[10] = omronFinsNet.ReadFloat("D5034").Content;
                int num = 11;
                for (int i = 1820; i < 1860; i += 2)
                {
                    data_r[num] = omronFinsNet.ReadFloat("D" + i.ToString()).Content;
                    num++;
                }

                for (int i = 1920; i <= 1940; i += 2)
                {
                    data_r[num] = omronFinsNet.ReadFloat("D" + i.ToString()).Content;
                    num++;
                }

                int d_len = data_r.Length;
                omronFinsNet.ConnectClose();
                LogHelper.LogHelper.Instance.WriteLog($"curCoter参数, {string.Join(",", data_r)}", LogHelper.LogType.Notice);
                return new Tuple<double[], int>(data_r, d_len);
            }
            catch (Exception e)
            {
                LogHelper.LogHelper.Instance.WriteLog("涂布机连接失败：" + e.ToString(), LogType.Error);
                return new Tuple<double[], int>(data_r, 0);
            }
        }

        public void GetData(ref CoaterDataModel result)
        {
            double[] data_r_c = new double[31];
            Tuple<double[], int> lineSpeedStandResult = Networks(coater_LineSpeedStand);

            if (lineSpeedStandResult.Item2 == 0)
            {
                LogHelper.LogHelper.Instance.WriteLog("测厚仪返回数据为空，连接测厚仪失败！", LogType.Error);
                return;
            }
            result.LineSpeed = lineSpeedStandResult.Item1[0];  //实时速度
            result.LineSpeed_Stand = lineSpeedStandResult.Item1[1]; //设定速度
            result.Pump = lineSpeedStandResult.Item1[2]; //实时泵速
            result.Leftclearance = lineSpeedStandResult.Item1[4]; //左刀距实时值
            result.Rightclearance = lineSpeedStandResult.Item1[6]; //右刀距实时值
            result.PumpRate = lineSpeedStandResult.Item1[8]; //泵压
            result.Coaterpressure = lineSpeedStandResult.Item1[9]; //涂布压力
            result.Backflowpressure = lineSpeedStandResult.Item1[10]; //回流压力

            if (AcquisitionSystem.RepetiStatic)
            {
                //此处解决BUG，如果测厚仪趟数一样会通过这个状态判断获不获取涂布机和T块的数据
                //1、如果执行timer2任务，测厚仪躺号重复，这样就不取涂布机和T块的数据，同事界面要显示涂布机参数信息。
                for (int i = 0; i < 31; i++)
                {
                    data_r_c[i] = Math.Round(lineSpeedStandResult.Item1[i], 3);
                }
                result.TempCoaterDatas = string.Join(",", data_r_c);
            }

            for (int i = 0; i < 31; i++)
            {
                data_r_c[i] = Math.Round(lineSpeedStandResult.Item1[i], 3);
            }

            result.CoaterDatas.Add(string.Join(",", data_r_c));

        }
    }
}
