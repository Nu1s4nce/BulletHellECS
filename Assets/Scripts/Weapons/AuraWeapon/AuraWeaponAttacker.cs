using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(WeaponDamageHandler))]
public class AuraWeaponAttacker : MonoBehaviour
{
    [SerializeField] private int _weaponId;
    
    private TimerService _timer;
    private List<Transform> _targetsList;
    
    private WeaponDamageHandler _weaponDamageHandler;
    
    private ITargetFinder _targetFinder;
    private IHeroProvider _heroProvider;
    private IConfigProvider _configProvider;
    private IProgressService _progressService;

    [Inject]
    public void Construct(IHeroProvider heroProvider, ITargetFinder targetFinder, IConfigProvider configProvider, IProgressService progressService)
    {
        _progressService = progressService;
        _configProvider = configProvider;
        _heroProvider = heroProvider;
        _targetFinder = targetFinder;
    }
    private void Awake()
    {
        _weaponDamageHandler = GetComponent<WeaponDamageHandler>();

        _targetsList = new List<Transform>();
        
        _timer = new TimerService(GetWeaponStats().AttackRate - _progressService.GetHeroData().AttackRateBonus);
        _targetFinder.targetsChanged += UpdateTargetsList;
    }
    
    private void Update()
    {
        _timer.UpdateTimer();
        if (_timer.CheckTimerEnd())
        {
            Attack();
            _timer.ResetTimer();
        }
    }

    private void Attack()
    {
        if(_targetsList.Count <= 0) return;
        foreach (var target in _targetsList.ToList())
        {
            if (Vector3.Distance(_heroProvider.GetHeroPosition(), target.position) 
                <= GetWeaponStats().AttackRange + _progressService.GetHeroData().AttackRangeBonus)
            {
                if(target.gameObject.activeSelf)
                    _weaponDamageHandler.DealDamage(target, GetWeaponStats().Damage);
            }
        }
    }

    private WeaponsConfigData GetWeaponStats()
    {
        return _configProvider.GetWeaponsConfig(_weaponId);
    }

    private void UpdateTargetsList()
    {
        _targetsList = _targetFinder.GetEnemiesList();
    }

    private void OnDestroy()
    {
        _targetFinder.targetsChanged -= UpdateTargetsList;
    }
}
