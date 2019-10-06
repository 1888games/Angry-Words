using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIt2 : MonoBehaviour
{
	float xPosition = 0f;
	public float xMove = 0.1f;

	// Start is called before the first frame update
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		xPosition += xMove;

		if (xMove > 0f) {

			if (xPosition > 1000f) {
				xPosition = -1000f;
			}
		} else {

			if (xPosition < -1000f) {
				xPosition = 1000f;
			}
		}

		GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0f, xPosition);
	}
}
