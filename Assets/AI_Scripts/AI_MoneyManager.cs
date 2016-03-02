using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AI_MoneyManager : MonoBehaviour {

	public static int money;        // The player's score.
	Text text;                      // Reference to the Text component.
	
	
	////void Awake ()
	void Start()
	{
		// Set up the reference.
		text = GetComponent <Text> ();
		
		// Reset the score.
		money = 0;
	}
	
	
	void Update ()
	{
		// Set the displayed text to be the word "Score" followed by the score value.
		text.text = "Money: " + money;
	}
}
