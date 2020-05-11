using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class Action
{
    private string actionName;
    private AudioClip audioClip;
    private string actionText;

    public static Action actionFactory(string line){
        string[] splittedLine = line.Split('|');

        if(splittedLine.Length > 3){
            return new QuestionAction(line);
        }
        return new Action(line);
    }

    public Action(string line){
        string[] splittedAction = line.Split('|');

        this.actionName = splittedAction[0];
        this.audioClip = this.loadAudio(splittedAction[1]);
        this.actionText = splittedAction[2];
    }

    public Action(){

        this.actionName = "defaultAction";
        this.actionText = "defaultText";
    }

    public AudioClip getAudioClip(){
        return this.audioClip;
    }

    public string getText(){
        return this.actionText;
    }

    public string getActionName(){
        return this.actionName;
    }

 public AudioClip loadAudio(string path)
    {
        AudioClip audio = Resources.Load<AudioClip>(path);
        return audio;
    }

}
