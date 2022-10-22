using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PolimonBase Base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;
    [SerializeField] bool canBeCaptured;
    private Polimon polimon;
    private Image image;

    public bool deslizar;

    public Transform originPoint;
    public Transform movePoint;  

    public Polimon PolimonAtual(){
        return polimon;
    }  

    public BattleUnit(bool isPlayerUnit_, bool canBeCaptured_) {
        IsPlayerUnit = isPlayerUnit_;
        CanBeCaptured = canBeCaptured_;
        if (IsPlayerUnit)
            GetComponent<Image>().sprite = Base.BackSprite;
        else
            GetComponent<Image>().sprite = Base.FrontSprite;
    }

    public void Setup(Polimon Polimon,bool isPlayerUnit_, bool canBeCaptured_) {
        transform.position = originPoint.position;
        IsPlayerUnit = isPlayerUnit_;
        CanBeCaptured = canBeCaptured_;
        if (IsPlayerUnit)
            GetComponent<Image>().sprite = Polimon.Base.BackSprite;
        else
            GetComponent<Image>().sprite = Polimon.Base.FrontSprite;
    }
    public bool CanBeCaptured { get => canBeCaptured; set => canBeCaptured = value; }
    public bool IsPlayerUnit { get => isPlayerUnit; set => isPlayerUnit = value; }

    void FixedUpdate()
    {
        if(deslizar){
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, 20f*Time.deltaTime);
        }
    }

    public void SetActive(bool active){
        this.gameObject.SetActive(active);
    }

    public void deslizarBatalha(){
        deslizar = true;
    }
    
}
