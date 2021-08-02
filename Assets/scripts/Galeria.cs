using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Galeria : MonoBehaviour
{
    public GameObject prefab;
    public GameObject scroll;
    DirectoryInfo dir;
    // Start is called before the first frame update
    void Start()
    {
        string subPath = Path.Combine(Application.persistentDataPath, "selfies");
        Debug.Log(subPath);
        dir = new DirectoryInfo(subPath);
        FileInfo[] info = dir.GetFiles("*.png");
        int i = 0;
        foreach (FileInfo f in info)
        {
            Debug.Log(f.FullName);
            GameObject temp = Instantiate(prefab, new Vector3(50, -200 -i, 0), Quaternion.identity, scroll.transform);
            temp.transform.GetChild(1).GetComponent<RawImage>().texture = LoadPNG(f);
            i += 600;
        }
        
    }

    public static Texture2D LoadPNG(FileInfo inf)
    {
        MemoryStream dest = new MemoryStream();
        Texture2D tex = null;
        //Read from each Image File
        using (Stream source = inf.OpenRead())
        {
            byte[] buffer = new byte[2048];
            int bytesRead;
            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                dest.Write(buffer, 0, bytesRead);
            }
        }

        byte[] imageBytes = dest.ToArray();
        tex = new Texture2D(2, 2);
        tex.LoadImage(imageBytes);
        return tex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
