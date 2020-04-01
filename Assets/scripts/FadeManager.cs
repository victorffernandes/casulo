using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FadeManager : MonoBehaviour
{
    public int fadeOutScene;

    public void changeScene()
    {
        SceneManager.LoadScene(fadeOutScene);
    }

}
