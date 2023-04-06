using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public VectorValue initialPlayerPos;

    void Start()
    {
        Debug.Log("Start gamemanager");
        initialPlayerPos.initalValue = new Vector2(-2.5f, 1.8f);
    }

    void Update()
    {
        
    }

}
