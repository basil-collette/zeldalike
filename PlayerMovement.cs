using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInputs;

    // Start is called before the first frame update
    void Start()
    {
        playerInputs = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 joystickInput = playerInputs.actions["Move"].ReadValue<Vector2>();
        if (joystickInput != Vector2.zero)
        {
            Debug.Log(joystickInput);
        }

        GetComponent<Rigidbody2D>().velocity = joystickInput.normalized;

        bool bPressed = playerInputs.actions["B"].ReadValue<float>() > 0;

        if (bPressed)
        {
            GetComponent<Rigidbody2D>().position = Vector3.zero;
        }
    }
}
