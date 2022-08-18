namespace AcquisitionSystem
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            bool CreateNew;
            using (Mutex mutex = new Mutex(true, Application.ProductName, out CreateNew))
            {
                if (CreateNew)
                {
                    ApplicationConfiguration.Initialize();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new AcquisitionSystem());

                }
                else
                {
                    MessageBox.Show("程序已经在运行中...");
                    System.Threading.Thread.Sleep(1000);
                    // 终止此进程并为基础操作系统提供指定的退出代码。   
                    System.Environment.Exit(1);
                }
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            // Application.Run(new AcquisitionSystem());
        }
    }
}