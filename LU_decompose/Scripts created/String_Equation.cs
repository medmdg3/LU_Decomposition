using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
public class String_Equation 
{
    private static char[] Valable = {'0','1','2','3','4','5','6','7','8','9','(',')','-','+','*','x','/','^','`','.',','};
    private static char[] Number = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', ',' };
    private static char[] Operation = {'-', '+', '*', 'x', '/', '^'};
    private class element
    {
        public int is_Operation { get; set; }
        public Rational Value { get; set; }
        public element(element E)
        {
            is_Operation = E.is_Operation;
            Value = new Rational(E.Value);
        }
        public element()
        {
            is_Operation = -1;
            Value = new Rational(0);
        }
    }
    public static Rational Decompose(string s,int l=0,int r=-1)
    {
        List<char> Temp= new List<char>();
        if (r == -1) r += s.Length;
        List<element> Ans=new List<element>();
        for(int i = l; i <= r; i++)
        {
            element T = new element();
            T.Value = new Rational();
            for (int j = 0; j < Number.Length; j++)
            {
                if (s[i] == Number[j])
                {
                    Temp.Add(s[i]);
                    T.Value = new Rational(0, 0);
                    break;
                }
            }
            if (Temp.Count==0&&((s[i] == '-') || (s[i]=='+'))&&(Ans.Count==0||Ans[Ans.Count-1].is_Operation!=-1))
            {
                Temp.Add(s[i]);
                T.Value = new Rational(0, 0);
            }
            if (T.Value==new Rational(0, 0)&&i!=r)
            {
                continue;
            }
                if (Temp.Count != 0)
                {
                    T.is_Operation = -1;
                    char[] T1 = new char[Temp.Count];
                    for(int k = 0; k < Temp.Count; k++)
                    {
                        T1[k] = Temp[k];
                    }
                    Rational H = T.Value;
                    T.Value = Rational.Decimal_From_String(new string(T1));
                Temp = new List<char>();
                    if(T.Value.Q==0)
                    {
                        Debug.LogError("Number not valid! " + new string(T1));
                        return new Rational(0, 0);
                    }
                Ans.Add(new element(T));
                if (H.Q == 0) break;
                }
            if (s[i] == ' ') continue;
            if (s[i] == '(')
            {
                int c = 1;int j = i;
                while (c != 0)
                {
                    j++;
                    if (j == r + 1)
                    {
                        Debug.LogError("String\"" + s + "\" not valid! Unclosed '('.");
                        return new Rational(0, 0);
                    }
                    if (s[j] == ')') c--;
                    if (s[j] == '(') c++;
                }
                if (Ans.Count != 0 && Ans[Ans.Count - 1].is_Operation == -1)
                {
                    T.is_Operation = 2;
                    T.Value = new Rational(0, 0);
                    Ans.Add(new element(T));
                }
                T.Value=(Decompose(s, i + 1, j - 1));
                T.is_Operation = -1;
                Ans.Add(new element(T));
                i = j;
                continue;
            }
            if (s[i] == ')')
            {
                Debug.LogError("String\"" + s + "\" not valid! Unopened ')'");
                return new Rational(0, 0);
            }
            for(int j = 0; j < Operation.Length; j++)
            {
                if (s[i] == Operation[j])
                {
                    T.is_Operation = j;
                    T.Value = new Rational(0, 0);
                    Ans.Add(new element(T));
                    break;
                }
            }
            if (s[i] == 'E')
            {
                T.is_Operation = 2;
                T.Value = new Rational(0, 0);
                Ans.Add(new element(T));
                T.is_Operation = -1;
                T.Value = new Rational(10);
                Ans.Add(new element(T));
                T.is_Operation = 5;
                T.Value = new Rational(0, 0);
                Ans.Add(new element(T));
            }
        }
        if (Ans.Count == 0)
        {
            return new Rational(0);
        }
        List<element> Tem = new List<element>();
        for (int j = 5; j >= 0; j--)
        {
            for (int i = 0; i < Ans.Count; i++)
            {
                if (Ans[i].is_Operation == j&&j==5)
                {
                    if (i == Ans.Count - 1 || Ans[i + 1].is_Operation != -1 ||Tem.Count==0 || Tem[Tem.Count-1].is_Operation != -1)
                    {
                        return new Rational(0, 0);
                    }

                    Rational H = Rational.Pow(Tem[Tem.Count-1].Value, Ans[i + 1].Value);
                    if (H.Q == 0)
                    {
                        Debug.LogError("Invalid power operation!");
                        return new Rational(0, 0);
                    }
                    Tem[Tem.Count - 1].Value = new Rational(H.P, H.Q);
                    Tem[Tem.Count - 1].is_Operation = -1;
                    i++;
                    continue;
                }
                if (Ans[i].is_Operation == 4 && j == 4)
                {
                    if (i == Ans.Count - 1 || Ans[i + 1].is_Operation != -1 || Tem.Count == 0 || Tem[Tem.Count - 1].is_Operation != -1)
                    {
                        return new Rational(0, 0);
                    }

                    Rational H = Tem[Tem.Count - 1].Value / Ans[i + 1].Value;
                    if (H.Q == 0)
                    {
                        Debug.LogError("Invalid division operation!");
                        return new Rational(0, 0);
                    }
                    Tem[Tem.Count - 1].Value = new Rational(H.P, H.Q);
                    Tem[Tem.Count - 1].is_Operation = -1;
                    i++;
                    continue;
                }
                if (Ans[i].is_Operation == j && (j == 2 || j==3))
                {
                    if (i == Ans.Count - 1 || Ans[i + 1].is_Operation != -1 || Tem.Count == 0 || Tem[Tem.Count - 1].is_Operation != -1)
                    {
                        return new Rational(0, 0);
                    }

                    Rational H = Tem[Tem.Count - 1].Value * Ans[i + 1].Value;
                    if (H.Q == 0)
                    {
                        Debug.LogError("Invalid product operation!");
                        return new Rational(0, 0);
                    }
                    Tem[Tem.Count - 1].Value = new Rational(H.P, H.Q);
                    Tem[Tem.Count - 1].is_Operation = -1;
                    i++;
                    continue;
                }
                if (Ans[i].is_Operation == j && j == 1 )
                {
                    if (i == Ans.Count - 1 || Ans[i + 1].is_Operation != -1)
                    {
                        return new Rational(0, 0);
                    }
                    if (Tem.Count == 0 || Tem[Tem.Count - 1].is_Operation != -1)
                    {
                        continue;
                    }
                    Rational H = Tem[Tem.Count - 1].Value + Ans[i + 1].Value;
                    if (H.Q == 0)
                    {
                        Debug.LogError("Invalid sum operation!");
                        return new Rational(0, 0);
                    }
                    Tem[Tem.Count - 1].Value = new Rational(H.P, H.Q);
                    Tem[Tem.Count - 1].is_Operation = -1;
                    i++;
                    continue;
                }
                if (Ans[i].is_Operation == j && j == 0)
                {
                    if (i == Ans.Count - 1 || Ans[i + 1].is_Operation != -1)
                    {
                        return new Rational(0, 0);
                    }
                    if (Tem.Count == 0 || Tem[Tem.Count - 1].is_Operation != -1)
                    {
                        Ans[i + 1].Value *= -1;
                        continue;
                    }
                    Rational H = Tem[Tem.Count - 1].Value  - Ans[i + 1].Value;
                    if (H.Q == 0)
                    {
                        Debug.LogError("Invalid substraction operation!");
                        return new Rational(0, 0);
                    }
                    Tem[Tem.Count - 1].Value = new Rational(H.P, H.Q);
                    Tem[Tem.Count - 1].is_Operation = -1;
                    i++;
                    continue;
                }
                Tem.Add(new element(Ans[i]));
            }
            Ans.Clear();
            for(int i = 0; i < Tem.Count; i++)
            {
                Ans.Add(new element(Tem[i]));
            }
            Tem.Clear();
        }
        return Ans[0].Value;
    }

}
