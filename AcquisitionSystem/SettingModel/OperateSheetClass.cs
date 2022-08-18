namespace AcquisitionSystem.SettingModel
{

    //SELECT TOP(1000) [Technology_Name]
    //,[BaseMaterial_Weight]
    //,[A_Net_Weight]
    //,[B_Net_Weight]
    //,[S_Tolerance]
    //,[S_Warning_Tolerance]
    //,[D_Tolerance]
    //,[D_Warning_Tolerance]
    //FROM[CloseLoopDataBase].[dbo].[OPERATION_SHEET]

    public class OperateSheetClass
    {
        public string Technology_Name { get; set; }

        public double BaseMaterial_Weight { get; set; }
        public double A_Net_Weight { get; set; }
        public double B_Net_Weight { get; set; }
        public double S_Tolerance { get; set; }

        public double S_Warning_Tolerance { get; set; }
        public double D_Tolerance { get; set; }
        public double D_Warning_Tolerance { get; set; }
    }
}
