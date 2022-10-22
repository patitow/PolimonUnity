using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSending : MonoBehaviour
{
    public Image animateImage;
    public Transform originPoint;
    public Transform movePoint;  
    public Transform finalPoint;  
    public int deslizar=0;
    public bool parado = true;
    public CharacterManager personagem;
    int frames=0;
    float tempo = 0;

    void Update(){
        if(parado){
            animateImage.sprite = personagem.PersonagemEscolhido.battleAnimation[0];
        }else{
            if(frames<4){
                if(Time.time - tempo > 0.2f){
                    tempo = Time.time;
                    frames++;
                    animateImage.sprite = personagem.PersonagemEscolhido.battleAnimation[frames];
                }
            }
        }

        if(deslizar==1){
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, 20f * Time.deltaTime);
        }
        else if(deslizar==2){
            transform.position = Vector3.MoveTowards(transform.position, finalPoint.position, 20f * Time.deltaTime);
        }
    }

    public void deslizarBatalha(){
        parado = true;
        frames = 0;
        tempo = 0;
        transform.position = originPoint.position;
        deslizar = 1;
    }
}
