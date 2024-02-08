using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        // server listens on this IP and port
        private const string IP = "127.0.0.1";
        private const int PORT = 8888;

        private static IPAddress ipAddress;
        private static TcpListener listener;

        private static IList online_clients;

        private static Queue _queue_messages;

        // Semaphores
        private static Semaphore semaphore_aval_messages;
        private static Semaphore semaphore_message_queue;
        private static Semaphore semaphore_online_clients;

        private static void Init(int hm)
        {
            ipAddress = IPAddress.Parse(IP);
            listener = new TcpListener(ipAddress, PORT);
            
            online_clients = new ArrayList();
            _queue_messages = new Queue();
            semaphore_aval_messages = new Semaphore(0);
            semaphore_message_queue = new Semaphore(1);
            semaphore_online_clients = new Semaphore(1);
            
            
            for (int i = 0; i < hm; i++)
            {
                new Thread(SendAMessageToAllAvalClients).Start();
            }
        }

        public static void Main(string[] args)
        {
            Init(10);

            listener.Start();
            Console.WriteLine("Server is listening on port " + PORT);
            
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client({0}) connected",client.Client.RemoteEndPoint);

                online_clients.Add(client);
                new Thread(ServeClient).Start(client);
            }
        }
       
        public static void ServeClient(Object param)
        {
            TcpClient client = (TcpClient)param;
            NetworkStream stream = client.GetStream();
            int my_port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
            // window_size
            byte[] buffer = new byte[1024];
            int mess_size;
            string message;
            while (true)
            {
                mess_size = stream.Read(buffer, 0, buffer.Length);
                message = Encoding.ASCII.GetString(buffer, 0, mess_size);

                /*
                 * M:message
                 * C:Close
                 */

                if (message.Length>0 && message[0] == 'M')
                {
                    message = $"({my_port}_"+message.Substring(2)+"\n";

                    semaphore_message_queue.Wait();
                    _queue_messages.Enqueue(message);
                    semaphore_message_queue.Signal();
                    
                    semaphore_aval_messages.Signal();

                    continue;
                }
                
                semaphore_online_clients.Wait();
                online_clients.Remove(client);
                semaphore_online_clients.Signal();
                
                Console.WriteLine("Client({0}) disconnected", client.Client.RemoteEndPoint);
                stream.Close();
                client.Close();
                return;

            }

        }

        public static void SendAMessageToAllAvalClients()
        {
            while (true)
            {
                semaphore_aval_messages.Wait();
                semaphore_message_queue.Wait();
                String current_message = (String)_queue_messages.Dequeue();
                semaphore_message_queue.Signal();

                byte[] responseData = Encoding.ASCII.GetBytes(current_message);

                for (int i = 0; i < online_clients.Count; i++)
                {
                    try
                    {
                        TcpClient client = (TcpClient)online_clients[i];
                        if (!client.Connected)
                        {
                            semaphore_online_clients.Wait();
                            online_clients.Remove(client);
                            semaphore_online_clients.Signal();
                            continue;
                        }

                        client.GetStream().Write(responseData, 0, responseData.Length);
                    }
                    catch (Exception ok_computer)
                    {
                        Console.WriteLine(ok_computer);
                    }

                }
            }
        }

    }
    
    
    
    
    
    
    
    
    
    
    
    
    class Semaphore
    {
        public int count;

        public Semaphore()
        {
            count = 0;
        }
        public Semaphore(int InitialVal)
        {
            count = InitialVal;
        }
        public void Wait()
        {
            lock (this)
            {
                count--;
                if (count < 0)
                    Monitor.Wait(this, Timeout.Infinite);
            }
        }
        public void Signal()
        {
            lock (this)
            {
                count++;
                if (count <= 0)
                    Monitor.Pulse(this);
            }
        }
    }

}