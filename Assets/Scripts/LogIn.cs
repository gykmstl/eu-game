using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading;
using UnityEngine.SceneManagement;

public class LogIn : MonoBehaviour
{
    [SerializeField] Button log_in_newButton;

    [SerializeField] InputField p_nameInputField;
    [SerializeField] InputField lang_codeInputField;

    [SerializeField] InputField respons;

    // Start is called before the first frame update
    void Start()
    {
        log_in_newButton.onClick.AddListener(() =>
        {
            string p_name = p_nameInputField.text;
            string lang_code = lang_codeInputField.text;

            StartCoroutine(Database.LogInNewPlayer(p_name, lang_code, (PlayerInfo pi) =>
            {
                if (pi.error)
                {
                    print("Databasfel");
                    respons.text = "allvarligt fel!";

                }
                else if (!pi.ok)
                {
                    print("Fel vid inloggning");
                    respons.text = "fel!";

                }
                else
                {
                    PlayerStorage.p_id = pi.p_id;
                    PlayerStorage.p_name = pi.p_name;
                    PlayerStorage.lang_code = pi.lang_code;
                    PlayerStorage.steps = pi.steps;
                    PlayerStorage.points = pi.points;

                    // Save prefs
                    PlayerPrefs.SetInt("p_id", pi.p_id);
                    PlayerPrefs.SetString("p_name", pi.p_name);

                    respons.text = "okej " + pi.p_name;

                    SceneManager.LoadScene("GameScene");
                }
            }));
        });


        // Get prefs - Log in
        int p_id_pref = 0; //= PlayerPrefs.GetInt("p_id");
        string p_name_pref = PlayerPrefs.GetString("p_name");

        if (p_id_pref != 0 && p_name_pref != "")
        {
            StartCoroutine(Database.LogInPlayer(p_id_pref, p_name_pref, (PlayerInfo pi) =>
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
                    PlayerStorage.p_id = pi.p_id;
                    PlayerStorage.p_name = pi.p_name;
                    PlayerStorage.lang_code = pi.lang_code;
                    PlayerStorage.steps = pi.steps;
                    PlayerStorage.points = pi.points;

                    SceneManager.LoadScene("GameScene");
                }
            }));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
