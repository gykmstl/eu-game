using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading;

public class GetData : MonoBehaviour
{
    [SerializeField] Button get_pButton;
    [SerializeField] Button log_inButton;
    [SerializeField] Button log_in_newButton;
    [SerializeField] Button get_qButton;
    [SerializeField] Button submit_aButton;
    [SerializeField] Button update_pointsButton;
    [SerializeField] Button update_stepsButton;

    [SerializeField] InputField p_idInputField;
    [SerializeField] InputField p_nameInputField;
    [SerializeField] InputField lang_codeInputField;
    [SerializeField] InputField section_nrInputField;
    [SerializeField] InputField alt_idsInputField;

    // Player
    int p_id;
    string p_name;
    string lang_code;   // p_lang_code
    int steps; // p_pos 
    int points; // p_pts

    // Question
    int q_id;
    string intro;
    string q;
    string type;
    string visual_path;
    string audio_path;
    AlternativeInfo[] altis;
    string comment;

    void Start()
    {
        log_inButton.onClick.AddListener(() =>
        {
            int p_id = int.Parse(p_idInputField.text);
            string p_name = p_nameInputField.text;

            StartCoroutine(Database.LogInPlayer(p_id, p_name, (PlayerInfo pi) =>
            {
                if (pi.error)
                {
                    print("Databasfel");
                }
                else if (!pi.ok)
                {
                    print("Fel vid inloggning");
                }
                else
                {
                    this.p_id = pi.p_id;
                    this.p_name = p_name;
                    this.lang_code = pi.lang_code;
                    this.steps = pi.steps;
                    this.points = pi.points;

                    print("Inloggad:");
                    print($"  p_id: {p_id}");
                    print($"  p_name: {p_name}");
                    print($"  lang_code: {lang_code}");
                    print($"  steps: {steps}");
                    print($"  points: {points}");

                    PlayerPrefs.SetInt("p_id", p_id);
                    PlayerPrefs.SetString("p_name", p_name);
                }
            }));
        });

        log_in_newButton.onClick.AddListener(() =>
        {
            string p_name = p_nameInputField.text;
            string lang_code = lang_codeInputField.text;

            StartCoroutine(Database.LogInNewPlayer(p_name, lang_code, (PlayerInfo pi) =>
            {
                if (pi.error)
                {
                    print("Databasfel");
                }
                else if (!pi.ok)
                {
                    print("Fel vid inloggning");
                }
                else
                {
                    this.p_id = pi.p_id;
                    this.p_name = p_name;
                    this.lang_code = pi.lang_code;
                    this.steps = 0;
                    this.points = 0;

                    print("Ny spelare inloggad:");
                    print($"  p_id: {p_id}");
                    print($"  p_name: {p_name}");
                    print($"  lang_code: {lang_code}");
                    print($"  steps: {steps}");
                    print($"  points: {points}");

                    PlayerPrefs.SetInt("p_id", p_id);
                    PlayerPrefs.SetString("p_name", p_name);
                }
            }));
        });

        get_qButton.onClick.AddListener(() =>
        {
            int section_nr = int.Parse(section_nrInputField.text);

            StartCoroutine(Database.GetQuestion(this.p_id, this.lang_code, section_nr, (QuestionInfo qi) =>
            {
                if (qi.error)
                {
                    print("Databasfel");
                }
                else if (!qi.ok)
                {
                    print("Fel vid frågehämtning");
                }
                else
                {
                    this.q_id = qi.q_id;
                    this.intro = qi.intro;
                    this.q = qi.q;
                    this.type = qi.type;
                    this.visual_path = qi.visual_path;
                    this.audio_path = qi.audio_path;
                    this.altis = qi.altis;
                    this.comment = qi.comment;

                    print("Fråga:");
                    print($"  q_id: {q_id}");
                    print($"  intro: {intro}");
                    print($"  q: {q}");
                    print($"  type: {type}");
                    print($"  visual_path: {visual_path}");
                    print($"  audio_path: {audio_path}");

                    foreach (var alti in altis)
                    {
                        print($"    ({alti.alt_nr}, {alti.alt_id}): {alti.alt}");
                    }
                }
            }));
        });

        submit_aButton.onClick.AddListener(() =>
        {
            string alt_ids = alt_idsInputField.text;

            StartCoroutine(Database.SubmitAnswer(this.p_id, this.q_id, alt_ids, (AnswerInfo ai) =>
            {
                if (ai.error)
                {
                    print("Databasfel");
                }
                else if (!ai.ok)
                {
                    print("Fel vid svarangivelse");
                }
                else
                {
                    print("Svar:");
                    print($"  ok_check: {ai.ok_check}");
                    print($"  ok_ratio: {ai.ok_ratio}");

                    print($"  comment: {this.comment}");

                    if (ai.ok_ratio == "1/1")
                    {
                        update_pointsButton.onClick.Invoke();
                    }
                }
            }));
        });

        update_pointsButton.onClick.AddListener(() =>
        {
            this.points++;

            StartCoroutine(Database.UpdatePoints(this.p_id, this.points, (UpdateInfo ui) =>
            {
                if (ui.error)
                {
                    print("Databasfel");
                }
                else if (!ui.ok)
                {
                    print("Fel vid uppdatering av poäng");
                }
                else
                {
                    print("Poängen uppdaterade");
                }
            }));
        });

        update_stepsButton.onClick.AddListener(() =>
        {
            this.steps++;

            StartCoroutine(Database.UpdateSteps(this.p_id, this.steps, (UpdateInfo ui) =>
            {
                if (ui.error)
                {
                    print("Databasfel");
                }
                else if (!ui.ok)
                {
                    print("Fel vid uppdatering av steg");
                }
                else
                {
                    print("Stegen uppdaterade");
                }
            }));
        });

        this.p_id = PlayerPrefs.GetInt("p_id");
        this.p_name = PlayerPrefs.GetString("p_name");

        // If not default values then login
        if (p_id != 0 && p_name != "")
        {
            p_idInputField.text = this.p_id.ToString();
            p_nameInputField.text = this.p_name;

            log_inButton.onClick.Invoke();
        }

        section_nrInputField.text = "1";
    }
}