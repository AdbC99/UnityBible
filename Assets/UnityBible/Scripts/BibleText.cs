using SQLite4Unity3d;

public class BibleText
{
	[PrimaryKey]
	public int Row { get; set; }
	public string BookCode { get; set; }
	public int Chapter { get; set; }
	public int Verse { get; set; }
	public string Text { get; set; }

	public override string ToString()
	{
		return string.Format("[BibleChapters: Row={0}, BookCode={1},  Chapters={2}, Verse={3}, Text={4}]", Row, BookCode, Chapter, Verse, Text);
	}
}