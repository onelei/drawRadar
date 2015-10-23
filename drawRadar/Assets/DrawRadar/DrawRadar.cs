using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawRadar : MonoBehaviour 
{
	//外围线圈
	LineRenderer _LineRenderBig;
	//内围线圈
	LineRenderer _LineRenderSmall;
	public Material _LineMat;
	public Material MeshMat;


	MeshFilter _Mesh;
	
	const float Radius = 60.0f;
	const float PI = 3.14f;

	public const int PointCout = 6;
	public  List<Vector3>PointsPos  = new List<Vector3>();

	Vector3[] newVertices;
	Vector2[] newUV;
	int[] newTriangles;

	List<float> _SliderValue = new List<float>();

	// Use this for initialization
	void Start () 
	{
		GameObject go = new GameObject("_LineRenderBig");
		go.transform.parent = gameObject.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = Vector3.one;
		go.layer = go.transform.parent.gameObject.layer;
		_LineRenderBig = go.AddComponent<LineRenderer>();
		_LineRenderBig.material = _LineMat;
		_LineRenderBig.SetVertexCount(PointCout);
		
		
		go = new GameObject("_LineRenderSmall");
		go.transform.parent = gameObject.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = Vector3.one;
		go.layer = go.transform.parent.gameObject.layer;

		_LineRenderSmall = go.AddComponent<LineRenderer>();
		_LineRenderSmall.material = _LineMat;
		_LineRenderSmall.SetVertexCount(PointCout);

		//================Coordinate=====================

//			new Vector3(transform.position.x + max/2,  transform.position.y + Mathf.Sqrt(3) * max/2, transform.position.z + 1.0f),
//			new Vector3(transform.position.x + max, transform.position.y                     , transform.position.z + 1.0f),
//			new Vector3(transform.position.x + max/2,  transform.position.y - Mathf.Sqrt(3) * max/2, transform.position.z + 1.0f),
//			new Vector3(transform.position.x - max/2,  transform.position.y - Mathf.Sqrt(3) * max/2, transform.position.z + 1.0f),
//			new Vector3(transform.position.x - max, transform.position.y                     , transform.position.z + 1.0f),
//			new Vector3(transform.position.x - max/2,  transform.position.y + Mathf.Sqrt(3) * max/2, transform.position.z + 1.0f),
//			new Vector3(transform.position.x + max/2,  transform.position.y + Mathf.Sqrt(3) * max/2, transform.position.z + 1.0f),
		PointsPos.Clear();
		PointsPos.Add(new Vector3(transform.position.x ,  transform.position.y + Radius, transform.position.z + 1.0f));
		PointsPos.Add(new Vector3(transform.position.x + Radius*Mathf.Cos((float)((90-360/5)*PI/180)), transform.position.y +Radius*Mathf.Sin((float)((90-360/5)*PI/180)), transform.position.z + 1.0f));
		PointsPos.Add(new Vector3(transform.position.x + Radius*Mathf.Sin((float)((360/5/2)*PI/180)),  transform.position.y - Radius*Mathf.Cos((float)((360/5/2)*PI/180)), transform.position.z + 1.0f));
		PointsPos.Add(new Vector3(transform.position.x - Radius*Mathf.Sin((float)((360/5/2)*PI/180)),  transform.position.y - Radius*Mathf.Cos((float)((360/5/2)*PI/180)), transform.position.z + 1.0f));
		PointsPos.Add(new Vector3(transform.position.x - Radius*Mathf.Cos((float)((90-360/5)*PI/180)), transform.position.y+Radius*Mathf.Sin((float)((90-360/5)*PI/180)), transform.position.z + 1.0f));
		PointsPos.Add(new Vector3(transform.position.x ,  transform.position.y + Radius, transform.position.z + 1.0f));
		//================Coordinate=====================


		//==============init vec points=========================
		_SliderValue.Clear();
		_SliderValue.Add(Radius);
		_SliderValue.Add(Radius);
		_SliderValue.Add(Radius);
		_SliderValue.Add(Radius);
		_SliderValue.Add(Radius);
		//==============over vec points=========================

		ChangeMesh ();
		
		GameObject _MeshObj = new GameObject("_MeshObj");
		_MeshObj.transform.parent = gameObject.transform;
		_MeshObj.transform.localPosition = Vector3.zero;
		_MeshObj.transform.localScale = Vector3.one;
		_MeshObj.layer = _MeshObj.transform.parent.gameObject.layer;

		_Mesh = _MeshObj.AddComponent<MeshFilter>();
		_Mesh.mesh = new Mesh();
		
		ApplyMesh ();
		MeshRenderer _render = _MeshObj.AddComponent<MeshRenderer>();
		_render.material = MeshMat;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		for(int i = 0; i < PointCout; i++)
		{
			_LineRenderBig.SetPosition(i, PointsPos[i]);
		}
		for(int i = 0; i < PointCout; i++)
		{			
			_LineRenderSmall.SetPosition(i, new Vector3(PointsPos[i].x * _SliderValue[i%5] / Radius,  PointsPos[i].y * _SliderValue[i%5] / Radius, PointsPos[i].z ));
		}
		ChangeMesh();
		ApplyMesh();
	}


	void OnGUI()
	{
		for(int i = 0; i < 5; i++)
		{
			_SliderValue[i] = GUI.HorizontalSlider(new Rect (25, 25 + i * 30, 100, 30), _SliderValue[i], 0.0f, Radius);		
		}		
	}
	
	void ChangeMesh ()
	{
		// ==========new Coordinate ================
		newVertices = new Vector3[]
		{
			PointsPos[0] - new Vector3(0,Radius,0),
			new Vector3(PointsPos[0].x * _SliderValue[0] / Radius,  PointsPos[0].y * _SliderValue[0] / Radius, PointsPos[0].z ),
			new Vector3(PointsPos[1].x * _SliderValue[1] / Radius,  PointsPos[1].y * _SliderValue[1] / Radius, PointsPos[1].z ),
			new Vector3(PointsPos[2].x * _SliderValue[2] / Radius,  PointsPos[2].y * _SliderValue[2] / Radius, PointsPos[2].z ),
			new Vector3(PointsPos[3].x * _SliderValue[3] / Radius,  PointsPos[3].y * _SliderValue[3] / Radius, PointsPos[3].z ),
			new Vector3(PointsPos[4].x * _SliderValue[4] / Radius,  PointsPos[4].y * _SliderValue[4] / Radius, PointsPos[4].z )
		};

		// ==========new Coordinate ================


		// center value;
		newUV = new Vector2[]
		{
			new Vector2(transform.position.x, PointsPos[0].z ),
			new Vector2(PointsPos[0].x * _SliderValue[0] / Radius, PointsPos[0].z ),
			new Vector2(PointsPos[1].x * _SliderValue[1] / Radius, PointsPos[1].z ),
			new Vector2(PointsPos[2].x * _SliderValue[2] / Radius, PointsPos[2].z ),
			new Vector3(PointsPos[3].x * _SliderValue[3] / Radius, PointsPos[3].z ),
			new Vector3(PointsPos[4].x * _SliderValue[4] / Radius, PointsPos[4].z )
		};


		newTriangles = new int[]
		{
			0,
			1,
			2,
			
			0,
			2,
			3,
			
			0,
			3,
			4,

			0,
			4,
			5,

			0,
			5,
			1
		};
	}
	
	void ApplyMesh ()
	{
		_Mesh.mesh.vertices = newVertices;
		_Mesh.mesh.uv = newUV;
		_Mesh.mesh.triangles = newTriangles;
	}
}
