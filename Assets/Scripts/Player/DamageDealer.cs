using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public Enemy_controller enemy_Controller;
    public Collider collider;
    public LayerMask enemylayer;
    bool canDealDamage;
    List<GameObject> hasDealtDamage;

    [SerializeField] float WeaponLength;
    [SerializeField] float WeaponDamage;

    void Start()
    {
        collider = GetComponent<Collider>();
        canDealDamage = false;
        hasDealtDamage=new List<GameObject>();
        enemy_Controller.Enemy_current_Health = 100f;
    }


    void Update()
    {
        if (canDealDamage)
        {
            RaycastHit hit;

            
            if (Physics.Raycast(transform.position, -transform.up, out hit, enemylayer))
            {
                Enemy_controller enemy_Controller = hit.collider.GetComponent<Enemy_controller>();
                if (hit.collider.CompareTag("Enemy") && enemy_Controller.Enemy_current_Health <= 0)
                {
                    //Destroy(hit.collider.gameObject);
                    enemy_Controller.death();
                }
                else
                {
                    enemy_Controller.Enemy_current_Health -= 0.8f;
                    Debug.Log("Enemy health is " + enemy_Controller.Enemy_current_Health);
                }
                //if (!hasDealtDamage.Contains(hit.transform.gameObject))
                //{
                //    print("damage");
                //    hasDealtDamage.Add(hit.transform.gameObject);

                //}
            }
        }
    }
    public void StartDealDamage()
    {
        canDealDamage=true;
        hasDealtDamage.Clear();
    }

    public void EndDealDamage()
    {
        canDealDamage=false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,transform.position - transform.up * WeaponLength);
    }
}

