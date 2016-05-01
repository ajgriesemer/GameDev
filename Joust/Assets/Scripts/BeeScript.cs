using UnityEngine;
using System.Collections;

public class BeeScript : MonoBehaviour
{
    public GameObject mainSprite;
    public GameObject secondarySprite;
    public GameObject warriorPrefab;
    public GameObject deadPrefab;
    public int TeamNumber;
    public int PlayerNumber;
    public Sprite[] RiderArray;
    public RuntimeAnimatorController[] MountArray;
    private bool lastStandingState = false;
    private SpriteBase mainSpriteScript;
    private SpriteBase secondarySpriteScript;

    // Use this for initialization
    void Start()
    {
        //When the Bee object is first created instantiate the mainSprite in the center of the screen
        mainSprite = (GameObject)Instantiate(warriorPrefab, this.transform.position, Quaternion.identity);
        mainSpriteScript = mainSprite.GetComponent<MountScript>();

        mainSprite.name = warriorPrefab.name;
        mainSpriteScript.OnBeeDeath += KillBee;
        mainSpriteScript.OnStandingEnter += StartStanding;

        mainSprite.GetComponent<Animator>().runtimeAnimatorController = MountArray[TeamNumber];
        mainSprite.transform.Find("Rider").GetComponent<SpriteRenderer>().sprite = RiderArray[PlayerNumber];

    }
    private void KillBee()
    {
        Vector3 spritePosition = mainSprite.transform.position;

        mainSpriteScript.OnBeeDeath -= KillBee;
        mainSpriteScript.OnStandingEnter -= StartStanding;

        Destroy(mainSprite);
        mainSprite = Instantiate(deadPrefab, spritePosition, Quaternion.identity) as GameObject;

        //Rename the object to prevent multiple (clone) labels from being added
        mainSprite.name = deadPrefab.name;

        if (secondarySprite != null)
        {
            Destroy(secondarySprite);
        }
    }

    private void StartStanding(bool standing)
    {
        mainSprite.SendMessage("StandingAnimation", standing);
        if (secondarySprite != null)
        {
            secondarySprite.SendMessage("StandingAnimation", standing);
        }
        lastStandingState = standing;
    }

    public void MoveBee(float? horizontal, bool? up)
    {
        if (horizontal != null)
        {
            mainSprite.SendMessage("MoveHorizontal", horizontal.Value);
            if (secondarySprite != null)
            {
                secondarySprite.SendMessage("MoveHorizontal", horizontal.Value);
            }
        }
        if (up != null)
        {
            mainSprite.SendMessage("Jump", up.Value);
            if (secondarySprite != null)
            {
                secondarySprite.SendMessage("Jump", up.Value);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float halfScreenWidth = Camera.main.orthographicSize * (float)Screen.width / (float)Screen.height;
        float halfSpriteWidth = mainSprite.GetComponent<SpriteRenderer>().bounds.size.x / (float)2.0;

        if (mainSprite.transform.position.x > (halfScreenWidth - halfSpriteWidth) || mainSprite.transform.position.x < -(halfScreenWidth - halfSpriteWidth))
        {
            if (secondarySprite == null)
            {
                if (mainSprite.transform.position.x > 0)
                {
                    //Create secondary sprite on left side of screen
                    secondarySprite = Instantiate(mainSprite, new Vector3(mainSprite.transform.position.x - 2 * halfScreenWidth, mainSprite.transform.position.y, 0), Quaternion.identity) as GameObject;
                    secondarySpriteScript = secondarySprite.GetComponent<MountScript>();
                    //Rename the object to prevent multiple (clone) labels from being added
                    secondarySprite.name = mainSprite.name;
                    secondarySprite.SendMessage("StandingAnimation", lastStandingState);

                }
                else
                {
                    //Create secondary sprite on right side of screen
                    secondarySprite = Instantiate(mainSprite, new Vector3(mainSprite.transform.position.x + 2 * halfScreenWidth, mainSprite.transform.position.y, 0), Quaternion.identity) as GameObject;
                    secondarySpriteScript = secondarySprite.GetComponent<MountScript>();
                    //Rename the object to prevent multiple (clone) labels from being added
                    secondarySprite.name = mainSprite.name;
                    secondarySprite.SendMessage("StandingAnimation", lastStandingState);
                }
                secondarySprite.GetComponent<Rigidbody2D>().isKinematic = true;
            }
            else
            {
                if (mainSprite.transform.position.x > (halfScreenWidth + halfSpriteWidth) || mainSprite.transform.position.x < -(halfScreenWidth + halfSpriteWidth))
                {
                    //Make the secondary sprite the main sprite
                    secondarySprite.gameObject.GetComponent<Rigidbody2D>().velocity = mainSprite.gameObject.GetComponent<Rigidbody2D>().velocity;
                    secondarySpriteScript.horizontalInput = mainSpriteScript.horizontalInput;
                    secondarySprite.GetComponent<Rigidbody2D>().isKinematic = false;
                    mainSprite.GetComponent<MountScript>().OnBeeDeath -= KillBee;
                    mainSprite.GetComponent<MountScript>().OnStandingEnter -= StartStanding;

                    Destroy(mainSprite);
                    mainSprite = secondarySprite;
                    mainSpriteScript = secondarySpriteScript;
                    mainSprite.GetComponent<MountScript>().OnBeeDeath += KillBee;
                    mainSprite.GetComponent<MountScript>().OnStandingEnter += StartStanding;

                    secondarySprite = null;
                }
                else
                {
                    //Move secondary sprite with main sprite
                    secondarySprite.gameObject.GetComponent<Rigidbody2D>().velocity = mainSprite.GetComponent<Rigidbody2D>().velocity;
                }
            }
        }
        else
        {
            if (secondarySprite != null)
            {
                Destroy(secondarySprite);
            }
        }
    }
}
