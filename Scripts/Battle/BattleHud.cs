using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BattleHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] HPBar HpBar;
    [SerializeField] TextMeshProUGUI HpPoints;

    Polimon polimon;

    public void SetData(Polimon Polimon) {

        polimon = Polimon;

        nameText.text = Polimon.Base.Name;
        levelText.text = "Lv "+Polimon.Level;
        HpBar.SetHp((float)Polimon.HP / Polimon.MaxHP);
        if (HpPoints != null) {
            HpPoints.text = Polimon.HP + "/" + Polimon.MaxHP;
        }
    }

    public IEnumerator UpdateHP()
    {
        if (HpPoints != null)
        {
            HpPoints.text = polimon.HP + "/" + polimon.MaxHP;
        }
        yield return HpBar.SetHpSmooth((float)polimon.HP / polimon.MaxHP);
        
    }

    public void desaparecer(){
        gameObject.SetActive(false);
    }

    public void aparecer(){
        gameObject.SetActive(true);
    }
}
