using UnityEngine;

public class CreateController : MonoBehaviour
{
    public static CreateController Instance;
    [SerializeField] GameObject[] prefabsObject;
    [SerializeField] Transform lineCreated;
    [SerializeField] SpriteRenderer nextObject;
    private GameObject created;

    private int nextObjectIndex;

    private float minX, maxX; // Переменные для границ экрана

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Изначально показываем спрайт первого случайного объекта
        SetNextObject();

        // Получаем границы экрана в мировых координатах
        Vector3 screenBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)); // Левый нижний угол
        Vector3 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)); // Правый верхний угол

        minX = screenBottomLeft.x; // Минимальная X-координата (левая граница)
        maxX = screenTopRight.x; // Максимальная X-координата (правая граница)
    }

    private void Update()
    {
        Vector3 posMouse = GetMousePosition();

        // Если нажали на экран, создаем объект
        if (Input.GetMouseButtonDown(0))
        {
            CreateObject(posMouse);
        }

        // Если удерживаем кнопку мыши, перемещаем объект
        if (Input.GetMouseButton(0) && created != null)
        {
            // Ограничиваем движение объекта по оси X
            float clampedX = Mathf.Clamp(posMouse.x, minX, maxX); // Останавливаем объект на границах экрана
            created.transform.position = new Vector3(clampedX, posMouse.y, posMouse.z);
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posMouse.z = 0;
        posMouse.y = lineCreated.position.y;
        return posMouse;
    }

    void CreateObject(Vector3 pos)
    {
        // Создаем объект по текущему индексу
        created = Instantiate(prefabsObject[nextObjectIndex], pos, Quaternion.identity);

        // После создания объекта обновляем следующий
        SetNextObject();

        // Если объект имеет Rigidbody2D, обнуляем скорость и угловую скорость
        Rigidbody2D rb = created.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    void SetNextObject()
    {
        // Случайным образом выбираем новый объект
        nextObjectIndex = Random.Range(0, prefabsObject.Length);

        // Обновляем спрайт в nextObject
        Sprite image = prefabsObject[nextObjectIndex].GetComponent<SpriteRenderer>().sprite;
        nextObject.sprite = image;
    }

    public void MergeCreated(Vector3 pos, int id)
    {
        // Создаем новый объект по переданному ID (типу) и позиции
        Instantiate(prefabsObject[id], pos, Quaternion.identity);
    }

    public void HandleMerge(GameObject obj1, GameObject obj2)
    {
        // Определяем ID (или тип) объектов, которые слились
        int obj1Index = System.Array.IndexOf(prefabsObject, obj1);
        int obj2Index = System.Array.IndexOf(prefabsObject, obj2);

        // Если они одинаковы, создаем новый объект
        if (obj1Index == obj2Index)
        {
            // В зависимости от типа создаем новый объект. Например, для красных создаем зеленый
            int newObjectIndex = GetNewObjectIndex(obj1Index);  // Получаем индекс нового объекта по правилам слияния
            Vector3 mergePosition = (obj1.transform.position + obj2.transform.position) / 2;  // Создаем объект между двумя объектами
            MergeCreated(mergePosition, newObjectIndex);

            // Удаляем старые объекты
            Destroy(obj1);
            Destroy(obj2);
        }
    }

    private int GetNewObjectIndex(int objectIndex)
    {
        // Тут будет логика, определяющая новый объект в зависимости от типа
        // Например, если два красных объекта сливаются в зеленый, то мы возвращаем индекс зеленого объекта
        switch (objectIndex)
        {
            case 0: // Красный
                return 1; // Зеленый
            case 1: // Зеленый
                return 2; // Желтый
            case 2: // Желтый
                return 3; // Синий
            default:
                return objectIndex; // В случае, если нет слияния
        }
    }
}
