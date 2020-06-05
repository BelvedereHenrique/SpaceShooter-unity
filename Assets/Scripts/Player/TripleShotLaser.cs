using UnityEngine;

public class TripleShotLaser : MonoBehaviour
{
    void Update()
    {
        if(gameObject.transform.childCount <= 0){
            Destroy(this.gameObject);
        }
    }
}
