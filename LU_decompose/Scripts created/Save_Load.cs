using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Save_Load 
{
    public static void Save_Matrix(string File_Name, MATRIX A, bool full_Path = false)
    {
        string Temp = MATRIX.Savable(A);
        Debug.Log(Temp);
        Debug.Log(MATRIX.Show_Matrix(A));
        Save_Data(File_Name, Temp, full_Path);
    }
    public static MATRIX Load_Matrix(string File_Name, bool full_Path = false)
    {
        string Temp= Load_Data(File_Name, full_Path);
        if (Temp == "")
        {
            return new MATRIX(-1);
        }
        return MATRIX.Readit(Temp);
    }
    public static int Save_Data(string File_Name, string Data, bool full_Path = false)
    {
        if (!full_Path)
        {
            File_Name = Path.Combine(Application.dataPath, File_Name);
        }
        try
        {
            using (StreamWriter writer = new StreamWriter(File_Name))
            {
                writer.Write(Data);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured while openeing the file!" + e.ToString());
            return -1;
        }
        return 0;
    }
    public static string Load_Data(string File_Name, bool full_Path = false)
    {
        if (!full_Path)
        {
            File_Name = Path.Combine(Application.dataPath, File_Name);
        }

        if (!File.Exists(File_Name))
        {
            Debug.LogError("File does not exist: " + File_Name);
            return "Error";
        }
        try
        {
            using (StreamReader reader = new StreamReader(File_Name))
            {
                return reader.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured while openeing the file!" + e.ToString());
            return "Error";
        }
        return "";
    }
}
