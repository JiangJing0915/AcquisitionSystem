namespace AcquisitionSystem.Model
{
    internal class AreaModel
    {
        //  public int ScanNum { get; set; }
        public int COMMAND { get; set; }////命令类型（1-注册命令,2-心跳命令,3-测量数据传输命令）
        public int ESNUM { get; set; } //设备 ID（数据类信息使用）
        public string CODE { get; set; } ////设备状态码(面密度仪运行状态)
        public string MSG { get; set; }////信息（状态码对应内容）
        public int DATATYPE { get; set; }//数据类型(1-毫米数据 2-分区数据)
        public int COUPLINGFB { get; set; }////耦合标志位（1-完成 0-未耦合）
        public List<string> DATA { get; set; }//数据（测量值按照分区/毫米的数列顺序进行发送）
        public int IDNUM { get; set; }////扫描序号
        public double KADJNUM { get; set; }////耦合系数值
        public double TARNUM { get; set; }//工艺目标值
        public double SPEUNUM { get; set; }//工艺规格上限
        public double SPEDNUM { get; set; }//工艺规格下限
        public double CONUNUM { get; set; }//工艺控制上限
        public double CONDNUM { get; set; }//工艺控制下限
        public int PARNUM { get; set; }//分区数据量/毫米数据量
        public string DeviceNO { get; set; }

        public int DETYPE { get; set; }
        public string TIME { get; set; } //时间
    }
}
