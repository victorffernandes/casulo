using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Action
{
    protected static bool isPlaying = false;
    protected string actionName;
    private AudioClip audioClip;
    private string actionText;
    public static AudioSource mainAudioSource;
    public static GameObject master;
    public delegate void PlayCallback();
    public static Action actionFactory(string line)
    {
        string[] splittedLine = line.Split('|');

        if(splittedLine.Length > 0 && splittedLine[0].IndexOf("$photo") != -1 ){
            return new PhotoAction(line);
        }

        if (splittedLine.Length > 3)
        {
            return new QuestionAction(line);
        }

        return new Action(line);
    }

    public Action(string line)
    {
        string[] splittedAction = line.Split('|');

        this.actionName = splittedAction[0];
        this.audioClip = this.loadAudio(splittedAction[1]);
        this.actionText = splittedAction[2];

        if (!Action.mainAudioSource)
        {
            Action.mainAudioSource = GameObject.FindObjectOfType<AudioSource>();
            master = GameObject.FindObjectOfType<GameMaster>().gameObject;
        }
    }

    public IEnumerator delayFill(string text, float time, PlayCallback callback)
    {
        Action.isPlaying = true;
        for (int i = 0; i < text.Length; i++)
        {
            yield return new WaitForSeconds((time / text.Length));
        }

        Action.isPlaying = false;
        callback();
        yield return 0;
    }

    public virtual void play(PlayCallback callback)
    {
        if (!Action.isPlaying)
        { // se não estiver nenhuma action rolando
            float time = this.getAudioClip().length;
            Action.mainAudioSource.PlayOneShot(this.getAudioClip());

            master.GetComponent<MonoBehaviour>().StartCoroutine(this.delayFill(this.getText(), time, callback));
        }
    }

    public static bool getPlayingState()
    {
        return Action.isPlaying;
    }
    public AudioClip getAudioClip()
    {
        return this.audioClip;
    }

    public string getText()
    {
        return this.actionText;
    }

    public string getActionName()
    {
        return this.actionName;
    }

    private void defaultCallback() {
        // do default stuff
    }

    public AudioClip loadAudio(string path)
    {
        AudioClip audio = Resources.Load<AudioClip>(path);
        return audio;
    }

}
