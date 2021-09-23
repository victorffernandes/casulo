using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;

public class Action
{
    protected static bool isPlaying = false;
    protected string actionName;
    private AudioClip audioClip;
    private string actionText;
    public static AudioSource mainAudioSource;
    public static GameObject master;
    public delegate void PlayCallback();

    public VideoPlayer videoPlayer;

    public Text text;
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
            text = GameObject.Find("TextArea").transform.Find("Text").GetComponent<Text>();
        }
        if(!text)
        {
            text = GameObject.Find("TextArea").transform.Find("Text").GetComponent<Text>();
        }
        if(!videoPlayer){
            videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
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
            text.text = "";
            Action.mainAudioSource.PlayOneShot(this.getAudioClip());
            videoPlayer.clip = (VideoClip)Resources.Load(urlVideo(this.getAudioClip().ToString()));
            videoPlayer.Play();
            master.GetComponent<MonoBehaviour>().StartCoroutine(setText(audioText(this.getAudioClip().ToString())));
            master.GetComponent<MonoBehaviour>().StartCoroutine(this.delayFill(this.getText(), time, callback));
        }
    }

    public IEnumerator setText(string textInfo)
    {
        for(int i = 0; i < textInfo.Length; i++)
        {
            text.text += textInfo[i];
            yield return new WaitForSeconds(0.08f);
        }
    }

    public string audioText(string audioName)
    {
        string trueName = audioName.Split(' ')[0];
        //fase1
        if (string.Equals(trueName,"inicio-ossos"))
            return "O corpo humano pode ser analizado sobre diversas áreas, podemos estudar as células, os tecidos, os orgãos, os sistemas, enfim aqui nesse momento estudaremos os ossos.";
        if (string.Equals(trueName,"esqueleto-geral"))
            return "Você sabia que o esqueleto humano é composto por 206 ossos que são movimentados pelos músculos e ligados uns aos outros pelas articulações.";
        if (string.Equals(trueName,"ossos-bracos-pernas-info"))
            return "Os ossos dos braços e das pernas sustentam os músculos responsáveis pelos movimentos.";
        if (string.Equals(trueName,"femur-info"))
            return "O femur é osso mais comprido do corpo humano e liga o quadril ao joelho.";
        if (string.Equals(trueName,"membros-inferiores-ossos-info"))
            return "O membro inferior possui 62 ossos, sendo 26 em cada pé, 2 tibias, 2 fíbulas 2 rotulas, 2 femurs e 2 coxas.";
        if (string.Equals(trueName,"caixa-toraxica-info"))
            return "A caixa toraxica fica no peito e protege o coração e os pulmões";
        if (string.Equals(trueName,"estribo-info"))
            return "O estribo é o menor osso do corpo humano e fica localizado dentro do nosso ouvido.";
        if (string.Equals(trueName,"estribo-pergunta"))
            return "Com o mouse, clique no menor osso do corpo humano";
        if (string.Equals(trueName,"femur-pergunta"))
            return "Com o mouse, clique no maior osso do corpo humano";
        if (string.Equals(trueName,"caixa-toraxica-pergunta"))
            return "Clique com o mouse onde fica a caixa toraxica";
        
        //fase 2
        if (string.Equals(trueName,"introducao"))
            return "O corpo humano é formado por cinco orgãos dos sentidos são eles a visão, o olfato, o paladar, a audicao e o tato.";
        if (string.Equals(trueName,"visao"))
            return "A visão, por meio dos olhos enxergamos o mundo e todas as coisas.";
        if (string.Equals(trueName,"audicao"))
            return "Audição, através dos nossos ouvidos discriminamos sons altos, baixos, fortes, fracos.";
        if (string.Equals(trueName,"olfato"))
            return "O olfato, através do nariz respiramos sentimos odores e cheiros que podem ser bons ou ruins.";
        if (string.Equals(trueName,"paladar"))
            return "O paladar pela boca podemos falar, respirar, beijar, sentir o gosto azedo, amargo, doce, salgado, gostoso ou ruim.";
        if (string.Equals(trueName,"tato"))
            return "Tato, por meio das mãos podemos tocar e sentir as texturas tais como aspero e liso, grosso e fino.";
        if (string.Equals(trueName,"pergunta-audicao"))
            return "Clique na parte do corpo que usamos para ouvir";
        if (string.Equals(trueName,"pergunta-paladar"))
            return "Clique no orgão do sentido do paladar";
        if (string.Equals(trueName,"pergunta-tato"))
            return "Clique na parte do corpo do membro superior que usamos para pegar um objeto";
        if (string.Equals(trueName,"pergunta-visao"))
            return "Clique nos órgãos dos sentidos que usamos para ver";

        
        Debug.Log(trueName);
        //fase 3
        if (string.Equals(trueName,"intro"))
            return "Venha jogar e se divertir aprendendo. Através de uma viagem no corpo humano. Clique aqui.";
        if (string.Equals(trueName,"cabelo-info"))
            return "Cabelo. O cabelo fica localizado na nossa cabeça. Ele pode ser longo ou curto. Pode ser liso ou crespo, encaracolado. Cabelo pode ser da cor: preto, castanho, loiro ou ruivo";
        if (string.Equals(trueName,"pe-direito"))
            return "Pé direito";
        if (string.Equals(trueName,"pes-info"))
            return "Pés. Os pés são importantes pois nos auxiliam na postura do corpo. Eles servem também para nos locomover de um local para o outro. Geralmente os pés são compostos por cinco dedos cada um.";
        if (string.Equals(trueName,"braco-esquerdo"))
            return "Braço esquerdo.";
        if (string.Equals(trueName,"maos-info"))
            return "Mãos. Geralmente temos duas mãos. Cada uma com cinco dedos, que servem para pegarmos as coisas e objetos. Além de fazermos carinho nas pessoas.";
        if (string.Equals(trueName,"olhos-info"))
            return "Os olhos fazem parte do sentido da visão. Através dele vemos o mundo e todas as coisas. Os olhos podem ser das cores: verde, preta, azul ou castanho. Você sabia que nem todas as pessoas veem igual? Há pessoas que veem diferente, por meio do tato através das mãos";
        if (string.Equals(trueName,"pergunta-pe-direito"))
            return "Clique no pé direito";
        if (string.Equals(trueName,"pergunta-braco-esquerdo"))
            return "Clique no braço esquerdo";
        if (string.Equals(trueName,"pergunta-cabelo"))
            return "Clique onde fica localizado o cabelo";

        //fase 4
        if (string.Equals(trueName,"intro-info"))
            return "O corpo humano precisa de cuidados: Lavar as mãos, tomar banho, pentear os cabelos e escovar os dentes. São tarefas diárias, importantes para nossa higiene e o bom funcionamento do corpo. Para aprender mais sobre higiene e saúde clique no botão abaixo";
        if (string.Equals(trueName,"fio-dental-info"))
            return "Fio dental. O fio dental serve para remover, ou seja retirar, a sujeira que fica entre nossos dentes após nos alimentarmos.";
        if (string.Equals(trueName,"escova-info"))
            return "Escova de dente. Para os dentes ficarem bem limpos e brancos temos que escova-los todos os dias após as refeições, usando a escova de dentes e o creme dental, ou a pasta de dente. Não se esqueça de limpar todos os cantinhos, e inclusive aqueles dentes localizados lá atrás. Há! Também não esqueça de limpar a sua língua!";
        if (string.Equals(trueName,"sabonete-info"))
            return "Sabonete. O sabonete é um produto que serve para limpar a sujeira, a poeira, o suor e as bactérias, que são aqueles bichinhos invisíveis a olho nu que podem estar presentes em nosso corpo pelo acumulo da sujeira do dia a dia. Por isso o sabonete é tão importante pra higiene do nosso corpo. Ele é muito cheiroso e vai limpar e tirar todos esses resíduos.";
        if (string.Equals(trueName,"shampoo-info"))
            return "Shampoo e condicionador. O shampoo e o condicionador são produtos destinados a cuidarem da saúde dos nossos cabelos. Eles mantem a limpeza do nosso couro cabeludo. Por isso é muito importante, sempre que tomarmos banho passarmos o shampoo e o condicionador, para que os nossos cabelos possam ficar sempre limpos e saudáveis. Além de cheirosos e macios";
        if (string.Equals(trueName,"pergunta-fio-dental"))
            return "Agora que você já aprendeu sobre a higiene bucal, responda: Qual produto usamos para tirar a sujeira entre os dentes?";
        if (string.Equals(trueName,"pergunta-shampoo"))
            return "Agora que você já aprendeu sobre higiene do corpo humano, responda: Qual produto usamos para limpar os cabelos?";
        
        return "";
    }

    public string urlVideo(string audioName){
        string trueName = audioName.Split(' ')[0];
        string phaseName = Action.master.GetComponent<GameMaster>().phaseName;
        return ("Videos/" + phaseName + "/" + trueName);
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
