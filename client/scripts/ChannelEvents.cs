using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json.Linq;
using Phoenix;
using WebSocketSharp;

class ChannelEvents
{
	static public void UpdatePlayers(Message body, Client client)
	{
		PlayerListPayload payload = body.Payload.Unbox<PlayerListPayload>();
		List<PlayerPayload> playersPayload = new List<PlayerPayload>(payload.players);

		playersPayload.RemoveAll(player =>
		{
			return player.id == client.local_id;
		});

		List<Player> players = new List<Player>();
		foreach (PlayerPayload payloadPlayer in playersPayload)
		{
			Player newPlayerObject = new Player
			{
				Position = new Vector2(payloadPlayer.x, payloadPlayer.y),
				last_time_moved = payloadPlayer.timestamp
			};
			players.Add(newPlayerObject);
		}

		client.EmitSignal("updatePlayers", players);
	}

	static public void UpdateArrows(Message body, Client client)
	{
		ArrowPayload payload = body.Payload.Unbox<ArrowPayload>();
		if (payload.id == client.local_id) return;
		// GD.Print(payload);
		Arrow arrow = new Arrow
		{
			Position = new Vector2(payload.x, payload.y),
			direction = new Vector2(payload.direction.x, payload.direction.y)
		};

		client.EmitSignal("updateArrows", arrow);
		// List<ArrowPayload> arrowsPayload = new List<ArrowPayload>(payload.arrows);

		// arrowsPayload.RemoveAll(arrow => {
		// 	return arrow.id == client.local_id;
		// });

		// List<Arrow> arrows = new List<Arrow>();
		// foreach (ArrowPayload payloadArrow in arrowsPayload) {
		// 	Arrow newArrowObject = new Arrow {
		// 		Position = new Vector2(payloadArrow.x, payloadArrow.y),
		// 		direction = (Vector2)payloadArrow.direction
		// 	};
		// 	arrows.Add(newArrowObject);
		// }

		// client.EmitSignal("updateArrows", arrows);

	}
}
