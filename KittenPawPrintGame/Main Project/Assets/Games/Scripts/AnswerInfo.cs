using UnityEngine;
using System.Collections;

public class AnswerInfo : MonoBehaviour
{
	public Texture Image { get; private set; }
	public string Text { get; private set; }
	public bool UsesImage { get; private set; }
	
	public Material[] Skins;
	
	public void Start()
	{
		renderer.material = Skins[Random.Range (0, Skins.Length)];
	}
	
	public void SetInfo(Texture image)
	{
		Image = image;
		UsesImage = true;
	}
	
	public void SetInfo(string text)
	{
		Text = text;
		UsesImage = false;
	}
}
