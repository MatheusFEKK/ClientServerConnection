using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace server
{
    internal class Server
    {
        static int PlayersCount = 1;
        static async Task<Socket> UserHandler(Socket handler, Dictionary<string, object> users)
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


                if (response.StartsWith("Username:"))
                {
                    nickName = response.Substring(response.IndexOf(":") + 1);
                    Console.WriteLine($"{nickName} has joined the server");
                    nicknameSet = true;
                    PlayersCount++;

                    Console.WriteLine("Generating the sessionid");
                    string sessionId = Guid.NewGuid().ToString();

                    Console.WriteLine("Will create the object that handles the user");

                    var user = new
                    {
                        AuthId = sessionId,
                        nickname = nickName,
                        socket = handler,
                        connected = true,
                    };

                    Console.WriteLine("Inserting the user in the list");
                    users.Add(sessionId, user);

                    await SendMessage(user.socket, Convert.ToString(user.AuthId));

                    Console.WriteLine("AuthID sended to the client");
                }
                else
                {
                    JsonElement userData = JsonSerializer.Deserialize<JsonElement>(response);
                    Console.WriteLine(userData);
                    var SessionID = Convert.ToString(userData.GetProperty("AuthId"));
                    if (users.TryGetValue(SessionID, out dynamic userInfo))
                    {
                        switch (Convert.ToString(userData.GetProperty("Command")))
                        {
                            case "sendingMessage":
                                Console.WriteLine($"A message was received from {userData.GetProperty("Nickname")} the message is {userData.GetProperty("Message")} your session id is {userData.GetProperty("AuthId")}");
                                await SendMessage(userInfo.socket, "This message is being sended by the server");
                                continue;
                            case "testing":
                                Console.WriteLine("The server received a test from the user");
                                continue;
                        }


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
                _ = UserHandler(handler, users);

                Console.WriteLine(PlayersCount);
                //string command = Console.ReadLine();
                

                if (PlayersCount <= 2)
                {
                    Console.WriteLine($"Connected players: {PlayersCount}/2");
                    Console.WriteLine($"User connected {handler.RemoteEndPoint}");
                    if (PlayersCount == 2)
                    {
                        Console.WriteLine("The server is full, starting the game now");
                       foreach (var userInfo in users.Values)
                        {
                            dynamic user = userInfo;
                        
                            await SendMessage(user.socket, "StartGame");
                            Console.WriteLine($"Sent start game command to {user.nickname} with session id {user.AuthId}");
                        }
                    }

                   //if (PlayersCount == 2)
                   //{
                   //     Console.WriteLine("The server is full");
                   //     handler.Close();
                   //}

                }
            
            

                
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
