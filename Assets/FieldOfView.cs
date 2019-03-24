using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {
	public float viewRad;
	[Range(0,360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	[HideInInspector]
	public List<Transform> visibleTargets=new List<Transform>();

	public float meshResolution;
	public MeshFilter viewMeshFilter;
	Mesh viewMesh;

	public bool playerInVision;
	private EnemyAI ai;



	void DrawFieldOfView(){
		int stepCount = Mathf.RoundToInt (viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;
		List<Vector3> viewPoints=new List<Vector3>();
		for(int i=0; i<=stepCount;i++){
			float angle=transform.eulerAngles.y-viewAngle/2+stepAngleSize*i;
			//Debug.DrawLine(transform.position,transform.position+DirFromAngle(angle,true)*viewRad,Color.green);
			ViewCastInfo newViewCast= ViewCast(transform.position,angle,viewRad,obstacleMask,transform.rotation.eulerAngles,0f);
			viewPoints.Add (newViewCast.point);
		}
		int vertexCount=viewPoints.Count+1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles=new int[(vertexCount-2)*3];

		vertices[0]=Vector3.zero;
		for(int i=0;i<vertexCount-1;i++){
			vertices[i+1]=transform.InverseTransformPoint(viewPoints[i]);
			if(i<vertexCount-2){
				triangles[i*3]=0;
				triangles[i*3+1]=i+1;
				triangles[i*3+2]=i+2;
			}
		}

		viewMesh.Clear ();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals ();
	}

	public static ViewCastInfo ViewCast(Vector3 pos,float globalAngle,float viewRad,LayerMask obstacleMask,Vector3 rot,float buffer){
		Vector3 dir = DirFromAngle (globalAngle, true,rot);
		RaycastHit hit;
		if (Physics.Raycast (pos, dir, out hit, viewRad, obstacleMask)) {
			Vector3 hitPoint=pos+dir*(hit.distance-buffer);

			return new ViewCastInfo (true, hitPoint, hit.distance-buffer, globalAngle);
		} else {
			return new ViewCastInfo(false,pos+dir*viewRad,viewRad,globalAngle);
		}
	}

	public struct ViewCastInfo{
		public bool hit;
		public Vector3 point;
		public float dist;
		public float angle;
		public ViewCastInfo(bool _hit, Vector3 _point,float _dist, float _angle){
			hit=_hit;
			point=_point;
			dist=_dist;
			angle=_angle;
		}
	}

	void Start(){
		viewMesh = new Mesh ();
		viewMesh.name="View Mesh";
		viewMeshFilter.mesh = viewMesh;
		ai = transform.GetComponent<EnemyAI> ();
		StartCoroutine ("FindTargetsWithDelay", 0.2);
	}

	IEnumerator FindTargetsWithDelay(float delay){
		while (true) {
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}

	void FindVisibleTargets(){
		playerInVision = false;
		visibleTargets.Clear ();
		Collider[] targetsInViewRad = Physics.OverlapSphere (transform.position, viewRad, targetMask);

		for (int i=0; i<targetsInViewRad.Length; i++) {
			Transform target=targetsInViewRad[i].transform;
			Vector3 dirToTarget=(target.position-transform.position).normalized;
			if(Vector3.Angle(transform.forward,dirToTarget)<viewAngle/2){
				float distToTarget=Vector3.Distance(transform.position,target.position);
				if(!Physics.Raycast(transform.position,dirToTarget,distToTarget,obstacleMask)){
					visibleTargets.Add (target);
					if(target.gameObject.tag=="Player" && !target.GetComponent<PlayerMovement>().hidden){playerInVision=true;}
				}
			}
		}
	}

	public static Vector3 DirFromAngle(float degAngle,bool angleIsGlobal,Vector3 rot){
		if (!angleIsGlobal) {
			degAngle+=rot.y;
		}
		return new Vector3 (Mathf.Sin (degAngle * Mathf.Deg2Rad), 0, Mathf.Cos (degAngle * Mathf.Deg2Rad));
	}

	// Update is called once per frame
	void LateUpdate () {
		DrawFieldOfView();
		if (playerInVision) {
			ai.seenPlayer = true;
		} else {
			ai.seenPlayer=false;
		}
	}
}
