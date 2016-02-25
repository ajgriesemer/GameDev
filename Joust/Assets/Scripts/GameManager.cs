using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using NDream.AirConsole;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class GameManager : MonoBehaviour
{   
    // class for holding team info
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
    
    // class for holding player info
    public class HumanPlayer
    {
        public int DeviceId { get; private set; }
        public bool IsReady { get; set; }

        public HumanPlayer(int deviceId)
        {
            this.DeviceId = deviceId;
            this.IsReady = false;
        }
    }
    
    // singleton instance of the GameManager
    public static GameManager instance = null;

    private bool gameStarting = false;
    private List<Team> Teams = new List<Team>();
    private List<HumanPlayer> Players = new List<HumanPlayer>();

    public int numTeams = 2;
    public int countdownTime = 3;
    public Text CountdownText;

    public void StartGame()
    {
        var message = new
        {
            type = "start",
        };
        AirConsole.instance.Broadcast(message);

        SceneManager.LoadScene("MainScene");
    }

    // Unity method for initialization
    private void Awake()
    {
        // manage the singleton instance of the GameManager
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        // create teams
        this.Teams = new List<Team>
        {
            new Team("Blue"),
            new Team("Green")
        };

        // set the menu text
        this.CountdownText.text = "";

        // register airconsole events
        AirConsole.instance.onConnect += AirConsole_onConnect;
        AirConsole.instance.onDisconnect += AirConsole_onDisconnect;
        AirConsole.instance.onCustomDeviceStateChange += AirConsole_onCustomDeviceStateChange;
        AirConsole.instance.onMessage += AirConsole_onMessage;
    }

    private void UpdatePlayerCount()
    {
        var numPlayers = this.Players.Count;
        var numPlayersText = GameObject.Find("NumPlayersText").GetComponent<Text>();
        numPlayersText.text = "# Players: " + numPlayers;

        var numReady = this.Players.Where(p => p.IsReady).Count();
        var numReadyText = GameObject.Find("NumReadyText").GetComponent<Text>();
        numReadyText.text = "# Ready Players: " + numReady;

        // if all the players are ready, begin counting down to the start of the game
        if (!gameStarting && numPlayers == numReady)
        {
            this.gameStarting = true;
            StartCoroutine(CountdownToStart(this.countdownTime));
        }
    }

    private IEnumerator CountdownToStart(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            this.CountdownText.text = "Starting in " + i;

            yield return new WaitForSeconds(1);
        }

        this.CountdownText.text = "GO!";
        this.StartGame();
    }

    private void AssignToTeam(HumanPlayer player)
    {
        var leastTeamMembers = this.Teams.Select(t => t.Players.Count).Min();
        var smallestTeam = this.Teams.First(t => t.Players.Count == leastTeamMembers);

        smallestTeam.Players.Add(player);

        var message = new
        {
            type = "team",
            data = smallestTeam.Color
        };
        AirConsole.instance.Message(player.DeviceId, message);
    }

    private void AirConsole_onConnect(int device_id)
    {
        var player = new HumanPlayer(device_id);

        this.Players.Add(player);
        this.AssignToTeam(player);

        this.UpdatePlayerCount();
    }

    private void AirConsole_onDisconnect(int device_id)
    {
        var player = this.Players.FirstOrDefault(p => p.DeviceId == device_id);
        if (player != null)
        {
            var playerTeam = this.Teams.FirstOrDefault(t => t.Players.Contains(player));
            if (playerTeam != null)
            {
                playerTeam.Players.Remove(player);
            }

            this.Players.Remove(player);
        }

        this.UpdatePlayerCount();
    }

    private void AirConsole_onCustomDeviceStateChange(int device_id, JToken custom_device_data)
    {
        Debug.Log("State changed");

        if (custom_device_data["isPlayerReady"] != null)
        {
            var player = this.Players.FirstOrDefault(p => p.DeviceId == device_id);
            if (player != null)
            {
                bool isReady;
                if (bool.TryParse(custom_device_data["isPlayerReady"].ToString(), out isReady))
                {
                    player.IsReady = isReady;
                }
            }

            this.UpdatePlayerCount();
        }
    }

    private void AirConsole_onMessage(int from, Newtonsoft.Json.Linq.JToken data)
    {
        Debug.Log("Message received");
    }
}
