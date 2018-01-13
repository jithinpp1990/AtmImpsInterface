using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AtmImpsServer
{
    public partial class Form1 : Form
    {
        private string Switch_status;
        Boolean Error_Flag = false;
        private Socket Listner;
        private char listenStatus = 'N';
        private TcpListener tcpListener;
        private Thread listenThread;
        private int connectedClients = 0;
        private delegate void WriteMessageDelegate(string msg, string strl_name);
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private TcpClient tcpClient;
        TextTracker tracker_obj = new TextTracker();
        private string ip;
        private string[] port;
        private string Ip
        {
            get { return ip; }
            set { ip = value; }
        }
        private string[] Port1
        {
            get { return port; }
            set { port = value; }
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void Server()
        {
            try
            {
                listenStatus = 'Y';
                //this.tcpListener = new TcpListener(IPAddress.Loopback, Convert.ToInt32(Port1[0])); // Change to IPAddress.Any for internet wide Communication
                this.tcpListener = new TcpListener(IPAddress.Parse(Ip), Convert.ToInt32(Port1[0]));
                host_ip.Text = IPAddress.Parse(((IPEndPoint)tcpListener.LocalEndpoint).Address.ToString()).ToString();
                host_ports.Text = ((IPEndPoint)tcpListener.LocalEndpoint).Port.ToString();
                this.listenThread = new Thread(new ThreadStart(ListenForClients));
                this.listenThread.Start();
            }
            catch (Exception ex)
            {
                tracker_obj.Text_Tracker("\t Exception205  :  " + ex + " -------------  ");
            }

        }
        private void StopServer()
        {
            try
            {
                if (MessageBox.Show("Do You Want To STOP?", "CCATM", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (listenStatus == 'Y')
                    {
                        if (connectedClients > 0)
                            tcpClient.Close();
                        tcpListener.Stop();                      
                    }
                    Dispose();
                }
                else
                {
                    Dispose();
                }
            }
            catch(Exception ex)
            {
                Dispose();
            }
        }
        private void ListenForClients()
        {
            try
            {
                this.tcpListener.Start();
                while (true) // Never ends until the Server is closed.
                {
                    //blocks until a client has connected to the server
                    TcpClient client = this.tcpListener.AcceptTcpClient();
                    //create a thread to handle communication  
                    //with connected client
                    connectedClients++; // Increment the number of clients that have communicated with us.
                                        // cons.Text = connectedClients.ToString();

                    WriteMessage("Switch Connected " + connectedClients.ToString(), "commtxt");
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);
                }
            }
            catch (Exception ex)
            {
                tracker_obj.Text_Tracker("\t Listen Exception  :  " + ex + " -------------  ");
            }
        }
        private void HandleClientComm(object client)
        {//{
            Response.SplitterSimulator splitter = new Response.SplitterSimulator();
            Response.ResponseBuilder RB = new Response.ResponseBuilder();
            string[] response_array = null;
            string[] request_array = null;
            String data = "";
            tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    connectedClients--;
                    //cons.Text = connectedClients.ToString();
                    break;
                }

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();

                // Convert the Bytes received to a string and display it on the Server Screen
                request_array = null;
                response_array = null;
                data = encoder.GetString(message, 0, bytesRead);
                tracker_obj.Directory_Tracker();
                tracker_obj.Text_Tracker("\tRequest   :  " + data);

                request_array = splitter.RequestSplitter(data);

                if (request_array == null)
                {
                    WriteMessage("Error From Simulator" + Convert.ToString(DateTime.Now) + "\r\n", "commtxt");
                }
                else
                {

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////Splitted Request Showing to TextBox///////////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    WriteMessage("Client   : " + data, "commtxt");
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (request_array[0] == "0810" && splitter.ValidationResponse_boolean == true)
                    {
                        if (request_array[70] == "001")
                        {
                            Switch_status = "available";
                            WriteMessage("Availlable From " + Convert.ToString(DateTime.Now), "switch_status");

                        }
                        else if (request_array[70] == "002")
                        {
                            Switch_status = "not available";
                            WriteMessage("Not Availlable From " + Convert.ToString(DateTime.Now), "switch_status");
                        }
                    }
                    else if (splitter.ValidationResponse_boolean == true)
                    {
                        response_array = RB.Response(request_array);
                        if (response_array == null)
                        {
                            Error_Flag = true;
                            WriteMessage("Error From ResponseBuilder" + Convert.ToString(DateTime.Now) + "\r\n", "commtxt");
                        }
                        else
                        {
                            if (response_array[70] == "001")
                            {
                                Switch_status = "available";
                                WriteMessage("Switch SignIn- Availlable From " + Convert.ToString(DateTime.Now), "switch_status");

                            }
                            else if (response_array[70] == "002")
                            {
                                Switch_status = "not available";
                                WriteMessage("Switch SignOff- Not Availlable From " + Convert.ToString(DateTime.Now), "switch_status");
                                WriteMessage("--------------------------", "handShake_status");

                            }
                            else if (response_array[70] == "301")
                            {
                                WriteMessage("Success From Switch on - " + Convert.ToString(DateTime.Now), "handShake_status");
                            }
                            var response = string.Join("", response_array);
                            response = Convert.ToString(response.Length).PadLeft(4, '0') + response;
                            Echo(response, encoder, clientStream);
                            //Grid_refresh();

                            tracker_obj.Text_Tracker("\tResponse  :  " + response + " -------------  ");
                            WriteMessage("Server :" + response, "commtxt");
                            response = null;
                        }
                    }
                    else
                    {

                    }

                }


            }

            tcpClient.Close();
        }
        private void WriteMessage(string msg, string ctrl_name)
        {
            try
            {
                switch (ctrl_name)
                {
                    case "switch_status":
                        if (this.switch_status.InvokeRequired)
                        {
                            WriteMessageDelegate d = new WriteMessageDelegate(WriteMessage);
                            this.switch_status.Invoke(d, new object[] { msg, ctrl_name });
                        }
                        else
                        {
                            this.switch_status.Text = msg;
                        }
                        break;
                    case "handShake_status":
                        if (this.handShake_status.InvokeRequired)
                        {
                            WriteMessageDelegate d = new WriteMessageDelegate(WriteMessage);
                            this.handShake_status.Invoke(d, new object[] { msg, ctrl_name });
                        }
                        else
                        {
                            this.handShake_status.Text = msg;
                        }
                        break;
                    case "host_ip":
                        if (this.host_ip.InvokeRequired)
                        {
                            WriteMessageDelegate d = new WriteMessageDelegate(WriteMessage);
                            this.host_ip.Invoke(d, new object[] { msg, ctrl_name });
                        }
                        else
                        {
                            this.host_ip.Text = msg;
                        }
                        break;
                    case "host_ports":
                        if (this.host_ports.InvokeRequired)
                        {
                            WriteMessageDelegate d = new WriteMessageDelegate(WriteMessage);
                            this.host_ports.Invoke(d, new object[] { msg, ctrl_name });
                        }
                        else
                        {
                            this.host_ports.Text = msg;
                        }
                        break;
                    case "commtxt":
                        if (this.commtxt.InvokeRequired)
                        {
                            WriteMessageDelegate d = new WriteMessageDelegate(WriteMessage);
                            this.commtxt.Invoke(d, new object[] { msg, ctrl_name });
                        }
                        else
                        {
                            this.commtxt.AppendText(msg + Environment.NewLine);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
            };

        }
        /// <summary>
        /// Echo the message back to the sending client
        /// </summary>
        /// <param name="msg">
        /// String: The Message to send back
        /// </param>
        /// <param name="encoder">
        /// Our ASCIIEncoder
        /// </param>
        /// <param name="clientStream">
        /// The Client to communicate to
        /// </param>
        private void Echo(string msg, ASCIIEncoding encoder, NetworkStream clientStream)
        {
            // Now Echo the message back
            byte[] buffer = encoder.GetBytes(msg);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            try
            {
                ServerLoadDefaults.getregistry();
                ServerLoadDefaults.getIpPorts();
                Ip = ServerLoadDefaults.Ip;
                Port1 = ServerLoadDefaults.Ports;
                Grid_refresh();

            }
            catch (Exception ex)
            {
                commtxt.Text = "Port Is Busy :" + ex.ToString() + "\r\n" + commtxt.Text;
                //errortxt.Text += "\r\n" + ex.ToString();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            commtxt.Text = "";
        }
        public static string StringToHexold(String input, System.Text.Encoding encoding)
        {
            Byte[] stringBytes = encoding.GetBytes(input);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }
        public static string StringToHex(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }
        private void manualListeningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Server();
        }
        private void reset_Click(object sender, EventArgs e)
        {
            //StartListen();
        }
        public void Grid_refresh()
        {
            MiddleTier MT_obj = new MiddleTier();
            DataTable dt = new DataTable();
            dt = (DataTable)MT_obj.History();
            dataGridView1.DataSource = dt;
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (commtxt.Visible == true)
            {
                Grid_refresh();
                commtxt.Visible = false;
                dataGridView1.Visible = true;
            }

            else
            {
                dataGridView1.Visible = false;
                commtxt.Visible = true; ;
            }
        }
        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do You Want to Exit?",
                               "CCATM",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information) == DialogResult.No)
            {
                e.Cancel = true;

            }
            else
            {
                StopServer();
                e.Cancel = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            StopServer();
        }
    }
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}
