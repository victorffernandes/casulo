using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galeria : MonoBehaviour
{
    public GameObject prefab;
    public GameObject scroll;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(prefab, new Vector3(50, -200 - (i * 600), 0), Quaternion.identity, scroll.transform);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
