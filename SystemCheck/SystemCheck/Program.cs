using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemCheck.Model;

namespace SystemCheck
{
    public class Program
    {
        private static Tcp _tcpManager;

        public static void Init()
        {
            _tcpManager = new Tcp();
            _tcpManager.SetPort();

            _tcpManager.ListenStart();

        }


        public static void Exit()
        {
            _tcpManager.ListenStop();
        }


    }
}
