// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [System.Serializable]
// public class PlayerInfo
// {
//     public string p_id;
//     public string lang_code;
//     public int country_id;
//     public string state;
//     public bool ok;

//     public static PlayerInfo CreateFromJSON(string jsonString)
//     {
//         return JsonUtility.FromJson<PlayerInfo>(jsonString);
//     }
// }

// [System.Serializable]
// public class AlternativeInfo
// {
//     public int alt_id;
//     public int alt_nr;
//     public string alt;
// }

// [System.Serializable]
// public class QuestionInfo
// {
//     public bool ok;
//     public int q_id;
//     public string intro;
//     public string q;
//     public string comment;
//     public string visual_path;
//     public string audio_path;
//     public AlternativeInfo[] alt;

//     public static QuestionInfo CreateFromJSON(string jsonString)
//     {
//         return JsonUtility.FromJson<QuestionInfo>(jsonString);
//     }
// }

// [System.Serializable]
// public class AnswerInfo
// {
//     public bool ok;
//     public string ok_ratio;

//     public static AnswerInfo CreateFromJSON(string jsonString)
//     {
//         return JsonUtility.FromJson<AnswerInfo>(jsonString);
//     }
// }

