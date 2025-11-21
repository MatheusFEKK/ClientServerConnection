using System;
using System.Net.Sockets;

class ServerList
{
	public string AuthID { get; set; }
	public string Nickname { get; set; }
	public Socket handler { get; set; }

	public ServerList(string AuthID, string Nickname, Socket handler)
	{
		this.AuthID = AuthID;
		this.Nickname = Nickname;
		this.handler = handler;
	}
}
