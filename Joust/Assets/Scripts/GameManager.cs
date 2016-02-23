using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // singleton instance of the GameManager
    public static GameManager instance = null;

    // Use this for initialization
    private void Awake()
    {
        // manage the singleton instance of the GameManager
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
	
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
