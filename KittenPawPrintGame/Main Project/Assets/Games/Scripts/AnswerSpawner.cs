using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnswerSpawner : MonoBehaviour
{
	public Transform ScrollingBackground;
	public Transform AnswerPrefab;
	public float SpawnDelay;
	public bool Running { get; private set; }
	
	private float spawnTimer = 0;
	private List<Answer> answerBatch = new List<Answer>();
	private List<Answer> correctAnswers = new List<Answer>();
	private List<Transform> answerObjects = new List<Transform>();
	private cscript_plane_game controller;
	private cscript_GUI_master GUIMaster;
	private int incorrectAnswers = 0;
	private float startDelay = 4;
	
	private Question currentQuestion { get { if (controller != null) {return controller.currentQuestion;} return null; } }
	
	void Start ()
	{
	}
	
	public int GetCorrectScore()
	{
		return correctAnswers.Count;
	}
	
	public int GetIncorrectScore()
	{
		return incorrectAnswers;	
	}
	
	
	public void Begin(cscript_plane_game controller)
	{
		this.controller = controller;
		this.GUIMaster = GameObject.FindGameObjectWithTag ("GUI Master").GetComponent<cscript_GUI_master>();
		ScrollingBackground.gameObject.SetActive(true);
		ScrollingBackground.renderer.material.mainTextureOffset = Vector2.zero;
		Running = true;
	}
	
	void Update ()
	{
		if (Running)
		{
			if (startDelay > 0)
			{
				startDelay -= Time.deltaTime;
				
				if (startDelay < 0)
				{
					startDelay = 0;	
				}
			}
			
			if (startDelay < 1)
			{
				spawnTimer += Time.deltaTime;
				if (spawnTimer >= SpawnDelay)
				{
					spawnTimer %= SpawnDelay;
					
					SpawnNextAnswer();
				}
				
			
				for (int i = answerObjects.Count - 1; i >= 0; i--)
				{
					answerObjects[i].position += new Vector3(-5f, 0, 0) * Time.deltaTime;
					
					if (answerObjects[i].position.x < -15)
					{
						Destroy(answerObjects[i].gameObject);
						answerObjects.RemoveAt(i);
					}
				}
			}
			
			ScrollingBackground.renderer.material.mainTextureOffset += new Vector2(0.1f, 0) * Time.deltaTime;
			if (ScrollingBackground.renderer.material.mainTextureOffset.x >= 1)
			{
				ScrollingBackground.renderer.material.mainTextureOffset = new Vector2(ScrollingBackground.renderer.material.mainTextureOffset.x % 1, 0);	
			}
		}
	}
	
	private void SpawnNextAnswer()
	{		
		if (answerBatch.Count == 0)
		{			
			// Refresh answers once all have previously been spawned.
			answerBatch.AddRange(currentQuestion.answers);
			
			// Don't cycle correct answers that are already taken
			for (int i = answerBatch.Count - 1; i >=0; i--)
			{
				if (answerBatch[i].correct == true)
				{
					for (int j = 0; j < correctAnswers.Count; j++)
					{
						if (answerBatch[i].texture != null && answerBatch[i].texture.Equals(correctAnswers[j].texture) ||
							answerBatch[i].texture == null && answerBatch[i].text == correctAnswers[j].text)
						{
							answerBatch.RemoveAt (i);
							break;	
						}
					}
				}
			}
		}
		
		int answer = Random.Range(0, answerBatch.Count);
		
		// Add a new AnswerCollectable prefab with the text set to the answer's text.
		AnswerPrefab.position = new Vector3(10, Random.Range (-2f, 4.5f), 0);
		answerObjects.Add ((Transform)Instantiate(AnswerPrefab));
		if(answerBatch[answer].texture == null)
		{
			answerObjects[answerObjects.Count - 1].GetComponent<AnswerInfo>().SetInfo(answerBatch[answer].text);
		}
		else
		{
			answerObjects[answerObjects.Count - 1].GetComponent<AnswerInfo>().SetInfo(answerBatch[answer].texture);
		}
		
		if (answerBatch[answer].correct == false)
		{
			for (int i = answerObjects.Count - 1; i >= 0; i--)
			{
				AnswerInfo info = answerObjects[i].GetComponent<AnswerInfo>();
			
				for (int j = i - 1; j >= 0; j--)
				{
					AnswerInfo otherInfo = answerObjects[j].GetComponent<AnswerInfo>();
				
					if (info.UsesImage && otherInfo.Image == info.Image || !info.UsesImage && otherInfo.Text == info.Text)	
					{
						answerObjects.RemoveAt (i);
						Destroy (answerObjects[i]);
						return;
					}
				}
			}
		}
		
		// Remove the answer so it isn't used again until all others are used.
		answerBatch.RemoveAt (answer);
	}
	
	public void CheckAnswer(GameObject answerObject)
	{
		foreach (Answer a in currentQuestion.answers)
		{
			AnswerInfo info = answerObject.GetComponent<AnswerInfo>();
			
			// Find the answer.
			if (info.UsesImage && a.texture == info.Image || !info.UsesImage && a.text == info.Text)
			{				
				if (a.correct)
				{
					correctAnswers.Add (a);
					
					for (int i = answerBatch.Count - 1; i >= 0; i--)
					{
						if (answerBatch[i].texture != null && answerBatch[i].texture.Equals(a.texture) ||
							answerBatch[i].texture == null && answerBatch[i].text == a.text)
						{
							answerBatch.RemoveAt (i);
							break;	
						}
					}
					
					// If the number of correct answers is equal to the correct answers collected, the player has won.
					if (currentQuestion.GetNumberOfCorrectAnswers() == correctAnswers.Count)
					{
						Running = false;
						controller.WinGame();
						
						foreach (Transform t in answerObjects)
						{
							Destroy (t.gameObject);	
						}
						
						return;
					}
				}
				else
				{
					//INCORRECT ANSWER, DO SOMETHING
					incorrectAnswers++;
				}				
				
				break;
			}
		}
		
		Destroy (answerObject);
		answerObjects.Remove (answerObject.transform);
	}
	
	public void Cleanup()
	{
		startDelay = 4;
		Running = false;
		spawnTimer = 0;
		correctAnswers.Clear ();
		answerBatch.Clear ();
		incorrectAnswers = 0;
		
		for (int i = 0; i < answerObjects.Count; i++)
		{
			if (answerObjects[i] != null)
			{
				Destroy (answerObjects[i].gameObject);	
			}
		}
		
		answerObjects.Clear ();
		ScrollingBackground.gameObject.SetActive(false);
	}
	
	void OnGUI()
	{		
		if (currentQuestion != null)
		{			
			if (GUIMaster != null && startDelay > 0)
			{
				GUI.Label (new Rect(Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100), startDelay < 1 ? "GO!" : Mathf.Floor(startDelay).ToString(), GUIMaster.questions.label);
				return;
			}
			
			Color prevColor = GUI.skin.label.normal.textColor;
			TextAnchor prevAnchor = GUI.skin.label.alignment;
			int prevSize = GUI.skin.label.fontSize;
			
			GUI.skin.label.normal.textColor = Color.black;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.skin.label.fontSize = 32 * Screen.width / 1024;
			
			foreach (Transform answer in answerObjects)
			{
				if (answer == null)
					break;
				
				Vector3 pos = Camera.main.WorldToScreenPoint(answer.position);
				
				AnswerInfo info = answer.GetComponent<AnswerInfo>();
				
				if (info.UsesImage)
				{
					GUI.DrawTexture(new Rect((int)pos.x - 64,
						Screen.height - (int)pos.y - 44,
						128, 
						100),
						info.Image);
				}
				else
				{
					GUI.Label (new Rect((int)pos.x - 200, Screen.height - (int)pos.y - 200, 400, 400), info.Text);
				}
			}		
			
			GUI.skin.label.normal.textColor = prevColor;
			GUI.skin.label.alignment = prevAnchor;
			GUI.skin.label.fontSize = prevSize;
		}
	}
}
