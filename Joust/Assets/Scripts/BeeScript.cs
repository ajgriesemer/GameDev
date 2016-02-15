using UnityEngine;
using System.Collections;

public class BeeScript : MonoBehaviour {
	public bool up = false;
	public bool left = false;
	public bool right = false;
	public GameObject mainSprite;
	public GameObject secondarySprite;
	public GameObject flyingPrefab;
	
	// Use this for initialization
	void Start () {
		//When the Bee object is first created instantiate the mainSprite in the center of the screen
		this.mainSprite = (GameObject) Instantiate (flyingPrefab, Vector3.zero, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
