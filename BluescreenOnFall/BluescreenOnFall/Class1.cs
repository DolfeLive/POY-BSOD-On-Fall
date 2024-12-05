using MelonLoader;
using System;
using System.Runtime.InteropServices;
using POKModManager;
using System.Collections;
using UnityEngine;

[assembly: MelonInfo(typeof(BluescreenOnFall.BluescreenOnFallInit), "BSOD On Fall", "1.0.0", "DolfeMods")]
[assembly: MelonGame("TraipseWare", "Peaks of Yore")]

namespace BluescreenOnFall
{
    public class BluescreenOnFallInit : MelonMod
    {
        public static MelonLogger.Instance ls;
        public override void OnLateInitializeMelon()
        {
            ls = new MelonLogger.Instance("BluescreenOnFall");

            ls.Msg("BluescreenOnFall loaded; Mod Has to be enabled in the mod manager Settings!");
            
            POKManager.RegisterMod(new BluescreenOnFall(), "BSOD On Fall", "1.0.1", "Gives you a bluescreen if you fall");
        }
    }

    public class BluescreenOnFall : ModClass
    {
        [DoNotSave] [Editable] public bool YouSure { get; set; }

        private bool oldVar = false;

        public override void Update(float deltaTime)
        {
            if (FallingEvent.fallenToDeath == true && oldVar == false)
            {
                oldVar = true;
                print("player fell to death");
                if (YouSure)
                {
                    printWarning("Blue screening :D");
                    MelonCoroutines.Start(DelayedBS());
                }
            }

        }

        IEnumerator DelayedBS()
        {
            yield return new WaitForSeconds(1f);
            BlueScreen.DO_NOT_RUN_UNLESS_YOU_WANT_A_BSOD();
        }


    }

    public class BlueScreen
    {
        private static uint STATUS_ASSERTION_FAILURE = 3735936685u;
        public static void DO_NOT_RUN_UNLESS_YOU_WANT_A_BSOD()
        {
            bool PreviousValue = false;
            RtlAdjustPrivilege(19, bEnablePrivilege: true, IsThreadPrivilege: false, out PreviousValue);
            uint Response = 0u;
            IntPtr intPtr = Marshal.StringToHGlobalAnsi("");
            NtRaiseHardError(STATUS_ASSERTION_FAILURE, 0u, 0u, IntPtr.Zero, 6u, out Response);
        }

        [DllImport("ntdll.dll")]
        private static extern uint RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        [DllImport("ntdll.dll")]
        private static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);
    }
}
