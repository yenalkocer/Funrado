using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int level = 2; 
    public float patrolSpeed = 5.0f; 
    public Transform[] patrolPoints; 
    private int currentPointIndex = 0;
    private Rigidbody rb;
    private Animator animator;
    public GameObject levelTextPrefab; 
    public float closeEnoughDistance = 0.5f; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from the EnemyCharacter game object.");
            return;
        }

        
        transform.position = patrolPoints[currentPointIndex].position;

        
        GameObject levelTextObject = Instantiate(levelTextPrefab, transform);
        levelTextObject.transform.localPosition = new Vector3(0, 2, 0); 
        TextMesh levelTextMesh = levelTextObject.GetComponent<TextMesh>();
        levelTextMesh.text = "Level: " + level.ToString();
    }

    void Update()
    {
        if (rb != null)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        
        Vector3 targetPosition = patrolPoints[currentPointIndex].position;
        Vector3 movement = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        
        float speed = (movement - transform.position).magnitude / Time.deltaTime;

        
        animator.SetFloat("Speed", speed);
        animator.speed = speed; 

        
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * patrolSpeed);

        
        rb.MovePosition(movement);

        
        if (Vector3.Distance(transform.position, targetPosition) < closeEnoughDistance)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player.level < level)
            {
                
                Debug.Log("Player lost!");
                
            }
            else
            {
                
                Destroy(gameObject);
            }
        }
    }
}
