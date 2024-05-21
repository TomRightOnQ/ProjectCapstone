public static class Enums
{
    public enum CHARACTER_TYPE
    {
        You,  // Character Type: Player
        Friend,  // Character Type: Friend
        Enemy,  // Character Type: Enemy
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

    public enum NOTE_TYPE
    {
        Note,  // NoteType: Regular information
        Item,  // NoteType: Items
        Report,  // NoteType: Daily Reports
    }

    public enum SCENE_TYPE
    {
        Battle,  // Scene Type: Battle Scene
        World,  // Scene Type: Wrold Scene
        Outside,  // Scene Type: Non-in-game Scene
    }

    public enum TASK_ACTION
    {
        Next,  // Screen Interaction Type: Go to next chat
        End,  // Screen Interaction Type: End the Interaction
        Claim,  // Screen Interaction Type: Claim something
        None,  // Screen Interaction Type: Doing nothing
        Choice,  // Screen Interaction Type: Showing choices
        Chat,  // Screen Interaction Type: Start a chat
        Teleport,  // Screen Interaction Type: Teleport the player
        StartGame,  // Screen Interaction Type: Bring the player to a game level
        ShowReminder,  // Screen Interaction Type: Show a reminder
        EnterActing,  // Screen Interaction Type: Start Acting mode
        ExitActing,  // Screen Interaction Type: End Acting mode
        TriggerTask,  // Screen Interaction Type: TriggerATask
        CompleteTask,  // Screen Interaction Type: Complete a task
        EnterGame,  // Screen Interaction Type: Enter a game
        AddInteraction,  // Task: Add interactions to the targeted NPC
        RemoveInteraction,  // Task: Remove interactions from the targeted NPC
        UnlockNextDay,  // Task: Unlock to the next day
        UnlockHint,  // Task: Unlock a hint
        ChangeNPCPosition,  // Task: Change NPC position
        UnlockInteraction,  // Task: Unlock an Interaction
        LockInteraction,  // Task: Lock an Interaction
        SaveGame,  // Task: Save the Current Game
        UnlockNotes,  // Task: Unlock a regular note
        UnlockItems,  // Task: Unlock am item note
    }

    public enum TASK_STATUS
    {
        NotTriggered,  // Task Status: Not triggered yet
        Triggered,  // Task Status: Already Triggered
        Complete,  // Task Status: Task Accomplished
    }

    public enum TASK_TYPE
    {
        Chat,  // Task System: Chat
        Game,  // Task System: Game
    }
}
