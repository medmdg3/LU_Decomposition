using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUImanager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float coef = 0.01f,coef2=0.008f;
    Slider Text_sz;
    [SerializeField] bool to_zoom=false;
    void Start()
    {
        float t = 1;
        if(to_zoom)t=GameObject.FindGameObjectWithTag("Text_sz").GetComponent<Slider>().value;
        coef *= t;
        coef2 *= t;
        set_sz();
    }
    public void set_sz()
    {
        GetComponent<Text>().fontSize = 1 + Mathf.Min((int)(coef * Screen.height),(int)(coef2*Screen.width));
    }
    // Update is called once per frame
    void Update()
    {
        set_sz();   
    }
}
