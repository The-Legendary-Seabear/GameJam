using UnityEngine;
using UnityEngine.SceneManagement; // if using SceneManager directly

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // the scene name
    [SerializeField] private bool useSceneLoader = true; // optional

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        // Check if the player touched it
        if (other.CompareTag("Player"))
        {
            
                // Or load directly using Unity's SceneManager
                SceneManager.LoadScene(sceneToLoad);
            
        }
    }
}