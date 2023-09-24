using Godot;
using System;
using System.Collections.Generic;

public class Root : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	public Client client;
	public Node2D entities;
	public Player localPlayer;
	private float timer = 0;
	private bool ready = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		client = GetNode<Client>("/root/Client");
		entities = GetNode<Node2D>("Entities");

		client.Connect("joined", this, "_onJoin");
		client.Connect("updatePlayers", this, "_onUpdatePlayers");
		client.Connect("updateArrows", this, "_onUpdateArrows");
	}

	public override void _PhysicsProcess(float delta)
	{
		if (ready)
		{
			timer += delta;
			if (timer > .02 && !localPlayer.velocity.Equals(Vector2.Zero))
			{
				client.MovePlayer(localPlayer);
				timer = 0;
			};
		};
	}

	private void _onJoin(string id)
	{
		localPlayer = entities.GetNode<Player>("Player");
		localPlayer.id = id;
		client.SendNewPlayer(localPlayer);
		ready = true;
	}

	private void _onUpdatePlayers(List<Player> players)
	{

		foreach (Player player in players)
		{
			// GD.Print(player.id);
			if (!entities.HasNode($"Player-{player.id}"))
			{
				PackedScene PackedPlayer = ResourceLoader.Load<PackedScene>("res://objects/player.tscn");
				Player newPlayer = PackedPlayer.Instance<Player>();

				newPlayer.id = player.id;
				newPlayer.Position = player.Position;
				newPlayer.is_local_player = false;
				newPlayer.Name = $"Player-{player.id}";
				entities.AddChild(newPlayer);
			}
			else
			{
				var actualPlayer = entities.GetNode<Player>($"Player-{player.id}");
				// actualPlayer.Position = player.Position;
				GD.Print(player.last_time_moved);
				Tween tween = actualPlayer.GetNode<Tween>("Tween");
				tween.InterpolateProperty(
					actualPlayer,
					"position",
					actualPlayer.Position,
					player.Position,
					(float)Math.Clamp(player.last_time_moved - Time.GetUnixTimeFromSystem(), 0, Mathf.Inf)
				);
				tween.Start();
			};
		}
	}

	private void _onUpdateArrows(Arrow arrow)
	{
		// foreach (Arrow arrow in arrows)
		// {
		// 	if (!entities.HasNode($"Arrow-{arrow.id}"))
		// 	{
		// GD.Print(arrow.Position);
		PackedScene Packedarrow = ResourceLoader.Load<PackedScene>("res://objects/arrow.tscn");
		Arrow newarrow = Packedarrow.Instance<Arrow>();

		newarrow.id = arrow.id;
		newarrow.Position = arrow.Position;
		newarrow.direction = arrow.direction;
		newarrow.Rotation = arrow.direction.Angle();
		// newarrow.is_local_arrow = false;
		newarrow.Name = $"Arrow-{arrow.id}";
		entities.AddChild(newarrow);
		// }
		// else
		// {
		// 	var actualarrow = entities.GetNode<Arrow>($"Arrow-{arrow.id}");
		// 	actualarrow.Position = arrow.Position;
		// };
		// }
	}
}
