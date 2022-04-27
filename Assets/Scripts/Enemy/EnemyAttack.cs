using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    IAEnemy iAEnemy;
    public AudioClip[] screamSound;

    void Start()
    {
        iAEnemy = GetComponentInParent<IAEnemy>();
    }

    public void Attack() //Función que se ejecuta como evento de la animación Attack
    {
        iAEnemy.Attack();
    }
    public void ReviveFromDeath() //Función llamada como evento en la animación ZombieScream
    {
        iAEnemy.Revive();
    }
    public void Scream() //Función llamada como evento en la animación Zombie Scream
    {
        SoundManager.instance.PlaySound(screamSound[Random.Range(0, screamSound.Length)]);
    }
}
