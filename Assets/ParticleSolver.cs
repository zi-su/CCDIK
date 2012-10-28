using UnityEngine;
using System.Collections;

public class ParticleSolver : MonoBehaviour {
	
	public GameObject constraint;	
	public GameObject skeletonList;
	public GameObject[] Bone;
	Transform[] children;
	int particleNum;
	public int TestNum;
	
	private Vector3 mouseOldPos;
	private Vector3 gizmoLine;
	
	private bool IsSolveIK{
		set;
		get;
	}
	
	// Use this for initialization
	void Start () {
		gizmoLine = new Vector3();
		mouseOldPos = Vector3.zero;
		children = skeletonList.transform.GetComponentsInChildren<Transform>();
		particleNum = 0;
		for(int i = 0 ; i < children.Length ; i++){
			if(children[i].tag != "Bone"){
				continue;
			}
			particleNum++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		MoveConstraint();
		SolveParticleIK();
		mouseOldPos = Input.mousePosition;
	}
	
	void SolveParticleIK(){
		Vector3[] joint = new Vector3[particleNum];
		for(int i = 0 , j = 0; i < children.Length ; i++){
			if(children[i].tag != "Bone"){
				continue;
			}
			joint[j++] = children[i].position;
		}
		
		Vector3[] particle = new Vector3[particleNum];
		for(int i = 0 , j = 0; i < children.Length ; i++){
			if(children[i].tag != "Bone"){
				continue;
			}
			 particle[j++] = children[i].position;
		}

		Vector3 rootPos =  skeletonList.transform.position;
		for(int num = 0 ; num < TestNum ; num++){
			particle[particleNum - 1] = constraint.transform.position;
			for(int i = particleNum - 1 ; i >= 1 ; --i){
				Vector3 dv = particle[i - 1] - particle[i];
				
				float dv_scale = (dv.magnitude - (joint[i-1] - joint[i]).magnitude) / dv.magnitude * 0.5f;
				dv *= dv_scale;
			
				particle[i - 1] -= dv;
				particle[i] += dv;
			}
			particle[0] = rootPos;
		}
		
		FitSkeletonToParticle(particle);
		GameObject[] display_particles = GameObject.FindGameObjectsWithTag("Particle");
		for(int i = 0 ; i < display_particles.Length ; i++){
			display_particles[i].transform.position = particle[i];
		}
	}
	
	void FitSkeletonToParticle(Vector3[] particle){
		Transform[] joint = new Transform[particleNum];
		for(int i = 0 ,j = 0; i < children.Length ;i++){
			if(children[i].tag != "Bone"){
				continue;
			}
			joint[j++] = children[i].transform;
		}
		
		for(int jid = 0 ; jid < joint.Length - 1 ; jid++){
			Quaternion qd = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
			
			Vector3 childDir = joint[jid+1].transform.position - joint[jid].transform.position;
			childDir = childDir.normalized;
			
			Vector3 particleDir = particle[jid+1] - joint[jid].transform.position;
			particleDir = particleDir.normalized;
			
			float dot = Vector3.Dot (childDir, particleDir);
			if(dot > 1.0f - 1.0e-6f){
				qd = Quaternion.identity;
			}
			else{
				qd = Quaternion.FromToRotation(childDir, particleDir);
				joint[jid].transform.rotation = qd * joint[jid].transform.rotation;
			}
		}
	}
	
	void MoveConstraint(){
		Vector3 mousePos = Input.mousePosition;
		Vector3 worldPos = Camera.mainCamera.WorldToScreenPoint(constraint.transform.position);
		Vector3 screenPos = Camera.mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, worldPos.z));
		constraint.transform.position = screenPos;
	}
}
