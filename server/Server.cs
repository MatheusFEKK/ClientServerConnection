using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    internal class Server
    {
        static async Task Main(string[] args)
        {


            var hostName = Dns.GetHostName();
            IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);
            IPAddress localIpAddress = localhost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(localIpAddress, 11000);
            Socket listener = new Socket(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
               );

            listener.Bind(ipEndPoint);
            listener.Listen(100);
            Console.WriteLine($"The server is listening on {Dns.GetHostAddresses(hostName)[2]}:{ipEndPoint.Port}");

            var handler = await listener.AcceptAsync();


            while (true)
            {
                Console.WriteLine($"User connected {localhost.AddressList.Count()}");

                byte[] buffer = new byte[1024];
                ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
                var received = await handler.ReceiveAsync(segment, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);

                if (response != "")
                {
                    Console.WriteLine($"A message was received from the client: {response}");
                }

                var command = Console.ReadLine().ToLower();
                if (command == "stop")
                {
                    listener.Close();
                    handler.Close();
                    Console.WriteLine("Server stopping...");
                }
            }








        }
    }
}
