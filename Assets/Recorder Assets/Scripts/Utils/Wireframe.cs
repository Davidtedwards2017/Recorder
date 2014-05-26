using UnityEngine;
using System.Collections;
using Vectrosity;

public class Wireframe : MonoBehaviour {

    public Vector3[] Points;
    VectorLine vecterLine;
	// Use this for initialization
	void Start () {

        Mesh mesh = GetComponent<MeshFilter>().mesh;

        vecterLine = new VectorLine("lines", Points, GetComponent<Renderer>().material, 1, LineType.Discrete);
        vecterLine.MakeWireframe(mesh);
        vecterLine.Draw3DAuto();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
