using Godot;
using System;
using Phoenix;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class Client : Node2D
{
	private Socket socket;
	private Channel channel;
	public override void _Ready()
	{
		GD.Print("dom bia");
		// testes
		
		// Login("0.tcp.sa.ngrok.io:11842", "nok");
		Login("localhost:4000", "nok");
	}

	public void Login(string url, string name)
	{
		var socketOptions = new Socket.Options(new JsonMessageSerializer());
		var socketAddress = $"ws://{url}/socket";
		var socketFactory = new Phoenix.WebSocketImpl.DotNetWebSocketFactory();


		var args = new Dictionary<string, string>();
		args.Add("name", name);

		socket = new Socket(socketAddress, args, socketFactory, socketOptions);

		socket.OnOpen += onOpenCallback;
		socket.OnMessage += onMessageCallback;
		socket.OnError += onErrorCallback;
		socket.OnClose += onCloseCallback;

		socket.Connect();
	}

	private void onCloseCallback(ushort code, string message)
	{
		GD.Print("closed: ", code, message);
	}
	private void onErrorCallback(string message)
	{
		GD.Print("erro: ", message);
	}
	private void onMessageCallback(Message message)
	{
		// mensagens globais, não específico de canal
		// GD.Print("recebido ", message.Event, " ", message.Payload.Unbox<JObject>());
	}
	private void onOpenCallback()
	{
		GD.Print("entrando");
	}
}
