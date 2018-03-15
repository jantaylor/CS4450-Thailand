using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

	/// <summary>
	/// The activity associated with this button (-1: menu, 1: vocab, 2: story, 3: game)
	/// </summary>
	public int activity;

	/// <summary>
	/// The activity associated with this button (-1: menu, #>0: round #)
	/// </summary>
	public int round;


	/// <summary>
	/// Calls the game state to load the appropriate scene using the activity and round.
	/// </summary>
	public void LoadScene()
	{
		GameState.Instance.LoadScene(activity, round);
	}
}
