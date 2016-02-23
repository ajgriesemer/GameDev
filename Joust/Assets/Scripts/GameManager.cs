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

    private List<HumanPlayer> Players = new List<HumanPlayer>();

    public void StartGame()
    {
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

        // register airconsole events
        AirConsole.instance.onConnect += AirConsole_onConnect;
        AirConsole.instance.onDisconnect += AirConsole_onDisconnect;
        AirConsole.instance.onCustomDeviceStateChange += AirConsole_onCustomDeviceStateChange;
        AirConsole.instance.onMessage += AirConsole_onMessage;
    }

    private void UpdatePlayerCount()
    {
        var numPlayersText = GameObject.Find("NumPlayersText").GetComponent<Text>();
        numPlayersText.text = "# Players: " + this.Players.Count;

        var numReadyPlayersText = GameObject.Find("NumReadyPlayersText").GetComponent<Text>();
        numReadyPlayersText.text = "# Ready Players: " + this.Players.Where(p => p.IsReady).Count();
    }

    private void AirConsole_onConnect(int device_id)
    {
        this.Players.Add(new HumanPlayer(device_id));

        this.UpdatePlayerCount();
    }

    private void AirConsole_onDisconnect(int device_id)
    {
        var player = this.Players.FirstOrDefault(p => p.DeviceId == device_id);
        if (player != null)
        {
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
