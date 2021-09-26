using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;


namespace CartWeightReloaded
{
    [BepInPlugin("blake.cartweightreloaded", "Cart Weight Reloaded", "0.0.0.1")]
    [BepInProcess("valheim.exe")]
    public class BepInExPlugin : BaseUnityPlugin
    {
        private static BepInExPlugin context;
        private Harmony harmony;
        public static ConfigEntry<float> massModifier;
        private void Awake()
        {
            context = this;
            massModifier = Config.Bind<float>("General", "Mass Modifier", 0.5f, "Weight reduction. 0.5 is 50%.");

            harmony = new Harmony(Info.Metadata.GUID);
            harmony?.PatchAll();

        }

        private void OnDestroy()
        {
            harmony?.UnpatchSelf();
        }

        [HarmonyPatch(typeof(Vagon), "SetMass")]
        static class MassPatch
        {
            static void Prefix(ref float mass)
            {
               // float before = mass;
                mass = Mathf.Max(0, mass * massModifier.Value);
            }
        }

    }
}
