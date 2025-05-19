using System;
using System.Collections.Generic;
using System.Configuration;
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
            Dictionary<string, Object> users = new Dictionary<string, Object>();
            int PlayersCount = 0;

            string hostName = Dns.GetHostName();
            IPAddress localIpAddress = IPAddress.Any;
            IPEndPoint ipEndPoint = new IPEndPoint(localIpAddress, 11000);

            Socket listener = new Socket(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
               );


            listener.Bind(ipEndPoint);
            listener.Listen(100);

            System.Net.IPAddress[] ips = new System.Net.IPAddress[Dns.GetHostAddresses(hostName).Length];

            Console.WriteLine($"The server is listening on this ip's :");
            for (var a = 0; a < Dns.GetHostAddresses(hostName).Length; a++)
            {
                Console.WriteLine(Dns.GetHostAddresses(hostName)[a]);
                ips[a] = Dns.GetHostAddresses(hostName)[a];
            }
            Console.WriteLine($"The server is listening on this port : {ipEndPoint.Port}");
            Console.WriteLine($"The server is listening on this host name : {hostName}");

            while (true)
            {
                var handler = await listener.AcceptAsync();
                if (PlayersCount < 2)
                {
                    PlayersCount++;
                    Console.WriteLine(users.ToArray().ToString());
                    Console.WriteLine($"Connected players: {PlayersCount}/2");
                    _ = UserHandler(handler, users);
                    Console.WriteLine($"User connected {handler.RemoteEndPoint}");

                }
                else
                {
                    Console.WriteLine("The server is full");
                    handler.Close();
                }


            }
        }

        static async Task<Socket> UserHandler(Socket handler, Dictionary<string, object> users)
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

                if (response.StartsWith("Username:"))
                {
                    nickName = response.Substring(response.IndexOf(":") + 1);
                    Console.WriteLine($"{nickName} has joined the server");
                    nicknameSet = true;

                    string sessionId = Guid.NewGuid().ToString();
                    users[sessionId] = new
                    {
                        SessionId = sessionId,
                        Nickname = nickName,
                        Socket = handler.RemoteEndPoint,
                        Connected = true,
                    };

                    await SendMessage(handler, users[sessionId].ToString());

                    if (users[sessionId] != "")
                    {
                        Console.WriteLine($"User: {users[sessionId].ToString()}");
                        users.Count();
                    }
                    else
                    {
                        Console.WriteLine("No users connected");

                        foreach (var test in users)
                        {
                            Console.WriteLine($"User: {test.Value.ToString()}");
                            users.Count();
                        }
                    }

                    if (response.StartsWith("Message:"))
                    {
                        message = response.Substring(response.IndexOf(":") + 1);
                        Console.WriteLine($"{nickName}: {message}");
                    }

                    //if (received == 0 && nicknameSet == true)
                    //{

                    //    Console.WriteLine($"{nickName} disconnected");
                    //}

                }

                return handler;


            }


        }
            static async Task SendMessage(Socket socket, string message)
            {
                byte[] data = new byte[1024];
                data = Encoding.UTF8.GetBytes(message);
                ArraySegment<byte> bytes = new ArraySegment<byte>(data);
                await socket.SendAsync(bytes, SocketFlags.None);


            }
    }
}