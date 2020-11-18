using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StageSelect : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int stageNum =0;

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerDataBase.instance.stageNum = stageNum;
        SceneManager.LoadScene(2);
    }
}
