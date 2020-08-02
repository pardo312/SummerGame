using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class MovementBall : MonoBehaviour
{
    public Transform kartNormal;
    public Transform kartModel;
    public GameObject PPV;
    public GameObject particleSys;
    public Rigidbody sphere;
    public float speed;
    public bool onFloor = false;
    public bool inicio = false;

    float currentSpeed;
    float rotate, currentRotate;

    private Vector3 ogRot;
    private Color colorParticle ;

    private Quaternion beforeTurnRotation;
    private bool startDerrape, endBoost, finDerrape = false;

    public float CONST_ACCELERATION = 0.5f;
    public float acceleration = 1f;
    private float super_acceleration = 2f;
    private float desacceleration = 0.1f;

    public float MAX_SPEED = 70f;
    public float MAX_SPEED_TEMP;
    public float MIN_SPEED = -25f;
    public float max_speed_bost = 0f;
    public float boost_acceleration = 0f;
    public Vector3 tempPos;
    public Quaternion tempRot;

    private float steering = 10f;
    public float cont = 0;
    private float duracionDerrape = 0;
    private LensDistortion lens = null;
    private ChromaticAberration chromAb = null;
    private ParticleSystem leftParticle;
    private ParticleSystem rightParticle;
    private ParticleSystem boostParticle;
    private string verticalAxis;
    private string horizontalAxis;
    private string jumpButton;
    public float timeOnAir = 0f;

    [Header("Parametros")]
    public bool player1;
    public float AccelerationTemp;
    public float SteeringTemp;
    public float gravity = 20f;
    public float jumpForce = 7f;
    public float offsetSphere = 0.4f;
    public LayerMask layerMask;

    void Awake()
    {
        if(player1)
        {
            verticalAxis="Vertical";
            horizontalAxis="Horizontal";
            jumpButton="Jump";
        }
        else{
            verticalAxis="ArrowVertical";
            horizontalAxis="ArrowHorizontal";
            jumpButton = "JumpP2";
        }
        SteeringTemp=steering;
        AccelerationTemp = acceleration;
        MAX_SPEED_TEMP=MAX_SPEED;
        PPV.GetComponent<PostProcessVolume>().profile.TryGetSettings(out lens);
        PPV.GetComponent<PostProcessVolume>().profile.TryGetSettings(out chromAb);
        //Vacio :D
        leftParticle = particleSys.transform.GetChild(0).GetComponent<ParticleSystem>();
        rightParticle = particleSys.transform.GetChild(1).GetComponent<ParticleSystem>();
        boostParticle = particleSys.transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        accelerateAndBrake();
        steeringCar();
        saltar();            
        //Asigna la direccion y steering del carro
        //currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f)
        currentSpeed = speed;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f); rotate = 0f;

    }
    //Avanzar y frenar
    private void accelerateAndBrake()
    {
        if (Input.GetAxis(verticalAxis) != 0)
        {
            inicio = true;
            //Si acelera
            if (Input.GetAxis(verticalAxis) > 0 && speed < 0)
            {
                speed += super_acceleration;
            }
            else if (Input.GetAxis(verticalAxis) < 0 && speed > 0)
            {
                speed -= super_acceleration;
            }
            else if (Input.GetAxis(verticalAxis) > 0)
            {
                speed += acceleration;
            }
            //Si frena
            else if (Input.GetAxis(verticalAxis) < 0)
            {
                speed -= desacceleration;
            }
        }

        if (Input.GetAxis(verticalAxis) == 0)
        {
            speed = Mathf.SmoothStep(speed, 0, desacceleration);
        }

        if (speed > MAX_SPEED + max_speed_bost)
        {
            speed = MAX_SPEED + max_speed_bost;
        }
        else if(speed < MIN_SPEED)
        {
            speed = MIN_SPEED;
        }


    }
    //Girar el carro
    private void steeringCar()
    {
        if (Input.GetAxis(horizontalAxis) != 0  && timeOnAir < 200)
        {
            int dir = Input.GetAxis(horizontalAxis) > 0 ? 1 : -1;
            float amount = Mathf.Abs((Input.GetAxis(horizontalAxis)));
            Steer(dir, amount);
            //Empieza el derrape
            if (Input.GetButtonDown(jumpButton))
            {
                saltar();
            }
        }
    }
    //Salta para iniciar derrape
    private void saltar()
    {
        if(!onFloor)
        {
            timeOnAir += Time.smoothDeltaTime*100;
        }
        if(timeOnAir > 200)
        {
            speed = 0;
            timeOnAir=0;
            sphere.transform.position = tempPos + 3*Vector3.back +new Vector3(0,5f,0);
            sphere.transform.rotation = tempRot ;
        }   
        //Si salta y esta en el piso
        if (Input.GetButtonDown(jumpButton) && onFloor)
        {
            duracionDerrape = 0;
            sphere.AddForce(new Vector3(0f, jumpForce * 10000, 0f));
            startDerrape = true;
            finDerrape = false;
        }
        //Si salta, no esta en el piso, empieza a derrapar.
        if (Input.GetButton(jumpButton))
        {
            if (!onFloor)
            {
                if (Input.GetAxis(horizontalAxis) != 0)
                {
                    if (!endBoost)
                    {
                        CancelBoost();
                    }
                    drift();
                }
            }
            else
            { 
                
                //Habilitar Particulas
                var em = rightParticle.emission;
                em.enabled = true;
                em = leftParticle.emission;
                em.enabled = true;
                //Color Derrape
                if(duracionDerrape < 150)
                {
                    
                    colorParticle = Color.clear;
                }
                if(duracionDerrape >= 150 && duracionDerrape < 400)
                {
               
                    colorParticle = Color.cyan;
                    cont = 10;
                }
                if (duracionDerrape >= 400 && duracionDerrape < 700)
                {
            
                    colorParticle = Color.green;
                    cont = -20;
                }
                if (duracionDerrape >= 700)
                {

                    colorParticle = Color.magenta;
                    cont = -50;
                }
                ParticleSystem.MainModule ma = leftParticle.main;
                ma.startColor = colorParticle;
                ma = rightParticle.main;
                ma.startColor = colorParticle;
            }
            duracionDerrape+=Time.smoothDeltaTime*100;
        }

        //Cuando deja de presionar el salto
        if (Input.GetButtonUp(jumpButton))
        {
            if (steering != 10f)
            {
                //Disable Particles Drift
                var em = rightParticle.emission;
                em.enabled = false;
                em = leftParticle.emission;
                em.enabled = false;
                //Reset speed and rotation
                MAX_SPEED = MAX_SPEED_TEMP;
                steering = SteeringTemp;
                kartModel.transform.localRotation = beforeTurnRotation;
                finDerrape = true;
                endBoost = false;
            }
        }
        //Inciar Boost
        if (finDerrape)
        {
            Boost();
        }
    }
    private void Boost()
    {

            float aceleracionTemp = acceleration;
            float lensTemp = 0;
            chromAb.intensity.value = 0;
            if (duracionDerrape >= 150 && duracionDerrape < 400)
            {
                max_speed_bost = 10;
                aceleracionTemp = CONST_ACCELERATION + 0.2f;
                lensTemp = -50;
                chromAb.intensity.value = 0.66f;
            }
            if (duracionDerrape >= 400 && duracionDerrape <= 700)
            {
                max_speed_bost = 20;
                aceleracionTemp = CONST_ACCELERATION + 0.3f;
                lensTemp = -60;
                chromAb.intensity.value = 0.66f;
            }
            if (duracionDerrape >= 700)
            {
                max_speed_bost = 30;
                aceleracionTemp = CONST_ACCELERATION + 0.5f;
                lensTemp = -70;
                chromAb.intensity.value = 1f;
            }
            var p = boostParticle.emission;
            if (cont < 200)
            {
                if (!endBoost)
                { 
                    if(lensTemp != 0){
                        p.enabled = true;
                        acceleration = aceleracionTemp;
                        lens.intensity.value = lensTemp;
                        cont+=Time.deltaTime*300;
                    }
                }
            }
            else
            {
                max_speed_bost = 0;
                p.enabled = false;
                duracionDerrape = 0;
                acceleration = CONST_ACCELERATION;
                lens.intensity.value = 0;
                chromAb.intensity.value = 0;
                cont = 50;
                endBoost = true;
            }
    }
    //Empezar derrape
    private void drift()
    {

        //Rota el modelo del auto y modifica el steering del auto.

        if (startDerrape)
        {
            beforeTurnRotation = kartModel.transform.localRotation;
            startDerrape = false;
        }
        if (!finDerrape)
        {
            float str = Mathf.Clamp01(2f * Time.deltaTime) ;
            float quantityRotation = 0;

            //Si va para la derecha
            if (Input.GetAxis(horizontalAxis) > 0)
            {
                quantityRotation = 0;
            }
            //Si va para la izquierda
            else
            {
                quantityRotation = 180;
            }
            kartModel.transform.localRotation = Quaternion.Lerp(kartModel.transform.localRotation, Quaternion.AngleAxis(quantityRotation, Vector3.up), str);
            MAX_SPEED = MAX_SPEED_TEMP - 10f;
            steering = SteeringTemp + 10f;
            //acceleration = normalAcceleration - 10f;
        }
    }
    void FixedUpdate()
    {
        //Seguir el collider sphere
        transform.position = sphere.transform.position - new Vector3(0f, offsetSphere, 0f);
        //Dar a el collider la velocidad actual
        sphere.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);

        //Gravedad del vehiculo
        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        //Steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);

        RaycastHit hitOn;
        RaycastHit hitNear;

        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitOn, 1.1f, layerMask);
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitNear, 2.0f, layerMask);

        //Normal Rotation
        kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.deltaTime * 8.0f);
        kartNormal.Rotate(0, transform.eulerAngles.y, 0);
    }
    //Rotar el carro poco a poco, segun steering
    public void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount;
    }

    public void CancelBoost()
    {
        //cancel latest boost
        var p = boostParticle.emission;
        p.enabled = false;
        max_speed_bost = 0;
        acceleration = CONST_ACCELERATION;
        duracionDerrape = 0;
        lens.intensity.value = 0;
        chromAb.intensity.value = 0;
        cont = 20;
        endBoost = true;
    }
}
