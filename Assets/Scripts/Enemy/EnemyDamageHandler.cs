﻿using UnityEngine;
using Zenject;

[RequireComponent(typeof(EnemyAnimator))]
public class EnemyDamageHandler : MonoBehaviour, IDamageable
{
    private float _currentHp;
    
    private EnemyAnimator _enemyAnimator;
    
    private IConfigProvider _configProvider;
    private ITargetFinder _targetFinder;
    private IEnemyPoolProvider _enemyPoolProvider;
    private IGameFactory _gameFactory;

    [Inject]
    public void Construct(IConfigProvider configProvider, ITargetFinder targetFinder, IEnemyPoolProvider enemyPoolProvider, IGameFactory gameFactory)
    {
        _gameFactory = gameFactory;
        _enemyPoolProvider = enemyPoolProvider;
        _targetFinder = targetFinder;
        _configProvider = configProvider;
    }
    private void Awake()
    {
        _currentHp = _configProvider.GetEnemyConfig(0).MaxHp;
        _enemyAnimator = GetComponent<EnemyAnimator>();
    }

    private void OnEnable()
    {
        _currentHp = _configProvider.GetEnemyConfig(0).MaxHp;
    }

    public void ApplyDamage(float damage)
    {
        _enemyAnimator.PlayDamageReceive();
        HandleTextPopup(damage);
        _currentHp -= damage;
        
        if (_currentHp <= 0)
        {
            Dead();
            SpawnCollectableAfterDeath();
        }
    }

    private void HandleTextPopup(float dmg)
    {
        Vector3 textPos = new Vector3(transform.position.x, transform.position.y, -2);
        _gameFactory.CreateTextPopup(dmg, textPos);
    }

    private void Dead()
    {
        _targetFinder.RemoveTarget(transform);
        _enemyPoolProvider.ReturnEnemy(gameObject);
    }

    private void SpawnCollectableAfterDeath()
    {
        _gameFactory.CreateCollectable(transform.position);
    }
}