using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Osso : MonoBehaviour
{
    public string action;
    private Sprite normalSprite;
    public Sprite selected;
    public bool isSelected = false;
    public bool isChecked = false;


    public void setSelected(bool value){
        this.isSelected = value;
        if(value && !isChecked){
            this.GetComponent<SpriteRenderer>().sprite = this.selected;
        } else{
            this.GetComponent<SpriteRenderer>().sprite = this.normalSprite;
        }
    }

    public void setChecked(bool value){
        this.isChecked = value;
        // adicionar código que da checked
    }

    public void OnMouseOver(){
        this.setSelected(true);
    }

    public void OnMouseExit(){
        this.setSelected(false);
    }

    private void OnMouseDown(){
        if(!isChecked){
            GameObject.FindGameObjectWithTag("game-master").GetComponent<GameMaster>().playGameAction(action);
            isChecked = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.normalSprite = this.GetComponent<SpriteRenderer>().sprite;
        this.setSelected(isSelected);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
