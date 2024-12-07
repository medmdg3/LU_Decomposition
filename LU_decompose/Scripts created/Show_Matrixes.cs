using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show_Matrixes : MonoBehaviour
{
    enum Ope_Type
    {
        Inverse_No_Swap,
        LU_Decomposition,
        LY_Solve,
        UX_Solve
    }
    // Start is called before the first frame update
    MATRIX A,B;
    int i=0;
    [SerializeField] Transform Canvas;
    [SerializeField] Text Explanation;
    [SerializeField] Step_By_Step.MATRIX_Explained[] C;
    [SerializeField] string name = "Matrix.txt",name2="";
    [SerializeField]  Ope_Type Type=Ope_Type.Inverse_No_Swap;
    [SerializeField] Show_Elements_In_Window SEW1, SEW2, SEW3;
    [SerializeField] GameObject Next, back;
    void Start()
    {
        A = Save_Load.Load_Matrix(name);
        B = Save_Load.Load_Matrix(name2);
        if (A.Rows < 0)
        {
            A = MATRIX.Identity(3);
        }
        if (B.Rows < 0)
        {
            B = new MATRIX(A.Rows, 1);
        }
        if (Type == Ope_Type.Inverse_No_Swap)
        {
            C = Step_By_Step.Inverse_No_Swap(A);
            if (C[C.Length - 1].Comment.Length >35)
            {
                Next.SetActive(false);
                back.SetActive(true);
            }
            else
            {
                C = Step_By_Step.LU_Decompose(A);
                Save_Load.Save_Matrix("L.txt",C[C.Length-2].A );
                Save_Load.Save_Matrix("U.txt", C[C.Length-1].A);
                C = Step_By_Step.Inverse_No_Swap(A);
                Next.SetActive(true);
                back.SetActive(true);
            }
        }
        else if (Type == Ope_Type.LU_Decomposition)
        {
            C = Step_By_Step.LU_Decompose(A);
            MATRIX Bt = Save_Load.Load_Matrix("B.txt");
            if(Bt.Columns!=1||Bt.Rows!=A.Rows)
            Save_Load.Save_Matrix("B.txt", new MATRIX(A.Rows, 1));
            SEW1.Upd_Matr(A);
            Next.SetActive(true);
            back.SetActive(true);
        }
        else if (Type == Ope_Type.LY_Solve)
        {
            C = Step_By_Step.LY_Solve(A, B);
            Save_Load.Save_Matrix("Y.txt", C[C.Length-1].A);
            SEW1.Upd_Matr(A);
            SEW3.Upd_Matr(B);
            Next.SetActive(true);
            back.SetActive(true);
        }
        else if (Type == Ope_Type.UX_Solve)
        {
            C = Step_By_Step.BC_Solve(A, B);
            SEW1.Upd_Matr(A);
            SEW3.Upd_Matr(B);
            Next.SetActive(true);
            back.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            i++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            i--;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            i = C.Length - 1 ;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            i=0;
        }
        if (i < 0) i = 0;
        if (i >= C.Length) i = C.Length - 1;
        if (Type == Ope_Type.LU_Decomposition && 2 * i + 1 >= C.Length) i = (C.Length - 1) / 2;
        if (i < 0||(C.Length < 2 && Type==Ope_Type.LU_Decomposition)) return;
        if (Type == Ope_Type.Inverse_No_Swap)
        {
            SEW1.Upd_Matr(C[i].A);
            Explanation.text = C[i].Comment;
        }
        if (Type == Ope_Type.LU_Decomposition)
        {
            SEW2.Upd_Matr(C[2 * i].A);
            SEW3.Upd_Matr(C[2 * i + 1].A);
            Explanation.text = C[2*i].Comment;
        }
        if (Type == Ope_Type.LY_Solve)
        {
            SEW2.Upd_Matr(C[i].A);
            Explanation.text = C[i].Comment;
        }
        if (Type == Ope_Type.UX_Solve)
        {
            SEW2.Upd_Matr(C[i].A);
            Explanation.text = C[i].Comment;
        }
    }
}
