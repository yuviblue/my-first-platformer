using Godot;

public partial class PlayerCSharp : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float JumpVelocity = -350.0f;
	private AnimatedSprite2D sprite = null;
	private Timer coyoteTimer = null;
	private Timer deathTimer = null;
	private CollisionShape2D collision = null;
	public bool Move {get; set;} = true;
	private bool IsAlive{get; set;} = true;
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
			sprite.Play("jump");		
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
