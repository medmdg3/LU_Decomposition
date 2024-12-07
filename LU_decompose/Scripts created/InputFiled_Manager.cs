using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputFiled_Manager : MonoBehaviour
{
    public Matrix_Groupe A;
    public int i, j;
    public float coefy=0.64f,coefx=0.5f;
    [SerializeField] InputField In;
    public Slider Font_Size;
    private Text Son_txt;

    // Start is called before the first frame update
    void Start()
    {
        In = GetComponent<InputField>();
        Son_txt = transform.Find("Text (Legacy)").GetComponent<Text>();
    }
    public void set_it()
    {
        In.text = A.Get_Text(i, j);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Set_Size(float x,float y)
    {
        if (Font_Size != null)
        {
            try
            {
                Son_txt.fontSize = (int)(Mathf.Min(coefy * y, coefx * x) * Font_Size.value) + 1;
            }
            catch
            {
                
            }
        }
        else Son_txt.fontSize= (int)(Mathf.Min(coefy * y, coefx * x)) +1;
    }
    public void Updated()
    {
        A.Set_Text(In.text,i, j);
        A.Set_Cell(i, j);
    }
}
