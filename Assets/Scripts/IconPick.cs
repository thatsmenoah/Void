using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class IconPick : MonoBehaviour
{
    public string itemName = "Предмет";
    public float speedPickUp = 12f;

    private Rigidbody rb;
    public Transform holdPoint;
    private bool isHold = false;
    private Collider itemCollider;
    private Collider playerCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
        playerCollider = GetComponent<CharacterController>();
        if(playerCollider == null){
            playerCollider = GetComponent<Collider>();
        }
    }

    public string GetDescription()
    {
        return isHold ? "Опустить " + itemName : "Взять " + itemName;
    }

    // Взять предмет
    public void PickUp(Transform targetHoldPoint, Collider currentPlayerCollider)
    {
        isHold = true;
        holdPoint = targetHoldPoint; // ИСПРАВЛЕНО: записываем точку удержания
        rb.useGravity = false;       // Отключаем гравитацию
        rb.drag = 10f;              // Гасим скорость
        playerCollider = currentPlayerCollider;
        if(itemCollider != null && playerCollider != null){
            Physics.IgnoreCollision(itemCollider, playerCollider, true);
        };
    }


    // Бросить / опустить предмет
    public void Drop()
    {
        isHold = false;
        holdPoint = null;
        rb.useGravity = true;        // Возвращаем гравитацию
        rb.drag = 1f;
        if(itemCollider != null && playerCollider != null){
            Physics.IgnoreCollision(itemCollider, playerCollider, false);
            playerCollider = null;
        }
    }

    private void FixedUpdate()
    {
        if (isHold && holdPoint != null)
        {
            // Рассчитываем вектор и задаем физическую скорость
            Vector3 direction = holdPoint.position - transform.position;
            rb.velocity = direction * speedPickUp;
        }
    }
}
