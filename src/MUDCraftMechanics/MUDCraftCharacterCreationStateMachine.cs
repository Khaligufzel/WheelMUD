// <copyright file="MUDCraftCharacterCreationStateMachine.cs" company="WheelMUD Development Team">
//   Copyright (c) WheelMUD Development Team.  See LICENSE.txt.  This file is
//   subject to the Microsoft Public License.  All other rights reserved.
// </copyright>
//-----------------------------------------------------------------------------
using System;
using WheelMUD.ConnectionStates;
using WheelMUD.Core;

namespace MUDCraftMechanics.CharacterCreation
{
    /// <summary>Default state machine for creating a new character.</summary>
    [ExportCharacterCreationStateMachine(1000)]
    public class MUDCraftCharacterCreationStateMachine : CharacterCreationStateMachine
    {
        /// <summary>Initializes a new instance of the <see cref="MUDCraftCharacterCreationStateMachine"/> class.</summary>
        /// <param name="session">The session.</param>
        public MUDCraftCharacterCreationStateMachine(Session session)
            : base(session)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MUDCraftCharacterCreationStateMachine"/> class.</summary>
        public MUDCraftCharacterCreationStateMachine()
            : this(null)
        {
        }

        /// <summary>Gets the next step in the creation process.</summary>
        /// <param name="current">The current (just executed step)</param>
        /// <param name="previousStatus">Whether the current step passed or failed</param>
        /// <returns>The next step in the character creation process, or null if it is finished</returns>
        public override CharacterCreationSubState GetNextStep(CharacterCreationSubState current, StepStatus previousStatus)
        {
            // If there is no state yet, set up the initial character creation state.
            if (current == null)
            {
                return new ConfirmCreationEntryState(Session);
            }

            // Otherwise either go forward to the next state, or back to the previous state if requested.
            return previousStatus == StepStatus.Success ? AdvanceState(current) : RegressState(current);
        }

        private CharacterCreationSubState AdvanceState(CharacterCreationSubState current)
        {
            // This character creation state machine can return actual creation state objects - if someone
            // were to expand and add new creation state(s) that are not MUD-agnostic, then they should also
            // add and use their own CreationStateMachine handling those states instead of this default one;
            // they could of course reuse some/all of the states below in addition to their own.
            if (current is ConfirmCreationEntryState)
            {
                return new GetNameState(Session);
            }
            else if (current is GetNameState)
            {
                return new GetPasswordState(Session);
            }
            else if (current is GetPasswordState)
            {
                return new ConfirmPasswordState(Session);
            }
            else if (current is ConfirmPasswordState)
            {
                return new PickGenderState(Session);
            }
            else if (current is PickGenderState)
            {
                return new GetDescriptionState(Session);
            }

            else if (current is GetDescriptionState)
            {
                // We are done with character creation!
                return null;
            }

            throw new InvalidOperationException("The character state machine does not know how to calculate the next step after '" + current.GetType().Name + "' succeeds");
        }

        private CharacterCreationSubState RegressState(CharacterCreationSubState current)
        {
            if (current is ConfirmPasswordState)
            {
                // If password confirmation failed, try selecting a new password.
                return new GetPasswordState(Session);
            }

            throw new InvalidOperationException("The character state machine does not know how to calculate the next step after '" + current.GetType().Name + "' fails");
        }
    }
}
