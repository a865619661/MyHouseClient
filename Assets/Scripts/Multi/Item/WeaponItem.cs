using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponItem : Item
{
    public Define.WeaponItem _weaponName;
    ItemCylinder _itemCylinder; // ������ ��ȯ�� ��
    void Start()
    {
        _itemType = Define.Item.Weapon;
        _itemCylinder = transform.parent.parent.GetComponent<ItemCylinder>();
        base.InitItem();
    }

    void Update()
    {
        base.Floating();
    }

    /// <summary>
    /// ���� ���� ������ ȹ��
    /// </summary>
    /// <param name="other"></param>
    public void TakeWeaponItem(Collider other)
    {
        PlayerStatus status = other.GetComponent<PlayerStatus>();
        if (status == null || base._itemType != Define.Item.Weapon) return;

        _itemCylinder.GetComponent<PhotonView>().RPC("HideSpawnItem", RpcTarget.AllBuffered);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�������� �����Ÿ� �ȿ� ����");

        WeaponManager playerWeaponManager = other.GetComponent<PlayerStatus>()._weaponManager;
        if(playerWeaponManager != null)
            playerWeaponManager.nearMeleeObject = gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("�������� �����Ÿ� �ȿ� ����");
        WeaponManager playerWeaponManager = other.GetComponent<PlayerStatus>()._weaponManager;

        if (playerWeaponManager == null) return;

        // �÷��̾ �ְ�, ��ó ���� ���� Ž���� �����߰�, ������ �ݱ� ��ư�� ������, ������ ��Ÿ�� �ƴ� ��
        if (playerWeaponManager.nearMeleeObject != null && playerWeaponManager._isPickUp && !_itemCylinder._usedItem)
        {
            TakeWeaponItem(other);
            playerWeaponManager._isUsePickUpWeapon = true;
        }
        playerWeaponManager._isPickUp = false;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("�������� �����Ÿ� ���");
        WeaponManager playerWeaponManager = other.GetComponent<PlayerStatus>()._weaponHolder.GetComponent<WeaponManager>();
        if (playerWeaponManager == null) return;

        playerWeaponManager.nearMeleeObject = null;
    }
}