using Godot;
using System;

public class Player : KinematicBody2D
{
	// ignora essa bagaça
	// private const double Tau = 6.2831853071795862;
	[Export]
	public int jump_force = 650;
	[Export]
	public float speed = 313;
	[Export]
	public int gravity = 1300;
	[Export]
	public int arrowCounter = 3;

	public string id = "";

	private Vector2 velocity = Vector2.Zero;

	private bool jump;
	private bool stopJump;
	private bool shoot;
	private bool fastFall;
	private float direction;

	public override void _PhysicsProcess(float delta)
	{
		HandleInput();
		MovePlayer(delta);
		MoveMira();
		Shooting();
		// var vetor = (GetLastSlideCollision()?.Position - GlobalPosition);
		// if (vetor != null) GD.Print(Vector2.Right.Rotated((float)(Math.Round((double)vetor?.Angle() / Tau * 8) * Tau / 8)).Snapped(Vector2.One));
	}

	private void Shooting()
	{
		if (shoot && arrowCounter > 0)
		{
			PackedScene packedArrow = ResourceLoader.Load<PackedScene>("res://objects/Arrow.tscn");
			Arrow newArrow = packedArrow.Instance<Arrow>();

			Position2D mira = GetNode<Position2D>("Mira");

			newArrow.GlobalPosition = mira.GlobalPosition;
			newArrow.direction = mira.Position.Normalized();
			newArrow.Rotation = mira.Position.Angle();

			GetParent<Node2D>().AddChild(newArrow);

			arrowCounter -= 1;
		}
	}

	private void MoveMira()
	{
		GetNode<Position2D>("Mira").Position = new Vector2(-40, 0) // sim numero mágico
		.Rotated(
			GlobalPosition.AngleToPoint(
				GetGlobalMousePosition()
				)
			);
	}

	public void HandleInput()
	{
		velocity.x = 0;

		fastFall = Input.IsActionPressed("down");

		jump = Input.IsActionJustPressed("jump");
		stopJump = Input.IsActionJustReleased("jump");

		shoot = Input.IsActionJustPressed("shoot");

		direction = Input.GetAxis("left", "right");

		if (IsOnFloor())
		{
			velocity.y = 0;
			if (jump)
			{
				velocity.y -= jump_force;
			}
		}
		else
		{
			if ((stopJump && velocity.y < 0) || IsOnCeiling())
			{
				velocity /= 3;
			}

			if (fastFall)
			{
				velocity.y += speed / 4;
			}
		}

		velocity.x = direction * speed;
	}

	public void MovePlayer(float delta)
	{
		velocity.y += gravity * delta;
		velocity.y = Mathf.Clamp((int)velocity.y, (int)(-gravity * 1.5), (int)(gravity * 1.5));
		velocity = MoveAndSlide(velocity, Vector2.Up);
	}
	public void _on_Grabber_body_entered(Arrow body)
	{
		arrowCounter += 1;
		body.QueueFree();
	}
	public void _on_HurtBox_body_entered(Arrow body)
	{
		GD.Print(body.Name, " TOME");
	}
}
