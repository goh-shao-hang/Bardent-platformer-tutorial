using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayeInputHandler : MonoBehaviour
{
    private Vector2 movementInput;

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        Debug.Log(movementInput);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started) //equivalent to GetButton
        {
            Debug.Log("jump pressed");
        }

        if (context.canceled) //equivalent to GetButtonUp
        {
            Debug.Log("jump released");
        }

        /* Some notes: 
         * context.started is when the button is pressed (same as GetButtonDown)
         * context.performed is when the button is pressed and reached a certain threshold (strength / held time or both) and only called once (NOT the same as held)
         * context.canceled is when the button is released (same as GetButtonUp)
         * there is nothing equivalent to GetButton in the new input system atm, but there are some more complicated workarounds
         */
    }
}
