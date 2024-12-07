using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI : MonoBehaviour
{
    [SerializeField] RectTransform MyRec;
    [SerializeField] float PD_t = 0, PD_r = 0, PH = 0, PW = 0, D_t = 0, D_r = 0, H = 0, W = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (PD_t == -1)
        {
            MyRec = gameObject.GetComponent<RectTransform>();
            PD_t = 1 - MyRec.anchoredPosition.y / Screen.height - 0.5f;
            PD_r = MyRec.anchoredPosition.x / Screen.width + 0.5f;
            PW = MyRec.sizeDelta.x / Screen.width * 2;
            PH = MyRec.sizeDelta.y / Screen.height * 2;
            D_t = 0;
            D_r = 0;
            H = 0;
            W = 0;
        }
        if (MyRec == null)
            MyRec = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = D_r + (PD_r-0.5f) * Screen.width;
        float y = -(D_t + (PD_t-0.5f) * Screen.height);
        MyRec.anchoredPosition = new Vector2(x, y);
        y = W + PH * Screen.height / 2;
        if (PW == -1) x = y;
        else x = H + PW * Screen.width/2;
        
        MyRec.sizeDelta = new Vector2(x, y);
    }
}
