using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : MonoBehaviour
{
    [SerializeField]
    Playfield playField;

    [SerializeField]
    Dice4 Dice;

    [SerializeField]
    Card cardToTurn;

    [SerializeField]
    TextMesh cardText;

    [SerializeField]
    float jumpAngle = 60f;

    int playerPosition = 0;
    int steps = -1;

    //radius of collider
    float sphereColliderRadius = 0;

    // GameObject lastHit;
    int lastStep;

    //state
    public enum PlayerState { diceroll, jump, drawcard };
    public PlayerState State { get; private set; }

    void Start()
    {
        print(PlayerStorage.p_name);

        //set lastHit to startpoint
        //lastHit = playField.getPoint(playerIndex);
        enableDice();

        //dimension of object
        sphereColliderRadius = GetComponent<SphereCollider>().radius; ;

    }

    public void startJumping(int steps)
    {
        StartCoroutine(Database.GetQuestion(PlayerStorage.p_id, PlayerStorage.lang_code, 1, (QuestionInfo qi) =>
        {
            if (qi.ok)
            {
                PlayerStorage.q_id = qi.q_id; // QuestionID    

                string questionText = qi.intro + "\n" + qi.q;
                questionText = questionText.Replace("<strong>", "");
                questionText = questionText.Replace("</strong>", "");
                cardText.text = TextFormatter.RowLenght(questionText, 70);

                cardToTurn.CreateButons(qi.altis);
            }
        }));

        State = PlayerState.jump;
        this.steps = steps;
        jump(playField.getPoint(++playerPosition));
    }

    public void enableDice()
    {
        //set state to roll dice and enable the dice
        State = PlayerState.diceroll;
        Dice.Rollable = true;
    }

    void jump(GameObject targ)
    {
        if (targ == null)
        {
            Debug.Log("Not jumping - no target point found.");
            return;
        }

        var rigid = GetComponent<Rigidbody>();
        Vector3 p = targ.transform.position;

        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = jumpAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion) + sphereColliderRadius / 2;

        // Distance along the y axis between objects
        float yOffset = transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects   
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (p.x > transform.position.x ? 1 : -1);

        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        // Fire!
        rigid.velocity = finalVelocity;

        // reduce nuber of steps left
        steps--;

        // Alternative way:
        // rigid.AddForce(finalVelocity * rigid.mass, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision col)
    {
        //keep on jumping - avoid double hits 
        if (col.gameObject.name != "Cube" && lastStep != steps)
        {
            lastStep = steps;

            if (steps == 0)
            {
                State = PlayerState.drawcard;
                cardToTurn.startAnimation();
                lastStep = -1; //reset to -1
            }
            else if (steps > 0)
            {
                jump(playField.getPoint(++playerPosition));
            }
        }
    }

    // IEnumerator GetQuestion(int p_id, string lang_code)
    // {
    //     string get_qUrl = "http://tor.skelamp.se/matste-81/w/benny/poc8/resp_get_q.php";
    //     UnityWebRequest www = UnityWebRequest.Get($"{get_qUrl}?p_id={p_id}&lang_code={lang_code}&section_nr=2");    // Alltid section_nr 2
    //     yield return www.SendWebRequest();

    //     if (www.isNetworkError || www.isHttpError)
    //     {
    //         print(www.error);
    //         cardText.text = "error";    // ändra
    //     }
    //     else
    //     {
    //         string s = www.downloadHandler.text;
    //         QuestionInfo qi = QuestionInfo.CreateFromJSON(s);

    //         if (qi.ok)
    //         {
    //             string questionText = qi.intro + "\n" + qi.q;
    //             questionText = questionText.Replace("<strong>", "");
    //             questionText = questionText.Replace("</strong>", "");
    //             cardText.text = TextFormatter.RowLenght(questionText, 70);

    //             cardToTurn.CreateButons(qi.altis);
    //         }
    //         else
    //         {
    //             print("not ok");
    //             cardText.text = "not ok";
    //         }
    //     }
    // }
}
