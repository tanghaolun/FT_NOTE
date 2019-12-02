using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TipFlow : MonoBehaviour
{
    float tiptime = 0;
    void OnEnable()
    {
        tiptime = 0;
    }

    void Update()
    {
        tiptime += Time.deltaTime;
        if (tiptime > 1.2f)
        {
            gameObject.SetActive(false);
        }
    }

}
