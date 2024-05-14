using System.Net.Sockets;
using System.Net;

IPAddress ipAddress = IPAddress.Parse("172.16.16.19");
int port = 3003;
int bufferSize = 1024;

TcpClient client = new TcpClient();
NetworkStream netStream;

// Connect to server
try
{
    client.Connect(new IPEndPoint(ipAddress, port));

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

netStream = client.GetStream();

// Read bytes from image
byte[] data = File.ReadAllBytes("C:\\Users\\BI1335\\Pictures\\Screenshots\\Screenshot (14).png");

// Build the package
byte[] dataLength = BitConverter.GetBytes(data.Length);
byte[] package = new byte[4 + data.Length];
dataLength.CopyTo(package, 0);
data.CopyTo(package, 4);

// Send to server
int bytesSent = 0;
int bytesLeft = package.Length;

while (bytesLeft > 0)
{

    int nextPacketSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;

    netStream.Write(package, bytesSent, nextPacketSize);
    bytesSent += nextPacketSize;
    bytesLeft -= nextPacketSize;

}

// Clean up
netStream.Close();
client.Close();