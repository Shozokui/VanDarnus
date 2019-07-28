--[[

Globals referenced in all of the lua scripts

--]]

-- ACTOR STATES

ACTORSTATE_PASSIVE = 0;
ACTORSTATE_DEAD1 = 1;
ACTORSTATE_ACTIVE = 2;
ACTORSTATE_DEAD2 = 3;
ACTORSTATE_SITTING_ONOBJ = 11;
ACTORSTATE_SITTING_ONFLOOR = 13;
ACTORSTATE_MOUNTED = 15;


-- MESSAGE
MESSAGE_TYPE_NONE       = 0;
MESSAGE_TYPE_SAY        = 1;
MESSAGE_TYPE_SHOUT      = 2;
MESSAGE_TYPE_TELL       = 3;
MESSAGE_TYPE_PARTY      = 4;
MESSAGE_TYPE_LINKSHELL1 = 5;
MESSAGE_TYPE_LINKSHELL2 = 6;
MESSAGE_TYPE_LINKSHELL3 = 7;
MESSAGE_TYPE_LINKSHELL4 = 8;
MESSAGE_TYPE_LINKSHELL5 = 9;
MESSAGE_TYPE_LINKSHELL6 = 10;
MESSAGE_TYPE_LINKSHELL7 = 11;
MESSAGE_TYPE_LINKSHELL8 = 12;

MESSAGE_TYPE_SAY_SPAM       = 22;
MESSAGE_TYPE_SHOUT_SPAM     = 23;
MESSAGE_TYPE_TELL_SPAM      = 24;
MESSAGE_TYPE_CUSTOM_EMOTE   = 25;
MESSAGE_TYPE_EMOTE_SPAM     = 26;
MESSAGE_TYPE_STANDARD_EMOTE = 27;
MESSAGE_TYPE_URGENT_MESSAGE = 28;
MESSAGE_TYPE_GENERAL_INFO   = 29;
MESSAGE_TYPE_SYSTEM         = 32;
MESSAGE_TYPE_SYSTEM_ERROR   = 33;

-- INVENTORY
INVENTORY_NORMAL = 0x0000; --Max 0xC8
INVENTORY_LOOT = 0x0004; --Max 0xA
INVENTORY_MELDREQUEST = 0x0005; --Max 0x04
INVENTORY_BAZAAR = 0x0007; --Max 0x0A
INVENTORY_CURRENCY = 0x0063; --Max 0x140
INVENTORY_KEYITEMS = 0x0064; --Max 0x500
INVENTORY_EQUIPMENT = 0x00FE; --Max 0x23
INVENTORY_EQUIPMENT_OTHERPLAYER = 0x00F9; --Max 0x23

-- INVENTORY ERRORS
INV_ERROR_SUCCESS = 0;
INV_ERROR_FULL = 1;
INV_ERROR_ALREADY_HAS_UNIQUE = 2;
INV_ERROR_SYSTEM_ERROR = 3;

-- CHOCOBO APPEARANCE
CHOCOBO_NORMAL = 0;

CHOCOBO_LIMSA1 = 0x1;
CHOCOBO_LIMSA2 = 0x2;
CHOCOBO_LIMSA3 = 0x3;
CHOCOBO_LIMSA4 = 0x4;

CHOCOBO_GRIDANIA1 = 0x1F;
CHOCOBO_GRIDANIA2 = 0x20;
CHOCOBO_GRIDANIA3 = 0x21;
CHOCOBO_GRIDANIA4 = 0x22;

CHOCOBO_ULDAH1 = 0x3D;
CHOCOBO_ULDAH2 = 0x3E;
CHOCOBO_ULDAH3 = 0x3F;
CHOCOBO_ULDAH4 = 0x40;

-- NPC LS
NPCLS_GONE     = 0;
NPCLS_INACTIVE = 1;
NPCLS_ACTIVE   = 2;
NPCLS_ALERT    = 3;

-- BATTLETEMP GENERAL PARAMETERS
NAMEPLATE_SHOWN = 0;
TARGETABLE = 1;
NAMEPLATE_SHOWN2 = 2;
NAMEPLATE_SHOWN2 = 3;
STAT_STRENGTH = 3;
STAT_VITALITY = 4;
STAT_DEXTERITY = 5;
STAT_INTELLIGENCE = 6;
STAT_MIND = 7;
STAT_PIETY = 8;
STAT_RESISTANCE_FIRE = 9;
STAT_RESISTANCE_ICE = 10;
STAT_RESISTANCE_WIND = 11;
STAT_RESISTANCE_LIGHTNING = 12;
STAT_RESISTANCE_EARTH = 13;
STAT_RESISTANCE_WATER = 14;
STAT_ATTACK = 17;
STAT_ACCURACY = 15;
STAT_NORMALDEFENSE = 18;
STAT_EVASION = 16;
STAT_ATTACK_MAGIC = 24;
STAT_HEAL_MAGIC = 25;
STAT_ENCHANCEMENT_MAGIC_POTENCY = 26;
STAT_ENFEEBLING_MAGIC_POTENCY = 27;
STAT_MAGIC_ACCURACY = 28;
STAT_MAGIC_EVASION = 29;
STAT_CRAFT_PROCESSING = 30;
STAT_CRAFT_MAGIC_PROCESSING = 31;
STAT_CRAFT_PROCESS_CONTROL = 32;
STAT_HARVEST_POTENCY = 33;
STAT_HARVEST_LIMIT = 34;
STAT_HARVEST_RATE = 35;

-- DAMAGE TAKEN TYPE
DAMAGE_TAKEN_TYPE_NONE = 0;
DAMAGE_TAKEN_TYPE_ATTACK = 1;
DAMAGE_TAKEN_TYPE_MAGIC = 2;
DAMAGE_TAKEN_TYPE_WEAPONSKILL = 3;
DAMAGE_TAKEN_TYPE_ABILITY = 4;

-- CLASSID
CLASSID_PUG = 2;
CLASSID_GLA = 3;
CLASSID_MRD = 4;
CLASSID_ARC = 7;
CLASSID_LNC = 8;
CLASSID_THM = 22;
CLASSID_CNJ = 23;

-- SPAWNS
SPAWN_NO_ANIM = 0x00;
SPAWN_ANIM1 = 0x02;
SPAWN_RETAINER = 0x03;
SPAWN_POPMOB = 0x4;
SPAWN_UKN1 = 0x5;
SPAWN_UKN2 = 0x7;
SPAWN_LOADING1 = 0x0F;
SPAWN_LOADING2 = 0x10;
SPAWN_INSTANCE_ERROR = 0x12;
SPAWN_CHOCOBO_GET = 0x13;
SPAWN_CHOCOBO_RENTAL = 0x14;
SPAWN_CUTTER_SANDS = 0x17;
SPAWN_NIGHTMARE = 0x18;

--UTILS

function kickEventContinue(player, actor, trigger, ...)
	player:kickEvent(actor, trigger, ...);
	return coroutine.yield("_WAIT_EVENT", player);
end

function callClientFunction(player, functionName, ...)
	player:RunEventFunction(functionName, ...);	
	return coroutine.yield("_WAIT_EVENT", player);	
end

function wait(seconds)
	return coroutine.yield("_WAIT_TIME", seconds);
end

function waitForSignal(signal)
	return coroutine.yield("_WAIT_SIGNAL", signal);
end

function sendSignal(signal)
	GetLuaInstance():OnSignal(signal);
end

function printf(s, ...)
    if ... then
        print(s:format(...));
    else
        print(s);
    end;
end;