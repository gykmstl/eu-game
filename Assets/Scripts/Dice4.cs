using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice4 : MonoBehaviour {
    [SerializeField] int minForce;
    [SerializeField] int maxForce;
    [SerializeField] int Rotation;

    [SerializeField] Player player;


    private bool rotating = false;
    private int nr = 3;

    public bool Rollable { get; set; }  //defaluts to false
    

	// Use this for initialization
	void Start ()
    {
       
	}

    
	
	// Update is called once per frame
	void Update () {

        Rigidbody r = GetComponent<Rigidbody>();

        if (rotating && r.IsSleeping())
        {
            nr = 0;
     

            double yMax = 0;
            foreach (Transform child in transform)
            {
                if (child.position.y > yMax)
                {
                    yMax = child.position.y;
                    nr = int.Parse(child.name);
                }
              
            }

            rotating = false;
            Rollable = false;

            player.startJumping(nr);
        }

	}

    private void OnMouseDown()
    {
        if(Rollable)
            throwDice();
    }

    private void OnCollisionEnter(Collision collision)
    {
      
        
    }


    public void throwDice()
    {
        int min = minForce;
        int max = maxForce;
        int r = Rotation;

        Rigidbody rbody = GetComponent<Rigidbody>();
        rbody.AddForce(Vector3.up * Random.Range(min, max), ForceMode.Impulse);
        rbody.AddTorque(new Vector3(Random.Range(-r, r), Random.Range(-r, r), Random.Range(-r, r)));

        rotating = true;
    }


}
