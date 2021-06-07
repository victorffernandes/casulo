using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FaderManager : MonoBehaviour
{
    public int fadeOutScene;
    private Animator animator;

    private void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    public void startFadeOut()
    {
        animator.SetTrigger("fadeOut");
    }

    public void startSuccessAnim()
    {
        animator.SetTrigger("successQuestion");
    } 

    public void startErrorAnim()
    {
        animator.SetTrigger("errorQuestion");
    }

    public void changeSceneAnimation(int scene){
        fadeOutScene = scene;
        startFadeOut();
    }

    public void changeScene()
    {
        SceneManager.LoadScene(fadeOutScene);
    }
    public void resetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void exit(){
        Application.Quit();
    }

}
