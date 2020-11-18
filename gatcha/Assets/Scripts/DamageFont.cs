using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFont : MonoBehaviour
{
    float time;
    bool move;

    private void Update()
    {
        if(move)
        {
            time += Time.deltaTime;
            transform.position += new Vector3(0, 1, 0) * Time.deltaTime; 
        }

        if(time >= 1.5f)
        {
            move = false;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        time = 0;
        move = true;
    }
}
