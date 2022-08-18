using System.Text;

namespace LogHelper
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        Error,
        Warning,
        Notice
    }
    public class LogHelper
    {
        /// <summary>
        /// 单个日志文件大小
        /// </summary>
        const int FILE_SIZE = 1024 * 2048;
        /// <summary>
        /// 日志队列  
        /// </summary>
        private Queue<string> LogQueue = new Queue<string>();
        /// <summary>
        /// 日志队列锁
        /// </summary>
        object queueLock = new object();
        /// <summary>
        /// 日志文件名列表
        /// </summary>
        private List<string> FileList = new List<string>();
        #region 日志队列使用方式
        private void AddLog(string strLog)
        {
            lock (queueLock)
            {
                LogQueue.Enqueue(strLog);
            }
        }
        private string GetLog()
        {
            string strLog = "";
            lock (queueLock)
            {
                if (LogQueue.Count > 0)
                    strLog = LogQueue.Dequeue();
            }
            return strLog;
        }

        private int CurrentLogCount()
        {
            int count = 0;
            lock (queueLock)
            {
                count = LogQueue.Count;
            }
            return count;
        }
        #endregion

        static LogHelper instance = null;
        public static LogHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new LogHelper();
                return instance;
            }
        }

        public LogHelper()
        {
            if (Directory.Exists(dicPath))
            {
                FileList.Clear();
                foreach (string fileName in Directory.GetFiles(dicPath, "*" + pattern))
                {
                    string name = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                    FileList.Add(name);
                }

            }
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                WriteLogFunc();
            });
        }

        void WriteLogFunc()
        {
            while (true)
            {
                if (CurrentLogCount() > 0)
                {
                    string strWriteLog = GetLog();
                    WriteLogToFile(strWriteLog);
                }
                else
                    Thread.Sleep(1000);
            }
        }

        #region property
        /// <summary>
        /// 日志文件存放文件夹
        /// </summary>
        private string dicPath { get { return AppDomain.CurrentDomain.BaseDirectory + "Log"; } }

        //private string pattern = ".log";
        //private string pattern { get; set; }

        private string _str = ".log";
        private string pattern
        {
            get { return _str; }
            set { _str = value; }
        }

        #endregion

        #region Method
        /// <summary>
        /// 检查日志存放目录是否存在，如果不存在就创建
        /// </summary>
        /// <returns></returns>
        private bool IsDirectoryExist()
        {
            if (!Directory.Exists(dicPath))
            {
                try
                {
                    Directory.CreateDirectory(dicPath);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 创建日志文件  如果已存在文件名就改变
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private bool IsCreateLogFile(ref string filePath)
        {
            if (File.Exists(filePath))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                int number = 1;
                string dateFileName = DateTime.Now.ToShortDateString().Replace('/', '_');
                while (true)
                {
                    string tempName = string.Format("{0}.{1}{2}", dateFileName, number, pattern);
                    if (FileList.Where(p => p == tempName).Count() == 0)
                        break;
                    number++;
                }
                filePath = dicPath + "\\" + string.Format("{0}.{1}{2}", dateFileName, number, pattern);
            }

            try
            {
                FileStream fs = File.Create(filePath, FILE_SIZE);
                if (fs != null)
                {
                    fs.Close();
                }
                //fs?.Close();
                FileList.Add(Path.GetFileName(filePath));
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }

        /// <summary>
        /// 检查写入的内容是否超过文件的大小限制
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="writeLog"></param>
        /// <returns></returns>
        private bool IsLogContentOutOfSize(string filePath, string writeLog)
        {
            FileInfo fInfo = new FileInfo(filePath);
            if (fInfo.Length + writeLog.Length < FILE_SIZE)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查日志目录下有没有最新的日志文件，没有就创建新的日文件
        /// </summary>
        /// <returns></returns>
        private bool IsLastFileExist()
        {
            if (FileList.Count == 0)
            {
                string filePath = dicPath + "\\" + DateTime.Now.ToShortDateString().Replace('/', '_') + pattern;
                try
                {
                    FileStream fs = File.Create(filePath, FILE_SIZE);
                    if (fs != null)
                    {
                        fs.Close();
                    }
                    // fs?.Close();
                    FileList.Add(Path.GetFileName(filePath));
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 日志写入文件
        /// </summary>
        /// <param name="writeLog"></param>
        void WriteLogToFile(string writeLog)
        {
            if (!IsDirectoryExist())
                return;
            if (!IsLastFileExist())
                return;
            string writeFilePath = dicPath + "\\" + FileList.Last();
            if (IsLogContentOutOfSize(writeFilePath, writeLog))
            {
                if (!IsCreateLogFile(ref writeFilePath))
                {
                    return;
                }
            }
            FileStream fstream = null;
            StreamWriter streamWriter = null;
            try
            {
                fstream = new FileStream(writeFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                streamWriter = new StreamWriter(fstream, Encoding.UTF8);
                streamWriter.WriteLine(writeLog);
                streamWriter.Flush();
            }
            catch (Exception)
            {

            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
                if (fstream != null)
                {
                    fstream.Close();
                }
                //streamWriter?.Close();
                //fstream?.Close();
            }
        }
        object objLock = new object();
        /// <summary>
        /// 写日志对外
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="logType"></param>
        public void WriteLog(object obj, LogType logType = LogType.Notice)
        {
            string strLog = "";
            lock (objLock)
            {
                strLog = obj.ToString();
            }
            if (string.IsNullOrEmpty(strLog))
                return;
            string strWriteLog = string.Format("{0:yyyy-MM-dd HH:mm:ss:fff}", DateTime.Now) + " " + string.Format("[{0}]", logType) + strLog + Environment.NewLine;
            AddLog(strWriteLog);
        }
        #endregion
    }
}
