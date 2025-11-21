using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace server
{
    internal class Server
    {
           public static List<ServerList> connectedClients = new List<ServerList>();
        
        static async Task<Socket> UserHandler(Socket handler)
        {
            bool nicknameSet = false;
            string nickName = string.Empty;
            string message = string.Empty;

            while (true)
            {
                byte[] buffer = new byte[1024];
                ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
                int received = await handler.ReceiveAsync(segment, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);


                JsonElement userData = JsonSerializer.Deserialize<JsonElement>(response);
                Console.WriteLine(userData);

                if (nicknameSet == false)
                {
                    switch (Convert.ToString(userData.GetProperty("Command")))
                    {
                        case "setNickname":
                            Console.WriteLine($"{userData.GetProperty("Nickname")} has joined the server");

                            Console.WriteLine("Generating the sessionid");
                            string sessionId = Guid.NewGuid().ToString();

                            Console.WriteLine("Will create the object that handles the user");

                            var user = new
                            {
                                AuthId = sessionId,
                                nickname = userData.GetProperty("Nickname"),
                                connected = true,
                            };
                            string payloadToClient = JsonSerializer.Serialize(user);
                            Console.WriteLine($"The user {user.nickname} has the session id {user.AuthId} the Nickname is {userData.GetProperty("Nickname")}");
                            Console.WriteLine("Inserting the user in the list");
                            Console.WriteLine("AuthID sended to the client");
                            await SendMessage(handler, payloadToClient);
                            var newClient = new ServerList(sessionId, userData.GetProperty("Nickname").ToString(), handler);
                            Console.WriteLine($"Connected players {connectedClients.Count + 1}/2");
                            nicknameSet = true;
                            break;
                        case "sendingMessage":
                            foreach (var client in connectedClients)
                            {
                                Console.WriteLine(client.AuthID.Contains(userData.GetProperty("AuthId").ToString()));
                            }
                            Console.WriteLine($"The message sended by the user is {userData.GetProperty("Message")}");
                            break;

                    }
                }


                if (connectedClients.Count == 2)
                {
                    Console.WriteLine("The server is full, starting the game now");
                    foreach (var userInfo in connectedClients)
                    {
                        dynamic user = userInfo;

                        await SendMessage(user.socket, "StartGame");
                        Console.WriteLine($"Sent start game command to {user.nickname} with session id {user.AuthId}");
                    }

                }

            }
        }
        static async Task SendMessage(Socket socket, string message)
        {
            byte[] data = new byte[1024];
            data = Encoding.UTF8.GetBytes(message);
            ArraySegment<byte> bytes = new ArraySegment<byte>(data);
            await socket.SendAsync(bytes, SocketFlags.None);
        }

        static async Task Main(string[] args)
        {
            Dictionary<string, Object> users = new Dictionary<string, Object>();


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

            while (true)
            {

                Socket handler = await listener.AcceptAsync();
                _ = UserHandler(handler);
                

            }
            //foreach (var user in users)
            //{
            //    var SessionId = user.Key;

            //    string authid = Convert.ToString(userData.AuthId);
            //    try
            //    {
            //    if (SessionId == Convert.ToString(userData.AuthId))
            //    {
            //        var ip = user.Key;
            //            await SendMessage(user.Value.Socket, messageReceived);

            //        }

            //    }catch (Exception error)
            //    {
            //        Console.WriteLine(error);
            //    }
            //}

            Console.WriteLine("In the end of the while");

                //Console.WriteLine($"this is the user session {SessionID}");






            }
        }
    }


