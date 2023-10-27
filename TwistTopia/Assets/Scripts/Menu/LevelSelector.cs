using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;
    public string sceneName;
    private int level;

    void Start()
    {

        //int level;

        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
        }
        else
        {
            PlayerPrefs.SetInt("Level", 0);
            level = 0;
        }



        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i;
            levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));

        }
    }

    void LoadLevel(int levelIndex)
    {
       /* if (level >= levelIndex)
        {
            if (levelIndex == 0)
            {
                SceneManager.LoadScene("Level_1");
                Time.timeScale = 1f;
            }
            else if (levelIndex == 1)
            {
                SceneManager.LoadScene("Level_2");
                Time.timeScale = 1f;
            }
            else if (levelIndex == 2)
            {
                SceneManager.LoadScene("Level_3");
                Time.timeScale = 1f;
            }
        }*/

        if (levelIndex == 0)
        {
            SceneManager.LoadScene("Level_1");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 1)
        {
            SceneManager.LoadScene("Level_2");
            Time.timeScale = 1f;
        }
        else if (levelIndex == 2)
        {
            SceneManager.LoadScene("Level_3");
            Time.timeScale = 1f;
        }


    }
}
