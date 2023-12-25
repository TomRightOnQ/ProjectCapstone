Using this Markdown file:

1. Paste this output into your source file.
2. See the notes and action items below regarding this conversion run.
3. Check the rendered output (headings, lists, code blocks, tables) for proper
   formatting and use a linkchecker before you publish this page.

Conversion notes:

* Docs to Markdown version 1.0β35
* Sun Dec 24 2023 17:03:50 GMT-0800 (PST)
* Source doc: Documentation
* This document has images: check for >>>>>  gd2md-html alert:  inline image link in generated source and store images to your server. NOTE: Images in exported zip file from Google Docs may not appear in  the same order as they do in your doc. Please check the images!


WARNING:
You have 5 H1 headings. You may want to use the "H1 -> H2" option to demote all headings by one level.

----->


<p style="color: red; font-weight: bold">>>>>>  gd2md-html alert:  ERRORs: 0; WARNINGs: 1; ALERTS: 3.</p>
<ul style="color: red; font-weight: bold"><li>See top comment block for details on ERRORs and WARNINGs. <li>In the converted Markdown or HTML, search for inline alerts that start with >>>>>  gd2md-html alert:  for specific instances that need correction.</ul>

<p style="color: red; font-weight: bold">Links to alert messages:</p><a href="#gdcalert1">alert1</a>
<a href="#gdcalert2">alert2</a>
<a href="#gdcalert3">alert3</a>

<p style="color: red; font-weight: bold">>>>>> PLEASE check and correct alert issues and delete this message and the inline alerts.<hr></p>



# **Project: FlashBack**


## ReadME & Code Documentation

24 December 2023

**Welcome to Project: FlashBack, an unnamed undergraduate Capstone project.**

Right now the game is halfway through, and we are about to add all game contents such as the main story based on the complete framework.

Game Genre:

Story-based 2D/2.5D Fantasy RPG+Visual Novel

Control:

WASD to move in any scene, with mouse clicking to control - the shortcut’s priority is not as high at the current stage.

In a 2D scene: AD to move left or right, Space to jump, and LShift to dash

Left Mouse to fire, and Q skill is not complete yet

**Note on Dec 24: The demo in Release can show a bare vertical slice of the game’s future, the current project cloned to Unity Editor may not work properly since we are refactoring some codes that are still in-progress.**


# Basic Structure

**Assets/Scripts/Contents:**

*Config.cs: 

Scriptable singleton source files.

Consts.cs, Enums.cs, and all source files under General folder: 

	Files automatically generated from Excel and csv sheets via Python Scripts

**Assets/Scripts/FrameWork and Assets/Scripts/GamePlay:**

Game logic codes

**Assets/Designs:**

*.csv: 

Enums and Constants data

Tools folder: 

Python scripts and .bat files to run the scripts -> load all sheets into C# source files for game configurations.

General folder:

	.xlsx sheets holding information on levels, NPCs, interactions, etc


# Singleton Managers

Each logic system contains a manager class as a singleton controller monitoring the system; each singleton manager must be attached to an empty game object when the game starts, except the temporary managers for 2D levels.

All *Config.cs must have a scriptable object asset created under Resource folder.


# FrameWork Systems:

**Pooling System**

PrefabConfig: manages the references of prefabs with metadata.

public struct PrefabData

{

    public string name;

    public string PrefabPath;

    public string TypeName;

    public int Count;

    public bool IsExpandable;

    public float ExpandableRatio;

    public bool bPoolable;

    public bool bPoolByDefault;

    public GameObject PrefabReference;

}

PrefabManager: manages the references of all objects, communicating with the pooling system.

**SetPoolUp(string typeName, GameObject prefabReference, int count, bool isExpandable, float expandableRatio): **

Add a type of object to the pooling system.

**InitPooling(): **

Initialize the pooling system according to the PrefabConfig, which some prefabs are pooled by default on scene loaded, such as the GameObject carrying sound effects.

**PrefabManager.Instantiate(string prefabName, Vector3 position, Quaternion rotation)**

**PrefabManager.Destroy(GameObject gameObject)**

Overloaded versions of the native Instantiate/Destroy; prefabs instantiate/destroy via this method must be defined in the PrefabConfig scriptable object asset. If the prefab supports pooling, then this method will check the pooling status and deal with the object, otherwise, it will use the native methods.

**Level System**

LevelConfig: controls how scenes/levels are defined.

Level2DData: holds additional metadata for a 2D battle scene and comes from Level2D.xlsx automatically.

public struct LevelData

{

    public int LevelID;

    public string SceneName;

    public string StringName; // Actual name

    public string NextScene; // The next scene after this level - used for 2D levels only

    public Enums.SCENE_TYPE SceneType;

    public bool bSaveable; // Whether the player in this scene will be saved

    public List&lt;Vector3> SpawnPoints;

    public string BGMName;

}    

public class Level2DDataStruct

{

        public int ID;

        public string Name;

        public string SceneName;

        public Enums.LEVEL_TYPE Type;

        public string TypeText;

        public int GroupID;

        public float TimeLimit;

        public int[] Hints;

        public string IntroText;

        public string IconPath;

        public string Next;

        public int[] Score;

        public int TaskComplete;

}

LevelManager: manages the scene loading and unloading with save and metadata supported.

**LevelManager.LoadScene(int id)**

**LevelManager.LoadScene(string sceneName):**

An async scene loading method wrapped upon the native scene management method.

**GameEffect System**

GameEffectConfig: base class of the effect config scriptable objects

Audio/Anim/VFXConfig: child classes of GameEffectConfig for the metadata of game effects

GameEffect: vase class of objects holding the game effects

Audio/Anim/VFXObjects: child classes of GameEffect to hold the component playing a specific type of effect.

LevelManager: controls the effect playing.

**InitGameEffectConfigs():**

Called when the game first starts to initialize dictionaries of string name and effect data

**PlaySound(string _name, Vector3 pos)**

**PlayAnim(string _name, Vector3 pos, Vector3 scale)**

**PlayVFX(string _name, Vector3 pos, Vector3 scale)**

Methods to play a particular effect.

**BGM System -> Unfinished**

MusicConfig: saves the metadata of a music clip

public struct MusicData

{

public string name;

public string path;

public int sceneID;

};

MusicManager: controls the loading and playing of music clips. _For now, it cannot loop the music or automatically switch to the next one._

**PlayMusic(string name)**

**PlayMusicLocal(string name)**

Play a music clip by its name in either the global space or spawn a local audio source

_Currently, it does not support configuring the local audio traits, such as “making a music clip played at position with specific soundness.”_

**PlayRandomMusic(int sceneID)**

Play a piece of random music based on the scene

_Not supported_

**UI System**

UIBase: base class of each game system’s UI object

UIConfig: scriptable singleton to record the reference and record of each UIBase prefab



<p id="gdcalert1" ><span style="color: red; font-weight: bold">>>>>>  gd2md-html alert: inline image link here (to images/image1.png). Store image on your image server and adjust path/filename/extension if necessary. </span><br>(<a href="#">Back to top</a>)(<a href="#gdcalert2">Next alert</a>)<br><span style="color: red; font-weight: bold">>>>>> </span></p>


![alt_text](images/image1.png "image_tooltip")


UIManager: controls the lifecycle of UIBase objects and keeps track of their references.

Each game system with UI has a UI Canvas object with a child class of the UIBase class; this ensures that each system can have decoupled UI components.

To create a new UIBase:



1. Attach a child class of UIBase to the Canvas game object of the UIs, which we define the detail of the UI in this script.
2. Saves the Canvas as a prefab and fills in its information into UIConfig.
3. **No repeating name or type name allowed for UIBase prefabs.**



<p id="gdcalert2" ><span style="color: red; font-weight: bold">>>>>>  gd2md-html alert: inline image link here (to images/image2.png). Store image on your image server and adjust path/filename/extension if necessary. </span><br>(<a href="#">Back to top</a>)(<a href="#gdcalert3">Next alert</a>)<br><span style="color: red; font-weight: bold">>>>>> </span></p>


![alt_text](images/image2.png "image_tooltip")


**public GameObject CreateUI(string uiName)**

Automatically check if the UIBase prefab with the given name is already created; if not, instantiate the UI object according to the metadata and reference gathered from UIConfig. This will return the GameObject of the UI.

**public GameObject ShowUI(string uiName)**

If the UI is not created, it will first call CreateUI with the given uiName parameter, and activate the UI’s GameObject if created successfully. Otherwise, it will activate the GameObject of the UI. This also returns the GameObject.

**public void HideUI(string uiName)**

Deactivate the GameObject of the UI with the given name; if the UI does not exist (such as not created), then it will directly return.

**Suggestions:**



1. Set each UIBase prefab deactivated by default.
2. Since ShowUI and HideUI only affect the UIBase objects’ activated status, and the UIBase objects themselves are only the Canvases of UI components, if a UIBase contains multiple panels, the internal logic of activation and deactivation must be determined by the UIBase’s own logic.

**Save System -> Unfinished**

SaveConfig: a scriptable object acts as the interface between the serialized JSON game save and the current game state data.



<p id="gdcalert3" ><span style="color: red; font-weight: bold">>>>>>  gd2md-html alert: inline image link here (to images/image3.png). Store image on your image server and adjust path/filename/extension if necessary. </span><br>(<a href="#">Back to top</a>)(<a href="#gdcalert4">Next alert</a>)<br><span style="color: red; font-weight: bold">>>>>> </span></p>


![alt_text](images/image3.png "image_tooltip")


SaveManager: provides methods to get and set data to the save, and manages the SaveConfig. _Now the Save System is only a debug version, which each save only lasts for one game session. The actual serialization to JSON is not complete yet._

**public void CreateNewSave()**

Call all methods in SaveConfig with an “Init Method” comment and create a new save.

**Get and Set method -> Depending on each system**

**Create a new data struct in the SaveConfig:**

To reduce system coupling, only SaveManager is allowed to access SaveConfig, and all get or set methods from SaveConfig must have a router in the SaveManager with the same name.

**Future Roadmap:**

Multiple saves will be supported, and saves will be saved in JSON format.

In the early design of the game, we chose to make it completely linear, so we only needed one save and all pre-programmed data for each day. However, as we decided to leave more playability to the players, hard-coded day configurations are not attainable anymore, indicating the need for multiple saves, a more traditional but better way.

**Event System**

GameEvent: static class for game event enums

EventManager: adds or removes the event subscriptions, and posts events when triggered.

**public void AddListener(GameEvent.Event eventType, System.Action listener)**

Bind a function to a certain type of game event.

**public void RemoveListener(GameEvent.Event eventType, System.Action listener)**

Remove the subscription of a function to a game event.

**public void PostEvent(GameEvent.Event eventType)**

Pose an event.

**If the object is about to be destroyed, make sure all listeners on it are removed.**

Since the subscription adds the bind methods to the list of actions; therefore, even after the end of the lifecycle of an object, because the EventManager is a persistent singleton, the reference of the methods still exists in the library. Therefore, when the PostEvent method invokes the methods from the destroyed objects, it will cause a null reference.

**Resource System **

ResourceManager: wrapped resource loading methods, supporting enums to mark certain directories to increase efficiency. _In future interactions, this may be changed to asynchronous loading._

**public string GetStringText(int stringID)**

Get a raw string defined in StringConst.xlsx

**public TextAsset LoadText(string path = Constants.NOTES_SOURCE_PATH, string fileName = "None")**

**public Sprite LoadImage(string path = Constants.IMAGES_SOURCE_PATH, string fileName = "None")**

Wrapped native Resources.Load methods for text files (to better support rich text) and images.


# Core Managers:

**PersistentGameManager**

Keeps the main gameloop, with the init() methods required for all managers and configs. It also manages vital game states such as pausing.

**PersistentDataManager**

Keeps the vital data and references for the game, such as the reference of the player object, and the current object UUID.


# GameLogic Systems:

**CharacterManager**

_Persistent during the entire game_

**public void LockCharacter2D(int characterID, Enums.LEVEL_TYPE levelType)**

**public void UnlockCharacter2D(int characterID, Enums.LEVEL_TYPE levelType)**

Lock/Unlock the character with the given ID for the given type of 2D mini-games.

**public void ShowCharacterPickerPanel(int levelID = 1)**

**public void HideCharacterPickerPanel()**

Show/Hide character selection panel UI.

**ChatInteractionManager**

_Persistent during the entire game_

**public void BeginInteraction(int chatID)**

**public void EndInteraction()**

Begin a chat according to the chat ID or terminate the chat.

**DayCycleManager**

_Persistent during the entire game_

**public void UnlockNextDay()**

Unlock the next in-game day.

**public void JumpToDay(int targetDay)**

**public void GoToNextDay()**

Go to the next or a certain in-game day; going to a specific day will activate a script hard-coded to set up the targeted day in order to set the main story correctly.

**GameManager2D**

_Only exists within a 2D level_

**public void SetGame(int gameID, int playerCharacterID)**

Set up the game scene according to the gameID and the corresponding metadata from Level2DData.

**public void StartGame()**

Begin the 2D mini-game after setting up.

**public void ShowGameStartPanel()**

**public void HideGameStartPanel()**

Show or hide the panel at the beginning of the mini-game, allowing players to check the 2D level detail and click to start.

**public void EndGame(bool bPass = true, bool bVictory = true)**

End the current game and show the panel with the current game’s summary. Currently, the bVictory parameter is not used.

**public void LeaveGameScene()**

Leave the scene according to the current mini-game metadata, which defines the “next” scene after the end of the game.

**HUDManager**

_Persistent during the entire game_

**public void EnterActingMode()**

**public void ExitActingMode()**

Control the acting mode that players are banned from any input and free from UIs.

**public void BeginHUDTimer()**

**public void EndHUDTimer()**

**public void UpdateHUDTimer(float time)**

Controls the timer on the top-left corner of the screen.

**public void ShowAllHUD()**

**public void HideAllHUD()**

**public void ShowUpperRightHUD()**

**public void HideUpperRightHUD()**

**public void ShowUpperLeftHUD()**

**public void HideUpperLeftHUD()**

The HUD is divided into multiple sections, which each section can be managed independently.

**HUDInteractionManager**

_Persistent during the entire game_

_Controls triggers that players can interact with options on the HUD, or triggers that invoke certain events directly._

**public void CreateStaticTrigger(string sceneName, int[] interactionIDs, Vector3 position)**

Create a static trigger and save it to SaveConfig.

**public void SpawnStaticTrigger(SaveConfig.TriggerSaveData triggerData)**

Spawn a static trigger from the save.

**public void DeleteStaticTrigger(int triggerID)**

Delete a static trigger.

**public void AddInteractionToUIList(int interactionID, HUDInteractionTrigger interactionTrigger)**

**public void RemoveInteractionFromUIList(int interactionID)**

Add or remove an interaction choice to the player’s HUD UI.

**public void EnableHUDInteractionUI()**

**public void DisableHUDInteractionUI()**

Enable or disable the HUDInteraction UI.

**public void AddInteraction(MObject target, int interactionID)**

**public void RemoveInteraction(MObject target, int interactionID)**

Take an MObject, the base class of this game’s object class, and change its intractability with other triggers.

**public void RefreshTriggerList()**

Reload the player’s interaction list.

**InputManager**

_Persistent during the entire game_

**public void SetController(PlayerController controller)**

Set to keep track of the reference of the current PlayerController.

**public void UnLockInput(Enums.SCENE_TYPE worldType, Enums.LEVEL_TYPE levelType = Enums.LEVEL_TYPE.None)**

Unlock the player’s input according to the current world and level type, since different levels require distinct control schema, while the player’s controller is the same for all scenes. In 2D levels, the Z-Axis is locked, and in 3D levels, jumping or dashing are banned,

**public void SetInputAsShooter()**

Unlock the player’s ability to fire for some 2D levels.

**public void LockInput()**

Lock player input.

**MenuManager**

_Persistent during the entire game_

**public void OpenMenu()**

**public void CloseMenu()**

Open or close the game menu’s UI.

**public void CloseMenuNoResume()**

Close the menu UI without resuming the game.

**public void ToggleMenu()**

Toggle the open/close state of the menu UI to the opposite state.

**public void SetCurrentDayText(int currentDay)**

Enables or disables the HUDInteraction UI.

**NotesManager**

_Persistent during the entire game_

_Players will unlock the records of important conversations and items; this can be triggered by adding the IDs of NotesData entries to the SaveConfig._

**public void ShowNotePanel()**

**public void CloseNotePanel()**

Open or close the notes system UI.

**NPCManager**

_Persistent during the entire game_

**public void SpawnNPC(int npcID)**

**public void SpawnNPC(int npcID, Vector3 position, Quaternion rotation)**

**public void SpawnNPC(SaveConfig.NPCSaveData npcData)**

Spawn an NPC according to the default data from NPC.xlsx, with specified position, or from the current game saved.

**public void DespawnNPC(int npcID)**

Despawn an NPC from the map.

**public void AddNewNPCToSave(int npcID, int sceneID, int positionID)**

Add an NPC to the save system without placing on in the current scene.

**public void AddInteractionToNPC(int npcID, int interactionID)**

**public void RemoveInteractionFromNPC(int npcID, int interactionID)**

Add/Remove HUDInteraction choices to/from an NPC to both the save and the instance in the current scene.

**public void ChangeNPCPositionAndScene(int npcID, string sceneName, Vector3 position)**

**public void ChangeNPCPositionAndScene(int npcID, int sceneID, int positionID)**

Change the position of an NPC in the save data only.

**public void SetNPCActive(int npcID, bool bActive)**

Set if an NPC is activated by default in the save data only.

**ReminderManager**

_Persistent during the entire game_

**public void ShowGeneralReminder(int id)**

Show a reminder according to the input ID mapped to Reminder.xlsx.

**public void ShowMapNameReminder(string name)**

Shows the current scene’s name.

**TaskManager**

_Persistent during the entire game_

_Each “Action” can carry certain action such as attaching a new interaction to an NPC, defined in Action.xlsx_

**public void ProcessActions(int[] actionIDs)**

Process a series of actions.

**public void CompleteTasks(int[] taskIDs)**

**public void CompleteTask(int taskID)**

Complete a series of tasks according to its ID.

**public void UnlockTasks(int[] taskIDs)**

**public void UnlockTask(int taskID)**

Unlock a series of tasks.
