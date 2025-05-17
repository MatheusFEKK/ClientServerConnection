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
            Console.WriteLine("Insert the IP Address: ");

            var port = Console.ReadLine();
            var ipAddress = port.Substring(0, port.IndexOf(":"));
            var portInserted = Convert.ToUInt64(port.Substring(port.IndexOf(":") + 1));

            IPHostEntry localhost = await Dns.GetHostEntryAsync(ipAddress);
            IPAddress localAddress = localhost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(localAddress, Convert.ToInt32(portInserted));
            

            Socket client = new Socket(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
                );

            try
            {
                await client.ConnectAsync(ipEndPoint);
               
                while (client.Connected == true)
                {
                    Console.WriteLine("Connected with the server");
    
                    var command = Console.ReadLine().ToLower();
                    if (command.ToString() == "disconnect")
                    {
                        client.Close();
                        client.Disconnect(false);
                        Console.WriteLine("Disconnect from server");
                        Console.ReadKey();
                        break;
                    }
                    if (command.ToString() == "message")
                    {
                        Console.WriteLine("Write your message to the server");
                        try
                        {
                            var message = Console.ReadLine();
                            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                            ArraySegment<byte> bytes = new ArraySegment<byte>(messageBytes);
                            await client.SendAsync(bytes, SocketFlags.None);
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
