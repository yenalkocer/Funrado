using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using System.Collections.Generic; 

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public Joystick joystick; 
    public Text levelText; 
    public int level = 1; 
    private HashSet<string> keysCollected = new HashSet<string>(); 
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateLevelText();
    }

    void Update()
    {
        
        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;

        
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        
        float animationSpeed = movement.magnitude;
        animator.SetFloat("Speed", animationSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Collectible"))
        {
            level++;
            UpdateLevelText();
            Destroy(other.gameObject);
            Debug.Log("Collectible collected!"); 
        }

        
        if (other.gameObject.CompareTag("Key"))
        {
            keysCollected.Add(other.gameObject.name);
            Destroy(other.gameObject);
            Debug.Log(other.gameObject.name + " collected!");
        }

        
        if (keysCollected.Contains(other.gameObject.name.Replace("Door", "Key")))
        {
            
            other.gameObject.transform.position += new Vector3(0, 0, -5); 
            Debug.Log(other.gameObject.name + " opened!");
        }

        
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                if (enemy.level > level)
                {
                    
                    Debug.Log("Player lost!");
                    RestartGame();
                }
                else if (enemy.level < level)
                {
                    
                    Debug.Log("Enemy destroyed!");
                    Destroy(other.gameObject);
                }
            }
        }
    }

    void UpdateLevelText()
    {
        levelText.text = "Level: " + level.ToString();
    }

    void RestartGame()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
