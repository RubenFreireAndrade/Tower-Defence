using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform rotatingObject;

    [SerializeField] private float range = 5f;
    [SerializeField] private float damage;
    [SerializeField] private float cooldownForShots;

    private Transform target;
    private GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        //rotatingObject.transform.rotation = lookRotation;
        
        Vector3 rotation = lookRotation.eulerAngles;

        rotatingObject.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void Shoot()
    {

    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
