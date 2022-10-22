using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartySelection : MonoBehaviour
{

    public List<GameObject> Polimon;
    public List<Image> PolIcon;
    public List<TextMeshProUGUI> PolName;

    public void SetActive(bool set){
        this.gameObject.SetActive(set);
    }

    public void SetPolimon(int pos, Polimon pol){
        Polimon[pos].SetActive(true);
        PolIcon[pos].sprite = pol.Base.FrontSprite;
        PolName[pos].text = pol.Base.Name;
    }

    public void UpdatePolimonSelection(int selectedAction, Polimon Polimon){
        for (int i = 0; i < PolName.Count; i++)
        {
            if (i == selectedAction)
            {
                if(Polimon.HP!=0){
                    PolName[i].color = Color.blue;
                }else{
                    PolName[i].color = Color.gray;
                }
            }
            else {
                PolName[i].color = Color.black;
            }
        }
    }


}
