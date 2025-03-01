using System.Threading.Tasks;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    private bool isMerged = false; // ���� ��� ������������, ��� �� ������ ��� �����
    [SerializeField] GameObject[] prefabsObject; // ������ ��������, ������� ����� ���� ������� ����� �������
    private static CreateController createController;

    private void Start()
    {
        // ������� CreateController, ���� �� �� ����� ����� ���������
        if (createController == null)
        {
            createController = CreateController.Instance;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ������ ��� ����� ��� � ��������, � ������� �����������, ������ �������
        if (isMerged || collision.GetComponent<MergeController>() == null) return;

        // �������� ������ ������ MergeController
        MergeController otherMergeController = collision.GetComponent<MergeController>();

        // ���� ����������� � �������� ���� �� ����, �������� �������
        if (otherMergeController != null && otherMergeController.name == gameObject.name)
        {
            HandleMerge(collision.gameObject);
        }
    }

     async void HandleMerge(GameObject otherObject)
    {
        // ���� ���� ������ ��� �����, �� ������ ������
        if (isMerged) return;

        // ������ �������: ������� ����� ������ � ������ ����� ����� ���������
        Vector3 mergePosition = (transform.position + otherObject.transform.position) / 2;
        int newObjectIndex = GetNewObjectIndex();  // �������� ����� ������ ������� ����� �������

        // ������� ����� ������
        

        // ������������� ����, ��� ������ �����
        isMerged = true;

        // ������� ��� ������ �������
        Destroy(gameObject);
        Destroy(otherObject);
        await Task.Delay(1000);
        createController.MergeCreated(mergePosition, newObjectIndex);
    }

    private int GetNewObjectIndex()
    {
        // ��� ����� ������ ������, �� ������� ��� ������� ������� �� �������
        // ��������, ���� ��������� ��� ������� �������, ��������� �������
        if (gameObject.name == "Red" && gameObject.name == "Red") // ������ ��� ��������
        {
            return 1; // �������
        }
        else if (gameObject.name == "Green" && gameObject.name == "Green") // ������ ��� ��������
        {
            return 2; // ������
        }
        // �������� ������ ������� �� �������������

        return 0; // ��� ������ ������, ���� �� ������� �� ����
    }
}
