using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

	public void LoadGameLevel()
    {
        GameManager.Instance.LoadScene("01_Game");
    }

    public void LoadLevel(string s)
    {
        GameManager.Instance.LoadScene(s);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
