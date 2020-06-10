using System;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed = 8f;

    private bool _isEnemyLaser = false;

    private void Update()
    {
        if (!_isEnemyLaser)
            MoveUp();
        else
            MoveDown();
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * (speed * Time.deltaTime));

        if (transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * (speed * Time.deltaTime));

        if (transform.position.y > 8f)
        {
            Destroy(this.gameObject);
        }
    }

    public void AssingEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !_isEnemyLaser) return;
        var player = other.GetComponent<Player>();
        player.TakeDamage(1);
    }
}
