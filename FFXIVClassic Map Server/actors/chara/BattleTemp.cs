﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVClassic_Map_Server.dataobjects.chara
{
    class BattleTemp
    {
        public const uint NAMEPLATE_SHOWN = 0;
        public const uint TARGETABLE = 1;
        //public const uint NAMEPLATE_SHOWN2 = 2;
        public const uint NAMEPLATE_SHOWN2 = 3;

        public const uint STAT_STRENGTH = 4;
        public const uint STAT_VITALITY = 5;
        public const uint STAT_DEXTERITY = 6;
        public const uint STAT_INTELLIGENCE = 7;
        public const uint STAT_MIND = 8;
        public const uint STAT_PIETY = 9;

        public const uint STAT_RESISTANCE_FIRE = 10;
        public const uint STAT_RESISTANCE_ICE = 11;
        public const uint STAT_RESISTANCE_WIND = 12;
        public const uint STAT_RESISTANCE_LIGHTNING = 13;
        public const uint STAT_RESISTANCE_EARTH = 14;
        public const uint STAT_RESISTANCE_WATER = 15;

        public const uint STAT_ATTACK = 18;
        public const uint STAT_ACCURACY = 16;
        public const uint STAT_NORMALDEFENSE = 19;
        public const uint STAT_EVASION = 17;
        public const uint STAT_ATTACK_MAGIC = 24;
        public const uint STAT_HEAL_MAGIC = 25;
        public const uint STAT_ENCHANCEMENT_MAGIC_POTENCY = 26;
        public const uint STAT_ENFEEBLING_MAGIC_POTENCY = 27;
                
        public const uint STAT_MAGIC_ACCURACY = 28;
        public const uint STAT_MAGIC_EVASION = 29;

        public const uint STAT_CRAFT_PROCESSING = 30;
        public const uint STAT_CRAFT_MAGIC_PROCESSING = 31;
        public const uint STAT_CRAFT_PROCESS_CONTROL = 32;

        public const uint STAT_HARVEST_POTENCY = 33;
        public const uint STAT_HARVEST_LIMIT = 34;
        public const uint STAT_HARVEST_RATE = 35;

        public float[] castGauge_speed = { 1.0f, 0.25f};
        public bool[]   timingCommandFlag = new bool[4];
        public ushort[] generalParameter = new ushort[32];
    }
}
