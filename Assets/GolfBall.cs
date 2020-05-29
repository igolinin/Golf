using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBall : MonoBehaviour
{
    
    private new Rigidbody rigidbody;
    public float mass = 0.0459f; // kg
    [Header("Shot settings")]
    public float horizontalDegrees = 45f;
    public float verticalDegrees = 0f;
    public int RPM=6000;
    public float ballSpeed;


    public float smashFactor = 1.49f; // clubhead-to-ball-initial speed ratio
    public float crossSectionalArea = 0.04267f * Mathf.PI / 4; //m^2
    Vector3 position;
    public Vector3 velocity;
    Vector3 acceleration;
    Vector3 angularVelocity;
    private Vector3 startPos;

    [Header("Nature")]
    public float gravityMagnitude = -9.8f; // 9.8 m/s^2
    public float airDensity = 1.2041f; // kg/m^3
       
    [Header("Aerodynamics properties ")]
    public float dragCoefficient =  0.4f;
    public float liftCoefficient = 0.00001f ; // made this up?
    //////////////////////////////////////////////////////////////////////////    
	[Header("Shot")]
	public float force = 20f;
	[Range(-1, 1), Tooltip("-1 backspin,+1 topspin")]
	public float backSpin;
	[Range(-1, 1), Tooltip("-1 left, +1 right")]
	public float sideSpin;

	public float magnusConstant = 0.03f;// Start is called before the first frame update
    ////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
	}
    void Start()
    {
        startPos = transform.position;
        getInitialVelocity();
        getInitialAngularVelocity();

       
    }

    // Update is called once per frame
    void Update()
    {
        acceleration = totalAcceleration();
        Vector3 initialVelocity = velocity;
        velocity = initialVelocity + acceleration*Time.deltaTime;
        transform.position = transform.position + velocity * Time.deltaTime;
    }
    Vector3 totalAcceleration(){
        Vector3 gravityAcceleration = new Vector3(0, gravityMagnitude, 0);
        Vector3 dragForceAcceleration = velocity * -1 * airDensity * crossSectionalArea / mass;
        Vector3 magnusForceAcceleration = Vector3.Cross(angularVelocity, velocity)*(liftCoefficient / mass) ; //for now currentPoint.angularVelocity.clone().cross(currentPoint.velocity).multiplyScalar(this.liftCoefficient / this.mass);

        return gravityAcceleration + dragForceAcceleration + magnusForceAcceleration;
    }
    void getInitialVelocity(){
        /* velocity.y = 1f;//Mathf.Sin(-1 * horizontalDegrees * Mathf.PI / 180);
        velocity.x = 0;//Mathf.Sin(verticalDegrees * Mathf.PI / 180);
        velocity.z = 1f; */
        
        //Mathf.Cos(verticalDegrees * Mathf.PI / 180);
        velocity = velocity.normalized * ballSpeed;
    }
    void getInitialAngularVelocity(){
        var spin = new Vector3(backSpin, sideSpin, 0);
        angularVelocity = spin.normalized * (RPM * 2 * Mathf.PI /60);
    }
    void hitBall(){
        rigidbody.AddRelativeForce(Vector3.forward * force, ForceMode.Impulse);

		// add spin
		var spin = new Vector3(backSpin, sideSpin, 0);
		rigidbody.AddRelativeTorque(spin, ForceMode.Impulse);
    }
    
        
}

