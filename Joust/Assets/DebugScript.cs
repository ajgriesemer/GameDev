using UnityEngine;
using System.Collections;

public class DebugScript : MonoBehaviour {
    public GameObject DebugSprite;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        bool up;
	    if(DebugSprite != null)
        {
            if(Input.GetAxis("Jump") > 0)
            {
                up = true;
            }
            else
            {
                up = false;
            }
            DebugSprite.GetComponent<BeeScript>().MoveBee(Input.GetAxis("Horizontal"), up);
        }
	}
}
