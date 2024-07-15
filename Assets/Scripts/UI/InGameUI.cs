using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    GameObject _player;
    PlayerStatus _status;
    WeaponManager _weaponManager;

    //UI ������

    // �ð�
    TextMeshProUGUI _timeSecond;
    float _timer;

    // �������ͽ�
    Slider _hpBar;
    Slider _spBar;

    // ����
    RawImage _weaponIcon;
    public Texture2D[] _weaponImages = new Texture2D[2];
    TextMeshProUGUI _currentBullet;
    TextMeshProUGUI _totalBullet;

    // ���ؼ�
    GameObject _crossHair;

    void Start()
    {
        _player = GameObject.Find("Player");
        _status = _player.GetComponent<PlayerStatus>();
        _weaponManager = _player.GetComponent<WeaponManager>();

        // �ð� ǥ���� ��
        _timeSecond = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        // Hp, Sp ǥ���� ��
        _hpBar = transform.GetChild(1).GetComponent<Slider>();
        _spBar = transform.GetChild(2).GetComponent<Slider>();

        // ���� ���� ǥ���� ��
        _weaponIcon = transform.GetChild(3).GetChild(0).GetComponent<RawImage>();
        _currentBullet = transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        _totalBullet = transform.GetChild(3).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

        // ���ؼ� UI
        _crossHair = transform.GetChild(5).gameObject;
    }

    void Update()
    {
        DisplayLivingTime();
        DisplayHp();
        DisplaySp();
        DisplayWeaponInfo();
    }

    public void DisplayLivingTime()
    {
        // ü���� 0�̸� ���߱�

        _timer += Time.deltaTime;
        _timeSecond.text = ((int)_timer).ToString();
    }

    public void DisplayHp()
    {
        _hpBar.value = _status.Hp / 100;
    }

    public void DisplaySp()
    {
        _spBar.value = _status.Sp / 100;
    }

    public void DisplayWeaponInfo()
    {
        //string weaponTag = _weaponManager.GetCurrentWeaponTag();
        //Debug.Log("���繫��: " + weaponTag);
        //if (weaponTag == "Gun") // ���Ÿ� ������ ���
        //{
        //    if(!_currentBullet.gameObject.activeSelf) _currentBullet.gameObject.SetActive(true);
        //    if(!_totalBullet.gameObject.activeSelf) _totalBullet.gameObject.SetActive(true);
        //    if(!_crossHair.activeSelf) _crossHair.SetActive(true);

        //    DisplayWeaponIcon(1);
        //    DisplayCurrentBullet();
        //    DisplayTotalBullet();
        //}
        //else // ���� ������ ���
        //{
        //    DisplayWeaponIcon(0);
        //    if (_currentBullet.gameObject.activeSelf) _currentBullet.gameObject.SetActive(false);
        //    if (_totalBullet.gameObject.activeSelf) _totalBullet.gameObject.SetActive(false);
        //    if (_crossHair.activeSelf) _crossHair.SetActive(false);
        //}
    }

    public void DisplayCurrentBullet()
    {
        _currentBullet.text =  _weaponManager._weaponList[1].GetComponent<Gun>().GetCurrentBullet().ToString();
    }

    public void DisplayTotalBullet()
    {
        _totalBullet.text = _weaponManager._weaponList[1].GetComponent<Gun>().GetTotalBullet().ToString();
    }

    public void DisplayWeaponIcon(int iconIndex)
    {
        _weaponIcon.texture = _weaponImages[iconIndex];
    }

    public void DisplayConnectedPlayers()
    {

    }
}
