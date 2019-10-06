using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cycle : MonoBehaviour
{

	public float HValue = 0f;
	public float PerFrame = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

		HValue += PerFrame;

		if (HValue > 1f) {
			HValue = 0f;
		}

		Camera.main.backgroundColor = Color.HSVToRGB (HValue, 0.53f, 0.75f);
        
    }
}
