using UnityEngine;

public enum ReticleTargetType
{
	Default,
	Enemies,
	Ui
}

public enum Equipment {
	Hand,
	Scooper,
	Hose,
	Cone
}

public enum TutorialType{
	Moved = 0,
	Switched,
	Grabbed,
	Placed,
	Scooped,
	Toppings,
	Syrups,
	Thrown,
	Info1,
	Info2
}

public enum EnemyType {
	Basic,
	SupportFire,
	Bruiser,
	Berzerker
}

public enum EntityTypes {
	PLAYER,
	ENEMY,
	WALL,
	ENEMY_TARGET
}

public enum CustomerPosition {
	Middle = 0,
	Left = 1,
	Right = 2
}

public enum ButtonType{
	Replay,
	NextLevel,
	Exit,
	Cancel,
	Home,
	Start,
	Quit
}

public enum ConePosition {
	Middle = 0,
	Left = 1,
	Right = 2,
	None = 3,
	Topping = 4
}

public enum SceneType {
	Start,
	Intro,
	Title,
	Tutorial,
	Game,
	End
}

public enum OrderType {
	Simple,
	Complicated
}

public enum ItemStatus {
	Locked,
	Unlocked
}

public enum CustomerPath {
	Enter,
	Standby,
	Exit
}

public enum CustomerPatience{
	Short,
	Medium,
	Long
}

public enum TargetType {
	Interactable,
	Noninteractable
}

public enum Tilt {
	Right,
	RightHeavy,
	Left,
	LeftHeavy,
	Idle
}

public enum CustomerFace {
	Happy,
	Impatient,
	Angry,
	Wrong,
	Correct
}

public enum BoardType {
	EndGame,
	Exit,
	EndDay,
	Gameover,
	Paused,
	Notif
}

public enum Gender {
	Male,
	Female
}

public enum UnlockType {
	Flavor,
	Topping,
	Syrup
}

public enum PlayerStance {
	Jab,
	Hook,
	Uppercut,
	Body
}

[System.Flags]
public enum AxisDirection
{
	None  = 0,
	Up    = 1, // << 0,
	Down  = 1 << 1,
	Right = 1 << 2,
	Left  = 1 << 3,
	Front = 1 << 4,
	Back  = 1 << 5
}

public class Constants {

	#region SAVE PATH
	#if UNITY_EDITOR
	public static string APP_DATAPATH	= Application.dataPath;
	#else
	public static string APP_DATAPATH	= Application.persistentDataPath;
	#endif

	public const string SAVE_DATA_DIRECTORY = "/SaveData";
	#endregion

	public const float PLAYER_HEAD_TILT_MIN_THRESHOLD = 15;
	public const float PLAYER_HEAD_TILT_MAX_THRESHOLD = 30;

	public const float PLAYER_HEAD_TILT_ANGLE = 55;

	public const float PLAYER_MAX_SPEED = 1.5f;
	public const float PLAYER_MIN_ACCELERATION = 0.8f;
	public const float PLAYER_MAX_ACCELERATION = 1.5f;
	public const float PLAYER_DECCELERATION = 0.5f;
	
	public static readonly Vector3 PLAYER_SIGHT_OFFSET = new Vector3( 0f, 2f, 0f );

	public const float CHARGE_AMOUNT_MIN = 0f;		// minimum amount of time that charge button can be held
	public const float CHARGE_AMOUNT_MAX = 750;//1f;	// maximum amount of time that charge button can be held

	public const float FLICK_SENSITIVITY = 0.001f;	// amount of delta gyro before flick registers/de-registers
	public const float FLICK_MAX = 0.1f;//0.00005f;	// amount of max delta gyro expected from flick, used for normalizing
	public const float MAX_FLICK_RESET_TIMER = 0.05f;	// when this amount of time elapses without the game 
														// registering a delta gyr value, consider the flick done
	public const float FLICK_FORCE_MULTIPLIER = 1000f;	// amount to multiply to flick power 

	public const float THROW_FORCE_MIN = 250;		// minimum amount of force applied to snowball to launch it
	public const float THROW_FORCE_MAX = 500f;		// maximum amount of force applied to snowball to launch it
	public const float THROW_VERT_OFFSET_NORMALIZED = 0.15f;	// y-offset used when calculating throw angle

	public const float MAX_PUNCH_RESET_TIMER = 0.05f;
	public const float PUNCH_VELOCITY_THRESHOLD = 1.0f;
	public const float PUNCH_ANGULAR_VELOCITY_THRESHOLD = 6.0f;
	public const float PUNCH_VELOCITY_RESET_THRESHOLD = 1.0f;
	public const float PUNCH_ANGULAR_VELOCITY_RESET_THRESHOLD = 2.0f;

	public const float RETICLE_SIZE = 0.01f;
	public const float RETICLE_TARGET_RADIUS = 0.5f;
	public const float RETICLE_OFFSETTOTARGET = 0.5f;
	public const int RETICLE_CAST_DIST = 40;
	public const int RETICLE_TARGET_DIST_DEFAULT = 5;

	public const float ITEM_ACTIVE_DIST = 0.7f;

	public const string TAG_WALL = "Wall";
	public const string TAG_RETICLETARGET = "ReticleTarget";
	public const string TAG_CONE = "Cone";
	public const string TAG_CUSTOMER = "Customer";

	public const float AI_WALL_SEARCH_RADIUS = 50f;	// AI will search around itself for nearby walls, using this radius

	#region AI evasion
	public const float AI_PROJECTILE_DETECTION_RADIUS = 3f;	// AI will detect projectiles landing at this radius around it
	public const float AI_EVADE_MIN_ANGLE = 10f;	// min amount of angle deviating from AI-to-player look-vector. Used for finding evasion spot 
	public const float AI_EVADE_MAX_ANGLE = 80f;	// max amount of angle deviating from AI-to-player look-vector. Used for finding evasion spot 
	public const float AI_EVADE_MIN_DISTANCE_MULT = 1f;	// min multiplier used to determine where to evade
	public const float AI_EVADE_MAX_DISTANCE_MULT = 2f;	// max multiplier used to determine where to evade
	#endregion

	#region Spawning
	public const int SPAWN_MAX_ENEMIES = 5;	// max number of enemies present before spawner pauses
	public const float SPAWN_REST_BTWN_WAVES = 5f;	// seconds between waves
	#endregion

	#region Audio

	public const string BGM_INGAME = "bgm_InGame";
	public const string BGM_MAINMENU = "bgm_MainMenu";
	public const string SFX_BELLRING = "sfx_BellRing";
	public const string SFX_BUTTON = "sfx_Button";
	public const string SFX_CONGRATULATIONS = "sfx_Congratulations";
	public const string SFX_CUSTOMER_JOY1 = "sfx_CustomerJoy1";
	public const string SFX_CUSTOMER_JOY2 = "sfx_CustomerJoy2";
	public const string SFX_CUSTOMER_JOY3 = "sfx_CustomerJoy3";
	public const string SFX_CUSTOMER_JOY1_BOY = "sfx_CustomerJoyV2Boy1";
	public const string SFX_CUSTOMER_JOY2_BOY = "sfx_CustomerJoyV2Boy2";
	public const string SFX_CUSTOMER_JOY1_GIRL = "sfx_CustomerJoyV2Girl1";
	public const string SFX_CUSTOMER_JOY2_GIRL = "sfx_CustomerJoyV2Girl2";
	public const string SFX_CUSTOMER_MAD1 = "sfx_CustomerMad1";
	public const string SFX_CUSTOMER_MAD2 = "sfx_CustomerMad2";
	public const string SFX_CUSTOMER_MAD3 = "sfx_CustomerMad3";
	public const string SFX_CUSTOMER_MUNCH = "sfx_CustomerMunch";
	public const string SFX_CUSTOMER_OPEN = "sfx_CustomerOpen";
	public const string SFX_CUSTOMER_OPENV2 = "sfx_CustomerOpenV2";
	public const string SFX_FLIP_SIGN = "sfx_FlipSignV3";
	public const string SFX_GRAB_CONE = "sfx_GrabCone";
	public const string SFX_GRAB_CONE2 = "sfx_GrabConeV2";
	public const string SFX_GRAB_HOSE = "sfx_GrabHose";
	public const string SFX_MOVE_STATION = "sfx_MoveStation";
	public const string SFX_MOVE_STATION2 = "sfx_MoveStationV2";
	public const string SFX_ORDER_IN = "sfx_OrderIn";
	public const string SFX_ORDER_IN2 = "sfx_OrderInV2";
	public const string SFX_PLAYER_LOSE = "sfx_PlayerLose";
	public const string SFX_PLAYER_WIN = "sfx_PlayerWin";
	public const string SFX_SCOOP = "sfx_Scoop";
	public const string SFX_SCOOP_IN = "sfx_ScoopIn";
	public const string SFX_SWAP_TOOL = "sfx_SwapTool";
	public const string SFX_SYRUP_SQUIRT = "sfx_SyrupSquirt1";
	public const string SFX_SYRUP_SQUIRT2 = "sfx_SyrupSquirt2";
	public const string SFX_THROW = "sfx_Throw";
	public const string SFX_THROW2 = "sfx_ThrowV2";
	public const string SFX_TICK_MARK = "sfx_TickMarkV3";
	public const string SFX_TIMER = "sfx_Timer";
	public const string SFX_TOPPINGS_FALL = "sfx_ToppingsFall";

	#endregion
}
