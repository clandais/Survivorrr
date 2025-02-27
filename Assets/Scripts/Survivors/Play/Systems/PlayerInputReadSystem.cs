using Latios;
using Survivors.Play.Authoring;
using Survivors.Play.Components;
using Survivors.Play.Input;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Survivors.Play.Systems
{
	public partial class PlayerInputReadSystem : SubSystem
	{ 
		PlayStateInput _playerStateInput;
		
		private bool _attackTriggered;

		protected override void OnCreate()
		{
			_playerStateInput = new PlayStateInput();
			_playerStateInput.Enable();

			_playerStateInput.Player.Attack.performed += AttackPerformed;

		}
		
		void AttackPerformed(InputAction.CallbackContext _) => _attackTriggered = true;

		protected override void OnDestroy()
		{
			_playerStateInput.Player.Attack.performed -= AttackPerformed;
			_playerStateInput.Disable();
		}

		protected override void OnUpdate()
		{
			var actions = _playerStateInput.Player;

			float2 movement      = actions.Move.ReadValue<Vector2>();
			bool   isSprinting   = actions.Sprint.ReadValue<float>() > 0.1f;
			float2 mousePosition = actions.MouseMoved.ReadValue<Vector2>();
			


			var ray = Camera.main.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, 0));
		
			Plane plane = new Plane(Vector3.up, Vector3.zero);
		
			if (plane.Raycast(ray, out float enter))
			{
				Vector3 hitPoint = ray.GetPoint(enter);
				mousePosition = new float2(hitPoint.x, hitPoint.z);
			}
			else
			{
				mousePosition = new float2(0, 0);
			}
			

			var inputState = new PlayerInputState
			{
				Direction = movement,
				IsSprinting = isSprinting,
				MousePosition = mousePosition,
				MainAttackTriggered = _attackTriggered,
			};
			
			_attackTriggered = false;

			foreach (var playerInputState in SystemAPI.Query<RefRW<PlayerInputState>>().WithAll<PlayerTag>())
			{
				playerInputState.ValueRW = inputState;
			}
		}
	}
}