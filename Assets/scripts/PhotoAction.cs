using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhotoAction : QuestionAction
{
    private string[] answers;
    private bool answered = false;

    private AudioClip successFeedback, errorFeedback;

    private GameObject pt;

    private string fileName;
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

        pt = GameObject.FindGameObjectWithTag("photo-taker");

        string phaseName = Action.master.GetComponent<GameMaster>().phaseName;
        fileName = phaseName + "_" + this.actionName;
    }

    public PhotoAction(string line) : base(line)
    {
        string[] splittedAction = line.Split('|');
        this.Start();
    }

    public override void play(PlayCallback callback)
    {
        if (!isPlaying)
        {
            isPlaying = true;
            float time = this.getAudioClip().length;
            mainAudioSource.PlayOneShot(this.getAudioClip());

            Action.master.GetComponent<MonoBehaviour>().StartCoroutine(this.delayFill(this.getText(), time, callback));

            PhoneCamera pc = GameObject.FindObjectOfType<PhoneCamera>();
            pt.GetComponent<Animator>().Play("photo-taker-clip");
            pc.activateCamera(this);
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
        Action.master.GetComponent<GameMaster>().StartCoroutine(successAnim(0));
        pt.GetComponent<Animator>().Play("no-photo-taker-clip");
        answered = true;
        return true;
    }
}
