using SQLite4Unity3d;

public class BibleChapters
{

	[PrimaryKey]
	public string BookCode { get; set; }
	public string BookName { get; set; }
	public int Chapters { get; set; }

	public override string ToString()
	{
		return string.Format("[BibleChapters: BookCode={0}, BookName={1},  Chapters={2}]", BookCode, BookName, Chapters);
	}
}