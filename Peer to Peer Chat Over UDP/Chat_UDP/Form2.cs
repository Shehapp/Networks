using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Chat_UDP
{
    public partial class Form2 : Form
    {
        private Socket _socket;
        private EndPoint DesEndPoint;
        private const int SRCPORT = 50020;
        private const int DESORT = 50010;
        private const string IP = "127.0.0.1";

        private Thread t1;
        private Thread t2;

        private Semaphore se1;
        private Semaphore se2;
        private Semaphore se3;

        private Queue _queue;

        private string symetric_key;
        private bool connection_secure = false;

        public Form2()
        {
            InitializeComponent();
            InitializeConnection();
            
            // generate symetric key
            generateSymetricKey();
            
            // can send and receive
            RunThreads();
        }
        void generateSymetricKey()
        {
            var aes = new AesManaged();
            aes.KeySize = 192;
            aes.GenerateKey();
            symetric_key = Encoding.ASCII.GetString(aes.Key);
        }
        void InitializeConnection()
        {
            _queue = new Queue();
            // for txt1
            se1 = new Semaphore(1);
            // for array
            se2 = new Semaphore(1);
            // cur arraysize
            se3 = new Semaphore(0);

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, SRCPORT));

            DesEndPoint = new IPEndPoint(IPAddress.Parse(IP), DESORT);

            t1 = new Thread(sendMessage);
            t2 = new Thread(receiveMessage);
        }


        void RunThreads()
        {
            t1.Start(Program.Params2Object(_socket, DesEndPoint));
            t2.Start(Program.Params2Object(_socket, DesEndPoint));
        }
        void sendMessage(object parameters)
        {
            Socket _socket = null;
            EndPoint Remote = null;
            string message;
            Program.Object2Params(parameters, ref _socket, ref Remote);
            while (true)
            {
                se3.Wait();
                
                se2.Wait();
                message = (string)_queue.Dequeue();
                se2.Signal();

                // send encrypted message
                byte[] data = Encoding.ASCII.GetBytes(message);
                data = Program.Encrypt_Sym(message, symetric_key);
                _socket.SendTo(data, data.Length, SocketFlags.None, Remote);
                
            }
        }

        void receiveMessage(object parameters)
        {
            Socket _socket = null;
            EndPoint Remote = null;
            byte[] data = new byte[1024];
            byte[] actualData = new byte[1024];
            Program.Object2Params(parameters, ref _socket, ref Remote);

            while (true)
            {
                
                int recv = _socket.ReceiveFrom(data, ref Remote);
                if (!connection_secure)
                {
                    // encrypt symetric key with public key and send it
                    byte[] data_t = Program.Encrypt_ASym(symetric_key,Encoding.ASCII.GetString(data, 0, recv));
                    _socket.SendTo(data_t, data_t.Length, SocketFlags.None, Remote);
                    connection_secure = true;
                    continue;
                }
                
                actualData = new byte[recv];
                Array.Copy(data, actualData, recv);
                
                se1.Wait();
                txt1.Text += DESORT + ": " + Program.Decrypt_Sym(actualData, symetric_key) + Environment.NewLine;
                se1.Signal();

            }


        }

        private void b1_Click(object sender, EventArgs e)
        {
            if (txt2.Text == "")
                return;

            if (!connection_secure)
            {
                // person-2 must wait for public key
                txt1.Text += "please wait the public_key" + Environment.NewLine;

                return;
            }
                
            se1.Wait();
            txt1.Text += "Me: " + txt2.Text + Environment.NewLine;

            se1.Signal();

            se2.Wait();
            _queue.Enqueue(txt2.Text);
            se2.Signal();

            se3.Signal();

        }
    }
}