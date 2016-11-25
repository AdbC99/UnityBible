using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Parse VPL Text.
/// 
/// The purpose of this file is to parse the VPL text so as to create
/// a separated variable file that can be read into an sqlite database.
/// The script assumes the files have been placed in the streaming assets
/// folder. As a user of this library you are unlikely to need this since
/// it was used to generate the sqlite database.
/// </summary>
public class ParseVPLText : MonoBehaviour {

	public string input_file = "eng-web_vpl.txt";
	public string output_file = "eng-web_sv.txt";
	public string output_bookdata_file = "eng-web_dat.txt";
	public string output_chapterdata_file = "eng-web_dat2.txt";

	public Dictionary<string, int> Chapters;
	public List<string> Books;

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
		{"PSA","Psalms"},
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

	// Use this for initialization
	void Start () {

		Chapters = new Dictionary<string, int>();

		int counter = 1;
		string line;

		FileStream fsr = File.OpenRead(Application.streamingAssetsPath + "/" + input_file);

		StreamWriter sw_t = File.CreateText(Application.streamingAssetsPath + "/" + output_file);

		StreamWriter sw_d2 = File.CreateText(Application.streamingAssetsPath + "/" + output_chapterdata_file);

		StreamReader sr = new StreamReader(fsr);

		while ((line = sr.ReadLine()) != null)
		{
			// Line format is XXX_CH:V_TEXT

			int firstspace = line.IndexOf(" ", 0, System.StringComparison.InvariantCulture);
			int secondspace = line.IndexOf(" ", 4, System.StringComparison.InvariantCulture);

			string book = line.Substring(0, firstspace);

			string chapterverse = line.Substring(firstspace + 1, secondspace - firstspace - 1);

			int chapter = int.Parse(chapterverse.Split(new string[] {":" }, System.StringSplitOptions.RemoveEmptyEntries)[0]);
			int verse = int.Parse(chapterverse.Split(new string[] { ":" }, System.StringSplitOptions.RemoveEmptyEntries)[1]);

			string text = line.Substring(secondspace + 1);

			sw_t.WriteLine(counter + "|" + book + "|" + chapter + "|" + verse + "|" + text);

			if (verse == 1) 
			{
				sw_d2.WriteLine(counter + "|" + book + "|" + chapter);
			}

			//Debug.Log("Book:" + book + " " + book.Length + " " + firstspace + " " + secondspace);
			//Debug.Log("ChapterVerse:" + chapterverse + " " + chapterverse.Length + " " + chapter);
			//Debug.Log("Text:" + text);

			if (!Books.Contains(book))
			{
				Books.Add(book);
			}

			Chapters[book] = chapter;

			counter++;
		}

		sr.Close();
		sw_t.Close();
		sw_d2.Close();

		StreamWriter sw_d = File.CreateText(Application.streamingAssetsPath + "/" + output_bookdata_file);

		foreach(string bookcode in Books)
		{
			sw_d.WriteLine(bookcode + "|" + BookNames[bookcode] + "|" + Chapters[bookcode]);
		}

		sw_d.Close();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
