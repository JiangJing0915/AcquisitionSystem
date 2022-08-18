namespace AcquisitionSystem.Model
{
    internal class CoaterDataModel
    {
        private double _lineSpeed; //当前涂布速度
        public double LineSpeed
        {
            get { return _lineSpeed; }
            set { _lineSpeed = value; }
        }

        private double _lineSpeed_stand;   //设定涂布速度
        public double LineSpeed_Stand
        {
            get { return _lineSpeed_stand; }
            set { _lineSpeed_stand = value; }
        }
        private double _pump { get; set; }               //泵速
        public double Pump
        {
            get { return _pump; }
            set { _pump = value; }
        }
        private double _pumpRate { get; set; }           //泵压
        public double PumpRate
        {
            get { return _pumpRate; }
            set { _pumpRate = value; }
        }
        private double _tapespeed { get; set; }         //走带速度

        public double Tapespeed
        {
            get { return _tapespeed; }
            set { _tapespeed = value; }
        }
        private double _leftclearance { get; set; } //左边刀距
        public double Leftclearance
        {
            get { return _leftclearance; }
            set { _leftclearance = value; }
        }
        private double _rightclearance { get; set; }         //右边刀距
        public double Rightclearance
        {
            get { return _rightclearance; }
            set { _rightclearance = value; }
        }
        private double _coaterpressure { get; set; }         //涂布压力
        public double Coaterpressure
        {
            get { return _coaterpressure; }
            set { _coaterpressure = value; }
        }
        private double _backflowpressure { get; set; }       //回流压力
        public double Backflowpressure
        {
            get { return _backflowpressure; }
            set { _backflowpressure = value; }
        }
        private double _pastetemperature { get; set; }       //浆料温度

        public double Pastetemperature
        {
            get { return _pastetemperature; }
            set { _pastetemperature = value; }
        }
        private double _pasteviscosity { get; set; }        //浆料粘度
        public double Pasteviscosity
        {
            get { return _pasteviscosity; }
            set { _pasteviscosity = value; }
        }
        private double _pastcontent { get; set; }          //浆料含量
        public double Pastcontent
        {
            get { return _pastcontent; }
            set { _pastcontent = value; }
        }

        private double _shimthickness { get; set; }         //刀垫厚度
        public double Shimthickness
        {
            get { return _shimthickness; }
            set { _shimthickness = value; }
        }
        private string _pasteformula { get; set; }          //浆料的配方
        public string Pasteformula
        {
            get { return _pasteformula; }
            set { _pasteformula = value; }
        }

        private List<string> coaterDatas = new List<string>();
        public List<string> CoaterDatas
        {
            get { return coaterDatas; }
            set { coaterDatas = value; }
        }

        private  string _tempcoaterDatas =string.Empty;
        public string TempCoaterDatas
        {
            get { return _tempcoaterDatas; }
            set { _tempcoaterDatas = value; }
        }
    }
}
