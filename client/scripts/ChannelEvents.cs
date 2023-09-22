using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json.Linq;
using Phoenix;

class ChannelEvents {
    static public void UpdatePlayers(Message body, Client client) {
		PlayerListPayload payload = body.Payload.Unbox<PlayerListPayload>();
		var players = new List<Player>(payload.players);//payload.players;
		players.RemoveAll(player => {
			return player.id == client.local_id;
		});
		client.EmitSignal("updatePlayers", players.ToArray<Player>());
	}

    static public void UpdateArrows(Message body, Client client) {}
}
