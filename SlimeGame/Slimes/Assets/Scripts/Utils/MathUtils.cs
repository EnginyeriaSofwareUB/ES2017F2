using System;
using UnityEngine;

public class MathUtils
{

	public static float Vect3ScalarProduct (Vector3 u, Vector3 w){
		return (u.x * w.x + u.y * w.y);
	}

}