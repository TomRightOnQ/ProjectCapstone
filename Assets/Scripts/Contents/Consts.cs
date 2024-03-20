public static class Constants
{
    // 3C
    public const float DASH_CD_PLAYER = 1f;  // Dash CD for player
    public const float CAMERA_PAN_SPEED = 10f;  // Speed of Camera Pan according to the player's movement
    public const float CAMERA_PAN_OFFSET = 0.9f;  // 
    public const float CAMERA_SNAP_THRESHOLD = 0.01f;  // 
    public const float CAMERA_ROTATED_THRESHOLD = 0.1f;  // 
    public const float PLAYER_JUMP_DETECT_RADIUS = 0.01f;  // From how far we should consider the player on the ground

    // Audio
    public const string SOUND_UI_CONFIRM = "Btn Confirm";  // UI Audio
    public const string SOUND_UI_CANCEL = "Btn Cancel";  // UI Audio
    public const string SOUND_UI_OPEN = "UI Open";  // UI Audio
    public const string SOUND_UI_CLOSE = "UI Close";  // UI Audio

    // C1
    public const int TEST_INT_C1 = 1;  // A normal value
    public const string TEST_STRING_C1 = "test";  // A normal value

    // C2
    public const string TEST_STRING_C2 = "2";  // A normal value

    // C3
    public const int TEST_INT_C3 = 3;  // A normal value
    public const string TEST_STRING_C3 = "unreal";  // A normal value

    // Note
    public const string NOTES_PREFIX = "You have unlocked a new piece of note: ";  // Reminding new note
    public const string ITEMS_PREFIX = "You have unlocked a new item: ";  // Reminding new item

    // Player
    public const float PLAYER_DAMAGE_CD = 0.25f;  // CD for player taking another damage

    // Reminder
    public const float REMINDER_LEVEL_TIME = 2f;  // Life time of the level reminder
    public const float REMINDER_LEVEL_FADE_TIME = 0.5f;  // Time for the level remidner to fade out

    // Resource
    public const string NOTES_SOURCE_PATH = "Texts/Notes";  // Path for Notes folder
    public const string IMAGES_SOURCE_PATH = "Images";  // Path for Images
    public const string LEVEL2D_TEXT_SOURCE_PATH = "Texts/Level2D";  // Path for 2D Level Info

    // SCENE_NAME
    public const string SCENE_NONE = "None";  // Empty Scene
    public const string SCENE_GAME_INIT = "GameInitScene";  // Game Entry Scene
    public const string SCENE_GAME_LOAD = "GameLoadingScene";  // Game Main Loading Scene
    public const string SCENE_MAIN_MENU = "MainMenu";  // Main Menu
    public const string SCENE_DEFAULT_LEVEL = "DefaultLevel";  // Deafult scene of a level
    public const string SCENE_AUDIENCE_LEVEL = "AudienceLevel";  // Audience Level
    public const string SCENE_AUDIENCELOW_LEVEL = "AudienceLowLevel";  // Audience Level - Lower stands
    public const string SCENE_ENTRANCE_LEVEL = "EntranceLevel";  // Entrance Level
    public const string SCENE_PLATFORMER_LEVEL = "PlatformerLevel";  // Platformer Level
    public const string SCENE_ROOMA_LEVEL = "RoomALevel";  // RoomA Level
    public const string SCENE_GUILD_LEVEL = "GuildScene";  // Guild Level
    public const string SCENE_MATCHING_LEVEL = "MatchingLevel";  // Matching Platform
    public const string SCENE_SHOOTER_LEVEL = "ShooterLevel";  // Shooter Level
    public const string SCENE_CONTROL_LEVEL = "ControlRoomLevel";  // Control Room Level

    // Save
    public const string SAVE_CURRENT_SAVE = "CurrentDaySave";  // Name for the current game save
    public const string SAVE_DAY_SAVE = "DaySave_";  // Prefix of each day save's name
    public const string SAVE_CORE_SAVE = "CoreSave";  // Name for the core save file
}
