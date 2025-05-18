using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    internal class Client
    {
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
                            string templateMSG = $"Message: ";
                            message = Console.ReadLine();
                            await SendMessage(socket, templateMSG + message);
                            Console.WriteLine("Message sended");
                            continue;
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
    }
}
