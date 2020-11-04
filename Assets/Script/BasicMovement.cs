using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicMovement : MonoBehaviour
{

	private Rigidbody2D objectRigidbody;
	public float speed;

	
	void Start()
	{
		objectRigidbody = transform.GetComponent<Rigidbody2D>();
		
		objectRigidbody.velocity = transform.up * speed; // Enemy are rotated at 180
		//print(objectRigidbody.velocity);
	}
}

