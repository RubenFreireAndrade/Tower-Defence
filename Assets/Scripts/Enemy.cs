using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float baseSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float health;

    private int damage; // When reaches to Players life points.
    private int killReward;

    private GameObject targetCube;

    private void Start()
    {
        speed = baseSpeed;
        targetCube = MapGenerator.startCube;
    }

    private void Update()
    {
        CheckPosition();
        MoveEnemy();
    }

    private void CheckPosition()
    {
        if (targetCube != null && targetCube != MapGenerator.finishCube)
        {
            float distance = (transform.position - targetCube.transform.position).magnitude;
            if(distance < 0.1f)
            {
                int currentIndex = MapGenerator.pathGrids.IndexOf(targetCube);
                targetCube = MapGenerator.pathGrids[currentIndex + 1];
            }
        }
    }

    private void ReceiveDamage(float amount)
    {
        health -= amount;
        if (health <= 0) Destroy(this.gameObject);
    }

    private void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetCube.transform.position, speed * Time.deltaTime);
    }
}
