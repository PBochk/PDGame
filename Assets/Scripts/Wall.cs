using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private bool isDone = false;
    public GameObject block;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Block") && !isDone)
        {
            Instantiate(block, transform.GetChild(0).position, Quaternion.identity);
            Instantiate(block, transform.GetChild(1).position, Quaternion.identity);
            Destroy(gameObject);
            isDone = true;
        }
    }
}
