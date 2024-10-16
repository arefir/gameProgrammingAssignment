using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rSpeed;
    private float sprintSpeed;
    private float oriSpeed;
    private Rigidbody rb;
    private Vector2 movementInput;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        sprintSpeed = speed * 2;
        oriSpeed = speed;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    // Update is called once per frame
    void Update()
    {
        movementInput.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        rb.AddRelativeForce(speed * movementInput.x * Time.deltaTime, 0, speed * movementInput.y * Time.deltaTime);


        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.Translate(0, 0, speed*Time.deltaTime);
        //}        
        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.Translate(0, 0, -speed * Time.deltaTime);
        //}        
        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.Translate(-speed * Time.deltaTime, 0, 0);
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.Translate(speed * Time.deltaTime, 0, 0);
        //}
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    transform.Translate(0, speed * Time.deltaTime, 0);
        //}
        //if (Input.GetKey(KeyCode.E))
        //{
        //    transform.Translate(0, -speed * Time.deltaTime, 0);
        //}
        float lookInput = Input.GetAxis("Mouse X");

        rb.AddRelativeTorque(0, rSpeed * lookInput * Time.deltaTime, 0);
        //float mouseY = Input.GetAxis("Mouse Y");
        //transform.Rotate(0, mouseX * rSpeed * Time.deltaTime, 0);
        //transform.Rotate(-mouseY * rSpeed * Time.deltaTime, 0, 0);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = oriSpeed;
        }



    }

    //IEnumerator RestoreSpeed(float time)
    //{
    //    yield return new WaitForSeconds(time);

    //    speed = speed / 10;
    //}
}
