using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public PlayerMove _playerMove;
    public CameraController _cameraController;

    #region ���� �� �ɷ�ġ, �̸� ��
    public bool _isLocalPlayer;
    public string _nickName;
    [field: SerializeField] public Define.Role Role = Define.Role.None;
    [field: SerializeField] public float Hp { get; set; } = 100;    // ü��
    [field: SerializeField] public float Sp { get; set; } = 100;    // ���׹̳�
    [field: SerializeField] public float MaxHp { get; private set; } = 100; // �ִ� ü��
    [field: SerializeField] public float MaxSp { get; private set; } = 100; // �ִ� ���׹̳�
    [field: SerializeField] public float Defence { get; private set; } = 1; // ����
    #endregion

    #region �ִϸ��̼� �� ����
    public Animator _animator;
    List<Renderer> _renderers;
    #endregion

    public Transform _weaponHolder;
    public Transform[] _weaponHolders;

    //public string nickname;

    //public TextMeshPro nicknameText;

    //public Transform TPWeaponHolder;

    public void IsLocalPlayer()
    {
        //TPWeaponHolder.gameObject.SetActive(false);

        _isLocalPlayer = true;
        _playerMove.enabled = true;         // PlayerMove Ȱ��ȭ
        _cameraController.gameObject.transform.parent.gameObject.SetActive(true);
    }

    [PunRPC]
    public void SetNickname(string _name)
    {
        _nickName = _name;
    }

    [PunRPC]
    public void SetRole(Define.Role role) // ���� ����
    {
        // ������ ���� �Ҵ�
        Debug.Log($"�� ����({_nickName}): " + Role);

        Role = role;

        if (Role == Define.Role.Robber)
            _animator = transform.GetChild(0).GetComponent<Animator>();
        else if (Role == Define.Role.Houseowner)
            _animator = transform.GetChild(1).GetComponent<Animator>();
    }

    [PunRPC]
    public void SetWeapon(int weaponIndex)
    {
        foreach (Transform weapon in _weaponHolder)
        {
            weapon.gameObject.SetActive(false);
        }

        Debug.LogWarning($"�ε��� �ƿ� ����?({_nickName}): " + weaponIndex);

        _weaponHolder.GetChild(weaponIndex).gameObject.SetActive(true);
    }


    /// <summary>
    /// ���
    /// </summary>
    public void Dead()
    {
        if (Role != Define.Role.None && Hp <= 0)
        {
            _animator.SetTrigger("setDie");
            Role = Define.Role.None; // ��ü
            StartCoroutine(DeadSinkCoroutine());
        }
    }

    void Awake()
    {
        // ���� ��������
        _renderers = new List<Renderer>();
        Transform[] underTransforms = GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < underTransforms.Length; i++)
        {
            Renderer renderer = underTransforms[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                _renderers.Add(renderer);
                // if (renderer.material.color == null) Debug.Log("�� ���� ��?");
            }
        }
    }

    void Update()
    {
        //if (!IsLocalPlayer) return;
        Dead();
    }

    /// <summary>
    /// ������ �Ա�
    /// </summary>
    /// <param name="attack"> ���� ���ݷ� </param>
    public void TakedDamage(int attack)
    {
        // ���ذ� ������� ȸ���Ǵ� ������ �Ͼ�Ƿ� ������ ���� 0�̻����� �ǰԲ� ����
        float damage = Mathf.Max(0, attack - Defence);
        Hp -= damage;

        Debug.Log(gameObject.name + "(��)�� " + damage + " ��ŭ ���ظ� �Ծ���!");
        Debug.Log("���� ü��: " + Hp);
    }


    #region ü�� �� ���׹̳� ����
    /// <summary>
    /// �ִ� ü���� 0.2��ŭ ȸ��
    /// </summary>
    public void Heal()
    {
        // ���� ü���� �ִ� ü�º��� ���� ���� ȸ�� ����
        if (Hp < MaxHp)
        {
            // ȸ����
            float healAmount = MaxHp * 0.2f;

            // ȸ������ ���� ü�°��� ���� �ִ� ü���� ���� �ʵ��� ����
            float healedAmount = Mathf.Clamp(Hp + healAmount, 0, MaxHp) - Hp;

            Debug.Log("���� ü��" + Hp);
            // ü�� ȸ��
            Hp += healedAmount;
            Debug.Log("ü���� " + healedAmount + "��ŭ ȸ��!");
            Debug.Log("���� ü��: " + Hp);
        }
        else
        {
            Debug.Log("�ִ� ü��. ȸ���� �ʿ� ����.");
        }
    }

    /// <summary>
    /// �ִ� ���׹̳����� ���� ȸ��
    /// </summary>
    public void SpUp()
    {
        // ���� ���׹̳��� �ִ� ���׹̳����� ���� ���� ȸ�� ����
        if (Sp < MaxSp)
        {
            // ȸ������ ���� ���׹̳����� ���� �ִ� ���׹̳��� ���� �ʵ��� ����
            float healedAmount = Mathf.Clamp(Sp + MaxSp, 0, MaxHp) - Sp;

            Debug.Log("���� ���׹̳�" + Sp);
            // ���׹̳� ȸ��
            Sp += healedAmount;
            Debug.Log("���� ȸ��! ���� Sp: " + Sp);
        }
        else
        {
            Debug.Log("�ִ� Sp. ȸ���� �ʿ� ����.");
        }
    }

    /// <summary>
    /// ���׹̳� ��������
    /// </summary>
    public void ChargeSp()
    {
        Sp += Time.deltaTime * 20;
        Sp = Mathf.Clamp(Sp, 0, MaxSp);
    }

    /// <summary>
    /// ���׹̳� ���̱�
    /// </summary>
    public void DischargeSp()
    {
        Sp -= Time.deltaTime * 20;
        Sp = Mathf.Clamp(Sp, 0, MaxSp);
    }

    /// <summary>
    /// ������, ���׹̳� ����
    /// </summary>
    public void JumpSpDown() => Sp -= 3;

    /// <summary>
    /// ���� ����
    /// </summary>
    public void DefenceUp()
    {

    }
    #endregion


    /// <summary>
    /// ���������� ����
    /// </summary>
    [PunRPC]
    public void TransformIntoRobber()
    {
        transform.GetChild(0).gameObject.SetActive(true); // ���� ��Ȱ��ȭ
        transform.GetChild(1).gameObject.SetActive(false);  // ������ Ȱ��ȭ

        Debug.Log("���� ����: " + Role);

        _cameraController.gameObject.GetComponent<CameraController>().SetRobberView(); // ������ �������� ����

        Debug.Log(gameObject.GetComponent<PlayerStatus>()._nickName + "(��)�� ������ ���� �Ϸ�");
    }


    /// <summary>
    /// ���������� ����
    /// </summary>
    [PunRPC]
    public void TransformIntoHouseowner()
    {
        transform.GetChild(0).gameObject.SetActive(false); // ���� ��Ȱ��ȭ
        transform.GetChild(1).gameObject.SetActive(true);  // ������ Ȱ��ȭ

        Debug.Log($"���� ����({transform.root.GetChild(2).GetComponent<PlayerStatus>()._nickName}): " + Role);

        _cameraController.gameObject.GetComponent<CameraController>().SetHouseownerView(); // ������ �������� ����

        Debug.Log(gameObject.GetComponent<PlayerStatus>()._nickName + "(��)�� ���������� ���� �Ϸ�");
    }

    /// <summary>
    /// ��ü �ٴ����� ����ɱ�
    /// </summary>
    /// <returns></returns>
    IEnumerator DeadSinkCoroutine()
    {
        yield return new WaitForSeconds(3f);
        while (transform.position.y > -1.5f)
        {
            transform.Translate(Vector3.down * 0.1f * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// ���� ������ Material �Ӱ� ��ȭ
    /// </summary>
    public void HitChangeMaterials()
    {
        // �±װ� ���� �Ǵ� ����

        for (int i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].material.color = Color.red;
            Debug.Log("�����Ѵ�.");
            Debug.Log(_renderers[i].material.name);
        }

        StartCoroutine(ResetMaterialAfterDelay(1.7f));

        //Debug.Log($"�÷��̾ {other.transform.root.name}���� ���� ����!");
        Debug.Log("���ݹ��� ���� ü��:" + Hp);
    }

    /// <summary>
    /// ���� �ް� Material ������� ����
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator ResetMaterialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < _renderers.Count; i++)
            _renderers[i].material.color = Color.white;
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    //// �ڱ� �ڽſ��� ���� ��� ����
    //    //if (other.transform.root.name == gameObject.name) return;
    //    if (other.tag == "Melee" || other.tag == "Gun" || other.tag == "Monster")
    //        HitChangeMaterials();
    //}



    //void OnTriggerEnter(Collider other)
    //{
    //    if (!IsServer) return;

    //    //if (other.tag == "Melee" || other.tag == "Gun" || other.tag == "Monster")
    //    //    HitChangeMaterials();


    //    Debug.Log(NetworkGameManager.instance.GetUsernameFromClientId(OwnerClientId) + "�� ����");

    //    Debug.Log(transform.root.GetComponent<NetworkObject>().OwnerClientId + "�� " + other.transform.root.GetComponent<NetworkObject>().OwnerClientId + "���� �ǵ帲!");
    //    // Ŭ���̾�Ʈ ID�� �޶�� ��
    //    if (other.gameObject.CompareTag("Melee") && transform.root.GetComponent<NetworkObject>().OwnerClientId != other.transform.root.GetComponent<NetworkObject>().OwnerClientId)
    //    {
    //        // GetComponent<PlayerStatus>().hp.Value -= 50;

    //        Debug.Log(transform.root.GetComponent<NetworkObject>().OwnerClientId + "�� " + other.transform.root.GetComponent<NetworkObject>().OwnerClientId + "���� ����!");

    //        PlayAttackedMaterialsClientRpc();   // �ִϸ��̼� ����϶�� ����
    //    }
    //}

    //[ServerRpc]
    //void PlayAttackedMaterialsServerRpc()
    //{
    //    PlayAttackedMaterialsClientRpc();
    //}

    //[ClientRpc]
    //void PlayAttackedMaterialsClientRpc()
    //{
    //    HitChangeMaterials();
    //}

    //public void SetRoleAnimator(RuntimeAnimatorController animController, Avatar avatar)
    //{
    //    _animator.runtimeAnimatorController = animController;
    //    _animator.avatar = avatar;

    //    // �ִϸ����� �Ӽ� ��ü�ϰ� ���ٰ� �Ѿ� ������
    //    _animator.enabled = false;
    //    _animator.enabled = true;
    //}

    public void ChangeIsHoldGun(bool isHoldGun)
    {
        if (Role != Define.Role.Houseowner) return;
        _animator.SetBool("isHoldGun", isHoldGun);
    }
}