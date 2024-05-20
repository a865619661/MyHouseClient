using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float floatHeight = 0.5f; // �������� ���ٴϴ� ����
    public float floatSpeed = 1.0f;  // �������� ���ٴϴ� �ӵ�
    public float rotateSpeed = 30f;  // �������� ȸ�� �ӵ�
    public float floatScale = 0.1f;  // sin �Լ��� ��ȯ ���� ������ �����ϸ� ����, �ϸ��ϰ� �����̰� �Ϸ��� ���

    // �� ������Ʈ�ϱ� �ڽ� ������Ʈ�� �ִ� Mesh �������� ���� ����
    Transform childMesh;

    // ������ Ÿ��
    [SerializeField]
    protected Define.Item itemType;

    void Start()
    {
        ItemInit();
    }

    void Update()
    {
        Floating();
    }


    // ������ �ʱ� ����
    protected void ItemInit()
    {
        // Mesh�� �����´�.
        childMesh = transform.GetChild(0);

        // SphereCollider ����
        SphereCollider itemCollider = GetComponent<SphereCollider>();
        if(itemCollider == null)
        {
            gameObject.AddComponent<SphereCollider>();
            itemCollider = GetComponent<SphereCollider>();
        }
        itemCollider.isTrigger = true;
        itemCollider.radius = 35f;
    }

    // ������ ���ڸ��� ���ٴϱ�
    protected void Floating()
    {
        // �������� ȸ�� (���� ��ǥ ����)
        // �ڱ� ��ġ�� �������� ���� ��ǥ ����(Vector3.up) ȸ��
        childMesh.RotateAround(childMesh.position, Vector3.up, rotateSpeed * Time.deltaTime);

        // childMesh.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        // �������� ���Ʒ��� ���ٴ� ����
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatScale + floatHeight;
        childMesh.position = new Vector3(childMesh.position.x, newY, childMesh.position.z);
    }
}