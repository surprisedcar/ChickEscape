using UnityEngine;

public class ChickenGroundScroll : MonoBehaviour
{
   
    public float scrollSpeed = 0.2f;
    private Material mat;
    private Vector2 offset = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset.x += scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}
