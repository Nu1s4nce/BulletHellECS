﻿using System;
using UnityEngine;

[Serializable]
public class EnemyConfigData
{
    public int EnemyId;
    public float Speed;
    public int MaxHp;
    public int Damage;
    public float DistanceToAttack;
    public float AttackRate;

    public GameObject EnemyPrefab;
}
