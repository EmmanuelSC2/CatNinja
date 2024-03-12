using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] Transform levelStart;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        GameManager.Instance.SpawnPlayer(levelStart);
    }
}
