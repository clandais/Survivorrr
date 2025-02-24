
using Latios;
using Latios.Kinemation;
using Latios.Mecanim;
using Survivors.Play.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Survivors.Play.Systems
{
	[RequireMatchingQueriesForUpdate]
	[BurstCompile]
	public partial struct PlayerAnimationSystem : ISystem
	{


		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<PlayerTag>();
		}


		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			state.Dependency = new AnimationJob()
				.ScheduleParallel(state.Dependency);

		}

		[WithAll(typeof(PlayerTag))]
		[BurstCompile]
		private partial struct AnimationJob : IJobEntity
		{

			[BurstCompile]
			public void Execute(
				OptimizedSkeletonAspect skeleton,
				MecanimAspect mecanimAspect,
				ref PlayerMotion motion)
			{

				var rotation = motion.Rotation;
				var rotatedVelocity = math.rotate(rotation, motion.Velocity);
				mecanimAspect.SetFloat("VelocityY", rotatedVelocity.z);
				mecanimAspect.SetFloat("VelocityX", rotatedVelocity.x);
			}
		}
	}

}