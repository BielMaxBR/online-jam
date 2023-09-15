using Godot;
using System;

public class player : KinematicBody2D {
    private int jump_force = 600;
    private float speed = 313;
    private Vector2 velocity = Vector2.Zero;
    private int gravity = 1200;

    public override void _PhysicsProcess(float delta) {
        HandleInput();
        MovePlayer(delta);
        GD.Print(GetLastSlideCollision()?.Position - GlobalPosition);
    }
    public void HandleInput() {
        velocity.x = 0;
        bool jump = Input.IsActionJustPressed("jump");
        bool stopJump = Input.IsActionJustReleased("jump");
        float direction = Input.GetAxis("left", "right");

        if (IsOnFloor()) {
            velocity.y = 0;
            if (jump) {
                velocity.y -= jump_force;
            }
        } else {
            if ((stopJump && velocity.y < 0) || IsOnCeiling()) {
                velocity /= 3;
            }
        }

        velocity.x = direction * speed;
    }

    public void MovePlayer(float delta) {
        velocity.y += gravity * delta;

        velocity = MoveAndSlide(velocity, Vector2.Up);
    }
}
