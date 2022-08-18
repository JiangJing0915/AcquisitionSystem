using System.Net;
using System.Net.Sockets;

namespace AcquisitionSystem.Model
{
    internal class AnYunTDieClass
    {

        private string _tpos_cmd = " 80 00 02| 00 01 00 |   00 B4 00 | 00 01 01 | 82 | 00 1e | 00 | 00 0B";

        public List<double> GetTDieData(string TDieIP, int TPort)
        {
            Tuple<byte[], int> tpos = Networks(_tpos_cmd, TDieIP, TPort);
            List<double> TDie_position = new List<double>();


            for (int i = 0; i < 11; i++)
            {
                byte[] _val = tpos.Item1[(14 + i * 2)..(14 + i * 2 + 2)];
                Array.Reverse(_val);
                double val1 = Math.Round(BitConverter.ToInt16(_val) / 5.0, 0);
                if (val1==-0)
                {
                    val1 = 0;
                }

                TDie_position.Add(val1);
            }

            return TDie_position;
        }

        private Tuple<byte[], int> Networks(string _cmd, string ip_address, int port)
        {
            _cmd = _cmd.Replace("\r", "").Replace("\n", "").Replace("|", "").Replace(" ", "");
            byte[] _cmd_bytes = BitConverts.HexString2Bytes(_cmd);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SendTimeout = 300;
            EndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip_address), port);
            socket.SendTo(_cmd_bytes, iPEndPoint);

            byte[] _data_r = new byte[4096];
            int len = socket.ReceiveFrom(_data_r, ref iPEndPoint);
            socket.Close();

            return new Tuple<byte[], int>(_data_r, len);
        }


    }
}
