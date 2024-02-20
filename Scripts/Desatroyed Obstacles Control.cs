using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesatroyedObstaclesControl : MonoBehaviour
{
    void IsHit()
    {
        Destroy(this.gameObject);
    }
}
