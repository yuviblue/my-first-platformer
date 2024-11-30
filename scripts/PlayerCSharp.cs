using System;
using System.ComponentModel.DataAnnotations;
using Godot;

public partial class PlayerCSharp : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float JumpVelocity = -350.0f;
	public const float Acceleration = 6.0f;
	public const float WallFriction = 40.0f;
	public const float WallJumpPushback = 400.0f;
	private bool IsWallSliding{get; set;} = false;
	public bool Move {get; set;} = true;
	private bool IsAlive{get; set;} = true;
	private AnimatedSprite2D sprite = null;
	private Timer coyoteTimer = null;
	private Timer deathTimer = null;
	private CollisionShape2D collision = null;
	private Vector2 velocity;
	
	public override void _Ready()
	{
		coyoteTimer = GetNode<Timer>("CoyoteTimer");
		deathTimer = GetNode<Timer>("DeathTimer");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		collision = GetNode<CollisionShape2D>("CollisionShape2DFrog");
		collision.Disabled = false;
	}

	public void Kill()
	{
		deathTimer.Start();
		collision.SetDeferred("disabled", true);
    	IsAlive = false;
    }



	//Resets game when the timer ends
	public void _on_death_timer_timeout()
	{
		GetTree().ReloadCurrentScene();	
	}


	//Handles jumping and wall jumping.
	//Not working yet.
	private void Jump()
	{
		if(Input.IsActionJustPressed("jump"))
		{		
			if(Input.IsActionPressed("move_right") && IsWallSliding)
			{
				velocity.X = -WallJumpPushback;
				velocity.Y = JumpVelocity;
			}
			else if(Input.IsActionPressed("move_left") && IsWallSliding)
			{
				velocity.X = WallJumpPushback;
				velocity.Y = JumpVelocity;
			}
			else if (IsOnFloor() || !coyoteTimer.IsStopped())
			{
				velocity.Y = JumpVelocity;	
			}
		}
	}

	//Handles wall sliding.
	private void WallSlide(double delta)
	{
		if(IsOnWallOnly() && Velocity.Y > 0 && (Input.IsActionPressed("move_left") || Input.IsActionPressed("move_right")))
		{
			IsWallSliding = true;
			velocity.Y += WallFriction * (float)delta;
			velocity.Y = Math.Min(velocity.Y, WallFriction);
		}
		else
		{
			IsWallSliding = false;
		}
	}

    

	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor() && !IsWallSliding)
		{
			velocity += GetGravity() * (float)delta;	
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

		// Handles animations.
		if(!IsAlive)
		{
			sprite.Play("hit");
		}
		else if(IsOnFloor())
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
		else if(IsWallSliding)
		{
			sprite.Play("wall_jump");
	}	
		else if(velocity.Y < 0)	
		{	
			sprite.Play("jump");
			
		}
		else
		{
			sprite.Play("fall");	
		}
		

		// Handle the movement/deceleration
		if (direction != Vector2.Zero)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, direction.X * Speed, Acceleration);
		}
		else if (Move)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Jump();
		WallSlide(delta);
		bool wasOnFloor = IsOnFloor();

		Velocity = velocity;

		MoveAndSlide();

		if (wasOnFloor != IsOnFloor()) 
		{
			coyoteTimer.Start();
		}

		
	}
}
