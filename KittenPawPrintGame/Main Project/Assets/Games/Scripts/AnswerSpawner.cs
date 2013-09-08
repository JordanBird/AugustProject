using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnswerSpawner : MonoBehaviour
{
	public Transform AnswerPrefab;
	public float SpawnDelay;
	public float TransitionDelay;
	
	private float spawnTimer = 0;
	private float transitionTimer = 0;
	private bool transitioning = true;
	private Question currentQuestion;
	private List<Answer> answerBatch = new List<Answer>();
	private List<Answer> correctAnswers = new List<Answer>();
	private List<Transform> answerObjects = new List<Transform>();
	
	private Game game;
	
	void Start ()
	{
		//DEBUG
		currentQuestion = new Question("DEBUGDEBUGDEBUG?",
			new Answer[] { new Answer(false, "false0"), new Answer(false, "false1"), new Answer(true, "true0"), new Answer(true, "true1"), new Answer(true, "true2") });
	}
	
	void Update ()
	{
		if (transitioning)
		{
			// Display question text of next question until transition has finished? Darken screen during this to divert attention from the game towards the text?
			
			transitionTimer += Time.deltaTime;
			if (transitionTimer >= TransitionDelay)
			{
				transitionTimer = 0;
				
				transitioning = false;
			}
		}
		else
		{
			spawnTimer += Time.deltaTime;
			if (spawnTimer >= SpawnDelay)
			{
				spawnTimer %= SpawnDelay;
				
				SpawnNextAnswer();
			}
		}
		
		for (int i = 0; i < answerObjects.Count; i++)
		{
			answerObjects[i].position += new Vector3(-0.1f, 0, 0);
		}
		
	}
	
	private void SpawnNextAnswer()
	{		
		if (answerBatch.Count == 0)
		{
			// If the number of correct answers is equal to the correct answers collected, the player can proceed to the next question.
			if (currentQuestion == null || currentQuestion.GetNumberOfCorrectAnswers() == correctAnswers.Count)
			{
				LoadNextQuestion();
				transitioning = true;
				spawnTimer = 0;
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
		AnswerPrefab.position = new Vector3(10, Random.Range (-5, 5), 0);
		AnswerPrefab.GetComponent<TextMesh>().text = answerBatch[answer].text;
		answerObjects.Add ((Transform)Instantiate(AnswerPrefab));
		
		// Remove the answer so it isn't used again until all others are used.
		answerBatch.RemoveAt (answer);
	}
	
	private void LoadNextQuestion()
	{
		currentQuestion = game.GetRandomQuestion ();
	}
}
