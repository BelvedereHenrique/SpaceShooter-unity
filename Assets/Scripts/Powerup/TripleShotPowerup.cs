using System.Collections;
using UnityEngine;

public class TripleShotPowerup : Powerup
{
    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private int _durationInSecs = 5;

    [SerializeField]
    private float _respawnFrequencyFrom = 10;

    [SerializeField]
    private float _respawnFrequencyTo = 25;

    [SerializeField]
    private AudioClip _collectSound = null;
    public override void Start()
    {
        base._movementSpeed = _speed;
        _audioClipBase = _collectSound;
    }

    public override IEnumerator SpawnRoutine(GameObject container)
    {
        var pos = new Vector3(Random.Range(-10f, 10f), 10, 0);
        var powerUp = Instantiate(this, pos, Quaternion.identity);
        powerUp.transform.SetParent(container.gameObject.transform);
        yield return new WaitForSeconds(Random.Range(_respawnFrequencyFrom, _respawnFrequencyTo));
    }

    public override void OnPlayerCollision2D(Player player)
    {
        player.ActivateTripleShotPowerup(_durationInSecs);
    }

}
