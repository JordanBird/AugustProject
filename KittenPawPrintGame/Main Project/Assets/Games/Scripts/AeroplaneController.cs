using UnityEngine;
using System.Collections;

public class AeroplaneController : MonoBehaviour
{
	public bool Controllable;
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
		Controllable = true;
	}
	
	void OnTriggerEnter(Collider other)
	{
		AnswerSpawner.GetComponent<AnswerSpawner>().CheckAnswer(other.gameObject);
	}
	
	void Update()
	{		
		if (Controllable)
		{
			rigidbody.WakeUp();
			
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
		}
		else
		{
			tilt = Mathf.Lerp (tilt, 0, 0.2f);		
		}
		
		skin.position = transform.position;
		skin.rotation = Quaternion.Lerp (skin.rotation, Quaternion.Euler(new Vector3(0, 0,  90 + Mathf.Clamp (30 * tilt, -30, 30))), 0.1f);
		
		if (Controllable)
		{
			rigidbody.velocity = Vector3.up * tilt * Time.deltaTime * 400;
		}
		else
		{
			rigidbody.velocity = Vector3.zero;	
		}
	}
}
