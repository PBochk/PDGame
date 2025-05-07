using UnityEngine;

public class DestroyIfIntersect : MonoBehaviour
{
    public GameObject blankRoom;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Room"))
        {
            Instantiate(blankRoom, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Debug.Log("Destroyed");
        }
    }
}
