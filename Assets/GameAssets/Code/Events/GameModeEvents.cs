public class GameModeEvents
{
    public class ChangeScene
    {
        public readonly string SceneName;

        public ChangeScene(string sceneName)
        {
            SceneName = sceneName;
        }
    }


    public class PlayerDeath
    {

    }
}
