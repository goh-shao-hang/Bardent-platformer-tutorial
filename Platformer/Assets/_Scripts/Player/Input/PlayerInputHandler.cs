using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Properties

    #region Move
    public Vector2 RawMovementInput { get; private set; }
    public int NormalizedInputX { get; private set; }
    public int NormalizedInputY { get; private set; }
    #endregion

    #region Jump
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    private float jumpInputBuffer = 0.2f;
    private float jumpInputStartTime;
    #endregion

    #region Wall Interaction
    public bool GrabInput { get; private set; }
    #endregion

    #endregion

    private void Update()
    {
        CheckJumpInputBuffer();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormalizedInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormalizedInputY = (int)(RawMovementInput * Vector2.up).normalized.y;

        /* Above 2 lines make sure that input in both axis are normalized to be either 0 or 1 depending if there is input. Normalizing a Vector2 directly only normalized the magnitude, not the x and y values.
         * This is NOT SUITABLE for games where the player can move diagonally, eg. top down games and 3d games, since you want the input direction normalized in those cases.
         */
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed) //equivalent to GetButton
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
        /* Some notes: 
         * context.started is when the button is pressed (same as GetButtonDown)
         * context.performed is when the button is pressed and reached a certain threshold (strength / held time or both) and only called once (NOT the same as held)
         * context.canceled is when the button is released (same as GetButtonUp)
         * there is nothing equivalent to GetButton in the new input system atm, but there are some more complicated workarounds
         */
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GrabInput = true;
        }

        if (context.canceled)
        {
            GrabInput = false;
        }
    }

    public void UseJumpInput() => JumpInput = false;

    private void CheckJumpInputBuffer()
    {
        if (Time.time >= jumpInputStartTime + jumpInputBuffer)
        {
            JumpInput = false;
        }
    }
}
