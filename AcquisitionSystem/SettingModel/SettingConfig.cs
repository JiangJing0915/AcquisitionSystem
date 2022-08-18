namespace AcquisitionSystem.SettingModel
{
    internal class SettingConfig
    {
        #region 模头配置

        public string TDieIP { get; set; }
        public int TDiePort { get; set; }

        #endregion

        #region 测厚仪配置

        public string AreaIP { get; set; }
        public int AreaPort { get; set; }

        #endregion

        #region 闭环Socket配置

        public string TcpIP { get; set; }
        public int TcpPort { get; set; }

        #endregion

        #region 涂布机配置

        public string CoaterIP { get; set; }
        public int CoaterPort { get; set; }

        /// <summary>
        ///浆料黏度
        /// </summary>
        public double Pasteviscosity { get; set; }
        /// <summary>
        /// 浆料含量
        /// </summary>
        public double Pastcontent { get; set; }
        /// <summary>
        /// 浆料配方
        /// </summary>
        public string Pasteformula { get; set; }

        /// <summary>
        /// 刀垫厚度
        /// </summary>
        public double Shimthickness { get; set; }

        #endregion
    }
}
