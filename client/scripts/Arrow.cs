using Godot;

public class Arrow : KinematicBody2D
{
	public float speed = 750;
	public int gravity = 750;
	public Vector2 direction = new Vector2(1, 0);
	private Vector2 velocity = new Vector2();
	private float counter = 0;
	private bool isDanderous = true;

	private const int Enemy = 2; 
	private const int Item = 3; 
	private const int PlayerHurtBox = 1; 
	private const int Grabber = 4;
	public override void _Ready()
	{
		velocity = direction * speed;
	}

	public override void _Process(float delta)
	{
		if (isDanderous)
		{
			counter += delta;
			if (counter > 0.5) velocity.y += gravity * delta;
			Rotation = velocity.Angle();
		} else {
			velocity = Vector2.Zero;
		}
		MoveAndSlide(velocity, Vector2.Up);

		var collision = GetLastSlideCollision();
		bool condition = collision == null;
		
		// GD.Print(collision?.Position);
		
		isDanderous = condition;
		//            bit a partir de 0, booleano
		SetCollisionLayerBit(Enemy, condition); 
		SetCollisionLayerBit(Item, !condition);
		
		SetCollisionMaskBit(PlayerHurtBox, condition);
		SetCollisionMaskBit(Grabber, !condition);
	}
}