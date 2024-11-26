using Godot;
using System;

public partial class KillPlayer : Node
{
    Timer timer = null;
    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
    }

    //Kills a given body
    public void Kill(Node2D body)
	{
		Engine.TimeScale = 0.5;
		body.GetNode("CollisionShape2D").QueueFree();

		timer.Start();

    }
    //Resets game when the timer ends
    public void _on_timer_timeout()
    {
		Engine.TimeScale = 1;
		GetTree().ReloadCurrentScene();	        
    }
}
