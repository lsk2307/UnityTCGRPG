using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gatcha : MonoBehaviour
{
    public GameObject effect;
    public UIManager uiM;
    public Text cashText;


    //소환 버튼 클릭시
    public void Summon()
    {
        GatchaEffect ge = effect.GetComponent<GatchaEffect>();
        if (PlayerDataBase.instance.cash >= 100)
        {
            PlayerDataBase.instance.cash -= 100;

            CashChange();
            uiM.cash.text = PlayerDataBase.instance.cash.ToString();

            int rate = Random.Range(0, 100);
            if (rate < 90)
            {
                
                int ran = Random.Range(0, PlayerDataBase.instance.PlayerRare.Count);
               
                effect.SetActive(true);
                ge.EffectOn(ran, false);
                PlayerDataBase.instance.CharaGet(ran,false);
            }
            else
            {
                int ran = Random.Range(0, PlayerDataBase.instance.PlayerSRare.Count);

                effect.SetActive(true);
                ge.EffectOn(ran, true);
                PlayerDataBase.instance.CharaGet(ran, true);
            }
        }
    }
   
    //가챠 창 끄기
    public void SetOff()
    {
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        CashChange();
    }

    void CashChange()
    {
        int cashing = PlayerDataBase.instance.cash;
        
        if(cashing < 1000)
        {
            cashText.text = cashing.ToString();
        }
        else if(cashing >= 1000 && cashing < 1000000)
        {
            cashing /= 1000;
            cashText.text = cashing + "K";
        }
        else if(cashing >= 1000000)
        {
            cashing /= 1000000;
            cashText.text = cashing + "M";
        }

    }
}
