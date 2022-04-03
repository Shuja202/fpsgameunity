
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyScript : MonoBehaviour
{
    NavMeshAgent myAgent;
    public LayerMask ground;
    Vector3 destinationPoint;
    bool destinationSet;
    float destinationRange;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<NavMeshAgent>();
        myAgent = FindObjectOfType<NavMeshAgent>();
        destinationRange = 5;
    }

    // Update is called once per frame
    void Update()
    {
        guarding();
    }

    private void guarding()
    {
        if (!destinationSet)
        {
            float tempxValue = Random.Range(-destinationRange, destinationRange); 
            float tempzValue = Random.Range(-destinationRange, destinationRange);

            destinationPoint = new Vector3(transform.position.x + tempxValue, transform.position.y, transform.position.z + tempzValue);
            destinationSet = true;
        }
        else
        {
            myAgent.SetDestination(destinationPoint);
            destinationSet = false;
        }
    }
}
