using UnityEngine;
using System.Collections;

public delegate void PlayerDeath();

public class MountScript : MonoBehaviour {

    public float horizontalInput = 0;
    public bool upInput = false;
    public event PlayerDeath OnPlayerDeath;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D (Collision2D coll)
    {
        if (coll.gameObject.tag == "Warrior")
        {
            Debug.Log("Collision");
            if (coll.gameObject.transform.position.y > this.gameObject.transform.position.y)
            {
                if (OnPlayerDeath != null)
                {
                    OnPlayerDeath();
                }
            }
        }
    }
}
