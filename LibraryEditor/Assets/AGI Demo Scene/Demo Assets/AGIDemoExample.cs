using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AGIDemoExample : MonoBehaviour
{

	// THIS IS AN ADAPTION OF AGIUNITYEXAMPLECALLS.CS FOR THE PURPOSE OF THIS DEMO SCENE. WE'D RECOMMEND YOU USE AGIUNITYEXAMPLECALLS.CS FOR USAGE AND NOT THIS.

	public Slider easySlider;
	public Slider hardSlider;

	// This checks to see whether the AGI is active in your game. Ensure the AGI prefab is active in the scene.
	void Start ()
	{
		if (GameObject.Find ("AGI") == null) {
			Debug.LogError ("Ensure you have the AGI prefab in your scene.", transform);
		}
	}

	// THESE TWO FUNCTIONS ARE JUST FOR THE DEMO SCENE. For usage with Unity UI sliders.
	public void SetEasyQuestSlider ()
	{
		AGIUnity.SubmitQuest ("easy_quest", easySlider.value);
	}

	public void SetHardQuestSlider ()
	{
		AGIUnity.SubmitQuest ("hard_quest", hardSlider.value);
	}

	// ENSURE YOU HAVE the "AGI" prefab active in your scene.
	// Below are example functions that can be called to trigger actions in the AGI.
	// Please use whatever you need. You can copy the contents of a function to use anywhere in your game.

	#region User Methods

	// Log the user in and grant access to information including username, UID and avatar.
	// To make use of this information, you must handle it in UnityAGI.cs (SetUsername, SetUID, SetAvatar).
	public void LogIn ()
	{
		AGIUnity.GetUser ();
	}

	#endregion

	#region Data Storage

	// Key is the name of the save, this could be "highscore" if you're saving a user's highscore.
	// Here, a random number is saved for testing purposes.
	public void Save (string key)
	{
		AGIUnity.SaveData (key, Random.Range (10, 100));
	}

	// Key identifies the data to load, in our example, this is "highscore".
	public void Load (string key)
	{
		AGIUnity.LoadGame (key);
	}

	// Erase a save with the given key. In our example, this is "highscore".
	public void Erase (string key)
	{
		AGIUnity.EraseGame (key);
	}

	// Add to your key. In this example, we would add 1 to the stored value.
	public void Increment (string key)
	{
		AGIUnity.IncrementGame (key, 1);
	}

	// Subtract from your key. In this example, we would add 1 to the stored value.
	public void Decrement (string key)
	{
		AGIUnity.DecrementGame (key, 1);
	}

	#endregion

	#region Quests

	// GetQuests retrieves any active quests for the user.
	// The quests in the array must be handled one at a time from the QuestRetrieved function in AGIUnity.cs
	public void Quests ()
	{
		AGIUnity.GetQuests ();
	}

	// Submit quest progress for the user. Progress is a value from 0.0 to 1.0, with 1.0 being 100% complete for a specific Quest ID.
	public void SetQuest (string s)
	{
		AGIUnity.SubmitQuest (s, 1);
	}

	// This is an example of how to submit a specific quest that we use in this demo.
	public void SetEasyQuest (int questValue)
	{
		AGIUnity.SubmitQuest ("easy_quest", questValue);
	}

	// This is an example of how to submit a specific quest that we use in this demo.
	public void SetHardQuest (int questValue)
	{
		AGIUnity.SubmitQuest ("hard_quest", questValue);
	}

	// Resets a quest's progress back to zero using a Quest ID.
	public void ResetAQuest (string s)
	{
		AGIUnity.ResetQuest (s);
	}

	// Resets progress on all quests. For this example we've reset two quests.
	public void ResetAllQuests ()
	{
		AGIUnity.ResetQuest ("easy_quest");
		AGIUnity.ResetQuest ("hard_quest");
	}

	#endregion

	#region Storefront

	// Displays the HTML Storefront modal window atop the game.
	public void Storefront (string sku = "")
	{
		AGIUnity.ShowStorefront (sku);
	}



	//Retrieve all purchases made by the user. These must be handled in AGIUnity.cs
	public void RetrieveAllPurchases ()
	{
		AGIUnity.RetrievePurchases ();
	}

	// The consume method will decrease a user’s item quantity by the given amount for the provided product SKU.
	public void ConsumeAPurchase (string sku)
	{
		AGIUnity.ConsumePurchase (sku, 1);
	}

	#endregion

	#region Extras

	// This will open a new tab in the users browser.
	public void OpenWebPage (string URL)
	{
		#if !UNITY_WEBGL
		Application.OpenURL (URL);
		#else
		AGIUnity.OpenPage(URL);
		#endif

	}

	#endregion
}
