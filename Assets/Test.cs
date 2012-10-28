using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
	
	public GameObject sphere;
	public GameObject sphere2;
	public GameObject sphere3;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 childdir  = sphere2.transform.position - sphere.transform.position;
		Vector3 pdir = sphere3.transform.position - sphere.transform.position;
		
		Vector3 cross = Vector3.Cross (childdir, pdir).normalized;
		float degree = Mathf.Acos(Vector3.Dot(childdir, pdir));
		sphere.transform.rotation = Quaternion.AngleAxis(degree, cross) * sphere.transform.rotation;
	}
}
