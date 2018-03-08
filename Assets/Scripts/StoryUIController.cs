using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryUIController : MonoBehaviour {

	public StoryController storyController;

	// Use this for initialization
	void Start () {
        storyController = GameObject.Find("Story Container").GetComponent<StoryController>();
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
            storyController.Proceed(1);
		}
		if (gameObject.name == "Arrow Left")
		{
            storyController.Proceed(-1);
		}
	}

	public void Back()
	{
        storyController.Proceed(-1);
	}

	public void Forward()
	{
        storyController.Proceed(1);
	}
}
