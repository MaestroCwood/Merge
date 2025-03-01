using System.Threading.Tasks;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    private bool isMerged = false; // Флаг для отслеживания, был ли объект уже слиян
    [SerializeField] GameObject[] prefabsObject; // Массив объектов, которые могут быть созданы после слияния
    private static CreateController createController;

    private void Start()
    {
        // Находим CreateController, если он не задан через инспектор
        if (createController == null)
        {
            createController = CreateController.Instance;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если объект уже слиян или с объектом, с которым столкнулись, нельзя слиться
        if (isMerged || collision.GetComponent<MergeController>() == null) return;

        // Получаем другой объект MergeController
        MergeController otherMergeController = collision.GetComponent<MergeController>();

        // Если столкнулись с объектом того же типа, вызываем слияние
        if (otherMergeController != null && otherMergeController.name == gameObject.name)
        {
            HandleMerge(collision.gameObject);
        }
    }

     async void HandleMerge(GameObject otherObject)
    {
        // Если этот объект уже слиян, не делаем ничего
        if (isMerged) return;

        // Логика слияния: создаем новый объект в центре между двумя объектами
        Vector3 mergePosition = (transform.position + otherObject.transform.position) / 2;
        int newObjectIndex = GetNewObjectIndex();  // Получаем новый индекс объекта после слияния

        // Создаем новый объект
        

        // Устанавливаем флаг, что объект слиян
        isMerged = true;

        // Удаляем оба старых объекта
        Destroy(gameObject);
        Destroy(otherObject);
        await Task.Delay(1000);
        createController.MergeCreated(mergePosition, newObjectIndex);
    }

    private int GetNewObjectIndex()
    {
        // Тут можно задать логику, по которой тип объекта зависит от индекса
        // Например, если сливаются два красных объекта, создается зеленый
        if (gameObject.name == "Red" && gameObject.name == "Red") // Пример для красного
        {
            return 1; // Зеленый
        }
        else if (gameObject.name == "Green" && gameObject.name == "Green") // Пример для зеленого
        {
            return 2; // Желтый
        }
        // Добавьте другие слияния по необходимости

        return 0; // Для общего случая, если не подошел ни один
    }
}
