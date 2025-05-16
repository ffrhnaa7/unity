using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool dodge;
		public bool sprint;
		public bool attack;
		public bool guard;
		public bool debug;
		public bool strongAttack;

        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputAction.CallbackContext context)
		{
			MoveInput(context.ReadValue<Vector2>());
		}
        public void OnLook(InputAction.CallbackContext context)
		{
			if(cursorInputForLook)
			{
				LookInput(context.ReadValue<Vector2>());
			}
		}

		public void OnDodge(InputAction.CallbackContext context)
		{
			DodgeInput(context.performed);
        }

		public void OnSprint(InputAction.CallbackContext context)
		{
            if (context.performed)
            {
                SprintInput(true);
            }
            else if (context.canceled)
            {
                SprintInput(false);
            }
        }
		public void OnAttack(InputAction.CallbackContext context)
		{
            if (context.performed)
            {
				AttackInput(true);
            }
        }

		public void OnStrongAttack(InputAction.CallbackContext context)
		{
			StrongAttackInput(context.performed);
		}
		public void OnGuard(InputAction.CallbackContext context)
		{
            GuardInput(context.performed);
		}
		public void OnDebug(InputAction.CallbackContext context)
		{
			DebugInput(context.performed);
		}
#endif

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void DodgeInput(bool newDodgeState)
		{
			dodge = newDodgeState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void GuardInput(bool newGuardState)
		{
			guard = newGuardState;
		}

		public void DebugInput(bool newDebugState)
		{
			debug = newDebugState;
		}

        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		private void AttackInput(bool newAttackState)
		{
			attack = newAttackState;
		}
		private void StrongAttackInput(bool newStrongAttackState)
		{
			strongAttack = newStrongAttackState;
		}
	}
	
}