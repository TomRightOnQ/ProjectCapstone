public static class GameEvent
{
    public enum Event
    {
        // NOTHING
        NONE = 0,

        // Custom Events
        EVENT_C_1 = 1,
        EVENT_C_2 = 2,
        EVENT_C_3 = 3,
        EVENT_C_4 = 4,
        EVENT_C_5 = 5,

        // Player
        PLAYER_DEATH = 6,

        // ChatInteraction
        EVENT_CHAT_BEGIN = 7,
        EVENT_CHAT_END = 8,

        // Level Manager
        EVENT_SCENE_UNLOADED = 9,
        EVENT_SCENE_LOADED = 10,

        // Game Manager
        EVENT_2DGAME_START = 11,
        EVENT_2DGAME_END = 12,
        EVENT_2DGAME_ALLDIR_BEGIN = 13,
        EVENT_2DGAME_ALLDIR_END = 14,

        // Wide Screen
        EVENT_WIDE_SCREEN_BEGIN = 15,
        EVENT_WIDE_SCREEN_END = 16,

        // Rotational Damage Taker
        EVENT_2DGAME_ROTDMG_ON = 17,
        EVENT_2DGAME_ROTDMG_OFF = 18,
        EVENT_2DGAME_ROTDMG_DMG = 19,
        EVENT_2DGAME_ROTDMG_HEAL = 20,

        // Shooter Level
        SHOOTER_LEVEL_BEGIN = 21,

        // Time Control
        TIME_MORNING = 22,        
        TIME_NOON = 23,
        TIME_SUNSET = 24,
        TIME_NIGHT = 25,
        TIME_DARK = 26,

        // Graphic Settings Applied
        GRAPHICS_LEVEL_LOW = 27,
        GRAPHICS_LEVEL_MEDIUM = 28,
        GRAPHICS_LEVEL_HIGH = 29,
        GRAPHICS_LEVEL_CHANGED = 30,
    }
}
