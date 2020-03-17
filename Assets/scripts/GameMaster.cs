using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class GameMaster : MonoBehaviour
{
    private bool isPlaying = false;
    public Dictionary<string, AudioSource> audios = new Dictionary<string, AudioSource>();
    public Dictionary<string, string> texts = new Dictionary<string, string>();


    public void playGameAction(string actionName)
    {
        if(!isPlaying){
            isPlaying = true;
            float time = audios[actionName].clip.length;
            audios[actionName].Play();
            StartCoroutine(delayFill(texts[actionName], time));
        }
    }

    IEnumerator delayFill(string text, float time){
        Text textObject = GameObject.FindGameObjectWithTag("text-box").GetComponent<Text>();
        textObject.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            yield return new WaitForSeconds((time / text.Length));
            textObject.text += text[i];
        }
        isPlaying = false;
        yield return 0 ;
    }

    // Start is called before the first frame update
    void Start()
    {
        string[] linhas = System.IO.File.ReadAllLines("./Assets/texts/texts-phase1");
        AudioSource[] allAudios = this.GetComponents<AudioSource>();
        if (allAudios.Length.Equals(linhas.Length))
        {
            for (int i = 0; i < linhas.Length; i++) // todos os audios estão mapeados a um texto
            {
                string[] linha = linhas[i].Split('|');
                texts.Add(linha[0], linha[1]);
                audios.Add(linha[0], allAudios[i]);
            }
        }
        playGameAction("intro");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
