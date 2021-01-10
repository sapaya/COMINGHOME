using UnityEngine;

public enum CameraLockState
{
    None = 0,
    Locked = 1
}

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour {
    #region Variables
    public static bool firstInteraction = false;

    [Header("Targeting")]
    public Transform target;
    public GameObject waterPlane;
    public bool stayAboveWater;

    // private Camera cam;

    private float distance = 5.0f;
    private float xSensitivity = 20.0f;
    private float ySensitivity = 20.0f;

    private float yMaxLimit = 90.0f;
    private float yMinLimit = -90.0f;

    private static float maxSmoothTime = 2.0f;
    private static float minSmoothTime = 5;
    private static float sT;

    [Header("Movement Settings")]
    public bool invertX = false;
    public bool invertY = false;

    private float rotationYAxis = 0.0f;
    private float rotationXAxis = 0.0f;
    private float velocityX = 0.0f;
    private float velocityY = 0.0f;

    private float rotationXBack;
    private float rotationYBack;

    private bool hit = false;

    public bool moveAroundAxis;
	public bool lockToXAxis = false;

    public Vector3 axis;
    public float angle;

    [Header("Camera State")]
    public bool active = true;
    public CameraLockState lockState = CameraLockState.None;
    
    #endregion

    #region Main Methods
    private void Awake()
    {
        Quaternion currentRotation = transform.rotation;
        rotationXAxis = currentRotation.eulerAngles.x;
        rotationYAxis = currentRotation.eulerAngles.y;
    }
    // Use this for initialization
    void Start() {
        sT = maxSmoothTime;

        rotationXAxis = -13.634f;
        rotationYAxis = 23.031f;

		invertX = CursorControl.invX;
		invertY = CursorControl.invY;
    }

    void LateUpdate()
    {
        if(active)
            UpdateCameraPosition();
    }

    public void OnDisable()
    {
        StopCamera();
    }
    #endregion

    #region Helper Methods
    public void StopCamera()
    {
        velocityX = 0;
        velocityY = 0;
    }

    public void RecenterCamera()
    {
        Quaternion currentRotation = transform.rotation;
        rotationXAxis = currentRotation.eulerAngles.x;
        rotationYAxis = currentRotation.eulerAngles.y;
    }

    private void UpdateCameraPosition()
    {
        if (target != null)
        {

            float xAngle = 0.0f;
            float yAngle = 0.0f;

            //grab input
            if (Input.GetMouseButton(0) && active)
            {
                FirstInteractionCheck();

                xAngle += Input.GetAxis("Mouse X");
                yAngle += Input.GetAxis("Mouse Y");

                //invert if necessary
                xAngle = invertX ? InvertAngle(xAngle) : xAngle;
                yAngle = invertY ? InvertAngle(yAngle) : yAngle;

                xAngle *= xSensitivity * 0.02f;
                yAngle *= ySensitivity * 0.02f;

            }

            rotationXBack = rotationXAxis;

            if (!lockToXAxis)
                rotationYBack = rotationYAxis;

            //update local transform and apply transformation
            velocityX += xAngle;
            velocityY += yAngle;

            rotationYAxis += velocityX;
            if (!lockToXAxis)
                rotationXAxis -= velocityY;

            rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);

            Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
            Quaternion rotation = toRotation;

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            if (!hit)
            {
                if (moveAroundAxis)
                {
                    DirectedMovement(position, rotation);
                }
                else
                {

                    transform.rotation = rotation;
                    transform.position = position;

                    velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * sT);
                    velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * sT);
                }
            }
            else
            {
                velocityX = 0;
                velocityY = 0;

                rotationXAxis = rotationXBack;
                rotationYAxis = rotationYBack;
            }

        }
    }

    private static void FirstInteractionCheck()
    {
        if (!firstInteraction)
        {
            firstInteraction = true;
            FindObjectOfType<TutorialManager>().HasClicked();
        }
    }

    public void InvertXMotion() {
        invertX = !invertX;
    }

    public void InvertYMotion() {
        invertY = !invertY;
    }

    public void SetSensitivity(int NewSensitivity) {
        xSensitivity = NewSensitivity;
        ySensitivity = NewSensitivity;
        
    }

    private void DirectedMovement(Vector3 position, Quaternion rotation) {

        Vector3 camVector = -position + target.position;
        float currentAngle = Vector3.Angle(axis, camVector);

        //if in cone
        if (currentAngle < angle) {

            //if randbereich von cone und active
            if (active && currentAngle > 0.7 * angle) {
                sT = minSmoothTime;
            } else {
                //else
                sT = maxSmoothTime;
            }

            transform.rotation = rotation;
            transform.position = position;

            velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * sT);
            velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * sT);
        } else {
            //else
            velocityX = 0;
            velocityY = 0;

            rotationXAxis = rotationXBack;
            rotationYAxis = rotationYBack;
        }
    }

    //inverts the given value in [-1, 1]
    private float InvertAngle(float angle) {
        return 0 - angle;
    }

    //clamps the given angle in to [-360, 360] and [min, max]
    private float ClampAngle(float angle, float min, float max) {
        if (angle < -360F) {
            angle += 360.0f;
        }
        if (angle > 360F) {
            angle -= 360.0f;
        }
        return Mathf.Clamp(angle, min, max);
    }

    public void TurnOn()
    {
        if (!active)
        {
            active = true;
            sT = maxSmoothTime;
        }
    }

    public void TurnOff()
    {
        if (active)
        {
            active = false;
            sT = minSmoothTime;
        }
    }
    #endregion
}