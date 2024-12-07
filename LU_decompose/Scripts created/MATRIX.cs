using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
public class MATRIX
{
    int n = 0, m = 0 ;
    public List<List<Rational>> M=new List<List<Rational>>();
    //Get 
    public int Rows
    {
        get { return n; }
    }
    public int Columns
    {
        get { return m; }
    }
    //Initialisation
    public MATRIX(int r=0,int c=-1,List<List<Rational>> A=null,bool clone=false){
        if (c == -1) c = r;
        n = r;
        m = c;
        if (A == null)
        {
            for(int i = 0; i < r; i++)
            {
                M.Add( new List<Rational>());
                for(int j = 0; j < c; j++)
                {
                    M[i].Add(new Rational());
                }
            }
            return;
        }
        for(int i = 0; i < n; i++)
        {
            M.Add( new List<Rational>());
            for (int j = 0; j < c; j++)
            {
                if (!clone && A.Count > i)
                    if (j >= A[i].Count)
                        M[i].Add(new Rational());
                    else M[i].Add(new Rational( A[i][j % A[i].Count]));
                else
                {
                    if (!clone &&(i>=A.Count|| j >= A[i%A.Count].Count))
                        M[i].Add(new Rational());
                    else M[i].Add(new Rational( A[i%A.Count][j % A[i%A.Count].Count]));
                }
            }
        }
    }
    public MATRIX(MATRIX T)
    { 
        int r =T.Rows, c = T.Columns;
        List<List<Rational>> A = T.M;
        if (c == -1) c = r;
        n = r;
        m = c;
        if (A == null)
        {
            for (int i = 0; i < r; i++)
            {
                List<Rational> tem = new List<Rational>();
                for (int j = 0; j < c; j++)
                {
                    tem.Add(new Rational());
                }
                M.Add(tem);
            }
            return;
        }
        for (int i = 0; i < n; i++)
        {
            List<Rational> tem = new List<Rational>();
            for (int j = 0; j < c; j++)
            {

                tem.Add(A[i][j]);
            }
            M.Add(tem);
        }
    }
    public static MATRIX Identity(int n)
    {
        MATRIX C = new MATRIX(n);
        for (int i = 0; i < n; i++) C.M[i][i] = 1;
        return C;
    }
    //Operations
    public void Fill(Rational a)
    {
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                M[i][j] = a;
    }
    public void Scale(Rational val)
    {
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                M[i][j] *= val;
    }
    public static bool operator ==(MATRIX A, MATRIX B)
    {
        if (A.Rows != B.Rows || A.Columns != B.Columns)
        {
            return false;
        }
        for (int i = 0; i < A.Rows; i++)
            for (int j = 0; j < A.Columns; j++)
                if (A.M[i][j] != B.M[i][j])
                    return false;
        return true;
    }

    public static bool operator !=(MATRIX A, MATRIX B)
    {
        return !(A == B);
    }
    public static MATRIX operator +(MATRIX A,MATRIX B)
    {
        if (A.Rows != B.Rows || A.Columns != B.Columns)
        {
            Debug.LogError("Invalid sum! " + A.M+" : " + B.M);
            return new MATRIX(-1, -1);
        }
        MATRIX C = new MATRIX(A.Rows, A.Columns, A.M);
        for (int i = 0; i < A.Rows; i++)
            for (int j = 0; j < A.Columns; j++)
                C.M[i][j] += B.M[i][j];
        return C;
    }
    public static MATRIX operator -(MATRIX A, MATRIX B)
    {
        if (A.Rows != B.Rows || A.Columns != B.Columns)
        {
            Debug.LogError("Invalid substraction! " + A.M + " : " + B.M);
            return new MATRIX(-1, -1);
        }
        MATRIX C = new MATRIX(A.Rows, A.Columns, A.M);
        for (int i = 0; i < A.Rows; i++)
            for (int j = 0; j < A.Columns; j++)
                C.M[i][j] -= B.M[i][j];
        return C;
    }
    public static MATRIX operator *(MATRIX A,MATRIX B)
    {
        if (A.Columns != B.Rows)
        {
            Debug.LogError("Invalid product! " + A.M + " : " + B.M);
            return new MATRIX(-1, -1);
        }
        MATRIX C = new MATRIX(A.Rows, B.Columns);
        for (int i = 0; i < A.Rows; i++)
            for (int j = 0; j < B.Columns; j++)
                for (int k = 0; k < A.Columns; k++)
                    C.M[i][j] += A.M[i][k] * B.M[k][j];
        return C;
    }
    public void Multiply_Line(int line,Rational coef)
    {
        if (line < 0 || line >= n)
        {
            Debug.LogError("Line does not exist! Line:" + line.ToString() + ". Size is : " + n.ToString());
            return;
        }
        for (int i = 0; i < m; i++) M[line][i] *= coef;
    }
    public void Multiply_Column(int column, Rational coef)
    {
        if (column < 0 || column >= m)
        {
            Debug.LogError("Column does not exist! Column:" + column.ToString() + ". Size is : " + m.ToString());
            return;
        }
        for (int i = 0; i < m; i++) M[i][column] *= coef;
    }
    public void Update_Line(int target,int ind, Rational coef)
    {
        if (target < 0 || target >= n)
        {
            Debug.LogError("Line does not exist! Line:" + target.ToString() + ". Size is : " + n.ToString());
            return;
        }
        if (ind < 0 || ind >= n)
        {
            Debug.LogError("Line does not exist! Line:" + ind.ToString() + ". Size is : " + n.ToString());
            return;
        }
        for (int i = 0; i < m; i++) M[target][i] += coef *M[ind][i];
    }
    public void Swap_Lines(int i,int j){
        if (i == j) return;
        if (i < 0 || i >= n || j < 0 || j >= n)
        {
            Debug.LogError("Cannot swap line " + i.ToString() + " with line " + j.ToString() + ". Index out of range!");
            return;
        }
        for(int k = 0; k < n; k++)
        {
            Rational t = M[i][k];
            M[i][k] = M[j][k];
            M[j][k] = t;
        }
        return;
    }
    public static MATRIX operator ~(MATRIX T)
    {
        MATRIX A = new MATRIX(T.n,T.m,T.M);
        if (A.n != A.m)
        {
            Debug.LogError("Matrix not square!" + A.M);
            return new MATRIX(-1);
        }
        MATRIX C = Identity(A.n);
        for(int i = 0; i < A.n; i++)
        {
            int ind = -1;
            for(int j = i; j < A.n; j++)
            {
                if(A.M[i][j].P!=BigInteger.Zero)
                {
                    ind = j;
                    break;
                }
            }
            if (ind == -1)
            {
                Debug.LogError("Matrix cannot be inversed, error occured at line :" + i.ToString());
                List<List<Rational>> temp = new List<List<Rational>>();
                temp.Add(new List<Rational>());
                temp[0].Add(new Rational(i, 0));
                return new MATRIX(1, 1, temp);
            }
            A.Swap_Lines(i, ind);
            C.Swap_Lines(i, ind);
            Rational t = 1 / A.M[i][i];
            A.Multiply_Line(i, t);
            C.Multiply_Line(i, t);
            for(int j = 0; j < A.n; j++)
            {
                if (i == j) continue;
                C.Update_Line(j, i, 0 - A.M[j][i]);
                A.Update_Line(j, i,0- A.M[j][i]);
                
            }
        }
        return C;
    }
    public static MATRIX operator ^(MATRIX T,int n)
    {
        if (T.n < n||T.m<n)
        {
            Debug.LogError("Oversized!");
            return new MATRIX(-1);
        }
        MATRIX A = new MATRIX(n, n, T.M);
        MATRIX C = Identity(A.n);
        for (int i = 0; i < A.n; i++)
        {
            
            if (A.M[i][i].P == 0)
            {
                Debug.LogError("Matrix cannot be inversed without swapping lines, error occured at line :"+i.ToString());
                List<List<Rational>> temp=new List<List<Rational>>();
                temp.Add(new List<Rational>());
                temp[0].Add(new Rational(i, 0));
                return new MATRIX(1,1,temp);
            }
            Rational t = 1 / A.M[i][i];
            A.Multiply_Line(i, t);
            C.Multiply_Line(i, t);
            for (int j = 0; j < A.n; j++)
            {
                if (i == j) continue;
                C.Update_Line(j, i, 0 - A.M[j][i]);
                A.Update_Line(j, i, 0 - A.M[j][i]);

            }
        }
        return C;
    }
    //Strings
    public static string Show_Matrix(MATRIX A)
    {
        string ans = "Matrix  (" + A.n.ToString() + " x " + A.m.ToString() + ") \n[";
        for (int i = 0; i < A.n; i++)
        {
            ans += "[";
            for (int j = 0; j < A.m - 1; j++)
            {
                ans += Rational.Show_Rational(A.M[i][j]) + ",";
            }
            ans += Rational.Show_Rational(A.M[i][A.m - 1]) + "]\n";
        }
        ans += "]";
        return ans;
    }
    public static string Savable(MATRIX A)
    {
        List<char> S=new List<char>();
        void Add_to_S(List<char> A,string B)
        {
            for (int i = 0; i < B.Length; i++) A.Add(B[i]);
        }
        void Add_R_to_S(List<char> A, Rational B)
        {
            Add_to_S(A, B.P.ToString());
            A.Add(',');
            Add_to_S(A, B.Q.ToString());
        }
        void Add_L_to_S(List<char> A, List<Rational> B)
        {
            for (int i = 0; i < B.Count; i++)
            {
                Add_R_to_S(A, B[i]);
                A.Add(',');
            }
        }
        void Add_LL_to_S(List<char> A, List<List<Rational>> B)
        {
            for (int i = 0; i < B.Count; i++)
            {
                Add_L_to_S(A, B[i]);;
            }
        }
        Add_to_S(S,A.Rows.ToString());
        S.Add(',');
        Add_to_S(S, A.Columns.ToString());
        S.Add(',');
        Add_LL_to_S(S, A.M);
        char[] T = new char[S.Count];
        for (int i = 0; i < T.Length; i++) T[i] = S[i];
        return new string(T);
    }
    public static MATRIX Readit(string A)
    {
        List<BigInteger> T= new List<BigInteger>();
        List<char> g=new List<char>();
        for (int i = 0; i < A.Length; i++)
        {
            if (A[i] == ',')
            {
                char[] temp = new char[g.Count];
                for (int j = 0; j < temp.Length; j++) temp[j] = g[j];
                g.Clear();
                T.Add(new Rational( Rational.Integer_From_String(new string(temp))).P);
            }else
            g.Add(A[i]);
        }
        if (T.Count < 2)
        {
            return new MATRIX(-1);
        }
        MATRIX M = new MATRIX(new Rational( T[0]),new Rational( T[1]));
        int t = 2;
        for(int i = 0; i < M.Rows; i++)
        {
            for(int j = 0; j < M.Columns; j++)
            {
                if (t + 2 > T.Count) break;
                M.M[i][j] =new Rational(  T[t] , T[t + 1]);
                t += 2;
            }
        }
        return M;
    }
}
