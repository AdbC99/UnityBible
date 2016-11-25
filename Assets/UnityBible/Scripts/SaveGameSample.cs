using UnityEngine;
using System.Collections;

/// <summary>
/// Save game sample.
/// </summary>
public class SaveGameSample : MonoBehaviour {


	/// <summary>
	/// The player slot - i.e. if 3 save game slots are available then this which one of those three is selected
	/// </summary>
	public int PlayerSlot = 1;

	/// <summary>
	/// The data service - an object representing the database connection.
	/// </summary>
	private DataService dataService;

	// Use this for initialization
	void Start () {
		dataService = new DataService("SAVE_GAME_DATA");

		// Comment in this line to recreate the table
		//dataService.CreateSaveGameDatabaseData();

		var data = dataService.GetSaveGameData(1);
		{
			Debug.Log("Received Data: " + data.ToString());

			data.Score += 1;

			dataService.UpdateSaveGameData(data);
		}
	}
}
