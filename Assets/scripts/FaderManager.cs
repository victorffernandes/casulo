using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FaderManager : MonoBehaviour
{
    public int fadeOutScene;

    public void startFadeOut(){
        this.gameObject.GetComponent<Animator>().SetInteger("fade", 1);
    }

    public void changeScene()
    {
        SceneManager.LoadScene(fadeOutScene);
    }

}
