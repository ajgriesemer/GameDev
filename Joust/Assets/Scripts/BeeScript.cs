using UnityEngine;
using System.Collections;

public class BeeScript : MonoBehaviour {
	public GameObject mainSprite;
	public GameObject secondarySprite;
    public GameObject warriorPrefab;
    public GameObject deadPrefab;
    public int TeamNumber;
    public int PlayerNumber;
    public RuntimeAnimatorController CyanAnimator;
    public RuntimeAnimatorController FieryAnimator;
    public RuntimeAnimatorController VultureAnimator;

    // Use this for initialization
    void Start()
    {
        //When the Bee object is first created instantiate the mainSprite in the center of the screen
        mainSprite = (GameObject)Instantiate(warriorPrefab, new Vector3(0, 1), Quaternion.identity);
        mainSprite.GetComponent<MountScript>().OnBeeDeath += KillBee;
        switch (TeamNumber)
        {
            case 1:
                mainSprite.GetComponent<Animator>().runtimeAnimatorController = CyanAnimator;
                break;
            case 2:
                mainSprite.GetComponent<Animator>().runtimeAnimatorController = FieryAnimator;
                break;
            case 3:
                mainSprite.GetComponent<Animator>().runtimeAnimatorController = VultureAnimator;
                break;
            default:
                mainSprite.GetComponent<Animator>().runtimeAnimatorController = CyanAnimator;
                break;
        }
        mainSprite.transform.Find("Rider").GetComponent<SpriteRenderer>().sprite = GetComponent<RiderSpriteArray>().SpriteArray[PlayerNumber];

    }
    private void KillBee()
    {
        Vector3 spritePosition = mainSprite.transform.position;
        Destroy(mainSprite);
        mainSprite = Instantiate(deadPrefab, spritePosition, Quaternion.identity) as GameObject;

        if(secondarySprite != null)
        {
            Destroy(secondarySprite);
        }
    }

    public void MoveBee(float? horizontal, bool? up)
    {
        if (horizontal != null)
        {
            mainSprite.GetComponent<MountScript>().horizontalInput = horizontal.Value;
            if(secondarySprite != null)
            {
                secondarySprite.GetComponent<MountScript>().horizontalInput = horizontal.Value;
            }
        }
        if (up != null)
        {
            mainSprite.GetComponent<MountScript>().upInput = up.Value;
            if (secondarySprite != null)
            {
                secondarySprite.GetComponent<MountScript>().upInput = up.Value;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        float halfScreenWidth = Camera.main.orthographicSize * (float)Screen.width / (float)Screen.height;
        float halfSpriteWidth = mainSprite.GetComponent<SpriteRenderer>().bounds.size.x/(float)2.0;

        if (mainSprite.transform.position.x > (halfScreenWidth - halfSpriteWidth) || mainSprite.transform.position.x < -(halfScreenWidth - halfSpriteWidth))
        {
            if (secondarySprite == null)
            {
                if (mainSprite.transform.position.x>0)
                {
                    secondarySprite = Instantiate(mainSprite, new Vector3(mainSprite.transform.position.x - 2 * halfScreenWidth, mainSprite.transform.position.y, 0), Quaternion.identity) as GameObject;

                }
                else
                {
                    secondarySprite = Instantiate(mainSprite, new Vector3(mainSprite.transform.position.x + 2 * halfScreenWidth, mainSprite.transform.position.y, 0), Quaternion.identity) as GameObject;
                }
                secondarySprite.GetComponent<Rigidbody2D>().isKinematic = true;
            }
            else
            {
                if (mainSprite.transform.position.x > (halfScreenWidth + halfSpriteWidth) || mainSprite.transform.position.x < -(halfScreenWidth + halfSpriteWidth))
                {
                    secondarySprite.gameObject.GetComponent<Rigidbody2D>().velocity = mainSprite.gameObject.GetComponent<Rigidbody2D>().velocity;
                    secondarySprite.GetComponent<Rigidbody2D>().isKinematic = false;
                    Destroy(mainSprite);
                    mainSprite = secondarySprite;
                    secondarySprite = null;
                }
                else
                {
                    if (mainSprite.transform.position.x > 0)
                    {
                        secondarySprite.gameObject.transform.position = new Vector3(mainSprite.gameObject.transform.position.x - 2 * halfScreenWidth, mainSprite.gameObject.transform.position.y, 0);
                    }
                    else
                    {
                        secondarySprite.gameObject.transform.position = new Vector3(mainSprite.gameObject.transform.position.x + 2 * halfScreenWidth, mainSprite.gameObject.transform.position.y, 0);
                    }
                }
            }
        }
        else
        {
            if(secondarySprite != null)
            {
                Destroy(secondarySprite);
            }
        }
    }
}
