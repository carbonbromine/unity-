using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour
{
    public GameObject gamemanager;
    // Start is called before the first frame update
    void Awake()
    {
        if(GameManager.Instance ==null)
        GameObject.Instantiate(gamemanager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
