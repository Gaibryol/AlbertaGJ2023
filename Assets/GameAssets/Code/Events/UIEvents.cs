using System.Collections.Generic;

public class UIEvents
{
    public class SetHealth
    {
        public readonly int Value;

        public SetHealth(int value)
        {
            Value = value;
        }
    }

    public class SetDash
    {
        public readonly int Value;
        public SetDash(int value)
        {
            Value = value;
        }
    }

    public class SetOrbs
    {
        public readonly int Value;
        public SetOrbs(int value)
        {
            Value = value;
        }
    }

    public class SetCombo
    {
        // False = Normal Attack, True = Special Attack
        public readonly List<bool> Combo;

        public SetCombo(List<bool> combo)
        {
            Combo = combo;
        }
    }
}
