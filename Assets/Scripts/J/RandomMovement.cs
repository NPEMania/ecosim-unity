using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    private GameObject target;
    private IEnumerator wanderNewMovement;
    private bool isWandering = false;
    private bool isRotLeft = false;
    private bool isRotRight = false;
    private bool isWalking = false;
    public float rotSpeed = 100f;
    private Vector3 velocity;
    public float moveSpeed = 10f;
    private void OnTriggerEnter(Collider other)
    {


    }
    void Start()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("walls"))
        {
            moveSpeed = moveSpeed * -1;
        }

    }

    void FixedUpdate()
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


            //  wander();
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

        StopAllCoroutines();
        isWandering = false;
        isRotLeft = false;
        isRotRight = false;
        isWalking = false;

    }
    IEnumerator wanderNew()
    {

        int rotTime = Random.Range(1, 3);
        int rotWait = Random.Range(1, 3);
        int rotLorR = Random.Range(1, 3);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(2, 7);

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
