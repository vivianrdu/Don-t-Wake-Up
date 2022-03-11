using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetCollider : MonoBehaviour
{
	// Returns whether the obj is touching a surface
	bool isObject(GameObject obj)
	{
        Debug.Log("object layer " + obj.layer.ToString());
		return obj.layer == 0;
	}

	// use coll.gameObject if you need a reference coll's GameObject
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (isObject(coll.gameObject))
		{
			GetComponentInParent<Player>().feetContact = true;
		}
	}

	void OnCollisionExit2D(Collision2D coll)
	{
		GetComponentInParent<Player>().feetContact = false;
	}
}
