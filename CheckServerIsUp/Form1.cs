using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckServerIsUp
{
    public partial class Form1 : Form
    {
        public static void IsServerUp(string server, int port, int timeout)
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IAsyncResult result = socket.BeginConnect(server, port, null, null);

                try
                {
                    bool success = result.AsyncWaitHandle.WaitOne(timeout, true);

                    if (!success)
                    {
                        // NOTE, MUST CLOSE THE SOCKET                         
                        throw new SocketException();
                    }

                }
                finally
                {
                    socket.Close();
                }

            }
            catch (SocketException e)
            {
                Trace.WriteLine(string.Format("TCP connection to server {0} failed.", server));
                MessageBox.Show("连接失败,可能你所在的网络无法连接服务器,请更换网络后重试!", "错误提示",
    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        static void ThreadCheckSever()
        {
            //onsole.WriteLine("Child Thread Start!");
            string ipAddress = "www.qzcool.com";
            int portNum = 9000;
            IPHostEntry ipHost = Dns.GetHostEntry(ipAddress);
            IsServerUp(ipHost.AddressList[0].ToString(), portNum, 4000);
        }       


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread th = new Thread(ThreadCheckSever);
            th.Start();
        }
    }
}
