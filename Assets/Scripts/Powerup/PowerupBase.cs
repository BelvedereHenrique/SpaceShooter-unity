using System.Collections;
using System.Security.Cryptography;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    [SerializeField]
    public virtual float _movementSpeed { get; set; }

    public virtual AudioClip _audioClipBase { get; set; }

    public virtual void Start()
    {
    }

    void Update()
    {
        HandleMovement();
    }

    public virtual void HandleMovement()
    {
        transform.Translate(Vector3.down * _movementSpeed * Time.deltaTime);
        HandleOnOutOfBoundsDestroy();
    }

    public virtual void HandleOnOutOfBoundsDestroy()
    {
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                OnPlayerCollision2D(player);
                if(_audioClipBase != null)
                    AudioSource.PlayClipAtPoint(_audioClipBase, gameObject.transform.position);

                Destroy(this.gameObject);
            }
        }
    }

    public abstract void OnPlayerCollision2D(Player player);

    public abstract IEnumerator SpawnRoutine(GameObject container);
}
