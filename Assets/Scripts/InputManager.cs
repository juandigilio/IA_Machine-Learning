using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private string moveAction = "Move";
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameManager gameManager;


    private void Start()
    {
        playerInput.ActivateInput();
        playerInput.currentActionMap.FindAction(moveAction).started += Move;
    }

    private void Move(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            gameManager.Move(callbackContext.ReadValue<Vector2>());
        }
    }
}
