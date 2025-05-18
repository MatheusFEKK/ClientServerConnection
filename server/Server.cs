using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace server
{
    internal class Server
    { 
        static async Task Main(string[] args)
        {

            var hostName = Dns.GetHostName();
            IPHostEntry localhost = await Dns.GetHostEntryAsync(hostName);
            IPAddress localIpAddress = IPAddress.Any;
            IPEndPoint ipEndPoint = new IPEndPoint(localIpAddress, 11000);

            Socket listener = new Socket(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
               );

            listener.Bind(ipEndPoint);
            listener.Listen(100);

            System.Net.IPAddress [] ips = new System.Net.IPAddress[7];

            Console.WriteLine($"The server is listening on this ip's :");
            for (var a = 0; a < Dns.GetHostAddresses(hostName).Length; a++)
            {
                Console.WriteLine(Dns.GetHostAddresses(hostName)[a]);
                ips[a] = Dns.GetHostAddresses(hostName)[a];
            }
            Console.WriteLine($"The server is listening on this port : {ipEndPoint.Port}");


            while (true)
            {
                var handler = await listener.AcceptAsync();
                _ = UserHandler(handler);
                Console.WriteLine($"User connected {handler.RemoteEndPoint}");
            }
        }
                
                static async Task UserHandler(Socket handler)
                {
                    bool nicknameSet = false;
                    string nickName = string.Empty;
                    string message = string.Empty;
                    byte[] buffer = new byte[1024];

                while (true)
                {
                        ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
                        int received = await handler.ReceiveAsync(segment, SocketFlags.None);
                        var response = Encoding.UTF8.GetString(buffer, 0, received);

                    if (response.StartsWith("Username:") && nicknameSet == false)
                    {
                        nickName = response.Substring(response.IndexOf(":") + 1);
                        Console.WriteLine($"{nickName} joined the server");
                        nicknameSet = true;
                    }

                    if (response.StartsWith("Message:"))
                    {
                        message = response.Substring(response.IndexOf(":") + 1);
                        Console.WriteLine($"{nickName}: {message}");
                    }

                    if (received == 0 && nicknameSet == true)
                    {
                        Console.WriteLine($"{nickName} disconnected");
                    }

                }


        }
            
    }
}
