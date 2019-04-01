using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class Card : MonoBehaviour
{
    [SerializeField]
    Player player;


    [SerializeField]
    Transform target;

    [SerializeField]
    float moveSpeed;


    [SerializeField]
    float rotateSpeed;

    [SerializeField]
    Transform button;

    bool go = false;

    public bool Clickable { get; set; }  //defaluts to false

    Vector3 startPos;
    Quaternion startRot;

    List<Transform> buttons = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;

    }

  

    public void CreateButons(AlternativeInfo[] alt)
    {
        float dx = 0.6f;

        for (int i = 0; i < alt.Length; i++)
        {
            Transform btn = Instantiate(button);
            btn.position = transform.position + new Vector3(0f, -0.01f, dx);
            btn.rotation = transform.rotation;
            btn.localScale = new Vector3(1.5f, 0.16f, 0.05f);
            btn.GetComponent<BtnClick>().Alt = alt[i];
            
            Transform text3d = btn.GetChild(0);
            text3d.GetComponent<TextMesh>().text = TextFormatter.RowLenght(alt[i].alt,60);
          
            btn.SetParent(transform);
            buttons.Add(btn);
            dx += 0.2f;
        }

        
    }

    private void RemoveAllButtons()
    {
        foreach (Transform b in buttons)
            Destroy(b.gameObject);

        buttons.Clear();        
    }


    public void startAnimation()
    {
        //print("startAnim");
        go = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (go)
            animate();
    }

    void animate()
    {
        //calculate rotation and movement
        Quaternion rot = Quaternion.RotateTowards(transform.rotation, target.rotation, rotateSpeed);
        float step = moveSpeed * Time.deltaTime;
        Vector3 mov = Vector3.MoveTowards(transform.position, target.position, step);

        //rotate and move
        transform.rotation = rot;
        transform.position = mov;

        //are we done?
        float rotDiff = Quaternion.Dot(transform.rotation, target.rotation);
        float distanceToGo = Vector3.Magnitude(target.position - mov);
        if (rotDiff > 0.99999 && distanceToGo < 0.005)
        {
            go = false;
            Clickable = true;
        }

    }

    public void ButtonPressed()
    {

        if (Clickable)
        {
            //return to startpoint
            transform.position = startPos;
            transform.rotation = startRot;

            //destroy alternative buttons
            RemoveAllButtons();

            go = false;
            Clickable = false;
            player.enableDice();
        }
    }




}
