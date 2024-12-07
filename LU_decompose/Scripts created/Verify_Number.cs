using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.UI;

public class Verify_Number : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] InputField A, B;
    void test()
    {
        B.text = Rational.Show_Rational(String_Equation.Decompose(A.text));
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

            test();
    }
}
