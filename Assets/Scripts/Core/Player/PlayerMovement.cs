using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// —крипт керуванн€ перем≥щенн€м гравц€
/// <para>ћ≥стить параметри перем≥щенн€ та FPS-камери</para>
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    // ѕосиланн€ на камеру гравц€
    [SerializeField] protected Transform cameraPivot;

    // ¬будований Unity-класс керуванн€ персонажем
    private CharacterController characterController;
    // ¬ектор швидкост≥ гравц€
    private Vector3 playerVelocity;
    // ‘лаг, €кий визначаЇ знаходженн€ персонажа на земл≥
    private bool groundedPlayer;

    // «наченн€ грав≥тац≥њ на земл≥
    private float groundedGravity = -3f;
    
    [Header("Movement Parameters")]
    // Ўвидк≥сть пересуванн€ гравц€
    [SerializeField] private float walkSpeed = 15.0f;
    //  оеф≥ц≥њЇнт б≥гу гравц€
    [SerializeField] private float sprintMultiplier = 2f;
    // —ила стрибка
    [SerializeField] private float jumpForce = 4f;
    // «наченн€ грав≥тац≥њ
    [SerializeField] private float gravity = -50f;

    [Header("Camera Parameters")]
    // „утлив≥сть миш≥
    [SerializeField] private float mouseSensitivity = .3f;
    // Ћ≥м≥т перегл€ду угору
    [SerializeField] private float lookUpLimit = 80.0f;
    // Ћ≥м≥т перегл€ду вниз
    [SerializeField] private float lookDownLimit = -80.0f;
    
    // ¬х≥дний вектор перем≥щенн€ гравц€
    [SerializeField] private Vector2 moveInput;
    // ¬х≥дний вектор погл€ду гравц€
    private Vector2 lookInput;
    // «наченн€ обертанн€ камери гравц€ по ос≥ X
    private float cameraRotationX = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        // ‘≥ксуЇмо курсор у центр≥ екрана та приховуЇмо його
        Cursor.lockState = CursorLockMode.Locked; 
    }

    private void Update()
    {
        // якщо поточний стан гри в≥др≥зна€Їтьс€ в≥д активного, завершуЇмо виконанн€
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;
        // ¬икликаЇмо обробник пересуванн€ гравц€
        HandleMovement();
        // ¬икликаЇмо обробник погл€ду гравц€
        HandleLook();
    }

    /// <summary>
    /// ћетод обробки пересуванн€ гравц€
    /// </summary>
    private void HandleMovement()
    {
        // ќтримуЇмо значенн€ флагу "заземленн€" в≥д контроллеру персонажа
        groundedPlayer = characterController.isGrounded;

        // якщо гравець на земл≥ та його шв≥дк≥сть по ос≥ Y менше нул€
        if (groundedPlayer && playerVelocity.y < 0)
        {
            // ¬становлюжмо грав≥тац≥ю по ос≥ Y
            playerVelocity.y = groundedGravity;
        }

        // ¬изначаЇмо вектор напр€мку
        Vector3 moveDirection = new(moveInput.x, 0, moveInput.y);
        // ѕеретворюЇмо локальний напр€мок у св≥товий прост≥р в≥дносно обертанн€ гравц€
        moveDirection = transform.TransformDirection(moveDirection); 

        // ѕерем≥щуЇмо гравц€ у напр€мку руху ≥з заданою швидк≥стю
        characterController.Move(Time.deltaTime * walkSpeed * moveDirection);

        // «астосовуЇмо грав≥тац≥ю
        playerVelocity.y += gravity * Time.deltaTime;
        // ѕерем≥щуЇмо гравц€
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    /// <summary>
    /// ћетод обробки погл€ду гравц€
    /// </summary>
    private void HandleLook()
    {
        // ќбертаЇмо гравц€ навколо ос≥ Y дл€ горизонтального погл€ду
        transform.Rotate(lookInput.x * mouseSensitivity * Vector3.up);

        // ќбертаЇмо камеру вгору та вниз дл€ вертикального погл€ду з урахуванн€м л≥м≥т≥в 
        cameraRotationX -= lookInput.y * mouseSensitivity;
        cameraRotationX = Mathf.Clamp(cameraRotationX, lookDownLimit, lookUpLimit);
        cameraPivot.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
    }

    /// <summary>
    /// ќбробник натисканн€ клав≥ш пересуванн€
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // «читуЇмо вх≥дний вектор пересуванн€
        moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ќбробник погл€ду
    /// </summary>
    public void OnLook(InputAction.CallbackContext context)
    {
        // «читуЇмо вх≥дний вектор погл€ду
        lookInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ќбробник натисканн€ клав≥ши стрибка
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // якщо клав≥шу натиснуто, та персонаж знаходитьс€ на земл≥
        if (context.performed && groundedPlayer)
        {
            // «астосовуЇмо силу стрибка
            playerVelocity.y = Mathf.Sqrt(jumpForce * -2.0f * gravity);
        }
    }

    /// <summary>
    /// ќбробник натисканн€ клав≥ши б≥гу
    /// </summary>
    public void OnSprint(InputAction.CallbackContext context)
    {
        // якщо натиснуто - множимо швидк≥сть на коеф≥ц≥Їнт б≥гу
        if (context.performed)
            walkSpeed *= sprintMultiplier;

        // якщо в≥дпущено - д≥лимо швидк≥сть на коеф≥ц≥Їнт б≥гу
        if (context.canceled)
            walkSpeed /= sprintMultiplier;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
                GameManager.Instance.PauseGame();
            else if (GameManager.Instance.CurrentState == GameManager.GameState.Paused)
                GameManager.Instance.ResumeGame();
        }
    }

    /// <summary>
    /// ќбробник з≥ткненн€ колайдеру гравц€
    /// </summary>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // якщо гравець з≥ткнувс€ з будь-€ким об'Їктом, €кий маЇ тег Wall (—т≥на)
        if (hit.gameObject.CompareTag("Wall"))
        {
            // «упин€Їмо гравц€
            playerVelocity.x = 0f;
        }
    }
}
