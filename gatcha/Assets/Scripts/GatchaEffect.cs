using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class GatchaEffect : MonoBehaviour
{
    public Light2D[] light;
    public Image hide;
    public Image chara;
    public Image[] stars;
    public Sprite[] starSprites;
    public Text rareText;

    float opacity = 0.5f;
    bool opacityUp;
    byte hideOpacity = 255;

    bool cancel;
    int rareNum;


    //이펙트 온
    public void EffectOn(int num, bool rare)
    {
        cancel = false;

        hideOpacity = 255;

       

        if(rare)
        {
            rareNum = PlayerDataBase.instance.PlayerSRare[num].rare;
            hide.color = new Color32(0, 0, 0, hideOpacity);
            chara.sprite = PlayerDataBase.instance.PlayerSRare[num].sprites[0];

            light[1].color = new Color32(224, 40, 0, 255);
            rareText.text = "SR";
            rareText.color = Color.yellow;

            for (int i = 0; i < PlayerDataBase.instance.PlayerSRare[num].starLevel; ++i)
            {
                stars[i].sprite = starSprites[1];
            }

            for (int i = PlayerDataBase.instance.PlayerSRare[num].starLevel; i < 5; ++i)
            {
                stars[i].sprite = starSprites[0];
            }
        }
        else
        {
            rareNum = PlayerDataBase.instance.PlayerRare[num].rare;
            hide.color = new Color32(0, 0, 0, hideOpacity);
            chara.sprite = PlayerDataBase.instance.PlayerRare[num].sprites[0];

            light[1].color = new Color32(11, 0, 255, 255);
            rareText.text = "R";
            rareText.color = Color.red;

            for (int i = 0; i < PlayerDataBase.instance.PlayerRare[num].starLevel; ++i)
            {
                stars[i].sprite = starSprites[1];
            }

            for (int i = PlayerDataBase.instance.PlayerRare[num].starLevel; i < 5; ++i)
            {
                stars[i].sprite = starSprites[0];
            }
        }

       
       
        opacity = 0.5f;
        light[1].volumeOpacity = 0;
        StartCoroutine("Effect");
    }

    //코루틴을 통한 가챠 애니메이션
    IEnumerator Effect()
    {
        yield return new WaitForSeconds(0.5f);

        cancel = true;

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 10; ++i)
        {
            light[1].volumeOpacity += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < 17; i++)
        {
            hideOpacity -= 15;
            hide.color = new Color32(0, 0, 0, hideOpacity);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(!opacityUp)
            opacity += 0.03f;
        else
            opacity -= 0.03f;
        light[0].volumeOpacity = opacity;

        if (opacity >= 1)
            opacityUp = true;
        else if (opacity < 0.5f)
            opacityUp = false;

        if(Input.GetMouseButtonDown(0) && cancel && rareNum == 0)
        {
            StopCoroutine("Effect");
            StartCoroutine("StopEffect");
        }
    }

    //레어도가 R인 경우 스킵가능 기능
    IEnumerator StopEffect()
    {
        light[1].volumeOpacity = 1;
        hide.color = new Color32(0, 0, 0, 0);
        

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
