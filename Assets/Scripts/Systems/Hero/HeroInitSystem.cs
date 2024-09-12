using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

public class HeroInitSystem : IEcsInitSystem
{
    readonly EcsCustomInject<SceneData> _sceneData = default;
    
    readonly EcsPoolInject<Unit> _unitPool = default;
    readonly EcsPoolInject<ControlledByPlayer> _controlledByPlayerPool = default;
    
    public void Init(IEcsSystems systems)
    {
        var playerEntity = _unitPool.Value.GetWorld().NewEntity();

        ref var unit = ref _unitPool.Value.Add(playerEntity);
        _controlledByPlayerPool.Value.Add(playerEntity);
        
        var playerGo = Object.Instantiate(_sceneData.Value.heroPrefab, Vector3.zero, Quaternion.identity);

        unit.Direction = 0;
        unit.Transform = playerGo.transform;
        unit.Position = Vector3.zero;
        unit.Rotation = Quaternion.identity;
        // тестовые значения.
        unit.Hp = 20;
        unit.Damage = 1;
        unit.MoveSpeed = 1f;
    }
}