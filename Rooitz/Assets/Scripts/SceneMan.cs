using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{
    static int currentSceneIndex = 0;

    private void Awake()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void SetScene(int _scene)
    {
        SetSceneStatic(_scene);
    }
    public static void SetSceneStatic(int _scene)
    {
        currentSceneIndex = _scene;
        SceneManager.LoadScene(currentSceneIndex);

        Debug.Log("Set scene to: " + _scene);
    }


    public void QuitGame()
    {
        QuiteGameStatic();
    }
    public static void QuiteGameStatic()
    {
        Application.Quit();

        Debug.Log("Quit application");
    }
}