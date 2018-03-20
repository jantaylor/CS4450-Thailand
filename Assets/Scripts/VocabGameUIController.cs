using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabGameUIController : MonoBehaviour {

	public VocabGameController vocabGameController;

	private GameObject forwardButton;
	private GameObject backButton;

	// Use this for initialization
	void Start () {
		vocabGameController = GameObject.Find("Vocab Container").GetComponent<VocabGameController>();
		forwardButton = transform.Find("Arrow Right").gameObject;
		backButton = transform.Find("Arrow Left").gameObject;
		forwardButton.SetActive(false);
		backButton.SetActive(false);
	}

	void OnClick(MonoBehaviour sender)
	{
		GameObject gameObject = sender.GetComponent<GameObject>();
		if (gameObject.name == "Arrow Right")
		{
			vocabGameController.Proceed(1);
		}
		if (gameObject.name == "Arrow Left")
		{
			vocabGameController.Proceed(-1);
		}
	}

	public void Back()
	{
		if (!vocabGameController.Proceed(-1))
		{
			backButton.SetActive(false);
		}

		forwardButton.SetActive(true);
	}

	public void Forward()
	{
		vocabGameController.Proceed(1);
		forwardButton.SetActive(false);
		backButton.SetActive(true);
	}

	public void Match()
	{
		forwardButton.SetActive(true);
	}

	public void Menu()
	{
		GameState.Instance.LoadMenu();
	}
}
