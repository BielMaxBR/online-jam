using Godot;
using System;
using Phoenix;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Godot.Collections;

public class Client : Node2D
{
	private Socket socket;
	private Channel channel;

	public string local_id;

	[Signal]
	public delegate void joined(string id);
	[Signal]
	public delegate void updatePlayers(Player[] players);
	[Signal]
	public delegate void updateArrows(Arrow[] arrows);
	public override void _Ready()
	{
		// testes
		GD.Print("dom bia");

		// Login("0.tcp.sa.ngrok.io:11842", "nok");
		Login("localhost:4000", "nok");
	}

	public void Login(string url, string name)
	{
		var socketOptions = new Socket.Options(new JsonMessageSerializer());
		var socketAddress = $"ws://{url}/socket";
		var socketFactory = new Phoenix.WebSocketImpl.DotNetWebSocketFactory();


		var args = new System.Collections.Generic.Dictionary<string, string>(){
  			{"nickname", name }
		};

		socket = new Socket(socketAddress, args, socketFactory, socketOptions);

		socket.OnClose += onCloseCallback;
		socket.OnError += onErrorCallback;
		socket.OnMessage += onMessageCallback;
		socket.OnOpen += onOpenCallback;

		socket.Connect();

		channel = socket.Channel("game:lobby");
		Push push = channel.Join();
		push.Receive(ReplyStatus.Timeout, reply => GD.Print("não foi possivel conectar ao canal"))
			.Receive(ReplyStatus.Error, reply => GD.Print("Erro ao conectar ao canal"))
			.Receive(ReplyStatus.Ok, reply =>
			{
				string _id = reply.Response.Unbox<IdPayload>().player_id;
				local_id = _id;
				GD.Print("conectado ao canal usando id ",_id);
				EmitSignal("joined", _id);
			});

		channel.On("update_players", body => ChannelEvents.UpdatePlayers(body, this));
		channel.On("update_arrows", body => ChannelEvents.UpdateArrows(body, this));

	}

	public void SendNewPlayer(Player player)
	{
		channel.Push("new_player", new PlayerPayload()
		{
			x = player.Position.x,
			y = player.Position.y
		});
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
