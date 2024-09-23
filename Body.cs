using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Body : MonoBehaviour
{
    private const double G = 0.00000000006674;

    //private const float TIME_SCALE = 500.0f;



    [SerializeField]
    private Body[] bodyList;
    public double mass = 1;
    [SerializeField]
    private Vector3 startVelocity;
    

    private Vector3Double velocity;
    private Vector3Double accel;
    

    private void Gravity() {
        accel.Mult(0);

        for (int i = 0; i < bodyList.Length; i++) {
            Vector3Double F;

            Vector3Double offset = new Vector3Double(bodyList[i].transform.position - transform.position);

            double distanceSqr = offset.SqrMagnitude();

            double Fmagnitude = (G * mass * bodyList[i].mass) / distanceSqr;

            Vector3Double Fdir = offset;
            Fdir.Normalize();

            F = new Vector3Double(Fdir, Fmagnitude);
            
            
            /*if (Time.time > 7.83f && Time.time < 7.85f) {
                Debug.Log("______" + gameObject.name + "______");
                Debug.Log("");
                Debug.Log("Offset: (" + offset.x + ", " + offset.y + ", " + offset.z + ")");
                Debug.Log("DistanceSqr: " + distanceSqr);
                Debug.Log("Fdir: (" + Fdir.x + ", " + Fdir.y + ", " + Fdir.z + ")");
                Debug.Log("Fmag: " + Fmagnitude);
                Debug.Log("Numerator: " + (G * mass * bodyList[i].mass));
            } */

            F.Mult( 1 / mass );

            accel.Add(F);
        }
    }

    private void Accelerate() {
        Vector3Double deltaV = accel.Delta();
        //deltaV.Mult(TIME_SCALE);
        velocity.Add(deltaV);
    }

    private void Move() {
        transform.position += velocity.Floatify() * Time.fixedDeltaTime /** TIME_SCALE*/;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        velocity = new Vector3Double(startVelocity);

    }

    // Update is called once per frame
    void Update() { 
        Gravity();
    }
    void FixedUpdate()
    {
        Accelerate();
        Move();
    }
}

public struct Vector3Double {
    public double x;
    public double y;
    public double z;

    public Vector3Double(Vector3Double dir, double magnitude){
        x = dir.x * magnitude;
        y = dir.y * magnitude;
        z = dir.z * magnitude;
    }
    public Vector3Double(Vector3 vector) {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public void Add(Vector3Double vector) {
        x += vector.x;
        y += vector.y;
        z += vector.z;
    }

    public void Mult(Double scalar) {
        x *= scalar;
        y *= scalar;
        z *= scalar;
    }

    public Vector3Double Delta() {
        Vector3Double delta;
        delta.x = x * Time.fixedDeltaTime;
        delta.y = y * Time.fixedDeltaTime;
        delta.z = z * Time.fixedDeltaTime;

        return delta;
    }

    public void Normalize() {
        double magnitude = Math.Sqrt(SqrMagnitude());

        x /= magnitude;
        y /= magnitude;
        z /= magnitude;    
    }

    public double SqrMagnitude() {
        return x * x + y * y + z * z;
    }


    public Vector3 Floatify() {
        return new Vector3((float) x, (float) y, (float) z);
    }
}