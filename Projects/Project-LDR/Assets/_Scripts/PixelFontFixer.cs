using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelFontFixer : MonoBehaviour {

    public Font[] fontsToFix;

	// Use this for initialization
	void Start () {
		foreach(Font f in fontsToFix)
        {
            f.material.mainTexture.filterMode = FilterMode.Point;
            f.material.mainTexture.anisoLevel = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
