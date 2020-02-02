using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    
    public void LoadNewScene(string scene)
    {
        Time.timeScale = 1;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Sound/ui_click");
        SceneManager.LoadScene(scene);
    }

    public void LoadNextScene()
    {
        Time.timeScale = 1;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Sound/ui_click");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ReloadScene()
    {
        Time.timeScale = 1;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Sound/ui_click");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LadSceneByBuildIndex(int buildIndex)
    {
        Time.timeScale = 1;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Sound/ui_click");
        SceneManager.LoadScene(buildIndex);

    }
    public void QuitButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Sound/ui_click");
        Application.Quit();
    }
}
