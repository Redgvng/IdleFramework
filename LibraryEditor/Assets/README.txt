Unity AGI
Welcome to the Unity AGI implementation! This README, along with the interactive documentation, should get you on your feet with using the AGI within Unity.

Usage
Firstly, to make AGI calls you’ll need to ensure that you have placed the AGI prefab in your scene.
This is a premade GameObject that already has the AGIUnity.cs attached. No AGI GameObject, no AGI calls!
Using AGI with Unity is as simple as it gets using our example script!
All you need to do is copy the contents of the function you’d like from within AGIUnityExampleCalls.cs or reference the static function from within AGIUnity.cs.
On line 49 within ag.min.jspre, ensure you enter the correct api key for your game. Visit the developer portal to obtain this key from the game’s settings page.
AGIUnity.cs Usage

   " // Log the user in and grant access to information including username, UID and avatar.
    // To make use of this information, you must handle it in UnityAGI.cs (SetUsername, SetUID, SetAvatar)."

    AGIUnity.GetUser ();

    "// Key is the name of the save, this could be called "highscore" if you are saving a users highscore.
    // Here, a random number is saved for testing purposes."

    AGIUnity.SaveData (key, Random.Range (10, 100));

    "// Key identifies the data to load, in our example, this is called "highscore"."

    AGIUnity.LoadGame (key);

    "// Erase a save with the given key. In our example, this is called "highscore"."

    AGIUnity.EraseGame (key);

    "// Add to your key. In this example, we would add 1 to the stored value."

    AGIUnity.IncrementGame (key, 1);

    "// Subtract from your key. In this example, we would add 1 to the stored value."

    AGIUnity.DecrementGame (key, 1);

    "// GetQuests retrieves any active quests for the user.
    // The quests in the array must be handled one at a time from the QuestRetrieved function in AGIUnity.cs"

    AGIUnity.GetQuests ();

    "// Submit quest progress for the user. Progress is a value from 0.0 to 1.0, with 1.0 being 100% complete for a specific Quest ID."

    AGIUnity.SubmitQuest (s, 1);

    "// This is an example of how to submit a specific quest that we use in this demo."

    AGIUnity.SubmitQuest ("easy_quest", questValue);

    "// This is an example of how to submit a specific quest that we use in this demo."

    AGIUnity.SubmitQuest ("hard_quest", questValue);

    "// Resets a quest's progress back to zero using a Quest ID."

    AGIUnity.ResetQuest (s);

    "// Displays the HTML Storefront modal window atop the game."

    AGIUnity.ShowStorefront ();

    "//Retrieve all purchases made by the user. These must be handled in AGIUnity.cs"

    AGIUnity.RetrievePurchases ();

    "// The consume method will decrease a user's item quantity by the given amount for the provided product SKU."

    AGIUnity.ConsumePurchase (sku, 1);

    "// This will open a new tab in the users browser."

    AGIUnity.OpenPage(URL);

Contents
AGI.jslib script - These are JS functions that link to the original Armor Games AGI JS file.

Demo Example Scene - A premade Unity scene with basic usage of the AGI within Unity. It contains all files needed with buttons linking to common AGI calls and is a great way to start your journey!

AGIUnity.cs script - This handles the interactions with JavaScript from the AGI.jslib file. These are the functions you’re able to call upon within Unity.

AGIUnityExampleCalls - A script full of example functions that interact with the AGI. You can copy any of these functions to other scripts or use this class as it is.

AGSplashScreen - A prefab pre-set up with the Armor Game splash screen. Please ensure you have assigned your camera to the video player component.

AGI Prefab - A prefab containing the AGIUnity.cs script already. Place this in your game as early as possible before making AGI Calls.

Help
If you have any questions, please contact: Tasselfoot@armorgames.com

or

Visit the Armorgames Readme.io