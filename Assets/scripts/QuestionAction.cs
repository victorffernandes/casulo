using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class QuestionAction : Action
{
    private string[] answer;

    private bool isAnswered = false;

    public bool isAnswer(){
        return isAnswered;
    }

    public QuestionAction(string line) : base(line){
        string[] splittedAction = line.Split('|');
        this.answer = splittedAction.Length > 3 ? splittedAction[3].Split(','): new string[0];
    }

    public bool checkAnswer(string answerTry){
        List<string> answers = new List<string>(this.answer);

        this.isAnswered = !string.IsNullOrEmpty(answers.Find(x => x.Contains(answerTry)));
        return isAnswered;
    }

}
