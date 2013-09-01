using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnswerSpawner : MonoBehaviour
{
	public Transform AnswerPrefab;
	public float SpawnDelay;
	public float NextQuestionDelay;
	
	private float spawnTimer = 0;
	private float nextQuestionTimer = 0;	
	private int lastAnswerSpawned = 0;
	private Question currentQuestion;
	private List<Answer> answerBatch = new List<Answer>();
	
	void Start ()
	{
		
	}
	
	void Update ()
	{
		spawnTimer += Time.deltaTime;
		if (spawnTimer >= SpawnDelay)
		{
			spawnTimer %= SpawnDelay;
			
			SpawnNextAnswer();
		}
		
		/*
		// -1 is being used as a flag to start and stop the timer.
		if (nextQuestionTimer != -1)
		{
			nextQuestionTimer += Time.deltaTime;
			if (nextQuestionTimer >= NextQuestionDelay)
			{
				nextQuestionTimer = -1;
				
				//currentQuestion = however we'll get the next question.
			}
		}*/
	}
	
	private void SpawnNextAnswer()
	{		
		if (answerBatch.Count == 0)
		{
			answerBatch.AddRange(currentQuestion.answers);	// Refresh answers once all have been spawned.
		}
		
		int answer = Random.Range(0, answerBatch.Count);
		
		//AnswerPreab.Text = answerBatch[answer].Text;
		Instantiate(AnswerPrefab);
		
		answerBatch.RemoveAt (answer);	// Remove the answer so it isn't used again until all others are used.
	}
	
	/*public void EndCurrentQuestion()
	{
		nextQuestionTimer = 0;
	}*/
}
