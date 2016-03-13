using System.Collections.Generic;

public class Team
{
    public int Number { get; private set; }
    public string Color { get; private set; }
    public List<HumanPlayer> Players { get; private set; }

    public Team(int number, string color)
    {
        this.Number = number;
        this.Color = color;
        this.Players = new List<HumanPlayer>();
    }
}