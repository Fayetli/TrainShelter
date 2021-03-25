using System.Collections.Generic;
using System.Numerics;
public static class VariablesExtenstion
{
    public static string ParseToString(this BigInteger integer)
    {
        List<string> chars = new List<string>
        {"K", "M", "B", "T", "q", "Q", "s", "S", "O", "N", "d", "U", "D", "Tre",
        "Qua", "Qui", "SE", "SEP", "OC", "NV", "VIG"};

        string str = integer.ToString();
        int ks = (str.Length - 1) / 3;
        //UnityEngine.Debug.Log("KS: " + ks);
        if (ks == 0)
        {
            UnityEngine.Debug.Log("Origin: " + integer);
            UnityEngine.Debug.Log("Result: " + str);
            return str;
        }

        int addChars = (str.Length - 1) % 3 + 2;
        //UnityEngine.Debug.Log("add: " + ks);
        string result = "";
        for (int i = 0; i < addChars; i++)
        {
            if (i == addChars - 1)
                result += '.';
            result += str[i];
        }
        result += chars[ks - 1];
        UnityEngine.Debug.Log("Origin: " + integer);
        UnityEngine.Debug.Log("Result: " + result);
        return result;
    }
}
