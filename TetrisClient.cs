using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Tetris201770001
{
    class TetrisClient
    {
        byte[] receiveData = new byte[128];
        string encodingData;

        Socket socket;
        Thread receiveThread;
        IPAddress connetIPAddress;
        int port = 1024;

        public void ConnetToServer()
        {
            IPEndPoint endPoint = new IPEndPoint(connetIPAddress, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);

            receiveThread = new Thread(new ThreadStart(ReceiveFromServer));
            receiveThread.Start();
        }
        
        private void ReceiveFromServer()
        {
            int receiveDataLength = socket.Receive(receiveData);
            encodingData = Encoding.UTF8.GetString(receiveData, 0, receiveDataLength);
        }

    }
}
