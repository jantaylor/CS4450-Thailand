using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabUIController : MonoBehaviour {

	public VocabController vocabController;

	private GameObject forwardButton;
	private GameObject backButton;
	private GameObject checkButton;

	// Use this for initialization
	void Start () {
		vocabController = GetComponent<VocabController>();
		forwardButton = transform.Find("Arrow Right").gameObject;
		backButton = transform.Find("Arrow Left").gameObject;
		checkButton = transform.Find("Check").gameObject;
		DisableBackward();
		DisableForward();
	}

	// Update is called once per frame
	void Update () {

	}

	public void Back()
	{
		vocabController.Proceed(-1);
		EnableBackward();
	}

	public void Forward()
	{
		vocabController.Proceed(1);
		DisableForward();
		EnableBackward();
	}

	public void EnableBackward()
	{
		backButton.SetActive(!vocabController.IsFirst());
	}

	public void DisableBackward()
	{
		backButton.SetActive(false);
	}

	public void EnableForward()
	{
		forwardButton.SetActive(!vocabController.IsLast());
		checkButton.SetActive(vocabController.IsLast());
	}

	public void DisableForward()
	{
		forwardButton.SetActive(false);
		checkButton.SetActive(false);
	}

	public void Finish()
	{
		GameState.Instance.LoadScene(4, GameState.Instance.ActiveRound);
	}

	public void Menu()
	{
		GameState.Instance.LoadMenu();
	}
}
