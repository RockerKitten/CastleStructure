// BuildIt-Castles
// a Valheim mod skeleton using Jötunn
// 
// File:    BuildIt-Castles.cs
// Project: BuildIt-Castles
using System.Reflection;
using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace BuildItCastles
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class BuildItCastles : BaseUnityPlugin
    {
        public const string PluginGUID = "com.RockerKitten.BuildItCastles";
        public const string PluginName = "BuildItCastles";
        public const string PluginVersion = "0.0.1";
        public AssetBundle assetBundle;
        public EffectList buildStone;
        public EffectList breakStone;
        public EffectList hitStone;
        public EffectList breakWood;
        public EffectList hitWood;
        public EffectList buildMetal;
        public EffectList breakMetal;
        public EffectList hitMetal;
        public EffectList hearthAddFuel;
        public EffectList fireAddFuel;
        public EffectList buildRug;
        public EffectList doorOpen;
        public EffectList doorClose;
        public EffectList buildWood;
        public AudioSource fireVol;

        public CustomPieceTable castleHammer;

        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private void Awake()
        {
            AddLocalizations();
            PrefabManager.OnVanillaPrefabsAvailable += LoadSounds;
            Jotunn.Logger.LogInfo("BuildIt-Castles has landed");

        }
        public void LoadAssets()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("buildit", Assembly.GetExecutingAssembly());
        }
        private void LoadSounds()
        {
            var sfxStoneBuild = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_stone");
            var vfxStoneBuild = PrefabManager.Cache.GetPrefab<GameObject>("vfx_Place_stone_wall_2x1");
            var sfxWoodBuild = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_wood");
            var sfxBreakStone = PrefabManager.Cache.GetPrefab<GameObject>("sfx_rock_destroyed");
            var sfxWoodBreak = PrefabManager.Cache.GetPrefab<GameObject>("sfx_wood_break");
            var sfxMetalBuild = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_metal");
            var vfxMetalHit = PrefabManager.Cache.GetPrefab<GameObject>("vfx_HitSparks");
            var vfxAdd = PrefabManager.Cache.GetPrefab<GameObject>("vfx_FireAddFuel");
            var sfxAdd = PrefabManager.Cache.GetPrefab<GameObject>("sfx_FireAddFuel");
            var sfxStoneHit = PrefabManager.Cache.GetPrefab<GameObject>("sfx_Rock_Hit");
            var vfxAddFuel = PrefabManager.Cache.GetPrefab<GameObject>("vfx_HearthAddFuel");
            var chestOpen = PrefabManager.Cache.GetPrefab<GameObject>("sfx_chest_open");
            var sfxTreeFall = PrefabManager.Cache.GetPrefab<GameObject>("sfx_tree_fall_hit");
            var vfxTreeFallHit = PrefabManager.Cache.GetPrefab<GameObject>("vfx_tree_fall_hit");
            var sfxTreeHit = PrefabManager.Cache.GetPrefab<GameObject>("sfx_tree_hit");
            var vfxBirch = PrefabManager.Cache.GetPrefab<GameObject>("vfx_birch1_cut");
            var sfxFall = PrefabManager.Cache.GetPrefab<GameObject>("sfx_tree_fall");
            var vfxWoodHit = PrefabManager.Cache.GetPrefab<GameObject>("vfx_SawDust");
            var vfxDestroyLogHalf = PrefabManager.Cache.GetPrefab<GameObject>("vfx_firlogdestroyed_half");
            var sfxBuildRug = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_default");
            var sfxDoorOpen = PrefabManager.Cache.GetPrefab<GameObject>("sfx_door_open");
            var sfxDoorClose = PrefabManager.Cache.GetPrefab<GameObject>("sfx_door_close");


            buildStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxStoneBuild }, new EffectList.EffectData { m_prefab = vfxStoneBuild } } };
            breakStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxBreakStone }, new EffectList.EffectData { m_prefab = vfxWoodHit } } };
            hitStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxStoneHit } } };
            buildWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxWoodBuild }, new EffectList.EffectData { m_prefab = vfxStoneBuild } } };
            breakWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxWoodBreak }, new EffectList.EffectData { m_prefab = vfxWoodHit } } };
            hitWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = vfxWoodHit } } };
            buildMetal = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxMetalBuild }, new EffectList.EffectData { m_prefab = vfxStoneBuild } } };
            breakMetal = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxBreakStone }, new EffectList.EffectData { m_prefab = vfxMetalHit } } };
            hitMetal = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = vfxMetalHit } } };
            hearthAddFuel = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxAddFuel }, new EffectList.EffectData { m_prefab = sfxAdd } } };
            fireAddFuel = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxAdd }, new EffectList.EffectData { m_prefab = sfxAdd } } };
            buildRug = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxBuildRug }, new EffectList.EffectData { m_prefab = vfxStoneBuild } } };
            doorOpen = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxDoorOpen } } };
            doorClose = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxDoorClose } } };

            fireVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
            PrefabManager.OnVanillaPrefabsAvailable -= LoadSounds;
        }
        private void AddLocalizations()
        {
            CustomLocalization customLocalization = new CustomLocalization();
            customLocalization.AddTranslation("English", new Dictionary<String, String>
            {
                { "piece_wallrkc", "Wall" }
            });
        }
    }
}

