using UnityEngine;
using System.Collections;

public class Axis_Rotate_Cir : MonoBehaviour {

	// Use this for initialization
	void Start () {
        cube = GameObject.FindWithTag("Cube_Cir");
	}
	
	// Update is called once per frame
    void Update()
    {
        if (isRotating) executeRotate();
    }

    // The control cube
    GameObject cube;
    GameObject child, child1, child2, child3;
    bool isRotating = false;
    Quaternion definedRotation = new Quaternion(0, 0, 0, 0);
    Vector3 rotateVector = new Vector3(1, 0, 0);
    float rotateVelocity = 0;
    float accelerateDuration = 0;
    float leftDuration = 0;
    float rotateDuration = 0;
    int rotateAxis = 0;
    float angleRange = 0;
    float deltaRotate = 0;
    // Acceleration when it is in acceleration process.
    float rotateAccleration = 0;
    CTRotationType rotationType;

    private void initRotateArgument(float _initAngleRange, int _initRotateAxis, float _initRotateDuration)
    {
        rotateAxis = _initRotateAxis;
        rotateDuration = _initRotateDuration;
        leftDuration = _initRotateDuration;
        angleRange = _initAngleRange;
        rotationType = CTRotationType.Uniform;
    }

    public void RotateTo(float _angleRange, int _axis, float _duration)
    {
        isRotating = false;
        rotationType = CTRotationType.Uniform;
        initRotateArgument(_angleRange, _axis, _duration);
        switch(rotateAxis)
        {
            case 0:    // Rotate around axis X
                {
                    rotateVector = Vector3.right;
                    break;
                }
            case 1:    // Rotate around axis Y
                {
                    rotateVector = Vector3.up;
                    break;
                }
            case 2:
                {
                    rotateVector = Vector3.forward;
                    break;
                }
            default: break;
        }
        deltaRotate = angleRange / rotateDuration;
        isRotating = true;
    }

    public void RotateTo(float _angleRange, int _axis, float _duration, float _acclerationDuration)
    {
        isRotating = false;
        rotationType = CTRotationType.AccelerateUniformly;
        rotateAccleration = 1 / ((rotateDuration - accelerateDuration) * accelerateDuration);
        initRotateArgument(_angleRange, _axis, _duration);
        switch (rotateAxis)
        {
            case 0:    // Rotate around axis X
                {
                    rotateVector = Vector3.right;
                    break;
                }
            case 1:    // Rotate around axis Y
                {
                    rotateVector = Vector3.up;
                    break;
                }
            case 2:
                {
                    rotateVector = Vector3.forward;
                    break;
                }
            default: break;
        }
        accelerateDuration = _acclerationDuration;
        isRotating = true;
    }

    void executeRotate()
    {
        switch(rotationType)
        {
            case CTRotationType.Uniform: uniformRotate();break;
            case CTRotationType.AccelerateUniformly: accelerateRotate();break;
        }
        leftDuration -= Time.deltaTime;
    }

    private void accelerateRotate()
    {
        if (leftDuration > (rotateDuration - accelerateDuration))
        {
            rotateVelocity = (float)(angleRange * (rotateDuration - leftDuration)) * rotateAccleration;
            cube.transform.Rotate(rotateVelocity * rotateVector * Time.deltaTime, Space.World);
        }
        else if (leftDuration > accelerateDuration)
        {
            rotateVelocity = (float)((angleRange * leftDuration) * rotateAccleration);
            cube.transform.Rotate(rotateVelocity * rotateVector * Time.deltaTime, Space.World);
        }
        else isRotating = false;
    }

    private void uniformRotate()
    {
        if (leftDuration > 0)
        {
            cube.transform.Rotate(rotateVector * deltaRotate * Time.deltaTime, Space.Self);
        }
        else isRotating = false;
    }
}
