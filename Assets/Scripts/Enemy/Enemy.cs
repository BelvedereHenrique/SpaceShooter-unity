using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {

        [SerializeField]
        private float speed = 4;

        [SerializeField]
        private GameObject laserPrefab = null;
    
        private float _fireRate = 3.0f;
        private float _canFire = -1;
    
        [SerializeField]
        private int damage = 1;

        [SerializeField]
        private int score = 10;

        private AudioSource _audioSource;

        private Player _playerRef;
        private Animator _animator;
        private BoxCollider2D _boxCollider;
        private static readonly int OnEnemyDeath = Animator.StringToHash("OnEnemyDeath");

        private void Start()
        {
            _playerRef = GameObject.Find("Player").GetComponent<Player>();
            if(_playerRef == null)
            {
                Debug.LogError("Null Player reference on Enemy");
            }
            _animator = GetComponent<Animator>();
            if(_animator == null)
            {
                Debug.LogError("Null Animator reference on Enemy");
            }

            _boxCollider = GetComponent<BoxCollider2D>();
            if(_boxCollider == null)
            {
                Debug.LogError("Null BoxCollider2D reference on Enemy");
            }
            _audioSource = GetComponent<AudioSource>();
            if(_audioSource == null)
            {
                Debug.LogError("Null AudioSource reference on Enemy");
            }
        }

        private void Update()
        {
            Move();
        
            if (!(Time.time > _canFire)) return;

            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            var enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            var lasers = enemyLaser.GetComponentsInChildren<Laser>();
            foreach (var t in lasers)
            {
                t.AssingEnemyLaser();
            }

        }

        private void Move()
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if (transform.position.y < -7)
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            var randomX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomX, 8);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag($"EnemyLaser") || other.CompareTag("Asteroid"))
            {
                return;
            }
            if (other.CompareTag("Enemy"))
            {
                Respawn();
                return;
            }

            speed = 0;
            _boxCollider.enabled = false;
            _animator.SetTrigger(OnEnemyDeath);

            if (other.CompareTag("Player"))
            {
                if (_playerRef != null)
                    _playerRef.TakeDamage(damage);
            }

            if (other.CompareTag("Laser"))
            {
                Destroy(other.gameObject);
                if (_playerRef != null)
                    _playerRef.AddScore(score);
            }
            _audioSource.Play();
            Destroy(this.gameObject,2.35f);
        }
    }
}
