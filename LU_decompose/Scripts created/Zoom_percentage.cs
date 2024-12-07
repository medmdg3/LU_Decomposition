using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zoom_percentage : MonoBehaviour
{
    [SerializeField] Text T;
    [SerializeField] float Coef1 = 0.03f,Coef2=0.02f;
    [SerializeField] Slider H;
    // Start is called before the first frame update
    void Start()
    {
        if (H != null) Auto_Adj();
    }
    public void Auto_Adj()
    {
        Adjust(H.value);
    }
    public void Adjust(float zoom)
    {
        string s = "",tem=T.text;
        for (int i = 0; i < tem.Length; i++)
            if (tem[i] != '=') s += tem[i];
            else break;
        T.text = s+"= "+((int)(100 * zoom)).ToString() + "%";
    }
    // Update is called once per frame
    void Update()
    {
        T.fontSize =(int)(Mathf.Min( Screen.width * Coef1,Screen.height*Coef2))+1;
    }
}
