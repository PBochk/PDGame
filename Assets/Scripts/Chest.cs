using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject reward;

    public void Spawn(Vector3 pos)
    {
        Instantiate(reward, pos, Quaternion.identity);
    }
}
