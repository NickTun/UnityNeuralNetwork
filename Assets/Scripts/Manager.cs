using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject AI;
    public int numberOfAIs;
    public int timeSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeSpeed;
        for(int i  = 0; i < (numberOfAIs - 1); i++)
        {
            GameObject.Instantiate(AI, AI.transform.position, AI.transform.rotation);
        }
    }
}
