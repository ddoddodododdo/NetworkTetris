using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Tetris201770001
{
    class TetrisServer
    {
        byte[] receiveData = new byte[128];
        string encodingData;

        Socket socket;
        Thread receiveThread;
        public IPAddress myIPAddress = Dns.Resolve(Dns.GetHostName()).AddressList[0];
        int port = 1024;

        public void OnServer()
        {
            IPEndPoint endPoint = new IPEndPoint(myIPAddress, port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(10);
            socket = socket.Accept();

            receiveThread = new Thread(new ThreadStart(ReceiveFromClient));
            receiveThread.Start();
        }

        private void ReceiveFromClient()
        {
            while (true)
            {
                int receiveDataLength = socket.Receive(receiveData);
                encodingData = Encoding.UTF8.GetString(receiveData, 0, receiveDataLength);
            }
        }

        public void SendToClient(string data)
        {
            byte[] sendData = Encoding.UTF8.GetBytes(data);
            socket.Send(sendData);
        }

    }
}
