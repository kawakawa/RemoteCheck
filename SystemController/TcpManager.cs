using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemController
{
    public class TcpManager
    {
        private string _ip;
        private int _port = 65321;

        public TcpManager(string ip, string port)
        {
            this._ip = ip;

            if (int.TryParse(port, out var i))
            {
                _port = i;
            }
        }

        public MemoryStream Send(string cmd)
        {
            var tcp = new System.Net.Sockets.TcpClient(_ip, _port);
            var ns = tcp.GetStream();

            //読み取り、書き込みのタイムアウトを10秒にする
            ns.ReadTimeout = 10000;
            ns.WriteTimeout = 10000;

            var sendBytes = Encoding.UTF8.GetBytes(cmd + "\n");

            Console.WriteLine(@"データを送信する");
            ns.Write(sendBytes, 0, sendBytes.Length);

            //返信データ受信
            var buffer = new byte[1024];
            var ms = new System.IO.MemoryStream();

            do
            {
                 var resSize = ns.Read(buffer, 0, buffer.Length);

                if (resSize == 0)
                {
                    Console.WriteLine(@"サーバー断");
                    break;
                }

                //受信データ蓄積
                ms.Write(buffer, 0, buffer.Length);
            } while (ns.DataAvailable);

            Console.WriteLine(@"Get return data");

            //閉じる
            ns.Close();
            tcp.Close();

            return ms;
        }

        public void SendNoReturn(string cmd)
        {
            var tcp = new System.Net.Sockets.TcpClient(_ip, _port);
            var ns = tcp.GetStream();

            //書き込みのタイムアウトを10秒にする
            ns.ReadTimeout = 10000;

            var sendBytes = Encoding.UTF8.GetBytes(cmd + "\n");

            Console.WriteLine(@"データを送信する");
            ns.Write(sendBytes, 0, sendBytes.Length);

        }
    }
}
