using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class GameMaster : MonoBehaviour
{
    enum GameMode { Information = 1, Quiz = 2 };

    private AudioSource mainAudioSource;
    private bool isPlaying = false;

    public Action runningAction;
    private GameMode actualMode = GameMode.Information;
    public string phaseName;
    TouchableObject[] allTouch;
    public Dictionary<string, Action> actionsDictionary = new Dictionary<string, Action>();

    // Start is called before the first frame update
    void Start()
    {
        string[] actionsStrings = System.IO.File.ReadAllLines("./Assets/texts/texts-" + phaseName);

        foreach (string actionString in actionsStrings)
        {
            Action action = new Action(actionString);
            this.actionsDictionary.Add(action.getActionName(), action);
        }

        this.mainAudioSource = this.gameObject.GetComponent<AudioSource>();
        this.allTouch = this.GetComponents<TouchableObject>();
        playGameAction("introduction");
    }

    public bool isEveryoneChecked()
    {
        foreach (TouchableObject item in this.allTouch)
        {
            if (!item.isChecked) return false;
        }
        return true;
    }

    public bool getIsPlaying()
    {
        return this.isPlaying;
    }

    public void playGameAction(string actionName)
    {
        if (!isPlaying)
        {
            isPlaying = true;

            if (this.actualMode.Equals(GameMode.Information))
            {
                if (actionsDictionary.TryGetValue(actionName, out this.runningAction))
                {
                    float time = this.runningAction.getAudioClip().length;
                    this.mainAudioSource.PlayOneShot(this.runningAction.getAudioClip());

                    StartCoroutine(delayFill(this.runningAction.getText(), time));
                    this.actionsDictionary.Remove(actionName);
                }
            } else if(this.actualMode.Equals(GameMode.Quiz)){

            }
        }
    }

    IEnumerator delayFill(string text, float time)
    {
        Text textObject = GameObject.FindGameObjectWithTag("text-box").GetComponent<Text>();
        textObject.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            textObject.text += text[i];
            yield return new WaitForSeconds((time / text.Length));
        }
        isPlaying = false;
        yield return 0;
    }
}
