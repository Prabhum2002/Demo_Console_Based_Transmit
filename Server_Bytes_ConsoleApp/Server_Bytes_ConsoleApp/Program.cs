using System.Net.Sockets;

TcpListener listen = new TcpListener(3003);
TcpClient client;
int bufferSize = 1024;
NetworkStream netStream;
int bytesRead = 0;
int allBytesRead = 0;

// Start listening
listen.Start();

// Accept client
client = listen.AcceptTcpClient();
netStream = client.GetStream();

// Read length of incoming data
byte[] length = new byte[4];
bytesRead = netStream.Read(length, 0, 4);
int dataLength = BitConverter.ToInt32(length, 0);

// Read the data
int bytesLeft = dataLength;
byte[] data = new byte[dataLength];

while (bytesLeft > 0)
{

    int nextPacketSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;

    bytesRead = netStream.Read(data, allBytesRead, nextPacketSize);
    allBytesRead += bytesRead;
    bytesLeft -= bytesRead;

}

// Save image to desktop
File.WriteAllBytes("C: \\Users\\BI1335\\Desktop\\Socket\\Tcpimage.jpg", data);

// Clean up
netStream.Close();
client.Close();
