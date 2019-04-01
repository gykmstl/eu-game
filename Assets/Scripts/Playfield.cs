using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield: MonoBehaviour {

    [SerializeField]
    GameObject startPoint;
    
    /*[SerializeField] GameObject end;*/

    [SerializeField]
    Material[] PathsMaterial;

    

    private List<GameObject> dots = new List<GameObject>();


	// Use this for initialization
	void Start () {

        //find all dots
        dots.Add(startPoint);
        dots.Add(FindClosestDot(startPoint));
        int i = GameObject.FindGameObjectsWithTag("dot").Length;
        while(i-->0)
           dots.Add(FindClosestDot(dots[dots.Count - 1]));


        //mark paths
        for (int p = 0; p < dots.Count-1; p++)
        {
            MarkPath(dots[p], dots[p + 1]);
        }

    }

    //get dot/point
    public GameObject getPoint(int index)
    {
        try
        {
            return dots[index];
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            return null;
        }
    }


    //draw a "line" between points
    void MarkPath(GameObject from, GameObject to)
    {
        //calculate midpoint
        Vector3 sToe = to.transform.position - from.transform.position;
        Vector3 pos = from.transform.position + (sToe * 0.5f);

        //create and place cube (with material)
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(0.2f, 0.1f, sToe.magnitude);
        cube.transform.position = pos;
        cube.GetComponent<Renderer>().materials = PathsMaterial;
  
        //rotate cube
        Quaternion rotation = Quaternion.LookRotation(sToe);
        cube.transform.rotation = rotation;
    }


    GameObject FindClosestDot(GameObject from)
    {
        //find dot gameobjekct
        GameObject[] gos = GameObject.FindGameObjectsWithTag("dot");

        //start pos
        Vector3 position = from.transform.position;

        //find closest
        GameObject closest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;  
            }
        }

        //remove tag
        closest.transform.tag = "Untagged";

        //return closest
        return closest;
    }


}
