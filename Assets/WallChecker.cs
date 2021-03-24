using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    public Transform TopLeft;
    public Transform TopRight;
    public Transform DownLeft;
    public Transform DownRight;
    public float CheckDistance = 0.05f;
    public LayerMask whatIsWall;
    public bool Top;
    public bool Bottom;
    public bool Left;
    public bool Right;

    private void FixedUpdate()
    {

        Top = !(Physics2D.Raycast(TopLeft.position, Vector3.up, CheckDistance, whatIsWall) || Physics2D.Raycast(TopRight.position, Vector3.up, CheckDistance, whatIsWall));
        Debug.DrawRay(TopLeft.position, Vector3.up*CheckDistance, Color.red, Time.fixedDeltaTime);
        Debug.DrawRay(TopRight.position, Vector3.up * CheckDistance, Color.red, Time.fixedDeltaTime);
        Bottom = !(Physics2D.Raycast(DownLeft.position, Vector3.down, CheckDistance, whatIsWall) || Physics2D.Raycast(DownRight.position, Vector3.down, CheckDistance, whatIsWall));
        Debug.DrawRay(DownLeft.position, Vector3.down * CheckDistance, Color.red, Time.fixedDeltaTime);
        Debug.DrawRay(DownRight.position, Vector3.down * CheckDistance, Color.red, Time.fixedDeltaTime);
        Left = !(Physics2D.Raycast(TopLeft.position, Vector3.left, CheckDistance, whatIsWall) || Physics2D.Raycast(DownLeft.position, Vector3.left, CheckDistance, whatIsWall));
        Debug.DrawRay(TopLeft.position, Vector3.left * CheckDistance, Color.red, Time.fixedDeltaTime);
        Debug.DrawRay(DownLeft.position, Vector3.left * CheckDistance, Color.red, Time.fixedDeltaTime);
        Right = !(Physics2D.Raycast(TopRight.position, Vector3.right, CheckDistance, whatIsWall) || Physics2D.Raycast(DownRight.position, Vector3.right, CheckDistance, whatIsWall));
        Debug.DrawRay(TopRight.position, Vector3.right * CheckDistance, Color.red, Time.fixedDeltaTime);
        Debug.DrawRay(DownRight.position, Vector3.right * CheckDistance, Color.red, Time.fixedDeltaTime);
    }
}
