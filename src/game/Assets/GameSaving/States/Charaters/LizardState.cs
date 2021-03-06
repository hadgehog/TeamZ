﻿using ZeroFormatter;

namespace GameSaving.States.Charaters
{
    [ZeroFormattable]
    public class LizardState : CharacterState
    {
        public override MonoBehaviourStateKind Type
        {
            get
            {
                return MonoBehaviourStateKind.Lizard;
            }
        }
    }
}