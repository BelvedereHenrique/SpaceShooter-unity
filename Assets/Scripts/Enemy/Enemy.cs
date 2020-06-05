using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4;

    [SerializeField]
    private int _damage = 1;

    [SerializeField]
    private int _score = 10;

    private Player _playerRef;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    void Start()
    {
        _playerRef = GameObject.Find("Player").GetComponent<Player>();
        if(_playerRef == null)
        {
            Debug.LogError("Null Player reference on Enemey");
        }
        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.LogError("Null Animator reference on Enemey");
        }

        _boxCollider = GetComponent<BoxCollider2D>();
        if(_boxCollider == null)
        {
            Debug.LogError("Null BoxCollider2D reference on Enemey");

        }
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -7)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        float randomX = Random.Range(-10f, 10f);
        transform.position = new Vector3(randomX, 8);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Respawn();
            return;
        }
        if (other.CompareTag("Asteroid"))
        {
            return;
        }

        _speed = 0;
        _boxCollider.enabled = false;
        _animator.SetTrigger("OnEnemyDeath");

        if (other.CompareTag("Player"))
        {
            if (_playerRef != null)
                _playerRef.TakeDamage(_damage);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (_playerRef != null)
                _playerRef.AddScore(_score);
        }
        Destroy(this.gameObject,2.35f);
    }
}
