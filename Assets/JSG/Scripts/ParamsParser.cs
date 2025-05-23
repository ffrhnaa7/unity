using System.Collections.Generic;
using UnityEngine;

public class ParamsParser
{
    public static Dictionary<string, float> ParseEventParams(string param)
    {
        Dictionary<string, float> parsed = new Dictionary<string, float>();
        string key = "";
        string value = "";
        bool keyProcessing = true;
        for (int i = 0; i < param.Length; i++)
        {
            if (param[i] == '{' || param[i] == '}') continue;
            else if (param[i] == ';')
            {
                parsed[key] = float.Parse(value);

                key = "";
                value = "";
                keyProcessing = true;
            }
            else if (param[i] == '=')
            {
                keyProcessing = false;
            }
            else
            {
                char c = char.ToLower(param[i]);
                if (keyProcessing)
                {
                    key += c;
                }
                else
                {
                    value += c;
                }
            }
        }
        return parsed;
    }
}
