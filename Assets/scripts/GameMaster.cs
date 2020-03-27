using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class GameMaster : MonoBehaviour
{
    enum GameMode { Information = 1, Quiz = 2 };

    public Action runningAction;
    public Dictionary<string, Action> actionsDictionary = new Dictionary<string, Action>();
    private GameMode actualMode = GameMode.Information;
    private TouchableObject[] allTouch;

    private AudioSource mainAudioSource;

    public string phaseName;
    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        string[] actionsStrings = System.IO.File.ReadAllLines("./Assets/texts/texts-" + phaseName);

        foreach (string actionString in actionsStrings)
        {
            Action action = Action.actionFactory(actionString);
            this.actionsDictionary.Add(action.getActionName(), action);
        }

        this.allTouch = GameObject.FindObjectsOfType<TouchableObject>();
        Debug.Log(allTouch.Length);
        this.mainAudioSource = this.gameObject.GetComponent<AudioSource>();
        playGameAction("introduction");
    }
    public bool isEveryoneChecked()
    {
        bool isChecked = true;
        foreach (TouchableObject item in this.allTouch)
        {
            if (!item.isChecked){
                isChecked = false;   
            }
        }
        return isChecked;
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

            }
            else if (this.actualMode.Equals(GameMode.Quiz))
            {
                QuestionAction question = (QuestionAction)this.runningAction;
                if (question.isAnswer(actionName))
                { // é resposta
                    Debug.Log("É resposta certa");
                }
                else
                { // não é resposta

                }
            }
        }
    }

    private void runQuestion()
    {
        string mainKey = "";
        foreach (string key in new List<string>(actionsDictionary.Keys))
        {
            if (mainKey.Equals(""))
            {
                Debug.Log(key);
                Debug.Log(actionsDictionary.ToString());
                mainKey = key;
                this.actionsDictionary.TryGetValue(mainKey, out this.runningAction);
                Debug.Log(this.runningAction.getActionName());

                float time = this.runningAction.getAudioClip().length;
                this.mainAudioSource.PlayOneShot(this.runningAction.getAudioClip());

                isPlaying = true;
                StartCoroutine(delayFill(this.runningAction.getText(), time));
                this.actionsDictionary.Remove(mainKey);
            }
        }

        if (mainKey.Equals(""))
        {
            // game over
        }
    }
    public IEnumerator delayFill(string text, float time)
    {
        Text textObject = GameObject.FindGameObjectWithTag("text-box").GetComponent<Text>();
        textObject.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            textObject.text += text[i];
            yield return new WaitForSeconds((time / text.Length));
        }
        isPlaying = false;

        if (this.isEveryoneChecked() && this.runningAction is QuestionAction)
        {
            this.actualMode = GameMode.Quiz;
            this.runQuestion();
        }

        yield return 0;
    }
}
