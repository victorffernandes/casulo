using System.Collections;
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

    public void setChecked(bool value)
    {
        this.isChecked = value;

        foreach (Transform item in this.GetComponentsInChildren<Transform>(true))
        {
            if (item.CompareTag("tick-check"))
            {
                this.over.SetActive(false);
                item.gameObject.SetActive(this.isChecked);
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
        if (!isChecked && !this.master.getIsPlaying())
        {
            master.playGameAction(action);
            this.setChecked(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.normalSprite = this.GetComponent<SpriteRenderer>().sprite;
        master = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();
        this.setSelected(isSelected);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
