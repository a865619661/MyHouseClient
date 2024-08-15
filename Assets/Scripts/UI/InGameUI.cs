using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] PlayerStatus _status;
    [SerializeField] WeaponManager _robberWeaponManager;
    [SerializeField] WeaponManager _houseownerWeaponManager;
    WeaponManager _weaponManager;

    //UI 변수들

    // 시간
    [SerializeField] TextMeshProUGUI _timeSecond;
    float _timer;

    // 스테이터스
    [SerializeField] Slider _hpBar;
    [SerializeField] Slider _spBar;

    // 무기
    RawImage _weaponIcon;
    public Texture2D[] _weaponImages = new Texture2D[2];
    [SerializeField] TextMeshProUGUI _currentBullet;
    [SerializeField] TextMeshProUGUI _totalBullet;

    // 조준선
    [SerializeField] GameObject _crossHair;

    void Start()
    {
        // 시간 표시할 곳
        _timeSecond = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        // Hp, Sp 표시할 곳
        _hpBar = transform.GetChild(1).GetComponent<Slider>();
        _spBar = transform.GetChild(2).GetComponent<Slider>();

        // 무기 정보 표시할 곳
        _weaponIcon = transform.GetChild(3).GetChild(0).GetComponent<RawImage>();
        _currentBullet = transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        _totalBullet = transform.GetChild(3).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

        // 조준선 UI
        _crossHair = transform.GetChild(4).gameObject;
    }

    void Update()
    {
        if(_status.Hp > 0)
        {
            DisplayLivingTime();
            DisplayHp();
            DisplaySp();
            DisplayWeaponInfo();
        }
        else
        {
            DisplayOut();
        }
    }

    public void DisplayLivingTime()
    {
        // 체력이 0이면 멈추기
        if (_status.Hp <= 0) return;

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
        _weaponManager = (_status.Role == Define.Role.Robber) ? _robberWeaponManager : _houseownerWeaponManager;
        string weaponTag = _weaponManager._selectedWeapon.tag;
        Debug.Log("현재무기: " + weaponTag);
        if (weaponTag == "Gun") // 원거리 무기일 경우
        {
            if (!_currentBullet.gameObject.activeSelf) _currentBullet.gameObject.SetActive(true);
            if (!_totalBullet.gameObject.activeSelf) _totalBullet.gameObject.SetActive(true);
            if (!_crossHair.activeSelf) _crossHair.SetActive(true);

            DisplayWeaponIcon(1);
            DisplayGunInfo();
        }
        else // 근접 무기일 경우
        {
            DisplayWeaponIcon(0);
            if (_currentBullet.gameObject.activeSelf) _currentBullet.gameObject.SetActive(false);
            if (_totalBullet.gameObject.activeSelf) _totalBullet.gameObject.SetActive(false);
            if (_crossHair.activeSelf) _crossHair.SetActive(false);
        }
    }

    public void DisplayGunInfo()
    {
        _currentBullet.text =  _weaponManager._selectedWeapon.GetComponent<Gun>().GetCurrentBullet().ToString();    // 현재 장정된 탄약
        _totalBullet.text = _weaponManager._selectedWeapon.GetComponent<Gun>().GetTotalBullet().ToString();         // 전체 탄약
    }

    public void DisplayWeaponIcon(int iconIndex)
    {
        _weaponIcon.texture = _weaponImages[iconIndex];
    }

    public void DisplayConnectedPlayers()
    {

    }

    public void DisplayOut()
    {
        if (_status.Hp <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
    }
}

