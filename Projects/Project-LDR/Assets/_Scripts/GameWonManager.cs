using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWonManager : MonoBehaviour {

	public void ReturnToStart()
    {
        GameManager.Instance.LoadScene("00a_MainMenu");
    }
}
