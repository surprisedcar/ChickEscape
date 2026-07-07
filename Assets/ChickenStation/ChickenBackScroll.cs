using UnityEngine;

public class ChickenBackScroll : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
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
