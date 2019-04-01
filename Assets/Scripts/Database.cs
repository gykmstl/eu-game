using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class PlayerInfo
{
    public bool error;
    public bool ok;

    public int p_id;
    public string p_name;
    public string lang_code;
    public int country_id;

    public int steps;
    public int points;

    public static PlayerInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PlayerInfo>(jsonString);
    }
}

[System.Serializable]
public class AlternativeInfo
{
    public int alt_id;
    public int alt_nr;
    public string alt;
}

[System.Serializable]
public class QuestionInfo
{
    public bool error;
    public bool ok;

    public int q_id;
    public string intro;
    public string q;
    public string comment;
    public string type;
    public string visual_path;
    public string audio_path;
    public AlternativeInfo[] altis;

    public static QuestionInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<QuestionInfo>(jsonString);
    }
}

[System.Serializable]
public class AnswerInfo
{
    public bool error;
    public bool ok;

    public string ok_check;
    public string ok_ratio;

    public static AnswerInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<AnswerInfo>(jsonString);
    }
}

[System.Serializable]
public class UpdateInfo
{
    public bool error;
    public bool ok;

    public static UpdateInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<UpdateInfo>(jsonString);
    }
}

public static class Database
{
    static string path = "http://tor.skelamp.se/matste-81/w/benny/poc8/";

    public static IEnumerator LogInPlayer(int p_id, string p_name, Action<PlayerInfo> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get($"{path}resp_log_in?p_id={p_id}&p_name={p_name}");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            PlayerInfo pi = new PlayerInfo();
            pi.error = true;
            callback(pi);
        }
        else
        {
            string s = www.downloadHandler.text;
            Debug.Log(s);
            PlayerInfo pi = PlayerInfo.CreateFromJSON(s);
            pi.p_id = p_id;
            pi.p_name = p_name;
            callback(pi);
        }
    }

    public static IEnumerator LogInNewPlayer(string p_name, string lang_code, Action<PlayerInfo> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get($"{path}resp_log_in_new.php?p_name={p_name}&lang_code={lang_code}");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            PlayerInfo pi = new PlayerInfo();
            pi.error = true;
            callback(pi);
        }
        else
        {
            string s = www.downloadHandler.text;
            PlayerInfo pi = PlayerInfo.CreateFromJSON(s);
            pi.p_name = p_name;
            pi.lang_code = lang_code;
            callback(pi);
        }
    }

    public static IEnumerator GetQuestion(int p_id, string lang_code, int section_nr, Action<QuestionInfo> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get($"{path}resp_get_q.php?p_id={p_id}&lang_code={lang_code}&section_nr={section_nr}");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            QuestionInfo qi = new QuestionInfo();
            qi.error = true;
            callback(qi);
        }
        else
        {
            string s = www.downloadHandler.text;
            Debug.Log(s);
            QuestionInfo qi = QuestionInfo.CreateFromJSON(s);
            callback(qi);
        }
    }

    public static IEnumerator SubmitAnswer(int p_id, int q_id, string alt_ids, Action<AnswerInfo> callback)
    {
        Debug.Log("a: " + p_id + " " + q_id + " " + alt_ids);

        UnityWebRequest www = UnityWebRequest.Get($"{path}resp_submit_a.php?p_id={p_id}&q_id={q_id}&alt_ids={alt_ids}");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            AnswerInfo ai = new AnswerInfo();
            ai.error = true;
            callback(ai);
        }
        else
        {
            string s = www.downloadHandler.text;
            AnswerInfo ai = AnswerInfo.CreateFromJSON(s);
            callback(ai);
        }
    }

    public static IEnumerator UpdatePoints(int p_id, int points, Action<UpdateInfo> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get($"{path}resp_update_points.php?p_id={p_id}&points={points}");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            UpdateInfo ai = new UpdateInfo();
            ai.error = true;
            callback(ai);
        }
        else
        {
            string s = www.downloadHandler.text;
            UpdateInfo ai = UpdateInfo.CreateFromJSON(s);
            callback(ai);
        }
    }

    public static IEnumerator UpdateSteps(int p_id, int steps, Action<UpdateInfo> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get($"{path}resp_update_steps.php?p_id={p_id}&steps={steps}");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            UpdateInfo ai = new UpdateInfo();
            ai.error = true;
            callback(ai);
        }
        else
        {
            string s = www.downloadHandler.text;
            UpdateInfo ai = UpdateInfo.CreateFromJSON(s);
            callback(ai);
        }
    }
}