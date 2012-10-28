using UnityEngine;
using System.Collections;

public class CCDSolver : MonoBehaviour {
	
	public GameObject target;
	public GameObject skeletonList;
	public int numIteratrions;
	public GameObject effector;
	public float speed;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Solver();
		MoveTarget();
	}
	
	void MoveTarget(){
		Vector3 mousePos = Input.mousePosition;
		Vector3 worldPos = Camera.mainCamera.WorldToScreenPoint(target.transform.position);
		Vector3 screenPos = Camera.mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, worldPos.z));
		target.transform.position = screenPos;
	}
	void Solver(){
		Transform[] childlen = skeletonList.transform.GetComponentsInChildren<Transform>();
		for(int i = 0 ; i < numIteratrions ; i++){
			for(int j = childlen.Length - 1 ; j >= 0 ; j--){
				Transform child = childlen[j];
				Vector3 effectorDir = Vector3.zero;
				Vector3 targetDir = Vector3.zero;
				effectorDir = ( effector.transform.position - child.position).normalized;
				targetDir = (target.transform.position - child.position).normalized;
				
				float dot = Vector3.Dot (effectorDir, targetDir);
				
				if(dot < 1.0f - 1.0e-6f){
					float rotAngle = Mathf.Acos(dot);
					Vector3 rotAxis = Vector3.Cross(effectorDir, targetDir).normalized;					
					if(child.tag == "Bone"){
						child.rotation = (Quaternion.AngleAxis(rotAngle, rotAxis)) * child.rotation;
					}
				}
			}
		}
	}
}
