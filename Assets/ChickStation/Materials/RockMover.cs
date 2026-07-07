using UnityEngine;

public class RockMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        if(transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}
