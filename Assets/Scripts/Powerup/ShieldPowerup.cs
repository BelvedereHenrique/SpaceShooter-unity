using System.Collections;
using System.Collections.Generic;
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

    public override void Start()
    {
        _movementSpeed = _speed;
    }

    public override void OnPlayerCollision2D(Player player)
    {
        player.ActivateShieldPowerup(_durationInSecs);
        Destroy(this.gameObject);
    }

    public override IEnumerator SpawnRoutine(GameObject container)
    {
        var pos = new Vector3(Random.Range(-10f, 10f), 10, 0);
        var powerUp = Instantiate(this, pos, Quaternion.identity);
        powerUp.transform.SetParent(container.gameObject.transform);
        yield return new WaitForSeconds(Random.Range(_respawnFrequencyFrom, _respawnFrequencyTo));
    }
}
