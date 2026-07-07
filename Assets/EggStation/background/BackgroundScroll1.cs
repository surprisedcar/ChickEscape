using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll1 : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    private Material mat;
    private Vector2 offset = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent < Renderer >().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset.x += scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}
