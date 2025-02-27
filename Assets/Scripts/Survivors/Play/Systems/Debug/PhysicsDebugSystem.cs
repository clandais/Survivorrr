using Latios;
using Latios.Psyshock;
using Latios.Transforms;
using Survivors.Play.Authoring;
using Unity.Burst;
using Unity.Entities;

namespace Survivors.Play.Systems.Debug
{
	[RequireMatchingQueriesForUpdate]
	public partial struct PhysicsDebugSystem : ISystem
	{

		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<LevelTag>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{

			var layer = state.GetLatiosWorldUnmanaged().sceneBlackboardEntity.GetCollectionComponent<EnvironmentCollisionLayer>();
			state.Dependency = PhysicsDebug.DrawLayer(layer.Layer).ScheduleParallel(state.Dependency);

			foreach (var (collider, transformAspect) in SystemAPI.Query<RefRO<Collider>, TransformAspect>().WithAll<LevelTag>())
			{
				var transform = transformAspect.worldTransform;
				PhysicsDebug.DrawCollider( in collider.ValueRO, in transform, UnityEngine.Color.green);
			}
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state)
		{

		}
	}
}