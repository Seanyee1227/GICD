using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 _moveDir;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float _hAxis = Input.GetAxisRaw("Horizontal");

        _moveDir = new Vector3(_hAxis, 0, 0);
        transform.position = _moveDir * moveSpeed * Time.deltaTime;
    }
}
