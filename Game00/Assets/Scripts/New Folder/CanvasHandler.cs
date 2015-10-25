using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour {

    private CanvasScaler scaler;

	// Use this for initialization
	void Start () {

        scaler = GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.matchWidthOrHeight = 0.5F;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
