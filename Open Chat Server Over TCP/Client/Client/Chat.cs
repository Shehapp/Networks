using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class Chat : Form
    {
       
        // server listens on this IP and port
        private const string IP = "127.0.0.1";
        private const int PORT = 8888;

        private static TcpClient client;
        private static NetworkStream stream;
        
        private Queue _queue_messages;

        private Semaphore semaphore_aval_messages;
        private Semaphore semaphore_message_queue;
        private string my_name="Unknown";
        
        public Chat()
        {
            InitializeComponent();
            Init();
            ConnectToServer();
           
            new Thread(ReceiveMessage).Start();
            new Thread(SendMessage).Start();
        }

        private void Init()
        {
            client = new TcpClient();
            _queue_messages = new Queue();
            semaphore_aval_messages = new Semaphore(0);
            semaphore_message_queue = new Semaphore(1);

        }
        private void ConnectToServer()
        {
            client.Connect(IP, PORT);
            stream = client.GetStream();
        }

        private void SendMessage()
        {
            while (true)
            {
                semaphore_aval_messages.Wait();
                
                semaphore_message_queue.Wait();
                string current_message = (string)_queue_messages.Dequeue();
                semaphore_message_queue.Signal();
                
                byte[] responseData = Encoding.ASCII.GetBytes(current_message);
                stream.Write(responseData, 0, responseData.Length);

            }

        }
        private void ReceiveMessage()
        {
            // window_size
            byte[] buffer = new byte[1024];
            int mess_size;
            string message;
            while (true)
            {
                mess_size = stream.Read(buffer, 0, buffer.Length);
                message = Encoding.ASCII.GetString(buffer, 0, mess_size);

                textBox1.Text += message + Environment.NewLine;
            }

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
                return;
            
            if(textBox3!=null && textBox3.Text!="" ) 
                my_name = textBox3.Text;
            
            semaphore_message_queue.Wait();
            _queue_messages.Enqueue("M:"+my_name+") "+textBox2.Text);
            semaphore_message_queue.Signal();
            
            semaphore_aval_messages.Signal();

        }


        private void Chat_Load(object sender, EventArgs e)
        {
            FormClosing += (senderr, ee) =>
            {
                stream.Close();
                client.Close();
            };
        }
    }
}
