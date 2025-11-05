using UnityEngine;

public class KillAllArrows : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Arrow")
        {
            Debug.Log("Hi");
            Destroy(other.gameObject);
        }
    }
}
