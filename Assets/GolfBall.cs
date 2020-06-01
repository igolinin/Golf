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
    public int RPM=7200;
    public float ballSpeed;


    public float smashFactor = 1.49f; // clubhead-to-ball-initial speed ratio
    public float crossSectionalArea; //m^2
    Vector3 position;
    public Vector3 velocity;
    Vector3 acceleration;
    Vector3 angularVelocity;
    private Vector3 startPos;

    [Header("Nature")]
    public float gravityMagnitude = -9.8f; // 9.8 m/s^2
    public float airDensity = 1.2041f; // kg/m^3

    
    [Range(0, 1)]
    public float restitutionCoeffient ;
    
    private bool isIdle;
       
    [Header("Aerodynamics properties ")]
    public float dragCoefficient =  0.4f;
    public float liftCoefficient = 0.00001f ; // made this up?
    //////////////////////////////////////////////////////////////////////////    
	[Header("Shot")]
	
	[Range(-1, 1), Tooltip("-1 backspin,+1 topspin")]
	public float backSpin;
	[Range(-1, 1), Tooltip("-1 left, +1 right")]
	public float sideSpin;

    


	public float magnusConstant = 0.03f;// Start is called before the first frame update
    ////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
	{
		
	}
    void Start()
    {
        crossSectionalArea = Mathf.Pow(0.04f, 2) * Mathf.PI / 4;
        startPos = transform.position;
        isIdle = false;
        getInitialVelocity();
        getInitialAngularVelocity();

       
    }

    // Update is called once per frame
    void Update()
    {
        if(!isIdle){
            if(transform.position.y <= 0){    //approximating the landing
                if(velocity.z > 0){
                    bounce();
                    fly();
                }else{
                    Debug.Log("hit the ground");
                    acceleration = new Vector3(0,0,0);
                    velocity = new Vector3(0,0,0);
                    transform.position = new Vector3(transform.position.x, 0 , transform.position.z);
                    isIdle = true;
                    
                }
        
            }else{
                fly();
            }
        }
        
    }
    Vector3 totalAcceleration(){
        Vector3 gravityAcceleration = new Vector3(0, gravityMagnitude, 0);
        Vector3 dragForceAcceleration = -0.5f* velocity * velocity.magnitude  * dragCoefficient * airDensity * crossSectionalArea /mass;
        Vector3 magnusForceAcceleration = Vector3.Cross(angularVelocity, velocity)*(liftCoefficient / mass) ; //
        return gravityAcceleration  + dragForceAcceleration + magnusForceAcceleration;
    }
    void fly(){
        acceleration = totalAcceleration();
        Vector3 currentVelocity = velocity;
        velocity = currentVelocity + acceleration*Time.deltaTime;
        transform.position = transform.position + velocity * Time.deltaTime;
        //transform.Rotate (angularVelocity);
    }
    void getInitialVelocity(){
        velocity.y = Mathf.Sin(verticalDegrees * Mathf.PI / 180);
        Debug.Log(velocity.y);
        velocity.x = 0;
        velocity.z = Mathf.Cos(verticalDegrees * Mathf.PI / 180);
        
        
        velocity = velocity.normalized * ballSpeed*smashFactor;
    }
    void getInitialAngularVelocity(){
        var spin = new Vector3(backSpin, sideSpin, 0);
        angularVelocity = spin.normalized * (RPM * 2 * Mathf.PI /60);
    }
   
    
    void bounce(){
        transform.position = new Vector3(transform.position.x, 0 , transform.position.z);
        velocity = new Vector3(velocity.x, velocity.y*-1, velocity.z)*restitutionCoeffient;//
        Debug.Log("bum");
    }
    
        
}

