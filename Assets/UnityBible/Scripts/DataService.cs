using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DataService  {

	private SQLiteConnection _connection;

	public DataService(string DatabaseName)
	{

#if UNITY_EDITOR
		var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
		_connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
		Debug.Log("Final PATH: " + dbPath);
	}

	public void CreateSaveGameDatabaseData()
	{
		_connection.DropTable<SaveGameData>();
		_connection.CreateTable<SaveGameData>();

		_connection.InsertAll(new[]{
			new SaveGameData{
				PlayerSlot= 1,
				PlayerName = "AAA",
				Level = 0,
				Score = 0
			},
			new SaveGameData{
				PlayerSlot= 2,
				PlayerName = "RAD",
				Level = 0,
				Score = 0
			},
			new SaveGameData{
				PlayerSlot= 3,
				PlayerName = "XXX",
				Level = 0,
				Score = 0
			},

		});
	}

	public SaveGameData GetSaveGameData(int PlayerSlot)
	{
		var data = _connection.Table<SaveGameData>();

		foreach (var d in data)
			return d;

		return null;
	}

	public void UpdateSaveGameData(SaveGameData data)
	{
		Debug.Log("Updating Data: " + data.ToString());
		_connection.Update(data);
		_connection.Commit();
	}

	public List<BibleChapters> GetChapters()
	{
		List<BibleChapters> rows = new List<BibleChapters>();

		var data = _connection.Table<BibleChapters>();

		foreach (var d in data)
			rows.Add(d);

		return rows;
	}

	public List<BibleText> GetBook(string BookCode){
		List<BibleText> rows = new List<BibleText>();

		var data = _connection.Table<BibleText>().Where(x => x.BookCode == BookCode);

		foreach (var d in data)
			rows.Add(d);

		return rows;
	}

	public int GetVersesInBook(string BookCode)
	{
		return (_connection.Table<BibleText>().Where(x => x.BookCode == BookCode)).Count();
	}

	public List<BibleText> GetChapter(string BookCode, int Chapter)
	{
		List<BibleText> rows = new List<BibleText>();

		var data = _connection.Table<BibleText>().Where(x => x.BookCode == BookCode).Where(x => x.Chapter == Chapter);

		foreach (var d in data)
			rows.Add(d);

		return rows;
	}

	public BibleText GetVerse(string BookCode, int Chapter, int Verse)
	{
		var data = _connection.Table<BibleText>().Where(x => x.BookCode == BookCode).Where(x => x.Chapter == Chapter).Where(x => x.Verse == Verse);

		foreach (var d in data)
			return d;

		return null;
	}

	public List<BibleText> GetRows(int RowStart, int RowEnd)
	{
		List<BibleText> rows = new List<BibleText>();

		var data = _connection.Table<BibleText>().Where(x => x.Row >= RowStart).Where(x => x.Row <= RowEnd);

		foreach (var d in data)
			rows.Add(d);

		return rows;
	}


}