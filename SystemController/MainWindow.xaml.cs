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
            try
            {
                var tcp=new TcpManager(IpTextBox.Text, PortTextBox.Text);
                var buf= tcp.Send(sendMsg.Text);

                var buf2 = buf.Where(n => n != 0).ToArray();

                var resMsg = Encoding.UTF8.GetString(buf2, 0, buf2.Length);
                this.Msg.Text = resMsg;

            }
            catch (Exception ex)
            {
                this.Msg.Text = ex.Message;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var tcp = new TcpManager(IpTextBox.Text, PortTextBox.Text);
                var buf = tcp.Send("cap");

                Console.WriteLine("GetData");

                var ms = new MemoryStream(buf);
                var bmp = new Bitmap(ms);
                var w = new CapForm {pictureBox1 = {Height = bmp.Height, Width = bmp.Width, Image = bmp}};
                w.Show();

            }
            catch (Exception ex)
            {
                this.Msg.Text = ex.Message;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // var tcp = new TcpManager(IpTextBox.Text, PortTextBox.Text);
            // tcp.SendNoReturn("app_end");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                var tcp = new TcpManager(IpTextBox.Text, PortTextBox.Text);
                var buf = tcp.Send("ps");
            
                var buf2 = buf.Where(n => n != 0).ToArray();
                var resMsg = Encoding.UTF8.GetString(buf2, 0, buf2.Length);
                this.Msg.Text = resMsg;

            }
            catch (Exception ex)
            {
                this.Msg.Text = ex.Message;
            }
        }
    }
}
