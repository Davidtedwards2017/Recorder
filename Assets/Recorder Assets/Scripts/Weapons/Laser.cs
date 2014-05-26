using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

    public int NumberOfBounces = 2;
    public float EffectLifeTime;

   
    public LineRenderer lr;
    // Use this for initialization
	void Start () 
    {
	
        lr = gameObject.AddComponent<LineRenderer>();
        
        lr.SetVertexCount(NumberOfBounces + 1);
        lr.SetWidth(1,1);
        lr.SetColors(Color.red, Color.red);

	}
	
	// Update is called once per frame
	void Update () {
	
        ArrayList points = new ArrayList();
        
        Vector3 direction = transform.forward;
        Vector3 position = transform.position;
        
        points.Add(position);
        
        RaycastHit hit;
        Ray ray;

        for( int i = 0; i < NumberOfBounces; i++)
        {
            ray = new Ray(position, direction);

            if (Physics.Raycast(ray, out hit)) 
            {
                
                Vector3 incomingVec = hit.point - position;

                points.Add(hit.point);
                position = hit.point;
                direction = Vector3.Reflect(incomingVec, hit.normal);
            }
        }
        
        for(int i = 0; i < points.Count; i++)
        {
            lr.SetPosition(i,(Vector3) points[i]);
        }
	}
}
