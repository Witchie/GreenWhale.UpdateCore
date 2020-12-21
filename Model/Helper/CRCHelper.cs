namespace GreenWhale.UpdateCore.Model
{
    internal class CRCHelper
    {
        public byte[] GetCRC16(byte[] data)
        {
            return CODER_GetCrc16(data);
        }
        /// <summary>
        /// 多项式
        /// </summary>
        public ushort pPoly { get; set; } = 0x1021;
        /// <summary>
        /// 初始值
        /// </summary>
        public ushort CRCInit { get; set; } = 0xffff;
        /// <summary>
        /// 获取CRC值
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private byte[] CODER_GetCrc16(byte[] buffer)
        {
            ushort vC, vCrc = CRCInit;
            var   pNum = buffer.Length;
            for (int i=0; i<pNum; i++)
            {
                vCrc =(ushort)((ushort)(vCrc&0xff00) | (ushort)((vCrc^buffer[i])&0xff));
                for(int j=0; j<8; j++)
                {
                    vC = vCrc;
                    vCrc =(ushort)((vCrc>>1)&0x7fff);
                    if((vC&0x0001)==1)
                    {
                        vCrc ^= pPoly;
                    }
                }
            }
            return new byte[] {(byte)(vCrc%0x100), (byte)(vCrc /0x100) };
        }
    }
}
