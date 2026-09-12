using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Настройки взаимодействия")]
    [Tooltip("Перетащи сюда Main Camera")]
    [SerializeField] private Transform playerCamera;

    [Tooltip("Максимальная дистанция взаимодействия")]
    [SerializeField] private float interactRange = 3.0f;

    [Tooltip("Слой, на котором находятся предметы")]
    [SerializeField] private LayerMask interactableLayerMask;

    [Header("Точка удержания")]
    public Transform holdPoint;

    // Ссылка на предмет, который прямо сейчас у нас в руках
    private IconPick currentlyHeldItem;
    // Скролл предмета
    public float scrollSpeed = 2f;
    public float minDistance = 0.9f;
    public float maxDistance = 3f;
    private Collider playerCollider;

    private void Awake(){
        playerCollider = GetComponent<Collider>();
        if(playerCollider == null){
            playerCollider = GetComponentInParent<Collider>();
        }
    }

    void Update()
    {
        // 1. Если предмет УЖЕ в руках — при нажатии E опускаем его
        if (currentlyHeldItem != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if(Mathf.Abs(scroll) > 0.01f){
                Vector3 currentPos = holdPoint.localPosition;
                currentPos.z += scroll * scrollSpeed;
                currentPos.z = Mathf.Clamp(currentPos.z, minDistance, maxDistance);
                holdPoint.localPosition = currentPos;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentlyHeldItem.Drop();
                currentlyHeldItem = null; // Руки снова пусты
            }
            return; // Выходим из Update, чтобы не запускать Raycast пока держим вещь
        }

        // 2. Если руки пусты — ищем предмет перед глазами
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactableLayerMask))
        {
            // Проверяем, есть ли на объекте скрипт IconPick
            if (hit.collider.TryGetComponent<IconPick>(out IconPick item))
            {
                Debug.Log(item.GetDescription());

                if (Input.GetKeyDown(KeyCode.E))
                {
                    item.PickUp(holdPoint, playerCollider); // Передаем точку HoldPoint в предмет
                    currentlyHeldItem = item; // Запоминаем текущий предмет
                }
            }
        }
    }
}