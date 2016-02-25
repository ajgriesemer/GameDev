using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class ControllerInterface : MonoBehaviour {

    public GameObject Bee;
    private GameObject[] Bees = new GameObject[8];
    
	void Awake() {
		AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
	}

	//Called when a new controller connects
	void OnConnect(int device_id) {
		//Assign all device IDs a player number (max players is 4)
		//Do not just use the device_id it is not a reliable indicator of that "player"
		AirConsole.instance.SetActivePlayers (4);
		//Get the player number of the player that just started
		int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
		//Create a new player GameObject for that person
		Bees [active_player] = (GameObject) Instantiate (Bee, Vector3.zero, Quaternion.identity);
        //Rename the object to prevent multiple (clone) labels from being added
        Bees[active_player].name = Bee.name;

        Bees[active_player].GetComponent<BeeScript>().PlayerNumber = active_player;
        Bees[active_player].GetComponent<BeeScript>().TeamNumber = active_player%2;

        Debug.Log (active_player);
	}

	//Called (a little bit after) a controller disconnects
	void OnDisconnect(int device_id) {
		//Get the player number of the player that just left
		int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
		Debug.Log (active_player);
		//Destroy the player GameObject associated with that person
		Destroy (Bees [active_player]);
	}

	//Called every time a message is received
	// Messages are sent every time a button is pressed on the controller
	//  - The left and right buttons are interpreted as press and hold
	//    so the player will continue to move horizontally as long as the button is pressed
	//  - The up button must be tapped. A single discrete force push 
	//    is applied each time the button is pressed.
	//
	void OnMessage(int device_id, JToken data) {

		Debug.Log (data);
		//Get the player number of the player that just pushed a button
		int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
		Debug.Log (active_player);
		//Check that the player number is correct
		if (active_player != -1) {
			//If there is data in the left attribute
			if (data ["left"] != null) {
				//If true the person just pressed the left button
				if ((bool) data ["left"] == true) {
                    //Set the appropriate variables in the player data component
                    Bees[active_player].GetComponent<BeeScript>().MoveBee(-1, null);
				}
			}
			//If there is data in the left attribute	
			if (data ["right"] != null) {
				//If true the person just pressed the right button
				if ((bool) data ["right"] == true)
                {
                    //Set the appropriate variables in the player data component
                    Bees[active_player].GetComponent<BeeScript>().MoveBee(1, null);
                }
			}	
			//If there is data in the up attribute
			if (data ["up"] != null) {
				if ((bool) data ["up"] == true)
                {
                    //Set the appropriate variables in the player data component
                    Bees[active_player].GetComponent<BeeScript>().MoveBee(null, true);
                }
			}	
		}

	}

	void OnDestroy() {
		// unregister airconsole events on scene change
		if (AirConsole.instance != null) {
			AirConsole.instance.onMessage -= OnMessage;
		}
	}

}
