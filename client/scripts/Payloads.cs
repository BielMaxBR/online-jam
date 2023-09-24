using Godot;

public class PlayerPayload
{
	public float x = 0;
	public float y = 0;
	public string id = "";
	public double timestamp = 0;
}

public class ArrowPayload
{
	public string id = "";
	public float x = 0;
	public float y = 0;
	public Vector2 direction = new Vector2(0, 0);
}

public class PlayerListPayload
{
	public PlayerPayload[] players;
}

public class ArrowListPayload
{
	public ArrowPayload[] arrows;
}

public class IdPayload
{
	public string player_id;
}