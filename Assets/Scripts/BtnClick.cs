using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class BtnClick : MonoBehaviour
{

    public AlternativeInfo Alt { get; set; }

    float delay = 0f;

    private void OnMouseDown()
    {
        if (transform.parent.GetComponent<Card>().Clickable)
        {

            //change color
            //GetComponent<Renderer>().material.color = Color.yellow;

            //debug
            print("xFrom BtnClick script: Player clicked. alt: " + Alt.alt + "  alt_id: " + Alt.alt_id + "  alt_nr: " + Alt.alt_nr);

            StartCoroutine(Database.SubmitAnswer(PlayerStorage.p_id, PlayerStorage.q_id, Alt.alt_id.ToString(), (AnswerInfo ai) =>
            {
                print(ai.ok);
                print(ai.ok_ratio);

                if (ai.ok_ratio == "1/1")
                {
                    GetComponent<Renderer>().material.color = Color.green;

                    // Update points
                    // StartCoroutine(Database.UpdatePoints(PlayerStorage.p_id, 99, (UpdateInfo ui) =>
                    // {
                    //     print("Points updated: " + ui.ok);
                    // }));
                }
                else
                {
                    GetComponent<Renderer>().material.color = Color.red;
                }
            }));

            print("efter");

            delay = 2.5f; // time (in seconds) to display card after user clicked an alternative
        }

    }

    private void Update()
    {

        if (delay > 0)
        {
            delay -= Time.deltaTime;

            if (delay < 0)
                transform.parent.GetComponent<Card>().ButtonPressed();
        }
    }
}
