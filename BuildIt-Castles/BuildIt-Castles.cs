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
            LoadAssets();
            PrefabManager.OnVanillaPrefabsAvailable += LoadSounds;
            Jotunn.Logger.LogInfo("BuildIt-Castles has landed");

        }
        public void LoadAssets()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("castles", Assembly.GetExecutingAssembly());
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


            LoadHammerTable();
            LoadBuild1();
            LoadBuild2();
            LoadBuild3();
            LoadBuild4();
            LoadBuild5();
            LoadBuild6();
            LoadBuild7();
            LoadBuild8();
            LoadBuild9();
            LoadBuild10();
            LoadBuild11();
            LoadBuild12();
            LoadBuild13();
            LoadBuild14();
            LoadBuild15();
            LoadBuild16();
            LoadBuild17();
            LoadBuild18();
            LoadBuild19();
            LoadBuild20();
            LoadBuild21();
            LoadBuild22();
            LoadBuild23();
            LoadBuild24();
            LoadBuild25();
            LoadBuild26();



            //fireVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;

            PrefabManager.OnVanillaPrefabsAvailable -= LoadSounds;
        }
        private void AddLocalizations()
        {
            CustomLocalization customLocalization = new CustomLocalization();
            customLocalization.AddTranslation("English", new Dictionary<String, String>
            {
                { "piece_castlewall", "Wall" },{"piece_castlebed","Bed"},{"piece_castledoor","Door"},{"piece_castlefloor","Floor"},
                {"piece_castlefence","Fence"},{"piece_castleshelf","Shelf"},{"piece_castletable","Table"},{"piece_castletree","Tree"},
                {"piece_castletrowel","Castle Trowel"},{"piece_castlewindow","Window"}
            });
        }
        private void LoadHammerTable()
        {
            var hammerTableFab = assetBundle.LoadAsset<GameObject>("_RKC_CustomTable");
            var castleTrowel = new CustomPieceTable(hammerTableFab,
            new PieceTableConfig
            {
                CanRemovePieces = true,
                UseCategories = false,
                UseCustomCategories = true,
                CustomCategories = new string[]
                {
                    "Structure", "Furniture", "Roofs", "Outdoors"
                }
            });

            PieceManager.Instance.AddPieceTable(castleTrowel);
            LoadHammer();
        }

        private void LoadHammer()
        {
            var itemFab = assetBundle.LoadAsset<GameObject>("rkc_trowel");
            var item = new CustomItem(itemFab, false,
                new ItemConfig
                {
                    Name = "$item_trowel",
                    Amount = 1,
                    Enabled = true,
                    PieceTable = "_RKC_CustomTable",
                    RepairStation = "piece_workbench",
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2},
                        new RequirementConfig {Item = "Stone", Amount = 2}
                    }
                });
            ItemManager.Instance.AddItem(item);
        }
        private void LoadBuild1()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_bed");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlebed",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild2()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_door");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castledoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild3()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_door2");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castledoor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild4()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_fence1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlefence",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild5()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_fence2");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlefence",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outdoors",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild6()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_floor1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlefloor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild7()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_floor2");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlefloor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild8()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_floor3");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlefloor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild9()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_floor4");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlefloor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild10()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_floor5");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlefloor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild11()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_floor6");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlefloor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild12()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_floor7");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlefloor",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild13()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_shelfshort");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castleshelf",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild14()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_shelflong");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castleshelf",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild15()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_table");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castletable",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild16()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_tableside");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlesidetable",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Furniture",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild17()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_tree1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castletree",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outside",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild18()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_tree2");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castletree",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Outside",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild19()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_wall1");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlewall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild20()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_wall2");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlewall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild21()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_wall3");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlewall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild22()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_wall4");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlewall",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild23()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_windowplain");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlewindow",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild24()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_windowshort");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlewindow",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild25()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_windowsmed");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlewindow",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        private void LoadBuild26()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_windowtall");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castlewindow",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }
        /*private void LoadBuild27()
        {
            var buildFab = assetBundle.LoadAsset<GameObject>("rkc_");
            var build = new CustomPiece(buildFab, false,
                new PieceConfig
                {
                    Name = "$piece_castle",
                    AllowedInDungeons = false,
                    PieceTable = "_RKCustomTable",
                    Category = "Structure",
                    Enabled = true,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }

                });
            var fxBuild = buildFab.GetComponent<Piece>();
            fxBuild.m_placeEffect = buildWood;

            var fxHit = buildFab.GetComponent<WearNTear>();
            fxHit.m_hitEffect = hitWood;
            fxHit.m_destroyedEffect = breakWood;

            PieceManager.Instance.AddPiece(build);
        }*/
    }
}

