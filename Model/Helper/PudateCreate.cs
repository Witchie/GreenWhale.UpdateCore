using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GreenWhale.UpdateCore
{
    public class PudateCreate
    {
        public PudateCreate() { }
        public CodeInfo CodeInfo { get; private set; } = new CodeInfo();
        public void CreateUpdateFile(string fileName)
        {
            CodeInfo.SourcePath = fileName;
        }
        public bool CreatePkg()
        {
            if (File.Exists(CodeInfo.SourcePath))
            {
                var codedata=  File.ReadAllBytes(CodeInfo.SourcePath);
                BinPkgFile binPkgFile = new BinPkgFile(codedata,CodeInfo);
                var bindata= JsonConvert.SerializeObject(binPkgFile);
                var dd=  Encoding.UTF8.GetBytes(bindata);
                AES aES = new AES(new byte[] {0,0,8,3,2,1,4,9, 0, 0, 8, 3, 2, 1, 4, 9 });
                var str=  aES.AESEncrypt(dd,new byte[] { 0x96, 0x97, 0x0C, 0x5C, 0xBF, 0x10, 0x5D, 0xC0, 0xD0, 0xB6, 0x67, 0xB0, 0x09, 0x60, 0x97, 0xE9 });
                File.WriteAllBytes(CodeInfo.SavePath, str);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 读出升级包
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public BinPkgFile GetBinPkgFile(string fileName)
        {
            try
            {
                var data = File.ReadAllBytes(fileName);

                AES aES = new AES(new byte[] { 0, 0, 8, 3, 2, 1, 4, 9, 0, 0, 8, 3, 2, 1, 4, 9 });
                var str = aES.AESDecrypt(data, new byte[] { 0x96, 0x97, 0x0C, 0x5C, 0xBF, 0x10, 0x5D, 0xC0, 0xD0, 0xB6, 0x67, 0xB0, 0x09, 0x60, 0x97, 0xE9 });
                var gg = Encoding.UTF8.GetString(str);
                var bindata = JsonConvert.DeserializeObject<BinPkgFile>(gg);
                return bindata;
            }
            catch (Exception err)
            {
                return null;
            }
        }
    }
}
