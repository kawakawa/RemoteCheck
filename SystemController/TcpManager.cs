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

        public byte[] Send(string cmd)
        {
            try
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
                var buffer = new byte[10240];
                var ms = new System.IO.MemoryStream();

                
                
                while (ns.Read(buffer, 0, buffer.Length) != 0)
                {
                    var buf2 = new byte[buffer.Length];
                    Array.Copy(buffer, buf2, buffer.Length);

                    if (buffer[buffer.Length - 1] == 0)
                    {
                        buf2 = new byte[]{};
                        buf2 = buffer.Where(n => n != 0).ToArray();
                        
                        if(buf2.Length==0)
                            break;
                    }
                    
                    
                    if (buf2[buf2.Length - 1] == 97)
                    {
                        if (buf2[buf2.Length - 2] == 116)
                        {
                            if (buf2[buf2.Length - 3] == 97)
                            {
                                if (buf2[buf2.Length - 4] == 100)
                                {
                                    if (buf2[buf2.Length - 5] == 95)
                                    {
                                        if (buf2[buf2.Length - 6] == 70)
                                        {
                                            if (buf2[buf2.Length - 7] == 79)
                                            {
                                                if (buf2[buf2.Length - 8] == 45)
                                                {
                                                    //受信データ蓄積
                                                    ms.Write(buf2, 0, buf2 .Length-9);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                   
                        
                    //受信データ蓄積
                    ms.Write(buffer, 0, buffer.Length);
                    
                }
                
                // int numBytesRead;
                // do {
                //     ms.Write(buffer, 0, buffer.Length);
                // }
                // while ((numBytesRead = ns.Read(buffer, 0, buffer.Length)) > 0);
                

                //閉じる
                ns.Close();
                tcp.Close();
                

                return ms.GetBuffer();
            }
            catch (Exception e)
            {
                //ASCII エンコード
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(e.Message);
                return buffer;
            }
            
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
