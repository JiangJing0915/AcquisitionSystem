using AcquisitionSystem.SettingModel;
using System.Configuration;
using System.Xml;

namespace AcquisitionSystem
{
    public partial class SettingConfiguration : Form
    {
        private SettingConfig settingConfig = new();

        public SettingConfiguration()
        {
            InitializeComponent();
        }

        private void SettingConfiguration_Load(object sender, EventArgs e)
        {
            #region 加载配置文件
            if (ConfigurationManager.AppSettings["CoaterIP"] != null)
            {
                settingConfig.CoaterIP = ConfigurationManager.AppSettings["CoaterIP"].ToString();
                textBoxEx6.Text = settingConfig.CoaterIP;
            }
            if (ConfigurationManager.AppSettings["CoaterPort"] != null)
            {
                settingConfig.CoaterPort = int.Parse(ConfigurationManager.AppSettings["CoaterPort"].ToString());
                textBoxEx7.Text = settingConfig.CoaterPort.ToString();
            }
            if (ConfigurationManager.AppSettings["Pasteviscosity"] != null)
            {
                settingConfig.Pasteviscosity = double.Parse(ConfigurationManager.AppSettings["Pasteviscosity"].ToString());
                textBoxEx1.Text = settingConfig.Pasteviscosity.ToString();
            }
            if (ConfigurationManager.AppSettings["Pastcontent"] != null)
            {
                settingConfig.Pastcontent = double.Parse(ConfigurationManager.AppSettings["Pastcontent"].ToString());
                textBoxEx3.Text = settingConfig.Pastcontent.ToString();
            }
            if (ConfigurationManager.AppSettings["Pasteformula"] != null)
            {
                settingConfig.Pasteformula = ConfigurationManager.AppSettings["Pasteformula"].ToString();
                textBoxEx2.Text = settingConfig.Pasteformula;
            }
            if (ConfigurationManager.AppSettings["Shimthickness"] != null)
            {
                settingConfig.Shimthickness = double.Parse(ConfigurationManager.AppSettings["Shimthickness"].ToString());
                textBoxEx5.Text = settingConfig.Shimthickness.ToString();
            }
            if (ConfigurationManager.AppSettings["AreaIP"] != null)
            {
                settingConfig.AreaIP = ConfigurationManager.AppSettings["AreaIP"].ToString();
                textBoxEx9.Text = settingConfig.AreaIP;
            }
            if (ConfigurationManager.AppSettings["AreaPort"] != null)
            {
                settingConfig.AreaPort = int.Parse(ConfigurationManager.AppSettings["AreaPort"].ToString());
                textBoxEx8.Text = settingConfig.AreaPort.ToString();
            }
            if (ConfigurationManager.AppSettings["TDieIP"] != null)
            {
                settingConfig.TDieIP = ConfigurationManager.AppSettings["TDieIP"].ToString();
                textBoxEx11.Text = settingConfig.TDieIP.ToString();
            }
            if (ConfigurationManager.AppSettings["TDiePort"] != null)
            {
                settingConfig.TDiePort = int.Parse(ConfigurationManager.AppSettings["TDiePort"].ToString());
                textBoxEx10.Text = settingConfig.TDiePort.ToString();
            }
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                //模头

                config.AppSettings.Settings["TDieIP"].Value = textBoxEx11.Text.Trim();

                config.AppSettings.Settings["TDiePort"].Value = textBoxEx10.Text.Trim();

                //测厚仪
                config.AppSettings.Settings["AreaIP"].Value = textBoxEx9.Text.Trim();

                config.AppSettings.Settings["AreaPort"].Value = textBoxEx8.Text.Trim();
                //涂布机
                config.AppSettings.Settings["CoaterIP"].Value = textBoxEx6.Text.Trim();

                config.AppSettings.Settings["CoaterPort"].Value = textBoxEx7.Text.Trim();

                config.AppSettings.Settings["Pasteviscosity"].Value = textBoxEx1.Text.Trim();

                config.AppSettings.Settings["Pastcontent"].Value = textBoxEx3.Text.Trim();

                config.AppSettings.Settings["Pasteformula"].Value = textBoxEx2.Text.Trim();

                config.AppSettings.Settings["Shimthickness"].Value = textBoxEx5.Text.Trim();

                config.Save(ConfigurationSaveMode.Modified);
                //刷新，否则程序读取的还是之前的值（可能已装入内存）
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");

            }
            catch (Exception ex)
            {


            }
            this.Close();
        }

        /// <summary>
        /// 更新配置文件，暂不启用
        /// </summary>
        /// <param name="AppKey"></param>
        /// <param name="AppValue"></param>
        private void UpdateConfig(string AppKey, string AppValue)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                //获取App.config文件绝对路径
                String basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                // basePath = basePath.Substring(, basePath.Length - );
                String path = basePath + "App.config";
                xDoc.Load(path);

                XmlNode xNode;
                XmlElement xElem1;
                XmlElement xElem2;
                //修改完文件内容，还需要修改缓存里面的配置内容，使得刚修改完即可用
                //如果不修改缓存，需要等到关闭程序，在启动，才可使用修改后的配置信息
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                xNode = xDoc.SelectSingleNode("//appSettings");
                xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");
                if (xElem1 != null)
                {
                    xElem1.SetAttribute("value", AppValue);
                    cfa.AppSettings.Settings["AppKey"].Value = AppValue;
                }
                else
                {
                    xElem2 = xDoc.CreateElement("add");
                    xElem2.SetAttribute("key", AppKey);
                    xElem2.SetAttribute("value", AppValue);
                    xNode.AppendChild(xElem2);
                    cfa.AppSettings.Settings.Add(AppKey, AppValue);
                }
                //改变缓存中的配置文件信息（读取出来才会是最新的配置）
                cfa.Save();
                ConfigurationManager.RefreshSection("appSettings");

                xDoc.Save(path);

                //Properties.Settings.Default.Reload();
            }
            catch (Exception e)
            {
                string error = e.Message;

            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }
}
