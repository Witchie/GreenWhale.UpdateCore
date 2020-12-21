using GreenWhale.UpdateCore.Model;
using GreenWhale.UpdateCore.Model.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GreenWhale.UpdateCore
{
    /// <summary>
    /// 更新帮助类
    /// </summary>
    public class UpdateHelper
    {
        public SerialPort SerialPort { get;private set; }
        public UpdateHelper(SerialPort serialPort)
        {
            SerialPort = serialPort ?? throw new ArgumentNullException(nameof(serialPort));
        }
        /// <summary>
        /// 开始升级[0]
        /// </summary>
        /// <param name="serverRequestUpdate"></param>
        /// <returns></returns>
        public async Task<DeviceRespondUpdate> BeginUpdate(ServerRequestUpdate serverRequestUpdate)
        {
            SerialPort.ReadExisting();
            SerialPort.DataReceived -= SerialPort_DataReceived;
            var res =await Request<DeviceRespondUpdate>(serverRequestUpdate);
            return res;
        }
        /// <summary>
        /// 读取设备版本
        /// </summary>
        /// <param name="serverRequestDeviceVersion"></param>
        /// <returns></returns>
        public async Task<DeviceRespondDeviceVersion> GetDeviceVersion(ServerRequestDeviceVersion serverRequestDeviceVersion)
        {
            SerialPort.DataReceived -= SerialPort_DataReceived;
            var res =await Request<DeviceRespondDeviceVersion>(serverRequestDeviceVersion);
            return res;
        }
        /// <summary>
        /// 服务端通知包总数[1]
        /// </summary>
        /// <param name="serverRequestPacketCount"></param>
        /// <returns></returns>
        public void SysncHardInfo(ServerRequestPacketCount serverRequestPacketCount)
        {
            SerialPort.DataReceived -= SerialPort_DataReceived;
            Request(serverRequestPacketCount);
        }
        /// <summary>
        /// 发送请求数据
        /// </summary>
        /// <param name="sendFrameBase"></param>
        /// <returns></returns>
        private async Task<T> Request<T>(SendFrameBase sendFrameBase) where T : ReceiveFrameBase, new()
        {
            if (sendFrameBase == null)
            {
                throw new ArgumentNullException(nameof(sendFrameBase));
            }
            var data= sendFrameBase.FullFrame;
            TXContainer container = new TXContainer(data);
            SerialPort.Write(container.FullFrame, 0, container.FullFrame.Length);
            WriteData?.Invoke(container.FullFrame);
            return await GetRespond<T>();
        }
        /// <summary>
        /// 发送出去的数据
        /// </summary>
        public event Action<Byte[]> WriteData;
        /// <summary>
        /// 读取到的数据
        /// </summary>
        public event Action<Byte[],string> ReadData;
        /// <summary>
        /// 获取解析结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private async Task<T> GetRespond<T>(int time=5) where T : ReceiveFrameBase, new()
        {
            var res2=  Task.Factory.StartNew<T>(new Func<T>(delegate 
            {
                int count = 0;
                List<byte> buffer = new List<byte>();
                SerialPort.ReadTimeout = 200;
                Label:
                try
                {
                    byte[] data2 = new byte[2048];
                    count++;
                    Thread.Sleep(100);
                    var read = SerialPort.Read(data2, 0, data2.Length);
                    if (read > 0)
                    {
                        var receive = data2.Take(read).ToArray();
                        buffer.AddRange(receive);
                        ReadData?.Invoke(receive, BitConverter.ToString(buffer.ToArray()));
                        RXContainer receiveFrameBase = new RXContainer(buffer.ToArray());
                        var res = receiveFrameBase.GetFrame<T>();
                        if (res.Item2)
                        {
                            return res.Item1;
                        }
                        else
                        {
                            if (count > time)
                            {
                                return null;
                            }
                            goto Label;
                        }
                    }
                    else
                    {
                        if (count > time)
                        {
                            return null;
                        }
                        goto Label;
                    }
                }
                catch (Exception err)
                {
                    Debug.WriteLine(err);
                    if (count > time)
                    {
                        return null;
                    }
                    goto Label;
                }
            }));
            return await res2;
        }
        /// <summary>
        /// 发送请求数据
        /// </summary>
        /// <param name="sendFrameBase"></param>
        private void Request(SendFrameBase sendFrameBase)
        {
            if (sendFrameBase == null)
            {
                throw new ArgumentNullException(nameof(sendFrameBase));
            }
            var data = sendFrameBase.FullFrame;
            TXContainer container = new TXContainer(data);
            SerialPort.Write(container.FullFrame, 0, container.FullFrame.Length);
            SerialPort.DataReceived += SerialPort_DataReceived;
        }

        private async void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
            var respond = await GetRespond<ReceiveFrameBase>();
            if (respond!=null&&respond.Code==Code.DeviceRequestPacket)
            {
                DeviceRequestPacket deviceRequestPacket = new DeviceRequestPacket();
                deviceRequestPacket.Create(respond.FullFrame);
                var data=  GetPacket?.Invoke(deviceRequestPacket.PacketIndex, deviceRequestPacket.MD5Number, deviceRequestPacket);
                if (data!=null)
                {
                    SerialPort.DataReceived -= SerialPort_DataReceived;
                    Request(data);
                }
                else
                {
                    Debug.Fail("找不到指定包");
                }
            }
            else if (respond != null&&respond.Code==Code.DeviceRespondUpdateResult)
            {
                DeviceRespondUpdateResult deviceRespondUpdate = new DeviceRespondUpdateResult();
                deviceRespondUpdate.Create(respond.FullFrame);
                UpdateSuccess?.Invoke(deviceRespondUpdate.State);
                SerialPort.DataReceived -= SerialPort_DataReceived;
            }
        }
        /// <summary>
        /// 升级成功
        /// </summary>
        public event Action<State> UpdateSuccess;
        /// <summary>
        /// 获取指定数据包
        /// </summary>
        public event GetPakcket GetPacket;
    }
    /// <summary>
    /// 获取指定包委托
    /// </summary>
    /// <param name="packetidnex"></param>
    /// <param name="md5number"></param>
    /// <returns></returns>
    public delegate ServerRespondPacket GetPakcket(int packetidnex, uint md5number, DeviceRequestPacket deviceRequestPacket);
}
