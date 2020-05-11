using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {

    }

    public void startGameButton() {
        GameObject.FindGameObjectWithTag("fade").GetComponent<Animator>().SetInteger("fade", 1);
        Debug.Log("Ajustando jogo");
    }

    // Update is called once per frame
    void Update() {
        
    }
}
