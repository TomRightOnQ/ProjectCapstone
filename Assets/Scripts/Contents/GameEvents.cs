public static class GameEvent
{
    public enum Event
    {
        // NOTHING
        NONE,
        
        // Custom Events
        EVENT_C_1,
        EVENT_C_2,
        EVENT_C_3,
        EVENT_C_4,
        EVENT_C_5,

        // Player
        PLAYER_DEATH,

        // ChatInteraction
        EVENT_CHAT_BEGIN,
        EVENT_CHAT_END,

        // Level Manager
        EVENT_SCENE_UNLOADED,
        EVENT_SCENE_LOADED,

        // Game Manager
        EVENT_2DGAME_START,
        EVENT_2DGAME_END,
        EVENT_2DGAME_ALLDIR_BEGIN,
        EVENT_2DGAME_ALLDIR_END,

        // Wide Screen
        EVENT_WIDE_SCREEN_BEGIN,
        EVENT_WIDE_SCREEN_END,

        // Rotational Damage Taker
        EVENT_2DGAME_ROTDMG_ON,
        EVENT_2DGAME_ROTDMG_OFF,
        EVENT_2DGAME_ROTDMG_DMG,
        EVENT_2DGAME_ROTDMG_HEAL,

        // Shooter Level
        SHOOTER_LEVEL_BEGIN,

        // Time Control
        TIME_MORNING,
        TIME_NOON,
        TIME_SUNSET,
        TIME_NIGHT,
        TIME_DARK,
    }
}
