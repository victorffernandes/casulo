using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestionAction : Action
{
    private string[] answers;
    private bool answered = false;

    public AudioClip successFeedback, errorFeedback, answerAudio;
    private FaderManager fader;

    private void Start()
    {
        successFeedback = (AudioClip)Resources.Load("Sounds/certo-feedback");
        errorFeedback = (AudioClip)Resources.Load("Sounds/errado-feedback");
        fader = GameObject.FindObjectOfType<FaderManager>();

        if (!Action.mainAudioSource)
        {
            Action.mainAudioSource = GameObject.FindObjectOfType<AudioSource>();
            Action.master = GameObject.FindObjectOfType<GameMaster>().gameObject;
        }

        string phaseName = Action.master.GetComponent<GameMaster>().phaseName;
        answerAudio = (AudioClip)Resources.Load("Sounds/" + phaseName + "/answers/" + actionName);
    }

    public QuestionAction(string line) : base(line)
    {
        string[] splittedAction = line.Split('|');
        this.answers = splittedAction.Length > 3 ? splittedAction[3].Split(',') : new string[0];
        this.Start();        
        if(!text)
        {
            text = GameObject.Find("TextArea").transform.Find("Text").GetComponent<Text>();
        }
    }

    public override void play(PlayCallback callback)
    {
        if (!isPlaying)
        {
            isPlaying = true;
            float time = this.getAudioClip().length;
            mainAudioSource.PlayOneShot(this.getAudioClip());
            text.text = "";
            master.GetComponent<MonoBehaviour>().StartCoroutine(setText(audioText(this.getAudioClip().ToString())));

            Action.master.GetComponent<MonoBehaviour>().StartCoroutine(this.delayFill(this.getText(), time, callback));
        }
    }
 
    IEnumerator successAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        mainAudioSource.PlayOneShot(this.successFeedback);
        this.fader.startSuccessAnim();
    }

    private void errorAnim()
    {
        mainAudioSource.PlayOneShot(this.errorFeedback);
        this.fader.startErrorAnim();
    }

    public bool answer(string name)
    {
        if (this.isAnswer(name))
        {
            mainAudioSource.PlayOneShot(answerAudio);
            Action.master.GetComponent<GameMaster>().StartCoroutine(successAnim(answerAudio.length));
            answered = true;
            return true;
        }
        else
        {
            errorAnim();
            return false;
        }
    }

    public bool isAnswer(string answerTry)
    {
        List<string> answers = new List<string>(this.answers);

        return !string.IsNullOrEmpty(answers.Find(x => x.Contains(answerTry)));
    }

    public bool gotAnswered()
    {
        return answered;
    }

}
