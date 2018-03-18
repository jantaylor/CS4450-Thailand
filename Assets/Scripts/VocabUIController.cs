using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabUIController : MonoBehaviour {

	public VocabController vocabController;

	// Use this for initialization
	void Start () {
		vocabController = GameObject.Find("Vocab Container").GetComponent<VocabController>();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnClick(MonoBehaviour sender)
	{
		Debug.Log("UI Click");
		GameObject gameObject = sender.GetComponent<GameObject>();
		if (gameObject.name == "Arrow Right")
		{
			vocabController.Proceed(1);
		}
		if (gameObject.name == "Arrow Left")
		{
			vocabController.Proceed(-1);
		}
	}

	public void Back()
	{
		vocabController.Proceed(-1);
	}

	public void Forward()
	{
		vocabController.Proceed(1);
	}

	public void Menu()
	{
		GameState.Instance.LoadMenu();
	}
}
