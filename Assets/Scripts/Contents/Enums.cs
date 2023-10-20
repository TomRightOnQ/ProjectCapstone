public static class Enums
{
    public enum INTERACTION_EVENT
    {
        CompleteTask,  // Screen Interaction Event: Complete a task
        None,  // Screen Interaction Event: Doing nothing
    }

    public enum INTERACTION_TYPE
    {
        Next,  // Screen Interaction Type: Go to next chat
        End,  // Screen Interaction Type: End the Interaction
        Claim,  // Screen Interaction Type: Claim something
        None,  // Screen Interaction Type: Doing nothing
        Choice,  // Screen Interaction Type: Showing choices
        Chat,  // Screen Interaction Type: Start a chat
        Teleport,  // Screen Interaction Type: Teleport the player
        StartGame,  // Screen Interaction Type: Bring the player to a game level
    }

    public enum LEVEL_TYPE
    {
        Platformer,  // Level:Platformer Stage
        Dual,  // Level:Battle Stage
        Shooter,  // Level:Weaponed Battle Stage
        Hide,  // Level:Hide
        Track,  // Level:Track
        None,  // Level:World or other
    }

    public enum NONE
    {
        PLACE_HOLDER,  // DEBUG
    }

    public enum SCENE_TYPE
    {
        Battle,  // Scene Type: Battle Scene
        World,  // Scene Type: Wrold Scene
        Outside,  // Scene Type: Non-in-game Scene
    }

    public enum TASK_ACTION
    {
        AddInteraction,  // Task: Add interactions to the targeted NPC
        RemoveInteraction,  // Task: Remove interactions from the targeted NPC
        None,  // Task: Doing nothing
        UnlockNextDay,  // Task: Unlock to the next day
    }

    public enum TASK_TYPE
    {
        Chat,  // Task System: Chat
        Game,  // Task System: Game
    }
}
