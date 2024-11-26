using Godot;
using System;

public partial class PurpleSlime : CharacterBody2D
{
	public const float Speed = 60.0f;
	RayCast2D rayCastRight = null;
	RayCast2D rayCastLeft = null;
	RayCast2D rayCastDownRight = null;
	RayCast2D rayCastDownLeft = null;
	AnimatedSprite2D sprite = null;
	KillPlayer killPlayer = null;
	int direction = 1;

	public override void _Ready()
    {
        rayCastRight = GetNode<RayCast2D>("RayCastRight");
		rayCastLeft = GetNode<RayCast2D>("RayCastLeft");
		rayCastDownRight = GetNode<RayCast2D>("RayCastDownRight");
		rayCastDownLeft = GetNode<RayCast2D>("RayCastDownLeft");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		killPlayer = GetNode<KillPlayer>("KillPlayer");
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Change direction in bumps into a wall
		
		if(rayCastRight.IsColliding())
		{
			direction = -1;
			sprite.FlipH = true;
		}
		if(rayCastLeft.IsColliding())
		{
			direction = 1;
			sprite.FlipH = false;
		}
		if(!rayCastDownRight.IsColliding())
		{
			direction = -1;
			sprite.FlipH = true;
		}
		if(!rayCastDownLeft.IsColliding())
		{
			direction = 1;
			sprite.FlipH = false;
		}
		velocity.X = direction * Speed;
		Velocity = velocity;
		MoveAndSlide();

		if(Position.Y > 152)
		{
			QueueFree();
		}
	}

	private void _on_hurtbox_body_entered(PlayerCSharp body)
	{
		if(body.Name == "Player")
		{
			Vector2 playervelocity = body.Velocity;
			float y_delta = Position.Y - body.Position.Y;
			GD.Print(y_delta);
			if(y_delta > 11)
			{
				GetNode("CollisionShape2D").QueueFree();
				playervelocity.Y = -270.0f;
				body.Velocity = playervelocity;				
			}
			else
			{
				body.Move = false;
				playervelocity.X = Velocity.X * 2;
				playervelocity.Y = -150.0f;

				body.Velocity = playervelocity;

				killPlayer.Kill(body);
			}
		}
	}
}
