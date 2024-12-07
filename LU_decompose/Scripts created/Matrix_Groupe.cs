using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
public class Matrix_Groupe : MonoBehaviour
{
    [SerializeField] Vector2 Window_size=new Vector2(Screen.height,Screen.width), Window_center=new Vector2(0,0),Position=new Vector2(0,0);
    [SerializeField] GameObject Input_Obj,Last_selected;
    [SerializeField] float coefx=0.64f,coefy=0.5f;
    [SerializeField] Transform Canvas;
    [SerializeField] InputField Test;
    [SerializeField] MATRIX A;
    public string[][] Text_Source;
    [SerializeField] float Zoom=1;
    [SerializeField] Slider Zommer, FontSz;
    [SerializeField] Scrollbar Posx, Posy;
    public Toggle Is_Decimal;
    [SerializeField] GameObject[][] Cells=new GameObject[0][];
    [SerializeField] Zoom_percentage Percent,Text_Percent;
    [SerializeField] string File_Name = "Matrix.txt";
    public string Get_Text(int i,int j)
    {
        return Text_Source[i][j];
    }
    public MATRIX Get_Matrix()
    {
        return A;
    }
    public void Set_Text(string s,int i, int j)
    {
         Text_Source[i][j]=s;
    }
    public void Set_Cell(int i,int j)
    {
         if(Is_Decimal!=null && Is_Decimal.isOn)
            Cells[i][j].GetComponent<InputField>().text = Rational.AllDeci(String_Equation.Decompose(Text_Source[i][j]));
        else
            Cells[i][j].GetComponent<InputField>().text = Rational.Show_Rational(String_Equation.Decompose(Text_Source[i][j]));
        A.M[i][j] = String_Equation.Decompose(Text_Source[i][j]);
    }
    public void Set_Zoom()
    {
        if (Zommer == null) return;
        float h = Zommer.value;
        if (h == 1)
        {
            Zoom = 1;
            return;
        }
        if (h > 1)
        {
            h = 1 + (h - 1) * (A.Rows - 1);
            Zoom = h;
            return;
        }
        Zoom = (h+1)/2;
    }
    void adjust_Scrollbar(Scrollbar S)
    {
        if (Zommer == null) Zoom = 1;
        if (S == null) return;
        if (Zoom <= 1)
        {
            if (S.gameObject.active)
            {
                S.value = 0;
            }
            S.gameObject.SetActive(false);
            return;
        }
        S.gameObject.SetActive(true);
        if (Zoom <= (A.Rows - 1))
            S.size = 1.0f / Zoom;
        else
        {
            S.size = 1.0f/A.Rows;
        }
    }
    void Adjust_Position()
    {
        if (Zommer == null) Zoom = 1;
        if (Zoom <= 1)
        {
            Position = new Vector2(0, 0);
            return;
        }
        if (!Posx.gameObject.active)
        {
            Position.x = 0;
        }
        else
        {
            Position.x =Posx.value*(1-1/Zoom);
        }
        if (!Posx.gameObject.active)
        {
            Position.y = 0;
        }
        else
        {
            Position.y = Posy.value * (1-1/Zoom);
        }
    }
    public void Set_Vision()
    {
        Set_Zoom();
        if(Percent!=null)
        Percent.Adjust(Zoom);
        if(Text_Percent!=null)
        Text_Percent.Adjust(FontSz.value);
        adjust_Scrollbar(Posx);
        adjust_Scrollbar(Posy);
        Adjust_Position();
        Vector2 T = new Vector2(Screen.width, Screen.height);
        Vector2 Block_Size = new Vector2(Window_size.x / A.Columns, Window_size.y / A.Rows);
        Vector2 Dime = Block_Size * 0.9f;
        Vector2 OPos = (Zoom*(Block_Size/2-Position*Window_size));
        Vector2 Pos = OPos;

        for(int i = 0; i < Cells.Length; i++)
        {
            Pos = new Vector2(OPos.x, Pos.y);
            for (int j = 0; j < Cells[i].Length; j++)
            {

                RectTransform R = Cells[i][j].GetComponent<RectTransform>();
                Vector2 Temp = Pos + Zoom*Block_Size/2-new Vector2(0.01f,0.01f) ;
                R.position = (new Vector2(Pos.x, Window_size.y - Pos.y) + Window_center) * T;
                R.sizeDelta = T * (Dime * Zoom);
                Cells[i][j].GetComponent<InputFiled_Manager>().Set_Size(T.x*Dime.x*Zoom,T.y * Dime.y * Zoom);
                if (Temp.x > Window_size.x+Window_center.x || Temp.y > Window_size.y+Window_center.y)
                {
                    R.position = new Vector2(-200000, -100000);
                    Pos += new Vector2(Dime.x * Zoom, 0);
                    continue;
                }
                Temp = Pos - Zoom * Block_Size/2 ;
                if (Temp.x < 0 || Temp.y <0)
                {
                    R.position = new Vector2(-200000, -100000);
                    Pos += new Vector2(Block_Size.x * Zoom, 0);
                    continue;
                }
                Pos += new Vector2(Block_Size.x*Zoom, 0) ;
            }
            Pos += new Vector2(0,Block_Size.y*Zoom);
        }
    }
    public void Set_Matrix(MATRIX B)
    {
        A = new MATRIX(B);
        for (int i = 0; i < Cells.Length; i++)
        {
            for(int j = 0; j < Cells[i].Length; j++)
            {
                Destroy(Cells[i][j]);
            }
        }
        
        Cells = new GameObject[A.Rows][];
        Text_Source = new string[A.Rows][];
        for(int i = 0; i < A.Rows; i++)
        {
            Cells[i] = new GameObject[A.Columns];
            Text_Source[i] = new string[A.Columns];
            for(int j = 0; j < A.Columns; j++)
            {
                Cells[i][j] = Instantiate(Input_Obj,Canvas);
                Cells[i][j].name += i.ToString() + j.ToString();
                InputFiled_Manager Tem=Cells[i][j].GetComponent<InputFiled_Manager>();
                if (Tem != null)
                {
                    Tem.A = GetComponent<Matrix_Groupe>();
                    Tem.i = i;
                    Tem.j = j;
                    Tem.coefx = coefx;
                    Tem.coefy = coefy;
                    Tem.Font_Size = FontSz;
                }
                Text_Source[i][j] = Rational.Show_Rational(A.M[i][j]);
                Set_Cell( i, j);
                
            }
        }
        Set_Vision();
    }
    public void Set_Value()
    {
        for(int i=0;i<A.Rows;i++)
            for(int j=0;j<A.Columns;j++) Set_Cell(i, j);
    }
    void Start()
    {
        A = Save_Load.Load_Matrix(File_Name);
        if (A.Rows < 0)
        {
            A = MATRIX.Identity(3);
            if (File_Name == "B.txt")
            {
                MATRIX tem= Save_Load.Load_Matrix("Matrix.txt");
                if (tem.Rows >0)
                {
                    A = new MATRIX(tem.Rows, 0);
                }
                else
                {
                    Save_Load.Save_Matrix("Matrix.txt",MATRIX.Identity(3));
                    A = new MATRIX(3, 1);
                }
            }
        }
        Set_Matrix(A);
        Last_selected = null;
    }
    public void Save_Matr()
    {
        Save_Load.Save_Matrix(File_Name, A);
    }
    public void Load_Save()
    {
        Save_Matr();
        SceneManager.LoadScene(2);
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            if (EventSystem.current.currentSelectedGameObject != Last_selected) {
                if (Last_selected!=null && Last_selected.GetComponent<InputFiled_Manager>() != null)
                    Last_selected.GetComponent<InputFiled_Manager>().Updated();
                Last_selected = EventSystem.current.currentSelectedGameObject;
                if(Last_selected!=null&& Last_selected.GetComponent<InputFiled_Manager>()!=null)
                    Last_selected.GetComponent<InputFiled_Manager>().set_it();
            }
        }
        catch
        {
            
        }
        Set_Vision();
    }
}
