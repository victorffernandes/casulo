using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Osso : MonoBehaviour
{
    private bool isSelected = false;
    private bool isChecked = false;


    public void setSelected(bool value){
        this.isSelected = value;
        // adicionar código que troca imagem
    }

    public void setChecked(bool value){
        this.isChecked = value;
        // adicionar código que da checked
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
