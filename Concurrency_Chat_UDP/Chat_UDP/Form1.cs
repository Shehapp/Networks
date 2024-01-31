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
    public partial class Form1 : Form
    {
        private Socket _socket;
        private EndPoint DesEndPoint;
        private const int SRCPORT = 50010;
        private const int DESORT = 50020;
        private const string IP = "127.0.0.1";

        private Thread t1;
        private Thread t2;

        private Semaphore se1;
        private Semaphore se2;
        private Semaphore se3;

        private Queue _queue;

        private string publicKey;
        private string privateKey;
        private string symmetric_key;

        private bool connection_secure = false;

        public Form1()
        {
            InitializeComponent();
            new Form2().Show();
            
            InitializeConnection();
            
            // public and private keys
            InitializeKeys();
            
            // can send and receive
            RunThreads();
        }

        void InitializeKeys()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                publicKey = rsa.ToXmlString(false);
                privateKey = rsa.ToXmlString(true);
            }
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
            byte[] data;
            Program.Object2Params(parameters, ref _socket, ref Remote);
            while (true)
            {
                // wait until user send message
                se3.Wait();
                
                // first message is public key
                if (!connection_secure)
                {
                    // send public key
                    data = Encoding.ASCII.GetBytes(publicKey);
                    _socket.SendTo(data, data.Length, SocketFlags.None, DesEndPoint);
                    
                    // wait until connection is secure
                    Thread.Sleep(15);
                    
                    se3.Signal();
                    continue;
                }

                se2.Wait();
                message = (string)_queue.Dequeue();
                se2.Signal();

                // send message
                data = Program.Encrypt_Sym(message, symmetric_key);
                _socket.SendTo(data, data.Length, SocketFlags.None, Remote);
            }
        }

        void receiveMessage(object parameters)
        {
            Socket _socket = null;
            EndPoint Remote = null;
            byte[] data = new byte[1024];
            Program.Object2Params(parameters, ref _socket, ref Remote);

            while (true)
            {
                int recv = _socket.ReceiveFrom(data, ref Remote);
                byte[] actualData = new byte[recv];
                Array.Copy(data, actualData, recv);
                
                // receive symmetric key
                if (!connection_secure)
                {
                    symmetric_key = Program.Decrypt_ASym(actualData, privateKey);
                    connection_secure = true;
                    continue;
                }

                se1.Wait();
                textBox1.Text += DESORT + ": " + Program.Decrypt_Sym(actualData, symmetric_key) + Environment.NewLine;
                se1.Signal();

            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
                return;

            se1.Wait();
                textBox1.Text += "Me: " + textBox2.Text + Environment.NewLine;
            se1.Signal();

            se2.Wait();
            _queue.Enqueue(textBox2.Text);
            se2.Signal();

            se3.Signal();

        }
    }
}
