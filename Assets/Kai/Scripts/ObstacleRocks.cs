using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ROCK_TYPE{
    Clusters,
    Whole
}

public class ObstacleRocks : MonoBehaviour
{

    [SerializeField] public ROCK_TYPE rock;

    // Update is called once per frame
    void Update(){

    }

    void Damage(ROCK_TYPE rt, Collider2D col) {
        if (col.GetComponent<PlayerController>() == null)
            return;

        PlayerController shipController = col.GetComponent<PlayerController>();

        switch (rt) {
            case ROCK_TYPE.Whole:
                shipController.GetDamage(shipController.health);
                break;
            case ROCK_TYPE.Clusters:
                shipController.GetDamage(shipController.health);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("COLLIDED?");
        Damage(rock, other);
    }
}
