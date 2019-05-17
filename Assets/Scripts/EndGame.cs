using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {
    
    public void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.AddScore(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
        }
        
    }
}
