using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static Camera mainCamera;
    public float turnSpeed;
    public ObservedVector3 PlayerDirection;
    public ObservedTransform PlayerTransform;
    private Vector2 moveInput;
    public Vector2 pitchLimits;
    private Vector2 angles;
    public bool invertControls;
    public Transform rotationTarget;
    public float rollTime;
    public AnimationCurve rollCurve;
    private float roll_v;
    public float rollSmoothTime;
    public float rollAmplitude;
    private void Awake()
    {
        PlayerDirection.SetReference(transform.forward);
        PlayerTransform.SetReference(rotationTarget);
    }
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal")*turnSpeed;
        float invertControlMultiplier = invertControls ? -1 : 1;
        moveInput.y = Input.GetAxis("Vertical")*turnSpeed*invertControlMultiplier;

        rollTime = Mathf.SmoothDamp(rollTime, moveInput.x, ref roll_v, rollSmoothTime);

        rollTime = Mathf.Clamp(rollTime, -1f,1f);
        
        RotationHandler();
    }

    void RotationHandler()
    {
        angles += moveInput;
        angles.y = Mathf.Clamp(angles.y, pitchLimits.x, pitchLimits.y);
        Quaternion yawRot = Quaternion.AngleAxis(angles.x, Vector3.up);
        Quaternion pitchRot = Quaternion.AngleAxis(angles.y, Vector3.right);
        Quaternion rollRot = Quaternion.AngleAxis(rollCurve.Evaluate(rollTime)*rollAmplitude, Vector3.forward);
        rotationTarget.localRotation = yawRot * pitchRot * rollRot;
    }
}

