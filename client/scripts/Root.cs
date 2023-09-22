using Godot;
using System;

public class Root : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	Client client;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		client = GetNode<Client>("/root/Client");

		client.Connect("joined", this, "_onJoin");
		client.Connect("updatePlayers", this, "_onUpdatePlayers");
	}

	private void _onJoin(string id)
	{
		Player player = GetNode<Player>("Player");
		player.id = id;
		client.SendNewPlayer(player);
	}
	
	private void _onUpdatePlayers(Player[] players) {
		GD.Print(players);
		// foreach (Player player in players) {

		// }
	}
}
