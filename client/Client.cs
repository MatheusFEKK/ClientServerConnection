using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameUI;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Runtime.InteropServices;


namespace client
{
    internal class Client
    {
        private static string sessionID;
        private new Form ActiveUI;

        private void openUI(Form instance)
        {
            if (ActiveUI != null)
            {
                ActiveUI.Close();
            }
            ActiveUI = instance;
            instance.Show();
        }

        static async Task<Socket> ConnectToServer(string ip, ulong port)
        {
            Console.WriteLine($"Connecting to {ip}:{port}...");
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), ((int)port));

            Socket client = new Socket(
                    ipEndPoint.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp
                    );

            try
            {
                await client.ConnectAsync(ipEndPoint);
                return client;
            }catch (SocketException error)
            {
                return null;
            }



            
        }

        static async Task SendMessage(Socket socket, string message)
        {
            try
            {
                byte[] data = new byte[1024];
                data = Encoding.UTF8.GetBytes(message);
                ArraySegment<byte> bytes = new ArraySegment<byte>(data);
                await socket.SendAsync(bytes, SocketFlags.None);
            }catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
        }

        static async Task<String> ReceiveData(Socket socket)
        {
            byte[] data = new byte[1024];
            ArraySegment<byte> bytes = new ArraySegment<byte>(data);
            int bytesRec = await socket.ReceiveAsync(bytes, SocketFlags.None);
            string messageReceived = Encoding.UTF8.GetString(data, 0, bytesRec);

            return messageReceived;
        }

        [STAThread]
        static async Task Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            
            string nickname = "";
            string message;

            Console.WriteLine("Insert the IP Address and the Port: ");
            var port = Console.ReadLine();
            var ipAddress = port.Substring(0, port.IndexOf(":"));
            var portInserted = Convert.ToUInt64(port.Substring(port.IndexOf(":") + 1));
            Socket socket = await ConnectToServer(ipAddress, portInserted);

            if (socket != null)
            {
                Console.WriteLine("Connected successfully");
            }
            else
            {
                Console.WriteLine("Connection failed, please check the IP and Port and try again. \nPress any key to close...");
                
                return;
            }


                while (true)
                {

                    if (nickname == "" && socket != null)
                    {
                        Console.WriteLine("Insert your username: ");
                        nickname = Console.ReadLine();
                        var payload = new
                        {
                            Nickname = nickname,
                            Command = "setNickname",
                        };
                        string payloadToServer = JsonSerializer.Serialize(payload);
                        await SendMessage(socket, payloadToServer);
                        Console.WriteLine("Username sended");
                        string response = await ReceiveData(socket);
                        JsonElement serverResponse = JsonSerializer.Deserialize<JsonElement>(response);
                        Console.WriteLine($"Your session id is {sessionID}");
                        Console.WriteLine("You can send messages to the server, type 'message' to send a message or 'disconnect' to disconnect from the server");
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
                                Nickname = nickname,
                                AuthId = sessionID,
                                Message = message,
                                Command = "sendingMessage",
                            };

                            string payloadToServer = JsonSerializer.Serialize(payload);
                            Console.WriteLine("Payload created");
                            await SendMessage(socket, payloadToServer);
                            Console.WriteLine(payloadToServer);
                            Console.WriteLine("Message sended");


                        string server = await ReceiveData(socket);

                            while (server == "")
                            {
                                Console.WriteLine("Waiting a response from the server");

                            }


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.Message}");
                        }

                    }

                


                if (command.ToString() == "testing" && nickname != "")
                    {
                        Console.WriteLine("Test will be sended");
                        var payload = new
                        {
                            Nickname = nickname,
                            AuthId = sessionID,
                            Message = "this is a test",
                            Command = "testing",
                        };
                        string payloadToServer = JsonSerializer.Serialize(payload);
                        await SendMessage(socket, payloadToServer);
                        Console.WriteLine(payloadToServer);
                        Console.WriteLine("Test sended");
                    }

                }
            }
        }
} 
