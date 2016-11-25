using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Bible access example.
/// </summary>
[RequireComponent(typeof(BibleAccessAPI))]
public class BibleAccessExample : MonoBehaviour {

	public Text Text;
	public Text Title;

	public string SearchString = "PSA 1";

	// Use this for initialization
	void Start () {

		var bible = this.GetComponent<BibleAccessAPI>();

		Text.text = bible.RetrieveVersesBySearchString(SearchString);

		Title.text = bible.RetrieveTitle(SearchString) + SearchString.Substring(3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
