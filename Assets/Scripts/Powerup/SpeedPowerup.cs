using System.Collections;
using UnityEngine;

public class SpeedPowerup : Powerup
{
    [SerializeField]
    private float _speed = 8.0f;

    [SerializeField]
    private float _speedBonus = 2.5f;

    [SerializeField]
    private int _durationInSecs = 5;

    [SerializeField]
    private float _respawnFrequencyFrom = 3;

    [SerializeField]
    private float _respawnFrequencyTo = 10;

    public override void Start()
    {
        _movementSpeed = _speed;
    }

    public override void OnPlayerCollision2D(Player player)
    {
        player.ActivateSpeedPowerup(_durationInSecs, _speedBonus);
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
