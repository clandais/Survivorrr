using Latios;
using Latios.Systems;
using Latios.Transforms.Systems;
using Survivors.Play.Systems.Debug;
using Survivors.Play.Systems.Initialization;
using Unity.Entities;
using VContainer;

namespace Survivors.Setup.Systems
{

	
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	public partial class InitializationSuperSystem : RootSuperSystem
	{
		protected override void CreateSystems()
		{
			GetOrCreateAndAddUnmanagedSystem<PlayerInitializationSystem>();
		}
	}
	
	public partial class BootstrapSuperSystem : RootSuperSystem
	{
		protected override void CreateSystems()
		{
			GetOrCreateAndAddManagedSystem<GlobalInputReadSystem>();
			GetOrCreateAndAddManagedSystem<PlayerSuperSystem>();
		}
	}

	[UpdateInGroup(typeof(SimulationSystemGroup))]
	[UpdateBefore(typeof(TransformSuperSystem))]
	public partial class AnimationSuperSystem : RootSuperSystem
	{
		protected override void CreateSystems()
		{
			GetOrCreateAndAddManagedSystem<PlayerAnimationSuperSystem>();
		}
	}
	
	[UpdateInGroup(typeof(FixedSimulationSystemGroup))]
	public partial class PhysicsRootSystem : RootSuperSystem
	{

		protected override void CreateSystems()
		{
			GetOrCreateAndAddManagedSystem<PhysicsSuperSystem>();
		}
	}
	
}