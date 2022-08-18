namespace AcquisitionSystem
{
    internal class BitConverts
    {
        public static int HexString2Int(string val)
        {
            byte[] _tmp = new byte[4];
            if (val.Length / 2 > 4)
            {
                throw new Exception("");
            }
            for (int i = 0; i < val.Length / 2; i++)
            {
                _tmp[i] = Convert.ToByte(val[(i * 2)..(i * 2 + 2)], 16);
            }
            return BitConverter.ToInt32(_tmp, 0);
        }

        public static double HexString2Double(string val)
        {
            byte[] _tmp = new byte[4];
            if (val.Length / 2 > 4)
            {
                throw new Exception("");
            }
            for (int i = 0; i < val.Length / 2; i++)
            {
                _tmp[i] = Convert.ToByte(val[(i * 2)..(i * 2 + 2)], 16);
            }
            //Array.Reverse(_tmp);
            return BitConverter.ToSingle(_tmp, 0);
        }

        public static byte[] HexString2Bytes(string val)
        {
            byte[] _tmp = new byte[val.Length / 2];
            for (int i = 0; i < val.Length / 2; i++)
            {
                _tmp[i] = Convert.ToByte(val[(i * 2)..(i * 2 + 2)], 16);
            }

            return _tmp;
        }


    }
}
