using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject Health;
    [SerializeField] float speed;
    [SerializeField] Color highLife;
    [SerializeField] Color midLife;
    [SerializeField] Color lowLife;
    Image image;


    private void Update()
    {
        if (Health.transform.localScale.x < 0)
            Health.transform.localScale = new Vector3(0,1f,1f);
        if (Health.transform.localScale.x > 1)
            Health.transform.localScale = new Vector3(1f,1f,1f);
        
    }

    public void SetHp(float hp) {
        image = Health.GetComponent<Image>();
        image.color = highLife;
        Health.transform.localScale = Vector3.Lerp(Health.transform.localScale, new Vector3(hp, 1f, 1f), speed);
        

    }

    public IEnumerator SetHpSmooth(float newHp) {
        float curHP = Health.transform.localScale.x;
        float changeAmt = curHP - newHp;

        while (curHP - newHp > Mathf.Epsilon) {
            curHP -= changeAmt * Time.deltaTime;
            Health.transform.localScale = new Vector3(curHP, 1f, 1f);
            if (highLife != null && midLife != null && lowLife != null)
            {
                if (curHP > 0.75f)
                    image.color = highLife;
                else if (curHP > 0.30f)
                    image.color = midLife;
                else
                    image.color = lowLife;
            }
            yield return null;
        }
        Health.transform.localScale = new Vector3(newHp, 1f, 1f);
    }
}
