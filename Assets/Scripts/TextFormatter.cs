using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class TextFormatter : MonoBehaviour
{

    public static string RowLenght(string input, int lenght)
    {

        string[] words = input.Split(' ');


        string ret = "";
        string row = words[0];
        //int count = 0;

        for (int i = 1; i < words.Length; i++)
        {
            if (row.Length + 1 + words[i].Length  < lenght)
                row += " " + words[i];
            else
            {
               // print(row + " : " + row.Length + " of " +  lenght);
                ret += "\n" + row;
                row = words[i];
            }

        }

        if (ret == "")
            ret = row;
        else
            ret += "\n" + row;

        return ret;
    }
}




