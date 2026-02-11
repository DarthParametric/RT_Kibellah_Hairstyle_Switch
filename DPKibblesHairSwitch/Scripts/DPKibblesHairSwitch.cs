using Kingmaker.Modding;
using Kingmaker.ResourceLinks;
using Kingmaker.View;
using Kingmaker.Visual.CharacterSystem;
using Owlcat.Runtime.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DPKibblesHairSwitch
{
    public class DPKibblesHairSwitch
    {
		internal static LogChannel Logger;
		internal static OwlcatModification DPMod;
        internal static string HairModelName;
        internal static string HairAssetID;
		public static Settings settings;
		
		public static string[] HairTypes =
		{
			"Anevia",
			"Bob Cut",
			"Side Kare"
		};
		/*
        internal static Dictionary<string, string> EE_IDs_Dict = new()
        {
			{"EE_Hair_Kibbles_Anevia_F_HM", "bd55c5f2632dc444fa651669be54d67f"},
        };
		*/
        internal static Dictionary<string, string> Vanilla_EE_IDs_Dict = new()
        {
			{"EE_Hair02MediumAnevia_F_HM", "d4d1a7937435fe54a889bc3522984205"},
			{"EE_Hair05BobCut_F_HM", "16befdf0bdb05424891138d02a718848"},
			{"EE_Hair08SideKare_F_HM", "3fa56cc5d206ca142bde8f93ad089a02"},
        };

        [OwlcatModificationEnterPoint]
        public static void EnterPoint(OwlcatModification modification)
		{
            try
            {
                DPMod = modification;
                Logger = DPMod.Logger;
                settings = DPMod.LoadData<Settings>();
				LogDebug("Loaded mod settings");
                DPMod.OnGUI += OnGUI;
				DPMod.OnLoadResource += EditUnitBundle;
            }
            catch (Exception ex)
            {
                Logger.Log($"Caught an exception in EnterPoint: \n{ex}");
            }
		}

        public static void OnGUI()
        {
            try
            {
                GUILayout.Label("<b>Enable Hair Swapping:</b>", GUILayout.ExpandWidth(false));
                GUILayout.BeginHorizontal();
                settings.ModActive = GUILayout.Toggle(settings.ModActive, "Replace Kibellah's hair");
                GUILayout.EndHorizontal();

                GUILayout.Space(15);
				
				var stylecentered = new GUIStyle(GUI.skin.toggle) { alignment = TextAnchor.MiddleLeft };
				stylecentered.onNormal.textColor = Color.green;

				GUILayout.Label("<b>Choose Hair Type:</b>", GUILayout.ExpandWidth(false));
				GUILayout.BeginHorizontal();
				settings.SelectedHairType = GUILayout.SelectionGrid(settings.SelectedHairType, HairTypes, 3, stylecentered);
                GUILayout.EndHorizontal();

                GUILayout.Space(15);
				
				/*
				// HIDE THIS OPTION IN THE GUI, REQUIRE MANUAL EDITING OF CONFIG FILE TO ENABLE.
                GUILayout.Label("<b>Enable Detailed Logging:</b>", GUILayout.ExpandWidth(false));
                GUILayout.Label("(Logspam for debugging purposes)", GUILayout.ExpandWidth(false));
                GUILayout.BeginHorizontal();
                settings.DetailedLogging = GUILayout.Toggle(settings.DetailedLogging, "Detailed Logs");
                GUILayout.EndHorizontal();

                GUILayout.Space(15);
				*/

                if (GUILayout.Button("Apply", GUILayout.ExpandWidth(false)))
                {
                    DPMod.SaveData(settings);
					LogDebug("Saved mod settings");
                }
				
				GUILayout.Space(5);
				
				GUILayout.Label("<i><b>N.B.:</b> You must reload or change locations for a hairstyle switch to take effect!</i>", GUILayout.ExpandWidth(false));
            }
            catch (Exception ex)
            {
                Logger.Log($"Caught an exception in OnGUI: \n{ex}");
            }
        }

        public static void LogDebug(string message)
        {
            if (settings.DetailedLogging)
            {
                Logger.Log($"DEBUG: {message}");
            }
		}

        public static string GetAssetIDFromIndex(int index)
        {
            return Vanilla_EE_IDs_Dict.Values.ElementAt(index);
        }
		
        public static string GetNameFromIndex(int index)
        {
            return Vanilla_EE_IDs_Dict.Keys.ElementAt(index);
        }

		private static void EditUnitBundle(object resource, string guid)
		{
            try
            {
				if (guid == "4523d1b44fbbcd84d9c61e615421971e")
				{
					LogDebug("Kibbles prefab loaded, patching.");
					
					HairAssetID = GetAssetIDFromIndex(settings.SelectedHairType);
					HairModelName = GetNameFromIndex(settings.SelectedHairType);
					var kibbleshair = "d4d82f25a1bbb8c4fa77d306e67d60af";	// EE_Hair31Kibellah_F_HM - 4th entry in array, zero indexed.
					var character = ((UnitEntityView)resource).GetComponentInChildren<Character>();
					var eelist = character.SavedEquipmentEntities;
					var rmplist = character.m_SavedRampIndices;
					
					LogDebug($"Number of AssetIDs in m_SavedEquipmentEntities = {eelist.Count}.");
					
					for (int i = 0; i < eelist.Count; i++)
					{
						var assetid = eelist[i].AssetId;

						if (settings.ModActive)
						{
							if (assetid == kibbleshair)
							{
								LogDebug($"AssetID at m_SavedEquipmentEntities index {i} is EE_Hair31Kibellah_F_HM ({assetid}). Replacing with selected hair {HairModelName} ({HairAssetID}).");

								eelist[i].AssetId = HairAssetID;
							}
						}
						else
						{
							LogDebug("Hair switching deactivated in settings, restoring vanilla Kibellah bowl cut.");
							
							for (int j = 0; j < Vanilla_EE_IDs_Dict.Count; j++)
							{
								if (assetid == GetAssetIDFromIndex(j))
								{
									eelist[i].AssetId = kibbleshair;
								}
							}
						}
					}
					
                    // Since Kibellah's hair is black, no need for dedicated ramp indicies since the default is 0 which is the black ramp.
                    LogDebug($"Number of AssetIDs in m_SavedRampIndices = {rmplist.Count}.");

                    for (int i = rmplist.Count - 1; i >= 0; i--) // Start at the end of the list and loop backwards to quickly catch a newly added entry.
                    {
                        var rmpee = rmplist[i].EquipmentEntityLink.AssetId;

                        if (rmpee == GetAssetIDFromIndex(1))
                        {
                            LogDebug($"AssetID at m_SavedRampIndices index {i} is {rmpee}. Replacing.");

                            rmplist[i].EquipmentEntityLink.AssetId = GetAssetIDFromIndex(2);
                            rmplist[i].PrimaryIndex = 17;

                            return;
                        }
                    }
                }
			}
			catch (Exception ex)
			{
				Logger.Log($"Caught exception in EditUnitBundle: \n{ex}");
			}
		}
    }

    [Serializable]
    public class Settings
    {
		public bool ModActive = true;
		public bool DetailedLogging = false;
        public int SelectedHairType = 0;
    }
}
