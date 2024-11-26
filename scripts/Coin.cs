using Godot;
using System;

public partial class Coin : Area2D
{
	GameManager gameManager = null;
	AnimationPlayer animationPlayer = null;
	public override void _Ready()
    {
        gameManager = GetNode<GameManager>("%GameManager");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
	private void _on_body_entered(Node2D body)
	{
		gameManager.AddPoints();
		animationPlayer.Play("pickup");
	}
}
