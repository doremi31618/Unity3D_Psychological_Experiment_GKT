using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Test : MonoBehaviour
{
    private void Start() {
        string csv_content = "A,B,c\n1,2,3\n4,5,6\n";
        StreamWriter sw = new StreamWriter(Path.Combine(Application.streamingAssetsPath, "Test.csv"));
        sw.Write(csv_content);
        sw.Close();
    }
}
