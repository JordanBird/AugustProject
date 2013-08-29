using UnityEngine;
using System.Collections;

public class AeroplaneController : MonoBehaviour
{
	public Transform SkinPrefab;
	private Transform skin;
	private float tilt;
	
	// Use this for initialization
	void Start ()
	{
		skin = (Transform)Instantiate (SkinPrefab, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
	
	void FixedUpdate()
	{
		/* DEBUG BECAUSE YOU CANT TEST ON PC WITH TILT!		
		if (Input.GetKey (KeyCode.D))
		{
			tilt = Mathf.Lerp (tilt, -1, 0.1f);
		}
		
		if (Input.GetKey(KeyCode.A))
		{
			tilt = Mathf.Lerp (tilt, 1, 0.1f);
		}*/
		
		// Comment out if iphone not present.
		tilt = iPhoneInput.acceleration.y;
		tilt *= Time.fixedDeltaTime;
		
		skin.position = transform.position;
		skin.rotation = Quaternion.Lerp (skin.rotation, Quaternion.Euler(new Vector3(0, 0,  90 + Mathf.Clamp (60 * tilt, -30, 30))), 0.1f);	// yeah....you cant rotate children so i had to make the part which rotates seperately. Go Unity!....¬___¬
		
		rigidbody.velocity = Vector3.zero;
		
		rigidbody.AddForce(Vector3.up * tilt * 400);
	}
}
