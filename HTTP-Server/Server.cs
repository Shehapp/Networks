using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        TcpListener serverSocket;
        private const string IP = "127.0.0.1";
        private int port;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO[3.1]: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            this.LoadRedirectionRules(redirectionMatrixPath);
            //TODO[3.2]: initialize this.serverSocket
            this.port = portNumber;
            this.serverSocket = new TcpListener(IPAddress.Parse(IP), port);
        }

        public void StartServer()
        {
            // TODO[4.1]: Listen to connections, with large backlog.
            serverSocket.Start(50);// can accept up to 50 clients
            Console.WriteLine("Server is listening on port " + port);
            while (true)
            {
                //TODO[4.2]: accept connections and start thread for each accepted connection.
                TcpClient client = serverSocket.AcceptTcpClient();
                Console.WriteLine("Client({0}) connected",client.Client.RemoteEndPoint);
                new Thread(HandleConnection).Start(client);
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO[5.1]: Create client socket 
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            TcpClient client = (TcpClient)obj;
            client.ReceiveTimeout = 0;
            
            NetworkStream stream = client.GetStream();
            // window_size
            byte[] buffer = new byte[1024*4];
            int mess_size;
            
            // TODO[5.2]: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO[5.2.1]: Receive request
                    mess_size = stream.Read(buffer, 0, buffer.Length);

                    // TODO[5.2.2]: break the while loop if receivedLen==0
                    if (mess_size == 0) break;

                    // TODO[5.2.3]: Create a Request object using received request string
                    Request request = new Request(Encoding.ASCII.GetString(buffer, 0, mess_size));

                    // TODO[5.2.4]: Call HandleRequest Method that returns the response
                    Response response = this.HandleRequest(request);

                    // TODO[5.2.5]: Send Response back to client
                    byte[] temp = Encoding.ASCII.GetBytes(response.ResponseString);
                    client.GetStream().Write(temp, 0, temp.Length);
                }
                catch (Exception ex)
                {
                    // TODO[5.3]: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO[5.4]: close client socket
            stream.Close();
            client.Close();
        }

        Response HandleRequest(Request request)
        {
            throw new NotImplementedException();
            string content;
            try
            {
                //TODO[6.1]: check for bad request 
                
                //TODO[6.2]: map the relativeURI in request to get the physical path of the resource.

                //TODO[6.3]: check for redirect

                //TODO[6.4]: check file exists

                //TODO[6.5]: read the physical file

                // Create OK response
            }
            catch (Exception ex)
            {
                // TODO[6.7]: log exception using Logger class
                // TODO[6.8]: in case of exception, return Internal Server Error. 
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            //TODO[7] return the redirected page path if exists else returns empty
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO[8]: check if filepath not exist log exception using Logger class and return empty string
            
            // else read file and return its content
            return string.Empty;
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO[9.1]: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary 
            }
            catch (Exception ex)
            {
                // TODO[9.2]: log exception using Logger class
                Environment.Exit(1);
            }
        }
    }
}
