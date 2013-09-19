using UnityEngine;
using System.Collections;

public class AeroplaneController : MonoBehaviour
{
	public Transform AnswerSpawner;
	public Transform SkinPrefab;
	public Transform skin;
	private float tilt;
	
	// Use this for initialization
	void Start ()
	{
		skin = (Transform)Instantiate (SkinPrefab, transform.position, transform.rotation);
	}
	
	public void Reset()
	{
		tilt = 0;
		transform.position = new Vector3(-5, 1, 0);
		skin.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
	}
	
	void OnTriggerEnter(Collider other)
	{
		AnswerSpawner.GetComponent<AnswerSpawner>().CheckAnswer(other.gameObject);
	}
	
	void Update()
	{
		rigidbody.WakeUp();
		
		if (AnswerSpawner.GetComponent<AnswerSpawner>().Running)
		{
			/* DEBUG BECAUSE YOU CANT TEST ON PC WITH TILT!		
			if (Input.GetKey (KeyCode.D))
			{
				tilt = Mathf.Lerp (tilt, -1, 0.1f);
			}
			
			if (Input.GetKey(KeyCode.A))
			{
				tilt = Mathf.Lerp (tilt, 1, 0.1f);
			}
			
			if (!Input.GetKey (KeyCode.D) && !Input.GetKey (KeyCode.A))
			{
				tilt = Mathf.Lerp (tilt, 0, 0.2f);	
			}*/
			
			// Comment out if iphone not present.
			Vector3 acceleration = Input.acceleration;
			acceleration.Normalize ();
			tilt = acceleration.x;
			
			skin.position = transform.position;
			skin.rotation = Quaternion.Lerp (skin.rotation, Quaternion.Euler(new Vector3(0, 0,  90 + Mathf.Clamp (30 * tilt, -30, 30))), 0.1f);
			
			rigidbody.velocity = Vector3.up * tilt * Time.deltaTime * 400;
			
		}
	}
}
