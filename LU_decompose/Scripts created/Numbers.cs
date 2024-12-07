using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;

public class Rational 
{
    public BigInteger P { get; private set; }
    public BigInteger Q { get; private set; }
    public Rational()
    {
        BigInteger A = BigInteger.Zero;
        BigInteger B = BigInteger.One;
        P = A;
        Q = B;
    }
    public Rational(BigInteger A)
    {
        BigInteger B = BigInteger.One;
        P = A;
        Q = B;
    }
    public Rational(BigInteger A , BigInteger B )
    {
        if (B == 0)
        {
            P = 0;
            Q = 0;
            return;
        }

        if (B < 0)
        {
            A *= -1;
            B *= -1;
        }
        P = A;
        Q = B;
        BigInteger T = BigInteger.GreatestCommonDivisor(A, B);
        P /= T;
        Q /= T;
    }
    public Rational(Rational A)
    {
        Rational C = new Rational(A.P, A.Q);
        P = C.P;
        Q = C.Q;
        
    }
    public static implicit operator Rational(int a)
    {
        return new Rational(a);
    }
    public static implicit operator int(Rational a)
    {
        int ans = 0,t=1;
        Rational q = new Rational(a.P, a.Q);
        if (q.P < 0) q.P *= -1;
        q.P -= BigInteger.Remainder(q.P, q.Q);
        q = Simplify(q);
        while (q.P!=0)
        {
            if (BigInteger.Remainder(q.P, 2)==1)
            {
                ans+=t;
            }
            q.P /= 2;
            if (t > int.MaxValue / 2)
            {
                Debug.LogError("Overflow!!");
                return 0;
            }
            t *= 2;
        }
        if (a.P < 0) ans *= -1;
        return ans;
    }
    public static implicit operator Rational(BigInteger a)
    {
        return new Rational(a);
    }
    public static Rational Simplify(Rational Val)
    {
        Rational A = Val;
        if (A.Q == 0)
        {
            A.P = 0;
            return A;
        }
        if (A.Q < 0)
        {
            A.Q *= -1;
            A.P *= -1;
        }
        BigInteger Temp = BigInteger.GreatestCommonDivisor(A.P, A.Q);
        A.P = A.P / Temp;
        A.Q = A.Q / Temp;
        return A;
    }
    public static Rational operator +(Rational A,Rational B)
    {
        Rational T=new Rational();
        T.P = A.P * B.Q + B.P * A.Q;
        T.Q = A.Q * B.Q;
        return Simplify(T);

    }
    public static Rational operator -(Rational A, Rational B)
    {
        Rational T = new Rational();
        T.P = A.P * B.Q - B.P * A.Q;
        T.Q = A.Q * B.Q;
        return Simplify(T);

    }
    public static Rational operator -(Rational A)
    {
        Rational T = new Rational(A);
        T.P *= -1;
        return Simplify(T);

    }
    public static Rational operator *(Rational A, Rational B)
    {
        Rational T = new Rational();
        T.P = A.P * B.P;
        T.Q = A.Q * B.Q;
        return Simplify(T);

    }

    public static Rational operator /(Rational A, Rational B)
    {
        Rational T = new Rational();
        if (B.P == 0||B.Q==0||A.Q==0)
        {
            Debug.LogError("Denied division: Dividing by zero!");
            return new Rational(0, 0);
        }
        T.P = A.P * B.Q;
        T.Q = A.Q * B.P;
        return Simplify(T);

    }
    public static bool operator ==(Rational A, Rational B)
    {
        if (A.Q == 0 && B.Q == 0) return true;
        if (A.Q == 0 || B.Q == 0) return false;
        if (A.Q * B.P == A.P * B.Q) return true;
        return false;
    }
    public static bool operator !=(Rational A, Rational B)
    {
        return !(A == B);
    }
    public static bool operator <(Rational A,Rational B)
    {
        if (B.Q == 0 || A.Q == 0)
        {
            Debug.LogError("Comparing an indefined Rational!");
            return false;
        }
        return A.P * B.Q < A.Q * B.P;
    }
    public static bool operator >(Rational A, Rational B)
    {
        if (B.Q == 0 || A.Q == 0)
        {
            Debug.LogError("Comparing an indefined Rational!");
            return false;
        }
        return A.P * B.Q > A.Q * B.P;
    }
    public static bool operator <=(Rational A, Rational B)
    {
        if (B.Q == 0 || A.Q == 0)
        {
            Debug.LogError("Comparing an indefined Rational!");
            return B.Q==A.Q;
        }
        return A.P * B.Q <= A.Q * B.P;
    }
    public static bool operator >=(Rational A, Rational B)
    {
        if (B.Q == 0 || A.Q == 0)
        {
            Debug.LogError("Comparing an indefined Rational!");
            return B.Q == A.Q;
        }
        return A.P * B.Q >= A.Q * B.P;
    }
    public static Rational Pow(Rational A, int B)
    {
        Rational T = new Rational();
        Debug.LogWarning(Show_Rational(A) + " ^ " + B.ToString() + "= "); 
        T.P = A.P;
        T.Q = A.Q;
        if (B < 0)
        {
            return Pow(1 / A, -B);
        }
        if (B == 0 && A.Q!=0)
        {
            return new Rational(1, 1);
        }
        T.P = BigInteger.Pow(A.P,B);
        T.Q =BigInteger.Pow(A.Q,B);
        Debug.LogWarning(Show_Rational(Simplify(T)));
        return Simplify(T);

    }
    public static string Show_Rational(Rational A,bool paren=false)
    {
        if (A.Q == 0)
        {
            return "Nan";
        }
        if (A.Q == 1)
            if (A >= 0||!paren)
                return A.P.ToString();
            else return "(" + A.P.ToString() + ")";
        if(!paren)
        return A.P.ToString() + "/" + A.Q.ToString();
        else return "("+A.P.ToString() + "/" + A.Q.ToString()+")";
    }
    public static Rational To_Integer(Rational Val)
    {
        Rational A = Val;
        A = Simplify(A);
        A.P -= A.P % A.Q;
        A=Simplify(A);
        if (A.Q != 1)
        {
            Debug.LogError("Something is WRONG!!");
            return new Rational(0, 0);
        }
        return A;
    }
    public static Rational Integer_From_String(string s,bool expected_Error=false)
    {
        if (s == "")
            s = "0";
        for (int i = 0; i < s.Length; i++)
        {
            if (i == 0 && s[i] == '-') continue;
            if (i == 0 && s[i] == '+') continue;
            int t = (int)s[i];
            if (t < 48 || t > (int)'9')
            {
                if (!expected_Error)
                    Debug.LogError("Invalid Integer :\"" + s + "\", error found at index :" + i.ToString());
                    return new Rational(0, 0);
            }
        }
        Rational T=new Rational();
        try
            {
            T.P = BigInteger.Parse(s);
        }catch (Exception ex)
        {
            if (!expected_Error)
                Debug.LogError(s + " couldn't be parsed!"+ex.ToString());
            return new Rational(0, 0);
        }
        T.Q = 1;
        return T;
    }
    public static Rational Decimal_From_String(string s,bool expected_Error=false)
    {
        if (s == "")
        {
            s = "0";
        }
        bool deci=false;
        int deci_place = -1;
        for(int i=0; i < s.Length; i++)
        {
            if (i == 0 && s[i] == '-') continue;
            if (i == 0 && s[i] == '+') continue;
            int t = (int)s[i];
            if (t < 48 || t > (int)'9')
            {
                if(!deci && (s[i] == '.' || s[i] == ','))
                {
                    deci = true;
                    deci_place = i;
                }
                else
                {
                    if(!expected_Error)
                    Debug.LogError("Invalid decimal :" + s + ", error found at index :" + i.ToString());
                    return new Rational(0, 0);
                }
            }
        }
        Rational T = new Rational();
        if (deci_place == -1)
        {
            T.Q = 1;
            try
            {
                T.P = BigInteger.Parse(s);
                return T;
            }catch(Exception ex)
            {
                if (!expected_Error)
                    Debug.LogError(s + " couldn't be parsed!" + ex.ToString()) ;
                return new Rational(0, 0);
            }
        }
        char[] WholeNumber = new char[s.Length - 1];
        int temp = 0;
        for (int i=0; i < s.Length; i++)
        {
            if (s[i] == '.' || s[i] == ',') continue;
            try
            {
                WholeNumber[temp] = s[i];
            }catch(Exception e)
            {
                Debug.LogError(s + " Coudln't be found!" + deci_place.ToString());
            }
            temp++;
        }
        T.P = BigInteger.Parse(new string(WholeNumber));
        T.Q = BigInteger.Pow(10,s.Length-deci_place-1);
        return Simplify(T);
    }
    
    public static string ToDecimal(Rational Ra,int nb_digits=5)
    {
        Rational A = new Rational(Ra);
        bool is_neg = (A.P < 0);
        if (A.P < 0) A.P *= -1;
        if (A.Q == 0) return "0";
        int Count_Shift(BigInteger A,BigInteger B)
        {
            if (A == 0) return 0;

            int t = 0;
            if (A >= B)
            {
                if (10 * B > A) return 0;
                if (100 * B > A) return 1;
                t =(int) (BigInteger.Log10(A) - BigInteger.Log10(B));
                return t + Count_Shift(A, B * BigInteger.Pow(10, t));
            }
            if (10 * A > B) return -1;
            if (100 * A > B) return -2;
            t = (int)(BigInteger.Log10(B) - BigInteger.Log10(A));
            return -t + Count_Shift(A* BigInteger.Pow(10, t), B );
        }
        int t = Count_Shift(A.P, A.Q);
        string Add="";
        if(t!=0)Add = "E" + t.ToString();
        BigInteger H = A.Q;
        BigInteger T = A.P;
        if (t > 0) H *= BigInteger.Pow(10, t);
        if (t < 0) T *= BigInteger.Pow(10, -t);
        string Ans ="";
        if (is_neg) Ans = "-";
        for(int i = 0; i < nb_digits; i++)
        {
            int t1 =(int)(T/H);
            Ans += t1.ToString();
            T -= t1 * H;
            T *= 10;
            if (T == 0) break;
            if (i == 0) Ans += '.';
        }
        return Ans+Add;
    }
    public static string AllDeci(Rational Ra, int nb_digits = 50)
    {
        Rational A = new Rational(Ra);
        bool is_neg = (A.P< 0);
        if (A.P < 0) A.P *= -1;
        if (A.Q == 0) return "0";
        BigInteger H = A.Q;
        BigInteger T = A.P;
        string Ans = "";
        if (is_neg) Ans = "-";
        for (int i = 0; i < nb_digits; i++)
        {
            BigInteger t1 = (T / H);
            Ans += t1.ToString();
            T -= t1 * H;
            T *= 10;
            if (T == 0) break;
            if (i == 0) Ans += '.';
        }
        return Ans ;
    }
}
