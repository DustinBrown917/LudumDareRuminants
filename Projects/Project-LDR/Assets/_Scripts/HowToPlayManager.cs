using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayManager : MonoBehaviour {

    [SerializeField] private HTPPhase[] phases;
    private int currentPhase = 0;
	// Use this for initialization
	void Start () {
        phases[0].PhaseDone += HowToPlayManager_PhaseDone;
	}

    private void HowToPlayManager_PhaseDone(object sender, System.EventArgs e)
    {
        phases[currentPhase].PhaseDone -= HowToPlayManager_PhaseDone;
        currentPhase++;
        if(currentPhase >= phases.Length) { return; }
        phases[currentPhase].gameObject.SetActive(true);
        phases[currentPhase].PhaseDone += HowToPlayManager_PhaseDone;
    }

    // Update is called once per frame
    void Update () {
        
        if (Input.GetKeyDown(KeyCode.Space)||Input.GetMouseButtonDown(0))
        {
            if (currentPhase >= phases.Length) { GameManager.Instance.LoadScene("00a_MainMenu"); }
            else { phases[currentPhase].Next(); }
            
        }
	}


}
