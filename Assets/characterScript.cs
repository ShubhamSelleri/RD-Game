using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;





public class characterScript : MonoBehaviour
{

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.CharacterControls.Move.started += context => { Debug.Log(context.ReadValue<Vector2>()); };
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    void Update()
    {

    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
