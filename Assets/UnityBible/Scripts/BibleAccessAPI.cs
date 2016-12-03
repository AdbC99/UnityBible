using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Bible access API.
/// 
/// This is the intended class to access biblical data with. It supports a range of
/// ways to access the data. 
/// </summary>
public class BibleAccessAPI : MonoBehaviour {

	/// <summary>
	/// The data service - an object representing the database connection.
	/// </summary>
	private DataService dataService;

	public bool addVerseNumbers = false;

	/// <summary>
	/// The book names - this list gives all the 3 letter codes for each of the books
	/// contained. Note that this is ever so slightly different from the list contained
	/// in the text parser. This list is meant for displaying titles; whereas the text
	/// parser is for parsing texts. The titles are slightly different, for instance
	/// the titles use Psalm; whereas the parsed text requires Psalms.
	/// </summary>
	public Dictionary<string, string> BookNames = new Dictionary<string, string>()
	{
		{"GEN","Genesis"},
		{"EXO","Exodus"},
		{"LEV","Leviticus"},
		{"NUM","Numbers"},
		{"DEU","Deuteronomy"},
		{"JOS","Joshua"},
		{"JDG","Judges"},
		{"RUT","Ruth"},
		{"1SA","1 Samual"},
		{"2SA","2 Samual"},
		{"1KI","1 Kings"},
		{"2KI","2 Kings"},
		{"1CH","1 Chronicles"},
		{"2CH","2 Chronicles"},
		{"EZR","Ezra"},
		{"NEH","Nehemiah"},
		{"EST","Esther"},
		{"JOB","Job"},
		{"PSA","Psalm"},
		{"PRO","Proverbs"},
		{"ECC","Ecclesiastes"},
		{"SOL","Song of Songs"},
		{"ISA","Isaiah"},
		{"JER","Jeremiah"},
		{"LAM","Lamentations"},
		{"EZE","Ezekiel"},
		{"DAN","Daniel"},
		{"HOS","Hosea"},
		{"JOE","Joel"},
		{"AMO","Amos"},
		{"OBA","Obadiah"},
		{"JON","Jonah"},
		{"MIC","Micah"},
		{"NAH","Nahum"},
		{"HAB","Habakkuk"},
		{"ZEP","Zephaniah"},
		{"HAG","Haggai"},
		{"ZEC","Zechariah"},
		{"MAL","Malachi"},
		{"TOB","Tobit"},
		{"JDT","Judith"},
		{"ESG","Greek Esther"},
		{"WIS","Wisdom of Solomon"},
		{"SIR","Wisdom of Sirach"},
		{"BAR","Baruch"},
		{"1MA","1 Maccabees"},
		{"2MA","2 Maccabees"},
		{"1ES","1 Esdras"},
		{"PRM","Prayer of Manasseh"},
		{"PSX","Psalm 151"},
		{"3MA","3 Maccabees"},
		{"4ES","4 Esdras"},
		{"4MA","4 Maccabees"},
		{"DNG","Daniel (Greek)"},
		{"MAT","Matthew"},
		{"MAR","Mark"},
		{"LUK","Luke"},
		{"JOH","John"},
		{"ACT","Acts"},
		{"ROM","Romans"},
		{"1CO","1 Corinthians"},
		{"2CO","2 Corinthians"},
		{"GAL","Galatians"},
		{"EPH","Ephesus"},
		{"PHI","Philippians"},
		{"COL","Colossians"},
		{"1TH","1 Thessalonians"},
		{"2TH","2 Thessalonians"},
		{"1TI","1 Timothy"},
		{"2TI","2 Timothy"},
		{"TIT","Titus"},
		{"PHM","Philemon"},
		{"HEB","Hebrews"},
		{"JAM","James"},
		{"1PE","1 Peter"},
		{"2PE","2 Peter"},
		{"1JO","1 John"},
		{"2JO","2 John"},
		{"3JO","3 John"},
		{"JUD","Jude"},
		{"REV","Revelation"},
	};

	/// <summary>
	/// Awake this instance. Just connect to the database once the scene is open
	/// </summary>
	void Awake()
	{
		dataService = new DataService("BIBLE_WEB");
	}

	/// <summary>
	/// Renders the text. 
	/// 
	/// Glues the retrieved verses together into one string. 
	/// If you want to render text differently then just replace this function.
	/// </summary>
	/// <returns>The text.</returns>
	/// <param name="data">Data.</param>
	private string RenderText(List<BibleText> data)
	{
		string verses = "";

		foreach (var verse in data)
		{
			// Add a double line space between verses
			if (verses.Length != 0)
				verses += "\r\n\r\n";

			if (addVerseNumbers)
				verses += verse.Verse + ". ";

			verses += verse.Text;
		}

		return verses;
	}

	/// <summary>
	/// Retrieves the title give a 3 digit book code. It just looks at the first
	/// 3 characters and is thus tolerant to being sent verse search strings like 'PSA 1:1-4'
	/// </summary>
	/// <returns>The title.</returns>
	/// <param name="versestring">Versestring.</param>
	public string RetrieveTitle(string versestring)
	{
		string bookcode = versestring.Substring(0, 3).ToUpper();

		return BookNames[bookcode];
	}

	/// <summary>
	/// Retrieve a verse or set of verses using a search string
	/// </summary>
	/// <returns>The verse.</returns>
	/// <param name="versestring"> The verse string takes the following format 'XXX C:VS-VF' 
	/// for a range of verses 'XXX C:V' for a single verse e.g. 'PSA 1:5-7'. If the verse is
	/// range is incorrect i.e. outside of the bounds of the chapter then it simply grabs
	/// text from the next chapter. Other options are 'PSA' for all psalms, or 'PSA 1' for all
	/// of Psalm 1 </param>
	public string RetrieveVersesBySearchString(string versestring)
	{
		// Decipher the string
		string bookcode = versestring.Substring(0, 3).ToUpper();

		if (versestring.Trim().Length == 3)
			return RetrieveWholeBook(bookcode);

		if (!versestring.Contains(":"))
		{
			int whole_chapter = int.Parse(versestring.Substring(4).Trim());
			return RetrieveWholeChapter(bookcode, whole_chapter);
		}

		string[] chapterverse = (versestring.Substring(3)).Split(new string[] {":"},System.StringSplitOptions.RemoveEmptyEntries);

		int chapter = int.Parse(chapterverse[0]);

		int verse_start = 0;
		int verse_end = 0;

		if (chapterverse[1].Contains("-"))
		{
			string[] versestartend = chapterverse[1].Split(new string[] { "-" }, System.StringSplitOptions.RemoveEmptyEntries);

			verse_start = int.Parse(versestartend[0]);
			verse_end = int.Parse(versestartend[1]);
		}
		else
		{
			verse_start = int.Parse(chapterverse[1]);
			verse_end = verse_start;
		}

		return RetrieveVerses(bookcode, chapter, verse_start, verse_end);
	}

	/// <summary>
	/// Retrieves a range of verses.
	/// </summary>
	/// <returns>The verses.</returns>
	/// <param name="bookcode">Bookcode e.g. 'PSA' </param>
	/// <param name="chapter">Chapter number </param>
	/// <param name="verse_start">Verse start number</param>
	/// <param name="verse_end">Verse end number</param>
	public string RetrieveVerses(string bookcode, int chapter, int verse_start, int verse_end)
	{
		// First we want the starting row of the chapter
		int chapter_start_row = dataService.GetVerse(bookcode, chapter, 1).Row;

		// Next we want to grab the correct verses

		var data = dataService.GetRows(chapter_start_row + verse_start - 1, chapter_start_row + verse_end - 1);

		// Glue the verses together
		return RenderText(data);
	}

	/// <summary>
	/// Retrieves a whole chapter of the bible
	/// </summary>
	/// <returns>The whole chapter.</returns>
	/// <param name="bookcode">Bookcode e.g. 'PSA'</param>
	/// <param name="chapter">Chapter number</param>
	public string RetrieveWholeChapter(string bookcode, int chapter)
	{
		var data = dataService.GetChapter(bookcode, chapter);

		// Glue the verses together
		return RenderText(data);
	}

	/// <summary>
	/// Retrieves the whole book.
	/// </summary>
	/// <returns>The whole book</returns>
	/// <param name="bookcode">Bookcode e.g. 'PSA'</param>
	public string RetrieveWholeBook(string bookcode)
	{
		var data = dataService.GetBook(bookcode);

		// Glue the verses together
		return RenderText(data);
	}
}
