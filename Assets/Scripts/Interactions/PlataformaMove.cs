using System;
using UnityEngine;

public class PlataformaMove : MonoBehaviour
{
    public Rigidbody plataformRb;
    public Transform[] plataformPositions;
    public int speed;
    private int nextPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextPosition = 1;
        plataformRb.transform.position = plataformPositions[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlataform();
    }

    private void MovePlataform()
    {
        plataformRb.MovePosition(Vector3.MoveTowards(plataformRb.position, plataformPositions[GetNextPosition()].position, speed * Time.deltaTime));
    }

    private int GetNextPosition()
    {
        if (plataformRb.position == plataformPositions[nextPosition].position)
        {
            if (++nextPosition == plataformPositions.Length)
            {
                nextPosition = 0;
            }
        }
        return nextPosition;
    }
}
