using Godot;
using System;
using System.Numerics;

public partial class PlayerCSharp : CharacterBody2D
{
	public const float Speed = 130.0f;
	public const float JumpVelocity = -270.0f;
	private AnimatedSprite2D sprite = null;
	private Timer coyoteTimer = null;
	public bool Move {get; set;} = true;
	public override void _Ready()
	{
		coyoteTimer = GetNode<Timer>("CoyoteTimer");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	public override void _PhysicsProcess(double delta)
	{
		Godot.Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && (IsOnFloor() || !coyoteTimer.IsStopped()))
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction:(-1,0,1).
		// As good practice, you should replace UI actions with custom gameplay actions.
		Godot.Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
		
		// Flip the sprite.
		if ( direction.X > 0)
		{
			sprite.FlipH = false;
		}
		else if(direction.X < 0)
		{
			sprite.FlipH = true;
		}

		// Play animations
		if(IsOnFloor())
		{
			if(direction.X == 0)
			{
				sprite.Play("idle");
			}
			else
			{
				sprite.Play("run");
			}
		}
		else
		{
			sprite.Play("jump");
		}

		// Handle the movement/deceleration
		if (direction != Godot.Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else if (Move)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		
		bool wasOnFloor = IsOnFloor();

		Velocity = velocity;

		MoveAndSlide();
		
		if (wasOnFloor != IsOnFloor()) 
		{
			coyoteTimer.Start();
		}	
	}
}
