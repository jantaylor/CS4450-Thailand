using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordAudioButtonController : MonoBehaviour {

	private WordController word;
	
	void Start () {
		word = transform.parent.GetComponent<WordController>();
	}

	void OnMouseUp() {
		Debug.Log("Clicked");
		word.PlaySound();
	}
}
