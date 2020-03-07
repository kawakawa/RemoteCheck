using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SystemCommon.Model;

namespace SystemController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IpTextBox.Text = SystemCommon.Util.Config.Get("ip");
            PortTextBox.Text = SystemCommon.Util.Config.Get("port");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var tcp=new TcpManager(IpTextBox.Text, PortTextBox.Text);
            var returnData= tcp.Send(sendMsg.Text);

            var buf = returnData.GetBuffer();
            var buf2 = buf.Where(n => n != 0).ToArray();

            var resMsg = Encoding.ASCII.GetString(buf2, 0, buf2.Length);
            this.Msg.Text = resMsg;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var tcp = new TcpManager(IpTextBox.Text, PortTextBox.Text);
            var returnData = tcp.Send("cap");

            Console.WriteLine("GetData");

            var w=new CapForm();
            Bitmap bmp = new Bitmap(returnData);

            w.pictureBox1.Height = bmp.Height;
            w.pictureBox1.Width = bmp.Width;
            w.pictureBox1.Image = bmp;
            w.Show();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var tcp = new TcpManager(IpTextBox.Text, PortTextBox.Text);
            tcp.SendNoReturn("app_end");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var tcp = new TcpManager(IpTextBox.Text, PortTextBox.Text);
            var returnData = tcp.Send("ps");
            var buf = returnData.GetBuffer();
            var buf2 = buf.Where(n => n != 0).ToArray();
            var resMsg = Encoding.ASCII.GetString(buf2, 0, buf2.Length);
            this.Msg.Text = resMsg;
        }
    }
}
