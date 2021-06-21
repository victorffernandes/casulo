using System.IO;
using System.Net.Mime;
using System;
using System.Collections.Generic;
using UnityEngine;


public class GameMaster : MonoBehaviour
{
    private GameMode actualMode = GameMode.Information;
    public enum GameMode { Information = 1, Quiz = 2 };

    public Action runningAction;
    public Dictionary<string, Action> actionsDictionary = new Dictionary<string, Action>();

    private TouchableObject[] touchableObjects;

    private AudioSource mainAudioSource;
    public string phaseName;
    void Start()
    {
        TextAsset resource = Resources.Load<TextAsset>("texts/texts-" + phaseName);
        string[] actionsStrings = resource.text.Split('\n'); // resource.text;
        mainAudioSource = gameObject.GetComponent<AudioSource>();
        touchableObjects = GameObject.FindObjectsOfType<TouchableObject>();


        foreach (string actionString in actionsStrings) // creating all actions
        {
            Action action = Action.actionFactory(actionString);
            this.actionsDictionary.Add(action.getActionName(), action);
        }

        foreach (TouchableObject item in touchableObjects) // subscribing on all touchable objects
        {
            item.TouchedObject += OnTouch;
        }

        //changeToQuiz();
        getGameAction("introduction").play(checkQuestionModeCallback);
    }

    public void OnTouch(object source, EventArgs e)
    { // callback sempre que um touchable object é tocado
        TouchableObject tObj = ((TouchableObject)source);
        String actionName = tObj.action;
        Debug.Log("Event called " + actionName);

        if (!Action.getPlayingState())
        {
            if (GameMode.Information.Equals(actualMode))
            { // atualiza a running action e roda ela
                runningAction = this.getGameAction(actionName);
                tObj.setChecked(true, "success");
                runningAction.play(checkQuestionModeCallback);
            }
            else
            { // tenta responder a pergunta da running action

                QuestionAction actual = (QuestionAction)this.runningAction;
                if (!Action.getPlayingState())
                {
                    if (actual.answer(actionName))
                    {
                        tObj.setChecked(true, "success");
                    }
                    else
                    {
                        tObj.setChecked(true, "error");
                    }
                }
            }
        }
    }

    public bool isEveryoneChecked()
    {
        bool isChecked = true;
        foreach (TouchableObject item in this.touchableObjects)
        {
            if (!(item.isChecked))
            {
                isChecked = false;
            }
        }
        return isChecked;
    }

    public void resetAll()
    {
        Debug.Log("Reseting all touchable objects");
        foreach (TouchableObject a in touchableObjects)
        {
            a.setChecked(false, "success");
            a.setChecked(false, "error");
            a.setSelected(false);
            a.over.SetActive(true);
        }
    }

    public Action getGameAction(string actionName)
    {
        Action action;
        actionsDictionary.TryGetValue(actionName, out action);
        actionsDictionary.Remove(actionName);
        return action;
    }

    public void nextQuestion() // chamada no final da animação de sucesso da pergunta
    {
        // QuestionAction answered = (QuestionAction)this.runningAction;
        // if (answered.gotAnswered())
        string mainKey = "";
        foreach (string key in new List<string>(actionsDictionary.Keys))
        {
            if (mainKey.Equals(""))
            {
                Debug.Log(actionsDictionary.ToString());
                mainKey = key;
                actionsDictionary.TryGetValue(mainKey, out runningAction);
                Debug.Log(runningAction.getActionName());

                runningAction.play(questionCallback);
                actionsDictionary.Remove(mainKey);
            }
        }

        if (mainKey.Equals(""))
        {
            GameObject.FindGameObjectWithTag("game-over").GetComponent<Animator>().Play("game-over");
        }
    }

    public void questionCallback()
    {
        resetAll();
    }

    public void acceptQuizInit()
    {
        GameObject.FindGameObjectWithTag("quiz-init").GetComponent<Animator>().Play("quiz-init-exit");
        nextQuestion();
    }

    private void changeToQuiz()
    {
        resetAll();
        this.actualMode = GameMode.Quiz;
        GameObject.FindGameObjectWithTag("quiz-init").GetComponent<Animator>().Play("quiz-init-enter");
    }

    private void checkQuestionModeCallback()
    {
        if (this.isEveryoneChecked())
        {
            changeToQuiz();
        }
    }

}
