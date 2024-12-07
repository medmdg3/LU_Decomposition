using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tools : MonoBehaviour
{
    [SerializeField]Vector2 Window_size = new Vector2(1,1), Window_center = new Vector2(0, 0);
    [SerializeField] GameObject[] Elements;
    [SerializeField] Vector2[] Position, Scale;
    
    void Set_Pos()
    {
        Vector2 T = new Vector2(Screen.width*Window_size.x, Screen.height*Window_size.y);
        for(int i = 0; i < Elements.Length; i++)
        {
            if (Elements[i] == null) continue;
            RectTransform A = Elements[i].GetComponent<RectTransform>();
            Vector2 size =Scale[i];
            if (size.x < 5)
            {
                size = new Vector2(size.x * T.x, size.y);
            }
            if (size.y < 5)
            {
                size = new Vector2(size.x ,T.y* size.y);
            }
            A.sizeDelta = size;
            Vector2 pos = Position[i];
            if (pos.x < 5)
            {
                pos = new Vector2(pos.x * T.x, pos.y);
            }
            if (pos.y < 5)
            {
                pos = new Vector2(pos.x, T.y*pos.y);
            }
            pos = new Vector2(pos.x, T.y - pos.y);
            A.position = new Vector2( Position[i].x,1-Position[i].y)*T+new Vector2( A.sizeDelta.x/2,-A.sizeDelta.y/2)+Window_center*T;
        }
    }
    void Update()
    {
        Set_Pos();
    }
}
