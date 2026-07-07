using UnityEngine;

public class EggInputController : MonoBehaviour
{
public EggController egg;


// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
{

}

// Update is called once per frame
void Update()
{
if (Input.GetKey(KeyCode.DownArrow))
{
egg.SetSlide();
}
else if (Input.GetKeyDown(KeyCode.Space) && egg.isGrounded)
{
    egg.SetJump();
}


if(Input.GetKeyUp(KeyCode.DownArrow))
{
egg.SetStand();
}





}
}