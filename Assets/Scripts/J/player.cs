using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class player : MonoBehaviour
{
    private GameObject target;
    private IEnumerator wanderNewMovement;
    private bool isWandering = false;
    private bool isRotLeft = false;
    private bool isRotRight = false;
    private bool isWalking = false;
    public float rotSpeed = 100f;
    private Vector3 velocity;

    private Rigidbody body;
    private float size=100;
    private bool changeDir=false;
    public float moveSpeed = 10f;
  
    void Start()
    {

        body = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("walls"))
        {
            moveSpeed = moveSpeed * -1;
            Debug.Log("wall");
         
            
           
        }
        if (collision.gameObject.CompareTag("foodnew"))
        {
            Debug.Log("food");
            Destroy(collision.gameObject);
            isWandering = false;
            target = null;

        }
    }

    void Update()
    {
        if (target == null)
        {
            if (isWandering == false)
            {

                StartCoroutine(wanderNew());
            }
           

            if (isRotRight == true)
            {
                transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
            }

            if (isRotLeft == true)
            {
                transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
            }

            if (isWalking == true)
            {
                transform.position += (transform.forward * Time.deltaTime * moveSpeed);

            }

        }
        else
        {
       
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

            

        }
    }

    private void wander()
    {
        transform.position += (transform.forward * Time.deltaTime * moveSpeed);
    }

    public void setTarget(GameObject other)
    {
     
        target = other;
        isWandering = true;
        isWalking = false;
        isRotRight = false;
        isRotLeft = false;

        Debug.Log(" target food set");
        StopAllCoroutines();


    }
    IEnumerator wanderNew()
    {

        int rotTime = Random.Range(1, 4);
        int rotWait = Random.Range(1, 2);
        int rotLorR = Random.Range(1, 3);
        int walkWait = Random.Range(1, 2);
        int walkTime = Random.Range(5, 7);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;

        yield return new WaitForSeconds(walkTime);
        isWalking = false;

        yield return new WaitForSeconds(rotWait);
        if (rotLorR == 1)
        {
            //right
            isRotRight = true;
            yield return new WaitForSeconds(rotTime);

            isRotRight = false;
        }
        if (rotLorR == 2)
        {
            //left

            isRotLeft = true;

            yield return new WaitForSeconds(rotTime);

            isRotLeft = false;
        }


        isWandering = false;
    }
}
