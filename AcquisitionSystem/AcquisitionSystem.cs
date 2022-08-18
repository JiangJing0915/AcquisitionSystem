using AcquisitionSystem.Model;
using AcquisitionSystem.SettingModel;
using HZH_Controls.Controls;
using LogHelper;
using System.Configuration;

namespace AcquisitionSystem
{
    public partial class AcquisitionSystem : Form
    {
        public static int ScanNum = 0;
        public static int PartScanNum = 0;
        public static int startSignal = 0;
        public static bool RepetiStatic = true;
        SettingConfig setting = new SettingConfig();
        private List<string> TDieDATA = new List<string>();
        private List<string> AreaDATA = new List<string>();
        private List<string> CoterDAtA = new List<string>();
        List<string> TotalList = new List<string>();

        private int flag = 0;
        public static double WeightUPLimit = 0.0;
        public  static double WeightDownLimit = 0.0;

        public static string Technology_Name = string.Empty; //品种
        public static double TargetValue = 0.0;
        public static double Tolerance = 0.0;
        public static double WarningTolerance = 0.0;

        public AcquisitionSystem()
        {
            InitializeComponent();
        }
        bool Stop = false;//标志位


        private void Main_load(object sender, EventArgs e)
        {

            LogHelper.LogHelper.Instance.Start();
            #region 加载配置文件


            if (ConfigurationManager.AppSettings["TcpIP"] != null)
            {
                setting.TcpIP = ConfigurationManager.AppSettings["TcpIP"].ToString();
                textBox1.Text = setting.TcpIP;
            }
            if (ConfigurationManager.AppSettings["TcpPort"] != null)
            {
                setting.TcpPort = int.Parse(ConfigurationManager.AppSettings["TcpPort"].ToString());
                textBox2.Text = setting.TcpPort.ToString();
            }
            if (ConfigurationManager.AppSettings["TDieIP"] != null)
            {
                setting.TDieIP = ConfigurationManager.AppSettings["TDieIP"].ToString();

            }
            if (ConfigurationManager.AppSettings["TDiePort"] != null)
            {
                setting.TDiePort = int.Parse(ConfigurationManager.AppSettings["TDiePort"].ToString());

            }

            #endregion




            DataViewTest();
            //string address = textBox1.Text.ToString().Trim();
            //int port = int.Parse(textBox2.Text.Trim());
            //client = new ChatClient(address, port);
            //client.ConnectAsync();
            //  button1.PerformClick();  //按钮自动点击
        }
        private void button1_Click(object sender, EventArgs e)
        {
            LogHelper.LogHelper.Instance.WriteLog("开启采集系统程序", LogType.Notice);
            string address = textBox1.Text.ToString().Trim();
            int port = int.Parse(textBox2.Text.Trim());
            client = new ChatClient(address, port);
            client.ConnectAsync();
            if (button1.Text == "开启")
            {
                button1.Text = "关闭";
                timer1.Enabled = true;
                timer2.Enabled = true;

            }
            else
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
                client.DisconnectAndStop();
                button1.Text = "开启";
                TDieDATA.Clear();
                AreaDATA.Clear();
                CoterDAtA.Clear();
                label9.Text = "涂布机状态";
                label9.ForeColor = Color.White;

            }

            //client.ConnectAsync();

            //client.SendAsync("1111");
            //client.DisconnectAndStop();

        }

        private ChatClient client;

        /// <summary>
        /// 获取毫米数据轮询任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                LogHelper.LogHelper.Instance.WriteLog("启动轮询任务", LogType.Notice);
                Application.DoEvents();

                DaChenAreaDensity daChenArea = new DaChenAreaDensity();
                AreaModel areahelp1 = new AreaModel();
                string json = daChenArea.GetmmData();
                if (json == "")
                {
                    LogHelper.LogHelper.Instance.WriteLog("趟号重复，导致结果为NULL", LogType.Warning);
                    return;
                }
                textBox3.Text = ScanNum.ToString();
                //   string json = "{\"COMMAND\":\"3\",\"ESNUM\":\"2,3\",\"CODE\":\"\",\"MSG\":\"\",\"DATATYPE\":\"1\",\"COUPLINGFB\":\"0\",\"DATA\":[\"0.00\",\"],\"IDNUM\":\"564\",\"KADJNUM\":\"0.98\",\"TARNUM\":\"300\",\"SPEUNUM\":\"310\",\"SPEDNUM\":\"290\",\"CONUNUM\":\"305\",\"CONDNUM\":\"295\",\"PARNUM\":\"1600\",\"TIME\":\"2022-07-08 15:08:14\",\"DeviceNO\":\"2\"}";

                //string json = "{\"COMMAND\":\"3\",\"ESNUM\":\"2,3\",\"CODE\":\"\",\"MSG\":\"\",\"DATATYPE\":\"1\",\"COUPLINGFB\":\"0\",\"DATA\":[\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"-39.728\",\"-39.347\",\"-38.868\",\"-38.35\",\"-37.74\",\"-36.868\",\"-34.777\",\"-31.736\",\"-27.661\",\"-22.948\",\"-17.65\",\"-11.26\",\"-4.639\",\"2.801\",\"10.951\",\"19.487\",\"29.395\",\"39.646\",\"48.705\",\"60.096\",\"72.02\",\"84.398\",\"98.704\",\"114.637\",\"132.254\",\"151.849\",\"172.17\",\"193.103\",\"217.694\",\"242.26\",\"267.274\",\"294.399\",\"321.42\",\"343.011\",\"349.931\",\"350.258\",\"350.412\",\"350.677\",\"350.899\",\"351.015\",\"351.344\",\"351.828\",\"351.974\",\"352.472\",\"352.672\",\"352.591\",\"352.859\",\"352.906\",\"353.223\",\"353.116\",\"353.152\",\"353.371\",\"353.384\",\"353.713\",\"353.821\",\"354.171\",\"354.076\",\"354.268\",\"354.336\",\"354.467\",\"354.635\",\"354.349\",\"354.076\",\"354.071\",\"354.039\",\"353.909\",\"353.81\",\"353.702\",\"353.579\",\"353.757\",\"353.655\",\"353.857\",\"353.702\",\"353.759\",\"353.76\",\"353.72\",\"353.498\",\"353.497\",\"353.342\",\"353.397\",\"352.943\",\"352.981\",\"353.268\",\"353.452\",\"353.617\",\"354.013\",\"354.226\",\"354.182\",\"354.174\",\"354.089\",\"353.815\",\"353.912\",\"353.658\",\"353.303\",\"352.839\",\"352.705\",\"352.455\",\"352.623\",\"352.408\",\"352.149\",\"351.912\",\"352.03\",\"352.02\",\"352.123\",\"351.816\",\"351.726\",\"351.285\",\"350.907\",\"351.098\",\"350.958\",\"350.507\",\"350.11\",\"350.4\",\"350.271\",\"349.914\",\"349.853\",\"348.445\",\"345.896\",\"339.543\",\"320.551\",\"267.233\",\"204.84\",\"149.827\",\"108.66\",\"76.736\",\"52.435\",\"30.911\",\"10.293\",\"-2.537\",\"-13.762\",\"-19.117\",\"-19.608\",\"-19.64\",\"-19.668\",\"-19.573\",\"-19.569\",\"-19.116\",\"-15.366\",\"-6.634\",\"6.067\",\"19.971\",\"38.007\",\"56.639\",\"79.275\",\"103.259\",\"132.23\",\"158.581\",\"182.349\",\"190.359\",\"193.065\",\"194.962\",\"195.736\",\"196.152\",\"196.241\",\"196.323\",\"196.353\",\"196.7\",\"197.014\",\"197.044\",\"196.894\",\"197.063\",\"196.974\",\"196.973\",\"196.885\",\"196.771\",\"196.796\",\"196.932\",\"196.875\",\"197.146\",\"197.156\",\"197.063\",\"197.285\",\"197.585\",\"197.665\",\"197.595\",\"197.44\",\"197.264\",\"197.339\",\"197.354\",\"197.321\",\"197.148\",\"197.121\",\"197.083\",\"197.009\",\"197.155\",\"197.117\",\"197.367\",\"197.44\",\"197.563\",\"197.732\",\"197.774\",\"197.797\",\"197.888\",\"197.999\",\"198.668\",\"200.654\",\"206.622\",\"216.01\",\"228.506\",\"242.137\",\"256.729\",\"272.271\",\"290.11\",\"310.899\",\"332.085\",\"345.284\",\"354.103\",\"355.732\",\"356.032\",\"356.141\",\"356.2\",\"356.003\",\"356.045\",\"355.746\",\"355.89\",\"355.766\",\"355.72\",\"356.038\",\"355.717\",\"355.829\",\"356.013\",\"355.989\",\"355.828\",\"356.047\",\"356.097\",\"356.108\",\"356.065\",\"355.924\",\"356.322\",\"356.181\",\"356.386\",\"356.357\",\"356.061\",\"356.268\",\"356.449\",\"356.133\",\"356.341\",\"355.886\",\"356.163\",\"355.773\",\"355.928\",\"355.703\",\"355.531\",\"355.828\",\"355.558\",\"355.665\",\"355.754\",\"355.869\",\"356.133\",\"356.262\",\"355.94\",\"356.14\",\"355.987\",\"356.171\",\"356.095\",\"356.356\",\"356.315\",\"356.203\",\"356.387\",\"356.266\",\"356.51\",\"356.476\",\"356.791\",\"356.607\",\"356.836\",\"356.651\",\"356.479\",\"356.403\",\"356.324\",\"356.258\",\"356.318\",\"355.956\",\"355.911\",\"355.973\",\"355.783\",\"355.641\",\"355.694\",\"355.785\",\"355.787\",\"355.719\",\"355.719\",\"355.662\",\"355.644\",\"355.581\",\"355.278\",\"355.344\",\"355.437\",\"355.535\",\"355.478\",\"355.658\",\"355.459\",\"355.638\",\"355.767\",\"355.717\",\"355.768\",\"355.58\",\"355.508\",\"355.468\",\"355.405\",\"355.094\",\"355.238\",\"355.164\",\"355.003\",\"355.059\",\"355.149\",\"355.417\",\"355.451\",\"355.577\",\"355.536\",\"355.48\",\"355.596\",\"355.642\",\"355.695\",\"355.507\",\"355.352\",\"355.451\",\"355.473\",\"355.381\",\"355.615\",\"355.526\",\"355.498\",\"355.755\",\"355.412\",\"355.825\",\"355.572\",\"355.6\",\"355.526\",\"355.467\",\"355.236\",\"355.154\",\"355.238\",\"355.018\",\"355.021\",\"354.935\",\"355.007\",\"355.038\",\"355.145\",\"355.152\",\"355.102\",\"355.082\",\"355.187\",\"355.14\",\"355.139\",\"355.229\",\"355.333\",\"354.995\",\"355.068\",\"355.085\",\"355.128\",\"355.142\",\"355.076\",\"355.092\",\"354.904\",\"355.111\",\"355.007\",\"355.022\",\"355.095\",\"354.939\",\"355.077\",\"355.072\",\"355.003\",\"355.211\",\"355.192\",\"355.469\",\"355.543\",\"355.757\",\"355.911\",\"355.9\",\"355.785\",\"355.932\",\"355.947\",\"355.895\",\"355.919\",\"355.651\",\"355.697\",\"355.9\",\"355.683\",\"355.635\",\"355.656\",\"355.52\",\"355.808\",\"356.108\",\"356.182\",\"355.86\",\"355.909\",\"355.672\",\"355.483\",\"355.586\",\"355.545\",\"355.533\",\"355.256\",\"355.076\",\"354.911\",\"354.778\",\"355.096\",\"355.182\",\"355.321\",\"355.457\",\"355.484\",\"355.669\",\"355.739\",\"355.702\",\"355.451\",\"355.364\",\"355.624\",\"355.466\",\"355.77\",\"355.874\",\"356.04\",\"355.988\",\"356.263\",\"356.008\",\"356.112\",\"356.522\",\"356.245\",\"356.309\",\"356.044\",\"356.155\",\"356.073\",\"356.142\",\"356.301\",\"356.381\",\"356.332\",\"356.336\",\"356.639\",\"356.666\",\"356.749\",\"357.089\",\"356.775\",\"356.61\",\"356.571\",\"356.39\",\"356.689\",\"356.604\",\"356.376\",\"356.487\",\"356.189\",\"356.382\",\"356.653\",\"356.786\",\"356.879\",\"356.818\",\"356.91\",\"356.795\",\"356.702\",\"356.604\",\"356.663\",\"356.489\",\"356.339\",\"356.365\",\"356.252\",\"356.306\",\"356.167\",\"356.066\",\"355.91\",\"355.84\",\"355.689\",\"355.894\",\"356.102\",\"355.848\",\"355.741\",\"355.806\",\"355.706\",\"355.685\",\"355.797\",\"355.623\",\"355.679\",\"355.708\",\"355.591\",\"355.774\",\"355.73\",\"355.919\",\"355.446\",\"355.409\",\"355.333\",\"355.119\",\"355.121\",\"354.863\",\"354.731\",\"354.929\",\"354.709\",\"355.078\",\"355.257\",\"355.38\",\"355.429\",\"355.519\",\"355.377\",\"355.285\",\"355.354\",\"355.234\",\"355.131\",\"354.819\",\"354.598\",\"354.385\",\"354.452\",\"354.538\",\"354.446\",\"354.385\",\"354.436\",\"354.424\",\"354.787\",\"354.49\",\"354.706\",\"354.565\",\"354.608\",\"354.521\",\"354.257\",\"354.113\",\"353.968\",\"353.586\",\"353.608\",\"353.541\",\"353.42\",\"353.452\",\"353.375\",\"353.479\",\"353.196\",\"352.991\",\"352.99\",\"352.804\",\"352.73\",\"352.532\",\"352.374\",\"352.292\",\"352.18\",\"352.223\",\"352.215\",\"352.33\",\"352.393\",\"352.742\",\"352.276\",\"352.336\",\"352.309\",\"352.273\",\"352.035\",\"351.793\",\"351.656\",\"351.487\",\"351.724\",\"351.47\",\"351.462\",\"351.946\",\"351.777\",\"351.961\",\"352.068\",\"351.833\",\"351.716\",\"351.688\",\"351.199\",\"350.951\",\"350.359\",\"349.932\",\"349.972\",\"349.733\",\"349.307\",\"348.768\",\"347.089\",\"343.583\",\"336.64\",\"309.135\",\"248.39\",\"188.025\",\"139.887\",\"100.355\",\"71.629\",\"47.713\",\"25.6\",\"7.994\",\"-6.47\",\"-16.423\",\"-19.425\",\"-19.581\",\"-19.574\",\"-19.51\",\"-19.473\",\"-19.399\",\"-18.164\",\"-11.691\",\"-1.351\",\"14.58\",\"27.757\",\"46.5\",\"64.603\",\"86.962\",\"112.303\",\"142.277\",\"168.675\",\"186.268\",\"191.459\",\"193.992\",\"195.753\",\"196.473\",\"196.903\",\"197.048\",\"196.986\",\"197.089\",\"197.471\",\"197.67\",\"197.705\",\"197.817\",\"197.972\",\"198.152\",\"198.376\",\"198.435\",\"198.321\",\"198.253\",\"198.051\",\"197.934\",\"197.847\",\"197.688\",\"197.681\",\"197.564\",\"197.557\",\"197.582\",\"197.528\",\"197.531\",\"197.583\",\"197.611\",\"197.727\",\"197.701\",\"197.733\",\"197.618\",\"197.637\",\"197.635\",\"197.626\",\"197.609\",\"197.622\",\"197.787\",\"197.954\",\"198.119\",\"198.129\",\"198.318\",\"198.378\",\"198.495\",\"199.025\",\"201.529\",\"208.311\",\"217.768\",\"229.946\",\"244.343\",\"259.202\",\"274.118\",\"291.775\",\"312.395\",\"332.03\",\"347.237\",\"353.613\",\"355.264\",\"355.862\",\"355.894\",\"355.889\",\"355.795\",\"355.874\",\"355.732\",\"355.749\",\"355.544\",\"355.501\",\"355.655\",\"356.166\",\"356.302\",\"356.242\",\"355.956\",\"355.986\",\"355.781\",\"355.64\",\"355.247\",\"355.483\",\"355.483\",\"355.722\",\"356.44\",\"356.626\",\"357.165\",\"357.197\",\"357.434\",\"357.287\",\"357.257\",\"356.989\",\"356.704\",\"356.74\",\"356.788\",\"356.754\",\"356.941\",\"357.087\",\"357.063\",\"357.139\",\"356.93\",\"356.823\",\"356.711\",\"356.874\",\"356.954\",\"356.857\",\"357.058\",\"357.177\",\"357.239\",\"357.311\",\"357.111\",\"357.145\",\"356.978\",\"357.064\",\"357.096\",\"357.367\",\"357.381\",\"357.238\",\"356.885\",\"356.676\",\"356.242\",\"356.466\",\"356.695\",\"356.949\",\"356.921\",\"357.112\",\"356.695\",\"356.501\",\"356.238\",\"355.936\",\"355.777\",\"355.635\",\"355.273\",\"354.918\",\"354.614\",\"354.253\",\"353.881\",\"352.323\",\"345.374\",\"329.093\",\"306.055\",\"282.387\",\"260.306\",\"235.042\",\"213.732\",\"193.569\",\"174.115\",\"156.231\",\"134.775\",\"130.32\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\",\"0\"],\"IDNUM\":\"564\",\"KADJNUM\":\"0.98\",\"TARNUM\":\"300\",\"SPEUNUM\":\"310\",\"SPEDNUM\":\"290\",\"CONUNUM\":\"305\",\"CONDNUM\":\"295\",\"PARNUM\":\"1600\",\"TIME\":\"2022-07-08 15:08:14\",\"DeviceNO\":\"2\"}";
                LogHelper.LogHelper.Instance.WriteLog("发送到闭环中的数据[" + json + "]", LogType.Warning);
                client.SendAsync(json);
            }
            catch (Exception ex)
            {
                timer1.Stop();
                timer2.Stop();
                timer1.Enabled = false;
                timer2.Enabled = false;
                button1.Text = "开启";
                TDieDATA.Clear();
                AreaDATA.Clear();
                CoterDAtA.Clear();
                label9.Text = "涂布机状态";
                label9.ForeColor = Color.Red;
                LogHelper.LogHelper.Instance.WriteLog("Timer1异常：" + ex.Message, LogType.Error);
                MessageBox.Show(ex.ToString());
            }
        }

        private void OnTimerEvent_CSVTask(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                List<double> TDie_position = new List<double>();
                TDie_position.Clear();
                LogHelper.LogHelper.Instance.WriteLog("启动生成CSV对位任务", LogType.Notice);
                Application.DoEvents();
                AnYunTDieClass anYunTDieClass = new AnYunTDieClass();


                TDie_position = anYunTDieClass.GetTDieData(setting.TDieIP, setting.TDiePort);
                List<object> lstSource = new List<object>();
                TDieModel model = new TDieModel()
                {
                    T1 = TDie_position[0],
                    T2 = TDie_position[1],
                    T3 = TDie_position[2],
                    T4 = TDie_position[3],
                    T5 = TDie_position[4],
                    T6 = TDie_position[5],
                    T7 = TDie_position[6],
                    T8 = TDie_position[7],
                    T9 = TDie_position[8],
                    T10 = TDie_position[9],
                    T11 = TDie_position[10],
                };
                lstSource.Add(model);
                this.ucDataGridView1.DataSource = lstSource;
                this.ucDataGridView1.First();
            }
            catch (Exception ex)
            {

                LogHelper.LogHelper.Instance.WriteLog(string.Format("CSV任务定时提醒异常：{0}", ex.Message), LogType.Error);

            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DaChenAreaDensity daChenArea = new DaChenAreaDensity();
            CoaterDataModel coaterDataModel = new CoaterDataModel();
            XJTCoaterClass xJTCoaterClass = new XJTCoaterClass();

            string strflag = string.Empty;
            #region
            Technology_Name = ConfigurationManager.AppSettings["Technology_Name"].ToString();
            TargetValue = double.Parse(ConfigurationManager.AppSettings["TargetValue"].ToString());
            WarningTolerance = double.Parse(ConfigurationManager.AppSettings["WarningTolerance"].ToString());
            Tolerance = double.Parse(ConfigurationManager.AppSettings["Tolerance"].ToString());
            WeightUPLimit = TargetValue + Tolerance;
            WeightDownLimit =TargetValue - Tolerance;
        
            #endregion

            string filePath = @"D:\Data\" + Technology_Name + "-" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            try
            {
                TotalList.Clear();
                string TempAreaData = daChenArea.GetData();
                if (TempAreaData.Length > 0)
                {
                    AreaDATA.Add(TempAreaData);
                }
                xJTCoaterClass.GetData(ref coaterDataModel);//涂布机数据
                if (RepetiStatic)
                {
                    CoterDAtA.Add(coaterDataModel.TempCoaterDatas);
                    TDieDATA.Add(SetColumn()); //测厚仪数据
                    //读取涂布机
                }
                label5.Text = coaterDataModel.LineSpeed_Stand.ToString("f2");
                label7.Text = coaterDataModel.LineSpeed.ToString("f2");
                label12.Text = coaterDataModel.Pump.ToString("f2");
                label14.Text = coaterDataModel.PumpRate.ToString("f2");

                label21.Text = coaterDataModel.Leftclearance.ToString("f2");
                label19.Text = coaterDataModel.Rightclearance.ToString("f2");
                label17.Text = coaterDataModel.Coaterpressure.ToString("f2");
                label15.Text = coaterDataModel.Backflowpressure.ToString("f2");
                label24.Text = coaterDataModel.Pastetemperature.ToString("f2");
                if (coaterDataModel.LineSpeed <= coaterDataModel.LineSpeed_Stand * 0.5)
                {
                    label9.Text = "涂布机停机";
                    label9.ForeColor = Color.White;
                    AreaDATA.Clear();
                    CoterDAtA.Clear();
                    TDieDATA.Clear();
                    TotalList.Clear();
                    flag = 0;
                    LogHelper.LogHelper.Instance.WriteLog("涂布机停机，当前：" + coaterDataModel.LineSpeed, LogType.Error);
                    return;
                }
                else
                {
                    label9.Text = "正常运行";
                    label9.ForeColor = Color.Red;

                    if (flag == 0)
                    {
                        if (AreaDATA.Count() == 19)
                        {
                            AreaDATA.RemoveRange(0, 19);
                            flag++;
                        }
                    }
                    else
                    {
                      
                        if (AreaDATA.Count() > 0)
                        {
                            for (int i = 0; i < AreaDATA.Count(); i++)
                            {
                                TotalList.Add(DateTime.Now.ToString("yyyy/MM/dd") + "," + DateTime.Now.ToString("HH:mm:ss:ffff") + "," + AcquisitionSystem.PartScanNum + ","
                                     + TargetValue + ","
                                     + Tolerance + ","
                                     + WarningTolerance + ","
                                     + WeightUPLimit + ","
                                     + WeightDownLimit + ","
                                     + CoterDAtA[i] + ","
                                     + ConfigurationManager.AppSettings["Pasteviscosity"] + ","
                                     + ConfigurationManager.AppSettings["Pastcontent"] + "," + ConfigurationManager.AppSettings["Shimthickness"] + ","
                                     + ConfigurationManager.AppSettings["Pasteformula"] + ","
                                     + TDieDATA[i] + ","
                                     + AreaDATA[i]);
                            }
                            TDieDATA.RemoveRange(0, AreaDATA.Count());
                            CoterDAtA.RemoveRange(0, AreaDATA.Count());
                            AreaDATA.RemoveRange(0, AreaDATA.Count());
                            SaveCSV(TotalList, filePath);
                        }
                    }
                }
                //丢数据对应T块和涂布机数据


            }
            catch (Exception exception)
            {

                timer1.Stop();
                timer2.Stop();
                timer1.Enabled = false;
                timer2.Enabled = false;
                button1.Text = "开启";
                TDieDATA.Clear();
                AreaDATA.Clear();
                CoterDAtA.Clear();
                TotalList.Clear();
                label9.Text = "涂布机状态";
                label9.ForeColor = Color.White;
                LogHelper.LogHelper.Instance.WriteLog("Timer2异常：" + exception.Message, LogType.Error);
                MessageBox.Show(exception.ToString());

                MessageBox.Show(exception.Message);
            }
        }

        public string SetColumn()
        {
            try
            {
                List<double> TDie_position = new List<double>();
                TDie_position.Clear();
                //LogHelper.LogHelper.Instance.WriteLog("启动生成CSV对位任务", LogType.Notice);
                //       Application.DoEvents();
                AnYunTDieClass anYunTDieClass = new AnYunTDieClass();


                TDie_position = anYunTDieClass.GetTDieData(setting.TDieIP, setting.TDiePort);
                List<object> lstSource = new List<object>();
                List<string> tempTdieData = new List<string>();
                TDieModel model = new TDieModel()
                {
                    T1 = TDie_position[0],
                    T2 = TDie_position[1],
                    T3 = TDie_position[2],
                    T4 = TDie_position[3],
                    T5 = TDie_position[4],
                    T6 = TDie_position[5],
                    T7 = TDie_position[6],
                    T8 = TDie_position[7],
                    T9 = TDie_position[8],
                    T10 = TDie_position[9],
                    T11 = TDie_position[10],
                };
                lstSource.Add(model);
                string TDATA = string.Join(",", TDie_position);


                this.ucDataGridView1.DataSource = lstSource;
                this.ucDataGridView1.First();
                return TDATA;
            }
            catch (Exception e)
            {
                LogHelper.LogHelper.Instance.WriteLog(string.Format("DataTable列名出错：{0}", e.Message), LogType.Error);
                throw e;
            }
        }


        private void SaveCSV(List<string> totalList, string filePath)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(filePath);
            System.IO.FileStream fs;
            System.IO.StreamWriter sw;
            if (!fi.Exists)
            {
                //  fi.Directory.Create();
                fs = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                //写入列名
                string dataColumns = "日期,时间,趟号,目标值,预警公差,公差,重量上限,重量下限,实时速度,设定速度,实时泵速,设定泵速,左刀距实时值,左刀距设定值,右刀距实时值,右刀距设定值,泵口压力,涂布压力,回流压力,涂布阀打开位置,涂布阀打开速度,涂布阀打开加速度,涂布阀打开减速度,涂布阀打开所需距离,涂布阀关闭位置,涂布阀关闭速度,涂布阀关闭加速度,涂布阀关闭减速度,涂布阀关闭所需距离,回流阀打开位置,回流阀打开速度," +
                    "回流阀打开加速度,回流阀打开减速度,回流阀打开所需距离,回流阀关闭位置,回流阀关闭速度," +
                    "回流阀关闭加速度,回流阀关闭减速度,回流阀关闭所需距离," +
                    "浆料粘度,浆料含量,刀垫厚度,浆料配方," +
                    "T1开量,T2开量,T3开量,T4开量,T5开量,T6开量,T7开量,T8开量,T9开量,T10开量,T11开量," +
                    "分区1,分区2,分区3,分区4,分区5,分区6,分区7,分区8,分区9,分区10," +
                    "分区11,分区12,分区13,分区14,分区15,分区16,分区17,分区18,分区19,分区20," +
                    "分区21,分区22,分区23,分区24,分区25,分区26,分区27,分区28,分区29,分区30," +
                    "分区31,分区32,分区33,分区34,分区35,分区36,分区37,分区38,分区39,分区40," +
                    "分区41,分区42,分区43,分区44,分区45,分区46,分区47,分区48,分区49,分区50,CPK,COV,AVG";

                sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
                sw.WriteLine(dataColumns);
                //sw.Close();
            }
            else
            {
                fs = new System.IO.FileStream(filePath, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
            }

            for (int i = 0; i < totalList.Count; i++)
            {
                string str = totalList[i].ToString();
                sw.WriteLine(str);
            }


            //  sw.Flush();
            sw.Close();
            fs.Close();
            //for (int i = 0; i < TotalList.Count(); i++)
            //{

            //}
        }

        public void DataViewTest()
        {
            List<DataGridViewColumnEntity> lstCulumns = new List<DataGridViewColumnEntity>();
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "T1", HeadText = "T1", Width = 9, WidthType = SizeType.Percent });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "T2", HeadText = "T2", Width = 9, WidthType = SizeType.Percent });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "T3", HeadText = "T3", Width = 9, WidthType = SizeType.Percent });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "T4", HeadText = "T4", Width = 9, WidthType = SizeType.Percent });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "T5", HeadText = "T5", Width = 9, WidthType = SizeType.Percent });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "T6", HeadText = "T6", Width = 9, WidthType = SizeType.Percent });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "T7", HeadText = "T7", Width = 9, WidthType = SizeType.Percent });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "T8", HeadText = "T8", Width = 9, WidthType = SizeType.Percent });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "T9", HeadText = "T9", Width = 9, WidthType = SizeType.Percent });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "T10", HeadText = "T10", Width = 9, WidthType = SizeType.Percent });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "T11", HeadText = "T11", Width = 9, WidthType = SizeType.Percent });

            this.ucDataGridView1.Columns = lstCulumns;
            //this.ucDataGridView1.IsShowCheckBox = true;

            // this.ucDataGridView1.DataSource = lstSource;
            //this.ucDataGridView1.First();
        }

        #region  属性方法
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }
        /// <summary>
        /// 处理界面刷新闪烁
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014) // 禁掉清除背景消息
                return;
            base.WndProc(ref m);
        }
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip1.Show();
            }
            if (e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
        }
        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
            }
            else
            {
                notifyIcon1.Visible = false;
            }
        }


        private void ShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            notifyIcon1.Dispose();

        }

        private void AcquisitionSystem_FormClosed(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Stop = true;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                this.notifyIcon1.Visible = true;
                this.Hide();
            }
        }

        #endregion

        private void SetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingConfiguration settingConfiguration = new SettingConfiguration();
            settingConfiguration.ShowDialog();
        }
        private void OperateSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OperateSetting frm = new OperateSetting();
            //   frm.Title = "操作单设置";
            //   frm.IsShowCloseBtn = true;
            frm.ShowDialog();

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


            config.AppSettings.Settings["TcpIP"].Value = textBox1.Text.Trim();

            config.Save(ConfigurationSaveMode.Modified);
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


            config.AppSettings.Settings["TcpPort"].Value = textBox2.Text.Trim();

            config.Save(ConfigurationSaveMode.Modified);
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }


    }
}