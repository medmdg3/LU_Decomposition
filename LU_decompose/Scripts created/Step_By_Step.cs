using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step_By_Step 
{
    public class MATRIX_Explained
    {
        public MATRIX A;
        public string Comment;
        public MATRIX_Explained(MATRIX B)
        {
            A = new MATRIX(B);
            Comment = "";
        }
        public MATRIX_Explained(MATRIX B,string comment)
        {
            A = new MATRIX(B);
            Comment = comment;
        }
        public MATRIX_Explained(MATRIX_Explained T)
        {
            A = new MATRIX(T.A);
            Comment =new string( T.Comment);
        }
    }
    public static MATRIX_Explained[] Inverse_No_Swap(MATRIX T)
    {
        MATRIX_Explained[] C = new MATRIX_Explained[1];
        C[0] = new MATRIX_Explained(new MATRIX(-1));
        MATRIX A = new MATRIX(T);
        if (A.Rows < 0||A.Rows!=A.Columns)
        {
            return C;
        }
        List<MATRIX_Explained> B=new List<MATRIX_Explained>();
        
        B.Add(new MATRIX_Explained(A));
        int n = A.Rows;
        for(int i = 0; i < n; i++)
        {
            if (A.M[i][i].P == 0)
            {
                C = new MATRIX_Explained[B.Count+1];
                for (int k = 0; k < B.Count; k++) C[k] = new MATRIX_Explained(B[k]);
                C[B.Count] = new MATRIX_Explained(A);
                C[B.Count].Comment = "Decomposition impossible! le determinant de delat["+i.ToString()+"] est 0!";
                return C;
            }
            for(int j = i+1; j < n; j++)
            {
                if (A.M[i][j].Q == 0) return C;
                MATRIX D = new MATRIX(A);
                if (A.M[j][i].Q != 0 && A.M[j][i].P!=0)
                {
                    D.Update_Line(j, i, -A.M[j][i] / A.M[i][i]);
                    B.Add(new MATRIX_Explained(D,"A["+j.ToString()+"] <-- A[" + j.ToString() + "] + "+" A[" + i.ToString()+"] * " +Rational.Show_Rational(A.M[j][i] / A.M[i][i])));
                    A = new MATRIX(D);
                }
            }
        }
        B.Add(new MATRIX_Explained(new MATRIX(A), "La matrice est decomposable!"));
        C = new MATRIX_Explained[B.Count];
        for (int i = 0; i < B.Count; i++) C[i] = new MATRIX_Explained(B[i]);
        return C;
    }
    public static MATRIX_Explained[] LU_Decompose(MATRIX T)
    {
        List<MATRIX_Explained> Ans=new List<MATRIX_Explained>();
        MATRIX L=new MATRIX(T.Rows), U=new MATRIX(T.Rows);
        int n = T.Rows;
        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < n; j++)
            {
                if (j > i) L.M[i][j] = 0;
                if (j == i) L.M[i][j] = 1;
                if (j < i) L.M[i][j] = new Rational(0,0);
                if (j >= i) U.M[i][j] = new Rational(0, 0);
                else U.M[i][j] = 0;
            }
        }
        Ans.Add(new MATRIX_Explained(L, ""));
        Ans.Add(new MATRIX_Explained(U, ""));
        for (int i = 0; i < n; i++)
        {
            for(int j = i; j < n; j++)
            {
                string explanation = "U[" + i.ToString() + "][" + j.ToString() + "] = " + Rational.Show_Rational(T.M[i][j],true);
                U.M[i][j] = new Rational(T.M[i][j]);
                for(int k = 0; k < i; k++)
                {
                    explanation += " - "+Rational.Show_Rational(L.M[i][k],true)+" * "+ Rational.Show_Rational(U.M[k][j],true);
                    U.M[i][j] -= L.M[i][k] * U.M[k][j];
                }
                Ans.Add(new MATRIX_Explained(L, explanation));
                Ans.Add(new MATRIX_Explained(U, ""));
            }
            for (int j = i+1; j < n; j++)
            {
                string explanation = "L[" + j.ToString() + "][" + i.ToString() + "] = (" + Rational.Show_Rational(T.M[j][i],true);
                L.M[j][i] = new Rational(T.M[j][i]);
                for (int k = 0; k < i; k++)
                {
                    explanation += " - " + Rational.Show_Rational(L.M[j][k],true) + " * " + Rational.Show_Rational(U.M[k][i],true);
                    L.M[j][i] -= L.M[j][k] * U.M[k][i];
                }
                explanation += ") / " + Rational.Show_Rational(U.M[i][i],true);
                L.M[j][i] /= U.M[i][i];
                Ans.Add(new MATRIX_Explained(L, explanation));
                Ans.Add(new MATRIX_Explained(U, ""));
            }
        }
        MATRIX_Explained[] A = new MATRIX_Explained[Ans.Count];
        for (int i = 0; i < A.Length; i++) A[i] = new MATRIX_Explained(Ans[i]);
        return A;
    }
    public static MATRIX_Explained[] LY_Solve(MATRIX L,MATRIX B)
    {
        MATRIX_Explained[] C = new MATRIX_Explained[1];
        C[0] = new MATRIX_Explained(new MATRIX(-1));
        if (B.Columns !=1 ||L.Rows!=B.Rows|| L.Rows != L.Columns||L.Rows<0)
        {
            return C;
        }
        int n = L.Rows;
        MATRIX Y = new MATRIX(n, 1);
        for (int i = 0; i < n; i++) Y.M[i][0] = new Rational(0, 0);
        C = new MATRIX_Explained[n+1];
        C[0] = new MATRIX_Explained(Y);
        for(int i = 0; i < n; i++)
        {
            string comment = "Y[" + i.ToString() + "] = B[" + i.ToString() + "]";
            Y.M[i][0] = new Rational(B.M[i][0]);
            for(int j = 0; j < i; j++)
            {
                comment += " - " + Rational.Show_Rational(L.M[i][j], true) + " * " + Rational.Show_Rational(Y.M[j][0],true);
                Y.M[i][0] -= L.M[i][j] * Y.M[j][0];
            }
            C[i + 1] = new MATRIX_Explained(Y, comment);
        }
        return C;
    }
    public static MATRIX_Explained[] BC_Solve(MATRIX U, MATRIX Y)
    {
        MATRIX_Explained[] C = new MATRIX_Explained[1];
        C[0] = new MATRIX_Explained(new MATRIX(-1));
        if (Y.Columns != 1 || U.Rows != Y.Rows || U.Rows != U.Columns || U.Rows < 0)
        {
            return C;
        }
        int n = U.Rows;
        MATRIX X = new MATRIX(n, 1);
        for (int i = 0; i < n; i++) X.M[i][0] = new Rational(0, 0);
        C = new MATRIX_Explained[n + 1];
        C[0] = new MATRIX_Explained(X);
        for (int i = n-1; i >=0; i--)
        {
            string comment = "X[" + i.ToString() + "] = (Y[" + i.ToString() + "]";
            X.M[i][0] = new Rational(Y.M[i][0]);
            for (int j = i+1; j < n; j++)
            {
                comment += " - " + Rational.Show_Rational(U.M[i][j], true) + " * " + Rational.Show_Rational(Y.M[j][0], true);
                X.M[i][0] -= U.M[i][j] * X.M[j][0];
            }
            comment += ") / " + Rational.Show_Rational(U.M[i][i], true);
            X.M[i][0] /= U.M[i][i];
            C[n-i] = new MATRIX_Explained(X, comment);
        }
        return C;
    }
}
