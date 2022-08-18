using AcquisitionSystem.DataBaseHelp;
using AcquisitionSystem.SettingModel;
using LogHelper;
using System.Collections;
using System.Configuration;
using System.Data;

namespace AcquisitionSystem
{
    public partial class OperateSetting : Form//FrmWithTitle
    {
        private bool UpdateFlag = true;
        private bool AddFlag = true;
        OperateSheetClass operate = new OperateSheetClass();
        //初始化绑定默认关键词（此数据源可以从数据库取）
        List<string> listOnit = new List<string>();
        //输入key之后，返回的关键词
        List<string> listNew = new List<string>();
        public delegate void ChangeFormColor();
        public OperateSetting()
        {
            InitializeComponent();
            ucPanelTitle1.Title = "重量参数";
            ucPanelTitle2.Title = "信息";
        }


        private void OperateSetting_Load(object sender, EventArgs e)
        {
            InitOperAtion();
        }

        ///初始化工艺
        private void InitOperAtion()
        {
            BindCombox();
            bindTreeView1();
        }
        //新增
        private void AddOperAtion()
        {
            DataBaseLayer db = new DataBaseLayer();
            var Insertsql = @"insert into OPERATION_SHEET (Technology_Name
      ,BaseMaterial_Weight
      ,A_Net_Weight
      ,B_Net_Weight
      ,S_Tolerance
      ,S_Warning_Tolerance
      ,D_Tolerance
      ,D_Warning_Tolerance) values
      ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
            try
            {
                var sql = string.Format(Insertsql, tb_1.Text.Trim(),
                    double.Parse(tb_2.Text.Trim()),
                    double.Parse(tb_3.Text.Trim()),
                    double.Parse(tb_4.Text.Trim()),
                    double.Parse(tb_5.Text.Trim()),
                    double.Parse(tb_6.Text.Trim()),
                    double.Parse(tb_7.Text.Trim()),
                    double.Parse(tb_8.Text.Trim()));
                int row = db.ExecuteSql(sql);
                if (row > 0)
                {
                    MessageBox.Show("新增操作单成功！");
                }
                //  MessageBox.Show()
            }
            catch (Exception e)
            {
                LogHelper.LogHelper.Instance.WriteLog("添加操作单工艺失败，原因:" + e.Message, LogType.Error);
                MessageBox.Show(String.Format("添加操作单失败，原因{0}", e.Message));
            }
        }
        //修改
        private void UpdateOperAtion()
        {
            try
            {
                DataBaseLayer db = new DataBaseLayer();
                var updatesql = @"update  [CloseLoopDataBase].[dbo].[OPERATION_SHEET] set BaseMaterial_Weight ='{1}'
                                 , A_Net_Weight='{2}'
                                 , B_Net_Weight='{3}' 
                                 , S_Tolerance='{4}' 
                                 , S_Warning_Tolerance='{5}'
                                 , D_Tolerance='{6}'
                                 , D_Warning_Tolerance='{7}'
                                 where  [Technology_Name] ='{0}'";
                var sql = string.Format(updatesql, tb_1.Text.Trim(),
                    double.Parse(tb_2.Text.Trim()),
                    double.Parse(tb_3.Text.Trim()),
                    double.Parse(tb_4.Text.Trim()),
                    double.Parse(tb_5.Text.Trim()),
                    double.Parse(tb_6.Text.Trim()),
                    double.Parse(tb_7.Text.Trim()),
                    double.Parse(tb_8.Text.Trim()));

                int row = db.ExecuteSql(sql);
                if (row > 0)
                {
                    MessageBox.Show("更新修改操作单成功！");
                }
            }
            catch (Exception e)
            {

            }
        }
        //保存
        private void SaveOperAtion() { }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                //判断工艺名称是否一样，一样就不增加
                DataBaseLayer db = new DataBaseLayer();
                DataTable dt;
                var sql =
                    "select Technology_Name from [CloseLoopDataBase].[dbo].[OPERATION_SHEET] where Technology_Name = '{0}'";

                sql = String.Format(sql, tb_1.Text.Trim());
                dt = db.ExecuteQuery(sql);
                if (dt.Rows.Count > 0)
                {
                    string TechnologyName = dt.Rows[0]["Technology_Name"].ToString();
                    MessageBox.Show(String.Format("品种【'{0}'】已经存在，请点击修改按钮更新!", TechnologyName));
                    return;

                }
                else AddOperAtion();
            }
            catch (Exception exception)
            {

            }
        }

        #region
        private void tb_1_TextChanged(object sender, EventArgs e)
        {
            operate.Technology_Name = tb_1.Text;
        }

        private void tb_2_TextChanged(object sender, EventArgs e)
        {
            operate.BaseMaterial_Weight = double.Parse(tb_2.Text);
        }

        private void tb_3_TextChanged(object sender, EventArgs e)
        {
            operate.A_Net_Weight = double.Parse(tb_3.Text);
        }

        private void tb_4_TextChanged(object sender, EventArgs e)
        {
            operate.B_Net_Weight = double.Parse(tb_4.Text);
        }

        private void tb_5_TextChanged(object sender, EventArgs e)
        {
            operate.S_Tolerance = double.Parse(tb_5.Text);
        }

        private void tb_6_TextChanged(object sender, EventArgs e)
        {
            operate.S_Warning_Tolerance = double.Parse(tb_6.Text);
        }

        private void tb_7_TextChanged(object sender, EventArgs e)
        {
            operate.D_Tolerance = double.Parse(tb_7.Text);
        }

        private void tb_8_TextChanged(object sender, EventArgs e)
        {
            operate.D_Warning_Tolerance = double.Parse(tb_8.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tb_1.Clear();
            tb_2.Clear();
            tb_3.Clear();
            tb_4.Clear();
            tb_5.Clear();
            tb_6.Clear();
            tb_7.Clear();
            tb_8.Clear();
        }
        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataBaseLayer db = new DataBaseLayer();
                DataTable dt;
                var sql =
                    "select Technology_Name from [CloseLoopDataBase].[dbo].[OPERATION_SHEET] where Technology_Name = '{0}'";

                sql = String.Format(sql, tb_1.Text.Trim());
                dt = db.ExecuteQuery(sql);
                if (dt.Rows.Count <= 0)
                {
                    // string TechnologyName = dt.Rows[0]["Technology_Name"].ToString();
                    MessageBox.Show(String.Format("品种【'{0}'】不存在，请点击新增按钮添加!", tb_1.Text));
                    return;
                }

                UpdateOperAtion();

            }
            catch (Exception exception)
            {

            }
        }

        private void BindCombox()
        {
            try
            {
                DataBaseLayer db = new DataBaseLayer();
                DataTable dt;

                var sql =
                    "select Technology_Name from [CloseLoopDataBase].[dbo].[OPERATION_SHEET]";

                dt = db.ExecuteQuery(sql);
                if (dt.Rows.Count <= 0)
                {
                    // string TechnologyName = dt.Rows[0]["Technology_Name"].ToString();
                    MessageBox.Show("加载品种出错，请关闭窗口重新打开，或者联系管理员处理（17775249070）");
                    return;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    listOnit.Add(dt.Rows[i]["Technology_Name"].ToString());
                }
                this.comboBox1.Items.AddRange(listOnit.ToArray());
            }
            catch (Exception e)
            {

            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataBaseLayer db = new DataBaseLayer();
                DataTable dt;
                var sql =
                    "select * from [CloseLoopDataBase].[dbo].[OPERATION_SHEET] where Technology_Name = '{0}'";

                sql = String.Format(sql, comboBox1.Text.Trim());
                dt = db.ExecuteQuery(sql);
                if (dt.Rows.Count > 0)
                {
                    string TechnologyName = dt.Rows[0]["Technology_Name"].ToString();
                    string BaseMaterial_Weight = dt.Rows[0]["BaseMaterial_Weight"].ToString();
                    string A_Net_Weight = dt.Rows[0]["A_Net_Weight"].ToString();
                    string B_Net_Weight = dt.Rows[0]["B_Net_Weight"].ToString();
                    string S_ToleranceeMaterial_Weight = dt.Rows[0]["S_Tolerance"].ToString();
                    string S_Warning_Tolerance = dt.Rows[0]["S_Warning_Tolerance"].ToString();
                    string D_Tolerance = dt.Rows[0]["D_Tolerance"].ToString();
                    string D_Warning_Tolerance = dt.Rows[0]["D_Warning_Tolerance"].ToString();
                    tb_9.Text = "品种：" + TechnologyName + "          " + "基材重量:" + BaseMaterial_Weight + "\r\n"
                                + "A面净重:" + A_Net_Weight
                                + "          " + "B面净重:" + B_Net_Weight + "\r\n"
                                + "单面公差:" + S_ToleranceeMaterial_Weight
                                + "          " + "单面预警公差:" + S_Warning_Tolerance + "\r\n"
                                + "双面公差:" + D_Tolerance
                                + "          " + "双面预警公差:" + D_Warning_Tolerance + "\r\n";
                }
            }
            catch (Exception exception)
            {

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            double Tolerance = 0.0;
            double WarningTolerance = 0.0;
            double TargetValue = 0.0;
            string strflag = string.Empty;
            try
            {
                DataBaseLayer db = new DataBaseLayer();
                DataTable dt;
                var sql =
                    "select * from [CloseLoopDataBase].[dbo].[OPERATION_SHEET] where Technology_Name = '{0}'";

                sql = String.Format(sql, comboBox1.Text.Trim());
                dt = db.ExecuteQuery(sql);
                if (comboBox1.Text.Trim().Contains("-"))
                {
                    string[] parts = comboBox1.Text.Trim().Split('-');
                    strflag = parts[parts.Length - 1];
                }
                if (dt.Rows.Count > 0)
                {

                    string TechnologyName = dt.Rows[0]["Technology_Name"].ToString();
                    string BaseMaterial_Weight = dt.Rows[0]["BaseMaterial_Weight"].ToString();
                    string A_Net_Weight = dt.Rows[0]["A_Net_Weight"].ToString();
                    string B_Net_Weight = dt.Rows[0]["B_Net_Weight"].ToString();
                    string S_ToleranceeMaterial_Weight = dt.Rows[0]["S_Tolerance"].ToString();
                    string S_Warning_Tolerance = dt.Rows[0]["S_Warning_Tolerance"].ToString();
                    string D_Tolerance = dt.Rows[0]["D_Tolerance"].ToString();
                    string D_Warning_Tolerance = dt.Rows[0]["D_Warning_Tolerance"].ToString();
                   // AcquisitionSystem.Technology_Name = TechnologyName;
                    if (strflag == "S")
                    {
                        TargetValue = double.Parse(BaseMaterial_Weight) + double.Parse(A_Net_Weight);
                      //  AcquisitionSystem.TargetValue = TargetValue;
                        Tolerance = double.Parse(S_ToleranceeMaterial_Weight);
                      //  AcquisitionSystem.Tolerance = Tolerance;
                        WarningTolerance = double.Parse(S_Warning_Tolerance);
                        //  AcquisitionSystem.WarningTolerance = WarningTolerance;
                        WriteConfig(TechnologyName,TargetValue,Tolerance, WarningTolerance);
                    }

                    if (strflag == "D")
                    {
                        TargetValue = double.Parse(BaseMaterial_Weight) + double.Parse(A_Net_Weight) + double.Parse(B_Net_Weight);
                       // AcquisitionSystem.TargetValue = TargetValue;
                        Tolerance = double.Parse(D_Tolerance);
                       // AcquisitionSystem.Tolerance = Tolerance;
                        WarningTolerance = double.Parse(D_Warning_Tolerance);
                        // AcquisitionSystem.WarningTolerance = WarningTolerance;
                        WriteConfig(TechnologyName, TargetValue, Tolerance, WarningTolerance);
                    }
                    ArrayList aList = new ArrayList();

                    string sql1 = @"update [CloseLoopDataBase].[dbo].[CHARTLINE] set  LINE = '" + TargetValue +
                                  " ' WHERE ID=1";
                    aList.Add(sql1);
                    sql1 = @"update [CloseLoopDataBase].[dbo].[CHARTLINE] set  LINE = '" + Tolerance +
                           "' WHERE ID=2";
                    aList.Add(sql1);
                    sql1 = @"update [CloseLoopDataBase].[dbo].[CHARTLINE] set  LINE = '" + WarningTolerance +
                           "' WHERE ID=3";
                    aList.Add(sql1);
                    sql1 = @"update [CloseLoopDataBase].[dbo].[CHARTLINE] set  LINE = '" + TechnologyName +
                           "' WHERE ID=4";
                    aList.Add(sql1);
                    db.ExecuteSqlTran(aList);
                    MessageBox.Show("选择成功");
                }

            }
            catch (Exception exception)
            {

            }

        }

        private void WriteConfig(string Technology_Name,double TargetValue,double Tolerance,double WarningTolerance) {
            try
            {
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings["Technology_Name"].Value = Technology_Name.ToString();

                config.AppSettings.Settings["TargetValue"].Value = TargetValue.ToString();

             
                config.AppSettings.Settings["Tolerance"].Value = Tolerance.ToString();

                config.AppSettings.Settings["WarningTolerance"].Value = WarningTolerance.ToString();
             
                config.Save(ConfigurationSaveMode.Modified);
                //刷新，否则程序读取的还是之前的值（可能已装入内存）
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");

            }
            catch (Exception ex)
            {


            }
            this.Close();
        }
        private void bindTreeView1()
        {
            DataBaseLayer db = new DataBaseLayer();
            DataTable dt;
            string sql = "select Technology_Name from [CloseLoopDataBase].[dbo].[OPERATION_SHEET]";
            try
            {
                dt = db.ExecuteQuery(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = dt.Rows[i]["Technology_Name"].ToString();
                    tn.Tag = "1";
                    treeViewEx1.Nodes.Add(tn);
                }
            }
            catch (Exception e)
            {

            }
        }

        private void treeViewEx1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var nodetext = e.Node.Text.ToString();
            DataBaseLayer db = new DataBaseLayer();
            DataTable dt;
            string sql = "select * from [CloseLoopDataBase].[dbo].[OPERATION_SHEET] where Technology_Name='" + nodetext + "'";
            try
            {
                dt = db.ExecuteQuery(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    tb_1.Text = dt.Rows[i]["Technology_Name"].ToString();
                    tb_2.Text = dt.Rows[i]["BaseMaterial_Weight"].ToString();
                    tb_3.Text = dt.Rows[i]["A_Net_Weight"].ToString();
                    tb_4.Text = dt.Rows[i]["B_Net_Weight"].ToString();
                    tb_5.Text = dt.Rows[i]["S_Tolerance"].ToString();
                    tb_6.Text = dt.Rows[i]["S_Warning_Tolerance"].ToString();
                    tb_7.Text = dt.Rows[i]["D_Tolerance"].ToString();
                    tb_8.Text = dt.Rows[i]["D_Warning_Tolerance"].ToString();
                }
            }
            catch (Exception exception)
            {

            }
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                //清空combobox
                this.comboBox1.Items.Clear();
                //清空listNew
                listNew.Clear();
                //遍历全部备查数据
                foreach (var item in listOnit)
                {
                    if (item.Contains(this.comboBox1.Text))
                    {
                        //符合，插入ListNew
                        listNew.Add(item);
                    }
                }
                //combobox添加已经查到的关键词
                this.comboBox1.Items.AddRange(listNew.ToArray());
                //设置光标位置，否则光标位置始终保持在第一列，造成输入关键词的倒序排列
                this.comboBox1.SelectionStart = this.comboBox1.Text.Length;
                //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
                Cursor = Cursors.Default;
                //自动弹出下拉框
                this.comboBox1.DroppedDown = true;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
