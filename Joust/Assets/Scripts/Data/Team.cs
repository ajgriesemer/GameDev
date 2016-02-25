using System.Collections.Generic;

public class Team
{
    public string Color { get; private set; }
    public List<HumanPlayer> Players { get; private set; }

    public Team(string color)
    {
        this.Color = color;
        this.Players = new List<HumanPlayer>();
    }
}