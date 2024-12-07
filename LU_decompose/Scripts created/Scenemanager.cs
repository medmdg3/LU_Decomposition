using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager :MonoBehaviour
{
    public void Load_Main()
    {
        SceneManager.LoadScene(0);
    }
    public void Load_Matrix_Edit()
    {
        SceneManager.LoadScene(1);
    }
    public void Load_Matrix_Calcule()
    {
        SceneManager.LoadScene(2);
    }
    public void Load_Editorial()
    {
        SceneManager.LoadScene(3);
    }
    public void Load_About()
    {
        SceneManager.LoadScene(4);
    }
    public void Load_Scene(int number)
    {
        SceneManager.LoadScene(number);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
