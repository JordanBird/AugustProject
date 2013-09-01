using UnityEngine;
using System.Collections;

public class Answer
{
	public bool correct;
	public string text = "";
	public Texture2D texture;

	//Type, 0 = Text, 1 = Image.
	public int type = 0;
	
	public Answer(bool iCorrect, string iText)
	{
		text = iText;
		correct = iCorrect;
		
		type = 0;
	}
	
	public Answer(bool iCorrect, Texture2D iTexture)
	{
		texture = iTexture;
		correct = iCorrect;
		
		type = 1;
	}
	
	public Answer(bool iCorrect, string iText, Texture2D iTexture)
	{
		text = iText;
		texture = iTexture;
		correct = iCorrect;
		
		type = 1;
	}
}