using Godot;
using System;

public partial class GreenSlime : Node2D
{
	public const float Speed = 60;
	int direction = 1;
	RayCast2D rayCastRight = null;
	RayCast2D rayCastLeft = null;
	AnimatedSprite2D sprite = null;
	private Vector2 slimePosition;
	public override void _Ready()
	{
		rayCastRight = GetNode<RayCast2D>("RayCastRight");
		rayCastLeft = GetNode<RayCast2D>("RayCastLeft");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		//slimePosition = new Vector2(872, -76);
		//Position = slimePosition;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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

		slimePosition.X = Position.X + direction * Speed * (float)delta;
		slimePosition.Y = Position.Y;
		//Position = new Vector2(Position.X + direction * Speed * (float)delta, Position.Y);
		Position = slimePosition;
	}
}
