using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnswerSpawner : MonoBehaviour
{
	public Transform AnswerPrefab;
	public float SpawnDelay;
	public bool Running { get; private set; }
	
	private float spawnTimer = 0;
	private List<Answer> answerBatch = new List<Answer>();
	private List<Answer> correctAnswers = new List<Answer>();
	private List<Transform> answerObjects = new List<Transform>();
	private cscript_plane_game controller;
	
	private Question currentQuestion { get { return controller.currentQuestion; } }
	
	void Start ()
	{
	}
	
	public void Begin(cscript_plane_game controller)
	{
		this.controller = controller;
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
				answerObjects[i].position += new Vector3(-0.1f, 0, 0);
				
				if (answerObjects[i].position.x < -15)
				{
					Destroy(answerObjects[i].gameObject);
					answerObjects.RemoveAt(i);
					i--;
				}
			}
		}
	}
	
	private void SpawnNextAnswer()
	{		
		if (answerBatch.Count == 0)
		{
			// If the number of correct answers is equal to the correct answers collected, the player has won.
			if (currentQuestion == null || currentQuestion.GetNumberOfCorrectAnswers() == correctAnswers.Count)
			{
				Running = false;
				controller.WinGame();
				return;
			}
			
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
		AnswerPrefab.position = new Vector3(10, Random.Range (-3, 3), 0);
		AnswerPrefab.GetComponent<TextMesh>().text = answerBatch[answer].text;
		answerObjects.Add ((Transform)Instantiate(AnswerPrefab));
		
		// Remove the answer so it isn't used again until all others are used.
		answerBatch.RemoveAt (answer);
	}
	
	public void CheckAnswer(GameObject answerObject)
	{
		foreach (Answer a in answerBatch)
		{
			// Find the answer.
			if (a.text == answerObject.GetComponent<TextMesh>().text)
			{
				if (a.correct)
				{
					correctAnswers.Add (a);	
				}
				else
				{
					//INCORRECT ANSWER, DO SOMETHING	
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
	}
}
