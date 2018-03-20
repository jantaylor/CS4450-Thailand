using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabUIController : MonoBehaviour {

	public VocabController vocabController;

	private GameObject forwardButton;
	private GameObject backButton;

	// Use this for initialization
	void Start () {
		vocabController = GameObject.Find("Vocab Container").GetComponent<VocabController>();
		forwardButton = transform.Find("Arrow Right").gameObject;
		backButton = transform.Find("Arrow Left").gameObject;
		backButton.SetActive(false);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnClick(MonoBehaviour sender)
	{
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
		if (!vocabController.Proceed(-1))
		{
			backButton.SetActive(false);
		}

		forwardButton.SetActive(true);
	}

	public void Forward()
	{
		if (!vocabController.Proceed(1))
		{
			forwardButton.SetActive(false);
		}
		backButton.SetActive(true);
	}

	public void Menu()
	{
		GameState.Instance.LoadMenu();
	}
}
