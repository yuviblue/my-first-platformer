using Godot;




public partial class Killzone : Area2D
{	
	//Kills the body that enters the Area2D
	private void _on_body_entered(PlayerCSharp body)
	{
		GD.Print("You died!");
		//body.IsAlive = false;
		body.Kill();
	}
}
