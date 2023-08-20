using UnityEngine;

public class GameStateEvents
{
    public class GetWorldCanvas
    {
        public GameObject WorldCanvas;
    }

    public class GetNextSceneName
    {
        public string Name;
    }

    public class SetPlayerControllerState
    {
        public readonly bool Active;

        public SetPlayerControllerState(bool active) { Active = active; }
    }
}
