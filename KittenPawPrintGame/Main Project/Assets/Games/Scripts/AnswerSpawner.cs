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
	private int runningScore = 0;
	
	private Question currentQuestion { get { return controller.currentQuestion; } }
	
	void Start ()
	{
	}
	
	public void Begin(cscript_plane_game controller)
	{
		this.controller = controller;
		ScrollingBackground.gameObject.SetActive(true);
		ScrollingBackground.renderer.material.mainTextureOffset = Vector2.zero;
		Running = true;
	}
	
	void Update ()
	{
		if (Running)
		{
			spawnTimer += Time.deltaTime;
			if (spawnTimer >= SpawnDelay)
			{
				spawnTimer %= SpawnDelay;
				
				SpawnNextAnswer();
			}
			
		
			for (int i = 0; i < answerObjects.Count; i++)
			{
				answerObjects[i].position += new Vector3(-5f, 0, 0) * Time.deltaTime;
				
				if (answerObjects[i].position.x < -15)
				{
					Destroy(answerObjects[i].gameObject);
					answerObjects.RemoveAt(i);
					i--;
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
						if (answerBatch[i] == correctAnswers[j])
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
		answerObjects[answerObjects.Count - 1].GetComponent<AnswerInfo>().SetInfo(answerBatch[answer].text);
		
		// Remove the answer so it isn't used again until all others are used.
		answerBatch.RemoveAt (answer);
	}
	
	public void CheckAnswer(GameObject answerObject)
	{
		Debug.Log ("Checking");
		
		foreach (Answer a in currentQuestion.answers)
		{
			AnswerInfo info = answerObject.GetComponent<AnswerInfo>();
			
			// Find the answer.
			if (info.UsesImage && a.texture == info.Image || !info.UsesImage && a.text == info.Text)
			{
				Debug.Log ("Identified");
				
				if (a.correct)
				{
					correctAnswers.Add (a);
					runningScore++;
					
					// If the number of correct answers is equal to the correct answers collected, the player has won.
					if (currentQuestion.GetNumberOfCorrectAnswers() == correctAnswers.Count)
					{
						Running = false;
						controller.WinGame();
						return;
					}
				}
				else
				{
					//INCORRECT ANSWER, DO SOMETHING
					runningScore--;
				}				
				
				break;
			}
		}
		
		Destroy (answerObject);
		answerObjects.Remove (answerObject.transform);
	}
	
	public void Cleanup()
	{
		Running = false;
		spawnTimer = 0;
		correctAnswers.Clear ();
		answerBatch.Clear ();
		
		foreach (Transform t in answerObjects)
		{
			Destroy (t.gameObject);	
		}
		
		answerObjects.Clear ();
		ScrollingBackground.gameObject.SetActive(false);
	}
	
	void OnGUI()
	{
		GUI.Label (new Rect(100, 100, 600, 200), "DEBUGTEXT! Correct Answers: " + correctAnswers.Count.ToString() + "/" + currentQuestion.GetNumberOfCorrectAnswers().ToString());
		GUI.Label (new Rect(100, 200, 600, 200), "DEBUGTEXT! Running Score  : " + runningScore.ToString());
		
		Color prevColor = GUI.skin.label.normal.textColor;
		TextAnchor prevAnchor = GUI.skin.label.alignment;
		int prevSize = GUI.skin.label.fontSize;
		
		GUI.skin.label.normal.textColor = Color.black;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.skin.label.fontSize = 32 * Screen.width / 1024;
		
		foreach (Transform answer in answerObjects)
		{
			Vector3 pos = Camera.main.WorldToScreenPoint(answer.position);
			
			AnswerInfo info = answer.GetComponent<AnswerInfo>();
			
			if (info.UsesImage)
			{
				GUI.DrawTexture(new Rect(
					(int)pos.x - info.Image.width / 2,
					Screen.height - (int)pos.y - info.Image.height / 2,
					info.Image.width,
					info.Image.height),
					info.Image);
			}
			else
			{
				GUI.Label (new Rect((int)pos.x - 64, Screen.height - (int)pos.y - 64, 128, 128), info.Text);
			}
		}		
		
		GUI.skin.label.normal.textColor = prevColor;
		GUI.skin.label.alignment = prevAnchor;
		GUI.skin.label.fontSize = prevSize;
	}
}
