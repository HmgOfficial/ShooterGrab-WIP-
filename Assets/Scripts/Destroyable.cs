using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    /// <summary>
    /// Destroy the object
    /// </summary>
    public void DeleteObject()
    {
        Destroy(gameObject);
    }
}
