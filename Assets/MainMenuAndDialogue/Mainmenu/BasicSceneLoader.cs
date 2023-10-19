using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSceneLoader : MonoBehaviour
{
    public int toThisSceneBASIC;

    public void LoadScene(int sceneName)
    {
        SceneManager.LoadScene(toThisSceneBASIC);
    }
}