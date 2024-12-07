using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Show_Elements_In_Window : MonoBehaviour
{
    [SerializeField] MATRIX A;
    [SerializeField] string Name,File_Location;
    [SerializeField] Text Head;
    [SerializeField] GameObject To_Spawn;
    [SerializeField] Transform Canvas;
    [SerializeField] Vector2 Window_Size,Window_Position;
    [SerializeField] GameObject[] Fields;
    public bool Deci = false;
    public void Switch_Deci()
    {
        Deci = !Deci;
        Set_Up();
    }
    void Set_Cell(int ind,int i,int j,Vector2 Pos,Vector2 Cell)
    {
        Fields[ind].name = Name + "[" + i.ToString() + "][" + j.ToString() + "]";
        if(Pos.x>=0)
            Fields[ind].GetComponent<RectTransform>().position = Pos*new Vector2(Screen.width,Screen.height);
        if(Cell.x>=0)
            Fields[ind].GetComponent<RectTransform>().sizeDelta = Cell* new Vector2(Screen.width, Screen.height);
        if (A.M[i][j].Q == 0)
        {
            if(A.Rows!=1&&A.Columns!=1)
            Fields[ind].GetComponent<InputField>().text= Name + "[" + i.ToString() + "][" + j.ToString() + "]";
             else if ( A.Columns != 1)
                Fields[ind].GetComponent<InputField>().text = Name + "[" + j.ToString() + "]";
            else Fields[ind].GetComponent<InputField>().text = Name + "[" + i.ToString() + "]";
        }
        else
        {
            if (!Deci)
            {
                Fields[ind].GetComponent<InputField>().text = Rational.Show_Rational(A.M[i][j]);
            }
            else
            {
                Fields[ind].GetComponent<InputField>().text = Rational.AllDeci(A.M[i][j]);
            }
        }
    }
    void Set_Up()
    {
        for (int i = 0; i < Fields.Length; i++) Destroy(Fields[i]);
        Vector2 Dime = new Vector2(Window_Size.x/A.Columns, Window_Size.y/A.Rows);
        Vector2 Cell = Dime * 0.9f;
        Vector2 Opos = Window_Position + Dime / 2;
        Vector2 Pos = Opos;
        Fields = new GameObject[A.Rows * A.Columns];
        for(int i = 0; i < A.Rows; i++)
        {
            Pos.x = Opos.x;
            for(int j = 0; j < A.Columns; j++)
            {
                Fields[i * A.Columns + j] = Instantiate(To_Spawn,Canvas);
                Set_Cell(i * A.Columns + j, i, j,new Vector2( Pos.x,1-Pos.y),Cell);
                Pos.x += Dime.x;
            }
            Pos.y += Dime.y;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        A = Save_Load.Load_Matrix(File_Location);
        if ( A.Rows < 0)
        {
            A = new MATRIX(MATRIX.Identity(3));
        }
        Head.text = Name;
        Set_Up();
    }
    public void Upd_Matr(MATRIX T,string N_Name="")
    {
        if (N_Name != "")  Name=N_Name;
        A = new MATRIX(T);
        Set_Up();
    }
    // Update is called once per frame
    void Update()
    {
        Set_Up();
    }
}
