using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    
    public float useStamina;
    
    [Header("Resource Gathering")]
    public bool doesGatherResources;
    
    [Header("Combat")] 
    public bool doesDealDamage;
    public int damage;
    
    private Animator animator;
    private Camera camera;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }
    
    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate);
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }
    
    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, attackDistance))
        {
            Debug.Log(ray.origin+"   " + ray.direction * attackDistance+ "  " );
            Debug.Log(hit.collider.name);
            if(doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal);
            }

            if (doesDealDamage && hit.collider.TryGetComponent(out IDamagable npc))
            {
                Debug.Log("hitCollider : " + hit.collider.name);
                npc.TakePhysicalDamage(damage);
            }
        }
    }
}
