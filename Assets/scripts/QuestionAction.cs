using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class QuestionAction : Action
{
    private string answer;
    public QuestionAction(string line) : base(line){
        string[] splittedAction = line.Split('|');
        this.answer = splittedAction.Length > 3 ? splittedAction[3]: "";
    }

    public string getAnswer(){
        return this.answer;
    }

}
