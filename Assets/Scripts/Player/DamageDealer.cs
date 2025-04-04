using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    bool canDealDamage;
    List<GameObject> hasDealtDamage;

    [SerializeField] float WeaponLength;
    [SerializeField] float WeaponDamage;

    void Start()
    {
        canDealDamage = false;
        hasDealtDamage=new List<GameObject>();
    }


    void Update()
    {
        if (canDealDamage)
        {
            RaycastHit hit;

            int layerMask = 1 << 9;
            if (Physics.Raycast(transform.position, -transform.up, out hit, layerMask))
            {
                if(!hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    print("damage");
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
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

