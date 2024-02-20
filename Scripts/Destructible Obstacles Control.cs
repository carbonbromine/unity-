using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObstaclesControl : MonoBehaviour
{
    public GameObject DestroyedObstacles;
    public GameObject Map;
    public  MapManager  MapManager;
    public void IsHit()
    {
        Map = GameObject.FindWithTag ("GameManager");
        MapManager = Map.GetComponent<MapManager>();
        GameObject go = Instantiate(DestroyedObstacles, transform.position, Quaternion.identity) as GameObject;
        go.transform.SetParent(MapManager.DestroyerObstacles);
        Destroy(this.gameObject);
    }
}
