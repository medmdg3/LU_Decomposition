using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Set_Matrix_Size : MonoBehaviour
{
    [SerializeField] GameObject Info_Area,Pre_Bt,Submit_Bt,Cancel_Bt;
    [SerializeField] Matrix_Groupe MG;
    // Start is called before the first frame update
    public void Cancel()
    {

        Pre_Bt.SetActive(true);
        Submit_Bt.SetActive(false);
        Cancel_Bt.SetActive(false);
        Info_Area.SetActive(false);
    }
    void Start()
    {
        
    }
    public void Activate()
    {
        Pre_Bt.SetActive(false);
        Submit_Bt.SetActive(true);
        Cancel_Bt.SetActive(true);
        Info_Area.SetActive(true);
        Info_Area.GetComponent<InputField>().text = MG.Get_Matrix().Rows.ToString();
    }
    public void Submit()
    {
        Rational A = Rational.Integer_From_String(Info_Area.GetComponent<InputField>().text);
        if (A.Q==0 || A < 1 || A > 100)
        {
            Cancel();
            return;
        }
        MG.Set_Matrix(new MATRIX(A,A,MG.Get_Matrix().M));
        Pre_Bt.SetActive(true);
        Submit_Bt.SetActive(false);
        Cancel_Bt.SetActive(false);
        Info_Area.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
