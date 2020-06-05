using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShieldPowerup : Powerup
{

    [SerializeField]
    private int _durationInSecs = 5;

    [SerializeField]
    private float _respawnFrequencyFrom = 3;

    [SerializeField]
    private float _respawnFrequencyTo = 10;

    [SerializeField]
    private float _speed = 2.5f;

    [SerializeField]
    private AudioClip _collectSound = null;
    public override void Start()
    {
        _movementSpeed = _speed;
        _audioClipBase = _collectSound;
    }

    public override void OnPlayerCollision2D(Player player)
    {
        player.ActivateShieldPowerup(_durationInSecs);
    }

    public override IEnumerator SpawnRoutine(GameObject container)
    {
        var pos = new Vector3(Random.Range(-10f, 10f), 10, 0);
        var powerUp = Instantiate(this, pos, Quaternion.identity);
        powerUp.transform.SetParent(container.gameObject.transform);
        yield return new WaitForSeconds(Random.Range(_respawnFrequencyFrom, _respawnFrequencyTo));
    }
}
