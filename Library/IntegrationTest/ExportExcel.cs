using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ExportExcel
{
    static string path = "/result.csv";
    public static void OutPutHeader(string fileName, params string[] headers)
    {
        using (var writer = new StreamWriter($"{Application.dataPath}/{fileName}.csv", append: false))
        {
            var text = "";
            foreach (var header in headers)
            {
                text += header + ",";
            }
            writer.WriteLine(text);
        }
    }
    public static void OutPutData(string fileName, params object[] datas)
    {
        using (var writer = new StreamWriter($"{Application.dataPath}/{fileName}.csv", append: true))
        {
            var text = "";
            foreach (var data in datas)
            {
                text += data.ToString() + ",";
            }
            writer.WriteLine(text);
        }
    }
}
