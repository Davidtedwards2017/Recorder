using UnityEngine;
using System.Collections;
using Vectrosity;

public class Border : MonoBehaviour {

    public Material borderMaterial;
    public Vector3[] points;
    public float Width;
    private VectorLine vecterline;


	// Use this for initialization
	void Start () {

        vecterline = new VectorLine(name, points, borderMaterial, Width,LineType.Discrete);
        vecterline.MakeWireframe(GetComponent<MeshFilter>().mesh);
	}
	
	// Update is called once per frame
	void Update () {
	
        vecterline.Draw3D();
	}
}
