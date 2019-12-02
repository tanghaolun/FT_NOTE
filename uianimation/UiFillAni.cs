using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFillAni : MonoBehaviour
{
    public Image FillImage;

    public float DelayTime;

    public float DelayTimeMove;

    public float FillDu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        FillDu = 0;
        DelayTimeMove = 0;
    }

    // Update is called once per frame
    void Update()
    {
        DelayTimeMove += Time.deltaTime;
        if (DelayTimeMove < DelayTime)
        {
            return;
        }

        if (FillDu < 1)
        {
            FillDu += Time.deltaTime*3f;
            FillImage.fillAmount = FillDu;
        }


    }
}
