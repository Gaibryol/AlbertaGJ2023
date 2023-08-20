using System.Collections.Generic;

public class AugmentEvents
{
    public class SetAugmentPanelVisibility
    {
        public readonly bool Visible;

        public SetAugmentPanelVisibility(bool visible)
        {
            Visible = visible;
        }
    }

    public class SelectAugment
    {
        public readonly AugmentBase Augment;

        public SelectAugment(AugmentBase augment)
        {
            Augment = augment;
        }
    }
}
