using UnityEngine;
using System.Collections;

public class BeeScript : MonoBehaviour {
	public GameObject mainSprite;
	public GameObject secondarySprite;
	public GameObject flyingPrefab;

	// Use this for initialization
	void Start () {
		//When the Bee object is first created instantiate the mainSprite in the center of the screen
		this.mainSprite = (GameObject) Instantiate (flyingPrefab, Vector3.zero, Quaternion.identity);
        this.mainSprite.GetComponent<MountScript>().OnPlayerDeath += KillBee;
	}

    private void KillBee()
    {
        Destroy(mainSprite);
        if(secondarySprite != null)
        {
            Destroy(secondarySprite);
        }
    }

    public void MoveBee(float? horizontal, bool? up)
    {
        if (horizontal != null)
        {
            this.mainSprite.GetComponent<MountScript>().horizontalInput = horizontal.Value;
        }
        if (up != null)
        {
            this.mainSprite.GetComponent<MountScript>().upInput = up.Value;
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
