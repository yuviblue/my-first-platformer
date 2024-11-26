using Godot;
using System;




public partial class Killzone : Area2D
{
	KillPlayer killPlayer = null;
	public override void _Ready()
	{
		killPlayer = GetNode<KillPlayer>("KillPlayer");
	}
	
	//Kills the body that enters the Area2D
	private void _on_body_entered(Node2D body)
	{
		GD.Print("You died!");
		killPlayer.Kill(body);
	}
}
