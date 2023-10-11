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
    }

    public enum NONE
    {
        PLACE_HOLDER,  // DEBUG
    }

    public enum TASK_TYPE
    {
        Chat,  // Task System: Chat
    }
}
