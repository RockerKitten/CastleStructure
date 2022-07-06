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
using System.IO;
using System.Linq;
using fastJSON;

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
        
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        private AssetBundle BuildItAssetBundle { get; set; }

        private Dictionary<BuildItMaterial, BuildItEffectLists> effects;

        private void Awake()
        {
            LoadEmbeddedAssembly("fastJSON.dll");
            this.BuildItAssetBundle = AssetUtils.LoadAssetBundleFromResources("buildit", Assembly.GetExecutingAssembly());

            PrefabManager.OnVanillaPrefabsAvailable += InitializeBuildItAssets;
            Jotunn.Logger.LogInfo("BuildIt-Castles has landed");
        }
        
        private void InitializeBuildItAssets()
        {
            var buildItPieces = LoadJsonFile<BuildItPieces>("builditpieces.json");

            foreach (var buildItPieceTable in buildItPieces.PieceTables)
            {
                foreach (var buildItPieceCategory in buildItPieceTable.Categories)
                {
                    foreach (var buildItPiece in buildItPieceCategory.Pieces)
                    {
                        var customPiece = this.BuildCustomPiece(buildItPieceTable, buildItPieceCategory, buildItPiece);

                        // load supplemental assets (sfx and vfx)
                        this.AttachEffects(customPiece.PiecePrefab, buildItPiece);

                        PieceManager.Instance.AddPiece(customPiece);
                    }
                }
            }
        }

        private Dictionary<BuildItMaterial, BuildItEffectLists> InitializeEffects()
        {
            Dictionary<string, GameObject> effectCache = new Dictionary<string, GameObject>();
            GameObject loadfx(string prefabName)
            {
                if (!effectCache.ContainsKey(prefabName))
                {
                    effectCache.Add(prefabName, PrefabManager.Cache.GetPrefab<GameObject>(prefabName));
                }
                return effectCache[prefabName]; 
            }
            EffectList createfxlist(params string[] effectsList) => new EffectList { m_effectPrefabs = effectsList.Select(fx => new EffectList.EffectData { m_prefab = loadfx(fx) }).ToArray() };

            var effects = new Dictionary<BuildItMaterial, BuildItEffectLists>
            {
                {
                    BuildItMaterial.Wood,
                    new BuildItEffectLists
                    {
                        Place = createfxlist("sfx_build_hammer_wood", "vfx_Place_stone_wall_2x1"),
                        Break = createfxlist("sfx_wood_break", "vfx_SawDust"),
                        Hit   = createfxlist("vfx_SawDust"),
                        Open  = createfxlist("sfx_door_open"),
                        Close = createfxlist("sfx_door_close"),
                        Fuel  = createfxlist("vfx_HearthAddFuel"),
                    }
                },
                {
                    BuildItMaterial.Stone,
                    new BuildItEffectLists
                    {
                        Place = createfxlist("sfx_build_hammer_stone", "vfx_Place_stone_wall_2x1"),
                        Break = createfxlist("sfx_rock_destroyed", "vfx_Place_stone_wall_2x1"),
                        Hit   = createfxlist("sfx_Rock_Hit"),
                        Open  = createfxlist("sfx_door_open"),
                        Close = createfxlist("sfx_door_close"),
                        Fuel  = createfxlist("vfx_HearthAddFuel"),
                    }
                },
                {
                    BuildItMaterial.Metal,
                    new BuildItEffectLists
                    {
                        Place = createfxlist("sfx_build_hammer_metal", "vfx_Place_stone_wall_2x1"),
                        Break = createfxlist("sfx_rock_destroyed", "vfx_HitSparks"),
                        Hit   = createfxlist("vfx_HitSparks"),
                        Open  = createfxlist("sfx_door_open"),
                        Close = createfxlist("sfx_door_close"),
                        Fuel  = createfxlist("vfx_HearthAddFuel"),
                    }
                }
            };

            return effects;

            // NOTE: not sure what to do with these yet.
            //fireVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
            //PrefabManager.OnVanillaPrefabsAvailable -= LoadSounds;

            // ORIGINAL REFERENCE DATA FROM BUILDIT

            //var sfxStoneBuild = loadfx("sfx_build_hammer_stone");
            //var vfxStoneBuild = loadfx("vfx_Place_stone_wall_2x1");
            //var sfxWoodBuild = loadfx("sfx_build_hammer_wood");
            //var sfxBreakStone = loadfx("sfx_rock_destroyed");
            //var sfxWoodBreak = loadfx("sfx_wood_break");
            //var sfxMetalBuild = loadfx("sfx_build_hammer_metal");
            //var vfxMetalHit = loadfx("vfx_HitSparks");
            //var vfxAdd = loadfx("vfx_FireAddFuel");
            //var sfxAdd = loadfx("sfx_FireAddFuel");
            //var sfxStoneHit = loadfx("sfx_Rock_Hit");
            //var vfxAddFuel = loadfx("vfx_HearthAddFuel");
            //var chestOpen = loadfx("sfx_chest_open");
            //var sfxTreeFall = loadfx("sfx_tree_fall_hit");
            //var vfxTreeFallHit = loadfx("vfx_tree_fall_hit");
            //var sfxTreeHit = loadfx("sfx_tree_hit");
            //var vfxBirch = loadfx("vfx_birch1_cut");
            //var sfxFall = loadfx("sfx_tree_fall");
            //var vfxWoodHit = loadfx("vfx_SawDust");
            //var vfxDestroyLogHalf = loadfx("vfx_firlogdestroyed_half");
            //var sfxBuildRug = loadfx("sfx_build_hammer_default");
            //var sfxDoorOpen = loadfx("sfx_door_open");
            //var sfxDoorClose = loadfx("sfx_door_close");


            //buildStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxStoneBuild }, new EffectList.EffectData { m_prefab = vfxStoneBuild } } };
            //breakStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxBreakStone }, new EffectList.EffectData { m_prefab = vfxWoodHit } } };
            //hitStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxStoneHit } } };
            //buildWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxWoodBuild }, new EffectList.EffectData { m_prefab = vfxStoneBuild } } };
            //breakWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxWoodBreak }, new EffectList.EffectData { m_prefab = vfxWoodHit } } };
            //hitWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = vfxWoodHit } } };
            //buildMetal = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxMetalBuild }, new EffectList.EffectData { m_prefab = vfxStoneBuild } } };
            //breakMetal = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxBreakStone }, new EffectList.EffectData { m_prefab = vfxMetalHit } } };
            //hitMetal = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = vfxMetalHit } } };
            //hearthAddFuel = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxAddFuel }, new EffectList.EffectData { m_prefab = sfxAdd } } };
            //fireAddFuel = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = vfxAdd }, new EffectList.EffectData { m_prefab = sfxAdd } } };
            //buildRug = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxBuildRug }, new EffectList.EffectData { m_prefab = vfxStoneBuild } } };
            //doorOpen = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxDoorOpen } } };
            //doorClose = new EffectList { m_effectPrefabs = new EffectList.EffectData[1] { new EffectList.EffectData { m_prefab = sfxDoorClose } } };
        }

        //private void AddLocalizations()
        //{
        //    CustomLocalization customLocalization = new CustomLocalization();
        //    customLocalization.AddTranslation("English", new Dictionary<String, String>
        //    {
        //        { "piece_wallrkc", "Wall" }
        //    });
        //}

        private CustomPiece BuildCustomPiece(BuildItPieceTable buildItPieceTable, BuildItPieceCategories buildItPieceCategory, BuildItPiece buildItPiece)
        {
            var buildItPiecePrefab = this.BuildItAssetBundle.LoadAsset<GameObject>(buildItPiece.PrefabName);

            var pieceConfig = new PieceConfig();
            // TODO: verify token string
            pieceConfig.Name = buildItPiece.DisplayNameToken;
            // NOTE: could move override to json config if needed.
            pieceConfig.AllowedInDungeons = false;
            pieceConfig.PieceTable = buildItPieceTable.TableName;
            pieceConfig.Category = buildItPieceCategory.CategoryTabName;
            pieceConfig.Enabled = buildItPiece.Enabled;

            var requirements = buildItPiece.Requirements
                .Select(r => new RequirementConfig(r.Item, r.Amount, recover: r.Recover));

            pieceConfig.Requirements = requirements.ToArray();
            var customPiece = new CustomPiece(buildItPiecePrefab, fixReference: false, pieceConfig);
            return customPiece;
        }

        private void AttachEffects(GameObject piecePrefab, BuildItPiece buildItPiece)
        {
            var pieceComponent = piecePrefab.GetComponent<Piece>();
            pieceComponent.m_placeEffect = this.effects[buildItPiece.Material].Place;

            var wearComponent = piecePrefab.GetComponent<WearNTear>();
            wearComponent.m_destroyedEffect = this.effects[buildItPiece.Material].Break;
            wearComponent.m_hitEffect = this.effects[buildItPiece.Material].Hit;

            if (buildItPiece.IsDoor)
            {
                var doorComponent = piecePrefab.GetComponent<Door>();
                doorComponent.m_openEffects = this.effects[buildItPiece.Material].Open;
                doorComponent.m_closeEffects = this.effects[buildItPiece.Material].Close;
            }

            if (buildItPiece.IsFire)
            {
                var fireComponent = piecePrefab.GetComponent<Fireplace>();
                fireComponent.m_fuelAddedEffects = this.effects[buildItPiece.Material].Fuel;                
            }
        }

        // LOADING EMBEDDED RESOURCES
        private void LoadEmbeddedAssembly(string assemblyName)
        {
            var stream = GetManifestResourceStream(assemblyName);
            if (stream == null)
            {
                Logger.LogError($"Could not load embedded assembly ({assemblyName})!");
                return;
            }

            using (stream)
            {
                var data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                Assembly.Load(data);
            }
        }

        private Stream GetManifestResourceStream(string filename)
        {
            var assembly = Assembly.GetCallingAssembly();
            var fullname = assembly.GetManifestResourceNames().SingleOrDefault(x => x.EndsWith(filename));
            if (!string.IsNullOrEmpty(fullname))
            {
                return assembly.GetManifestResourceStream(fullname);
            }

            return null;
        }

        private T LoadJsonFile<T>(string filename) where T : class
        {
            var jsonFile = LoadJsonText(filename);
            T result;
            try
            {
                result = string.IsNullOrEmpty(jsonFile) ? null : JSON.ToObject<T>(jsonFile);
            }
            catch (Exception)
            {
                Logger.LogError($"Could not parse file '{filename}'! Errors in JSON!");
                throw;
            }

            return result;
        }

        private string LoadJsonText(string filename)
        {
            var jsonFileName = GetAssetPath(filename);
            return !string.IsNullOrEmpty(jsonFileName) ? File.ReadAllText(jsonFileName) : null;
        }

        private string GetAssetPath(string assetName)
        {
            var assembly = Assembly.GetCallingAssembly();
            var assetFileName = Path.Combine(Path.GetDirectoryName(assembly.Location) ?? string.Empty, assetName);
            if (!File.Exists(assetFileName))
            {
                Logger.LogError($"Could not find asset ({assetName})");
                return null;
            }

            return assetFileName;
        }
    }
}

