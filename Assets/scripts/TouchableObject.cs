using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TouchableObject : MonoBehaviour
{
    public string action;
    private Sprite normalSprite;
    public GameObject over;
    public Sprite selected;
    public bool isSelected = false;
    public bool isChecked = false;
    public GameMaster master;

    public delegate void TouchedObjectEventHandler(object source, EventArgs e);

    public event TouchedObjectEventHandler TouchedObject;

    void Start()
    {
        this.normalSprite = this.GetComponent<SpriteRenderer>().sprite;
        master = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();
        this.setSelected(isSelected);
    }

    public void OnTouch(){
        if(TouchedObject != null){
            Debug.Log("Calling event handler");
            TouchedObject(this, EventArgs.Empty);
        }
    }


    public void setSelected(bool value)
    {
        this.isSelected = value;
        if (value && !isChecked)
        {
            this.over.SetActive(false);
            this.GetComponent<SpriteRenderer>().sprite = this.selected;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        }
    }

    public void setChecked(bool value, string sufix)
    {
        this.isChecked = value;

        foreach (Transform item in this.GetComponentsInChildren<Transform>(true))
        {
            if (item.tag.Contains("tick-check"))
            {
                if(item.CompareTag("tick-check-" + sufix)){
                    item.gameObject.SetActive(value);
                }
            }
        }
    }

    public void OnMouseOver()
    {
        if (!this.isChecked)
        {
            this.over.SetActive(false);
            this.setSelected(true); 
        }
    }

    public void OnMouseExit()
    {
        if(!isChecked){
            this.over.SetActive(true);
        }
        this.setSelected(false);
    }

    private void OnMouseDown()
    {
        if (!isChecked && !Action.getPlayingState()) // TODO: checar se está jogando para ativar ou desativar o checked  
        {
            OnTouch();
        }
    }
}
