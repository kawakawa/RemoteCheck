using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemCommon.Util;

namespace SystemCheck.Model
{
    public class Tcp
    {
        private int _port;

        private TcpListener _listener;
        private TcpClient _tcpClient;
        /// <summary>
        /// Thread停止判定用
        /// </summary>
        private volatile bool _isThreadLoop;

        /// <summary>
        /// 受信用Thread
        /// </summary>
        private Thread _tcpReceiveThread;

        public void SetPort()
        {
            var strPort = Config.Get("port");
            if (int.TryParse(strPort, out var i))
            {
                _port = i;
                return;
            }

            _port = 65321; //初期値
        }


        public void ListenStart()
        {
            //受信Threadループ判定：ON
            _isThreadLoop = true;

            //受信用Thread開始
            _tcpReceiveThread = new Thread(TcpReceiveProc);
            _tcpReceiveThread.Start();


        }

        public void ListenStop()
        {
            _tcpReceiveThread.Interrupt();  //Threadの停止
            _isThreadLoop = false;      //Threadルール停止

            _listener?.Stop();

            _tcpReceiveThread.Join(1000);//Threadが停止するor1000ミリ秒間　待機
        }


        private void TcpReceiveProc()
        {
            //ListenするIPアドレス
            var ipString = "127.0.0.1";
            var ipAdd = System.Net.IPAddress.Parse(ipString);
            
            
            //TcpListenerオブジェクトを作成する
            _listener = new System.Net.Sockets.TcpListener(ipAdd, _port);
            
            
            //Listenを開始する
            _listener.Start();


            while (_isThreadLoop)
            {
                try
                {
                    _tcpClient = _listener.AcceptTcpClient();

                    var ns = _tcpClient.GetStream();

                    var buffer = new byte[1024]; //受信データ（都度、データが分割されて送られてくるので、その分割されたデータ受信用）

                    while (ns.Read(buffer, 0, buffer.Length) != 0)
                    {
                        try
                        {
                            //0Byte除外
                            var buffer2 = buffer.Where(n => n != 0);

                            //文字列化
                            var command = Encoding.ASCII.GetString(buffer2.ToArray()).TrimEnd('\n'); ;




                            
                            var returnSendMsgByte = Analyze.Exec(command);
                            //データを送信する
                            ns.Write(returnSendMsgByte, 0, returnSendMsgByte.Length);
                            
                        }
                        catch
                        {
                            //
                        }
                        finally
                        {
                            buffer = new byte[1024];
                        }
                    }

                    _tcpClient.Close();
                }
                catch
                {
                    //
                }
            }
        }







    }
}