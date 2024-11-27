using Godot;

public partial class GameManager : Node
{
    Label coinsLabel = null;
    int score = 0;
    public override void _Ready()
    {
        coinsLabel = GetNode<Label>("%CoinsLabel");
    }

    //Adds the collected coins and displays them
    public void AddPoints()
    {
        score++;
        coinsLabel.Text ="Coins: " + score.ToString();
    }

}
