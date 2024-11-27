using Godot;
using System;

public partial class SlimeEnemy : CharacterBody2D
{
	public const float Speed = 60.0f;
	RayCast2D rayCastRight = null;
	RayCast2D rayCastLeft = null;
	RayCast2D rayCastDownRight = null;
	RayCast2D rayCastDownLeft = null;
	AnimatedSprite2D sprite = null;
	Area2D killzone = null;
	int direction = 1;

	public override void _Ready()
    {
        rayCastRight = GetNode<RayCast2D>("RayCastRight");
		rayCastLeft = GetNode<RayCast2D>("RayCastLeft");
		rayCastDownRight = GetNode<RayCast2D>("RayCastDownRight");
		rayCastDownLeft = GetNode<RayCast2D>("RayCastDownLeft");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		killzone = GetNode<Area2D>("%Killzone");
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
			sprite.FlipH = false;
		}
		if(rayCastLeft.IsColliding())
		{
			direction = 1;
			sprite.FlipH = true;
		}
		if(!rayCastDownRight.IsColliding())
		{
			direction = -1;
			sprite.FlipH = false;
		}
		if(!rayCastDownLeft.IsColliding())
		{
			direction = 1;
			sprite.FlipH = true;
		}
		velocity.X = direction * Speed;
		Velocity = velocity;
		MoveAndSlide();

		//removes the enemy afters it dies.
		// if(Position.Y > killzone.Position.Y)
		// {
		// 	QueueFree();
		// }
	}

	private void _on_hurtbox_body_entered(PlayerCSharp body)
	{
		if(body.Name == "Player")
		{
			Vector2 playervelocity = body.Velocity;
			float y_delta = Position.Y - body.Position.Y;
			GD.Print(y_delta);
			if(y_delta > 20)
			{	
				sprite.Play("death");
				GetNode("CollisionShape2D").SetDeferred("disabled", true);
				playervelocity.Y = -350.0f;
				body.Velocity = playervelocity;				 
			}
			else
			{
				body.Move = false;
				body.Kill();
				playervelocity.X = Velocity.X * 2;
				playervelocity.Y = -150.0f;

				body.Velocity = playervelocity;

				body.IsAlive = false;
			}
		}
	}
}
