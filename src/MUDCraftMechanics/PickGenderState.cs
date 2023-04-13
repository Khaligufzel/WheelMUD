using WheelMUD.ConnectionStates;
using WheelMUD.Core;
using WheelMUD.Data;
using WheelMUD.Server;

namespace MUDCraftMechanics.CharacterCreation
{
    /// <summary>
    /// The character creation step where the player will pick their gender.
    /// </summary>
    public class PickGenderState : CharacterCreationSubState
    {

        private static readonly OutputBuilder prompt = new OutputBuilder().Append("Pick you character genderTESTTESTTEST: ");

        /// <summary>
        /// Initializes a new instance of the <see cref="PickGenderState"/> class.
        /// </summary>
        /// <param name="session">
        /// The session.
        /// </param>
        public PickGenderState(Session session) : base(session)
        {
            Session.WriteLine("You will now pick your character's gender.");          

        }

        /// <summary>
        /// ProcessInput is used to receive the user input during this state.
        /// </summary>
        /// <param name="command">The command text to be processed.</param>
        public override void ProcessInput(string command)
        {
            Session.Thing.Gender = command;
            StateMachine.HandleNextStep(this, StepStatus.Success);
        }


        public override OutputBuilder BuildPrompt()
        {
            return prompt;
        }
    }
}