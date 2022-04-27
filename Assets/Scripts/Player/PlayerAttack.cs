using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damageToEnemy;
    public float radiusSphereCast;

    public Transform sphereCastSpawn;
    public LayerMask zombieMask;
    public GameObject weaponHolder;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();   
    }

    public void DamageWeapon(int damageWeapon) //Función para saber el daño del arma actual llamada desde PickUpWeapon
    {
        damageToEnemy = damageWeapon;
    }
    public void AttackWeapon() //Función llamada en la animación de Attack
    {
        //Esfera para que cuando se dañe al enemigo este pase al estado de persecución
        Collider[] zombie = Physics.OverlapSphere(sphereCastSpawn.position, radiusSphereCast, zombieMask);
        for (int i = 0; i < zombie.Length; i++)
        {
            zombie[i].GetComponent<IAEnemy>().OnHit(damageToEnemy);
        }
    }
    private void OnDrawGizmos() //Función para visualizar el radio del ataque
    {
        Gizmos.DrawWireSphere(sphereCastSpawn.position, radiusSphereCast);

    }
}
