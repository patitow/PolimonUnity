using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemySending : MonoBehaviour
{
    public Image npcImageSprite;
    public Transform originPoint;
    public Transform movePoint;  
    public Transform finalPoint;  
    public int deslizar=0;

    public void deslizarBatalha(Npcs npc){
        npcImageSprite.sprite = npc.npcImage;
        transform.position = originPoint.position;
        deslizar = 1;
    }

    public void SetActive(bool active){
        this.gameObject.SetActive(active);
    }

    void Update(){

        if(deslizar==1){
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, 20f * Time.deltaTime);
        }
        else if(deslizar==2){
            transform.position = Vector3.MoveTowards(transform.position, finalPoint.position, 20f * Time.deltaTime);
        }
    }

}
