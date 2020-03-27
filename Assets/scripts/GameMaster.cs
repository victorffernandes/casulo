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

        this.allTouch = this.GetComponents<TouchableObject>();
        this.mainAudioSource = this.gameObject.GetComponent<AudioSource>();
        playGameAction("introduction");
    }

    void Update()
    {
        if (this.actualMode.Equals(GameMode.Information) && !isPlaying && this.isEveryoneChecked())
        {
            this.actualMode = GameMode.Quiz;
            this.runQuestion();
        }
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
            }
            else if (this.actualMode.Equals(GameMode.Quiz))
            {
                QuestionAction question = (QuestionAction)this.runningAction;
                if(this.runningAction.getActionName().Equals(actionName)){ // é resposta
                } else { // não é resposta

                }
            }
        }
    }

    private void runQuestion()
    {
        string mainKey = "";
        foreach (string key in actionsDictionary.Keys)
        {
            if(mainKey.Equals("")){
            mainKey = key;
            this.actionsDictionary.TryGetValue(mainKey, out this.runningAction);

            float time = this.runningAction.getAudioClip().length;
            this.mainAudioSource.PlayOneShot(this.runningAction.getAudioClip());

            isPlaying = true;
            StartCoroutine(delayFill(this.runningAction.getText(), time));
            this.actionsDictionary.Remove(mainKey);
            }
        }

        if(mainKey.Equals("")){
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
    yield return 0;
}
}
