using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice6 : MonoBehaviour {
    [SerializeField] int minForce;
    [SerializeField] int maxForce;
    [SerializeField] int Rotation;
    private bool rotating = false;

    private int nr = 3;
    

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        Rigidbody r = GetComponent<Rigidbody>();

        if (rotating && r.IsSleeping())
        {
            Debug.ClearDeveloperConsole();

            if (Vector3.Dot(Vector3.up, transform.up) > 0.9)
                nr = 4;
            else if (Vector3.Dot(Vector3.up, -transform.up) > 0.9)
                nr = 2;
            else if (Vector3.Dot(Vector3.up, transform.right) > 0.9)
                nr = 5;
            else if (Vector3.Dot(Vector3.up, -transform.right) > 0.9)
                nr = 6;
            else if (Vector3.Dot(Vector3.up, transform.forward) > 0.9)
                nr = 3;
            else if (Vector3.Dot(Vector3.up, -transform.forward) > 0.9)
                nr = 1;
            else
                nr = 0;
            

            rotating = false;
            Debug.Log(nr);
        }

        
       

	}

    private void OnMouseDown()
    {
        int min = minForce;
        int max = maxForce;
        int r = Rotation;

        Rigidbody rbody = GetComponent<Rigidbody>();
        rbody.AddForce(Vector3.up * Random.Range(min,max), ForceMode.Impulse);
        rbody.AddTorque(new Vector3(Random.Range(-r, r), Random.Range(-r, r), Random.Range(-r, r)));

        rotating = true;
        
        
    }


}
