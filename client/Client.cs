using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace client
{
    internal class Client
    {
        private static string sessionID;

        static async Task<Socket> ConnectToServer(string ip, ulong port)
        {
            Console.WriteLine($"Connecting to {ip}:{port}...");
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), ((int)port));

            Socket client = new Socket(
                    ipEndPoint.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp
                    );

            await client.ConnectAsync(ipEndPoint);

            return client;


        }

        static async Task SendMessage(Socket socket, string message)
        {
            byte[] data = new byte[1024];
            data = Encoding.UTF8.GetBytes(message);
            ArraySegment<byte> bytes = new ArraySegment<byte>(data);
            await socket.SendAsync(bytes, SocketFlags.None);

        }

        static async Task<String> ReceiveUserData(Socket socket)
        {
            byte[] data = new byte[1024];
            ArraySegment<byte> bytes = new ArraySegment<byte>(data);
            int bytesRec = await socket.ReceiveAsync(bytes, SocketFlags.None);
            string messageReceived = Encoding.UTF8.GetString(data, 0, bytesRec);

            return messageReceived;
        }

        static async Task Main(string[] args)
        {
            string nickname = "";
            string message;
            Socket socket = null;

            Console.WriteLine("Insert the IP Address and the Port: ");
            var port = Console.ReadLine();
            var ipAddress = port.Substring(0, port.IndexOf(":"));
            var portInserted = Convert.ToUInt64(port.Substring(port.IndexOf(":") + 1));
            socket = await ConnectToServer(ipAddress, portInserted);

            try
            {
                if (socket != null)
                {
                    Console.WriteLine("Connected with the server");

                }

                while (true)
                {
                    if (nickname == "")
                    {
                        Console.WriteLine("Insert your username: ");
                        nickname = Console.ReadLine();
                        string formatedNickname = $"Username: {nickname}";
                        await SendMessage(socket, formatedNickname);
                        Console.WriteLine("Username sended");
                        sessionID = await ReceiveUserData(socket);
                        Console.WriteLine($"Your session id is {sessionID}");
                    }
                    var command = Console.ReadLine().ToLower();

                    if (command.ToString() == "disconnect")
                    {

                        Console.WriteLine("Disconnecting from the server...");
                        break;
                    }
                    if (command.ToString() == "message" && nickname != "")
                    {
                        Console.WriteLine("Write your message to the server");
                        try
                        {
                            message = Console.ReadLine();
                            var payload = new
                            {
                                AuthId = sessionID,
                                Message = message,
                                Command = "sendingMessage",
                            };

                            string payloadToServer = JsonConvert.SerializeObject(payload);
                            Console.WriteLine("Payload created");
                            await SendMessage(socket, payloadToServer);
                            Console.WriteLine(payloadToServer);
                            Console.WriteLine("Message sended");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.Message}");
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}
