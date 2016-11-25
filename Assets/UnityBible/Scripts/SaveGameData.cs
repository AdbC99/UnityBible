using SQLite4Unity3d;

/// <summary>
/// Save game data.
/// 
/// Edit this to your hearts content, you can use the create database function in DataService to create
/// a new table and populate it with data. This is designed to be the sort of console save game system 
/// where a characer has around 3 players and continues with a player. Instead of storing a slot
/// number you could store a date for a different style of save game functionality
/// </summary>
public class SaveGameData
{
	[PrimaryKey][NotNull]
	public int PlayerSlot { get; set; }
	public string PlayerName { get; set; }
	public int Level { get; set; }
	public int Score { get; set; }

	public override string ToString()
	{
		return string.Format("[SaveGameData: Slot={0}, Name={1}, Level={2}, Score={3}]", PlayerSlot, PlayerName, Level, Score);
	}
}
