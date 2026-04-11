using Elin.Plugin.Generated;
using Elin.Plugin.Main.PluginHelpers;

namespace Elin.Plugin.Main.Models.Impl
{

    public static class DramaCustomSequenceImpl
    {
        #region define

        private static class LanguageId
        {
            /// <summary>
            /// 共に暮らさないか
            /// </summary>
            /// <remarks>Lang!General: daInvite</remarks>
            public const string InviteHome = "daInvite";

            /// <summary>
            /// もう待機する必要はない
            /// </summary>
            /// <remarks>Lang!General: enableMove</remarks>
            public const string EnableMove = "enableMove";
        }

        private static class JumpId
        {
            /// <summary>
            /// 添い寝して欲しい/添い寝をやめたい のジャンプID
            /// </summary>
            public const string SleepBeside = "_sleepBeside";

            /// <summary>
            /// もう待機する必要はない のジャンプID
            /// </summary>
            public const string EnableMove = "_enableMove";
            /// <summary>
            /// <see cref="EnableMove"/> に対して差し込むMod用のジャンプID
            /// </summary>
            /// <remarks>このID自体は Mod 内で使用を完結させ、表示用に Elin を経由することにはなるが最終的には <see cref="EnableMove"/> を指すようにすること。</remarks>
            public const string HookEnableMove = "_enableMove@" + Package.Id;

        }

        #endregion

        #region property

        private static Chara? BuildArgumentCharacter { get; set; }

        #endregion

        #region function

        #endregion

        #region DramaCustomSequence

        internal static bool BuildPrefix(DramaCustomSequence instance, Chara c)
        {
            BuildArgumentCharacter = c;
            return true;
        }

        internal static void BuildPostfix(DramaCustomSequence instance, Chara c)
        {
            BuildArgumentCharacter = null;
        }

        public static bool Choice2Prefix(DramaCustomSequence instance, string lang, ref string idJump)
        {
            // 特定のセリフを選択肢に表示させないようにしたりジャンプ先加工したり、忙しい子
            ModHelper.LogDev($"Choice2Prefix: lang={lang}, idJump={idJump}");

            if (lang == LanguageId.InviteHome)
            {
                ModHelper.LogDev($"[ignore] {lang}, {idJump}");
                return false;
            }

            if (lang == LanguageId.EnableMove)
            {
                // フックした「もう待機する必要はない」が指定されている場合は Elin 側の正式なジャンプIDに差し替え
                if (idJump == JumpId.HookEnableMove)
                {
                    idJump = JumpId.EnableMove;
                    ModHelper.LogDev($"[hook] {nameof(idJump)}: {JumpId.HookEnableMove} -> {idJump}");
                    return true;
                }

                ModHelper.LogDev($"[ignore] {lang}, {idJump}");
                return false;
            }

            return true;
        }

        public static bool ChoicePrefix(DramaCustomSequence instance, string lang, string idJump, bool cancel)
        {
            var currentCharacter = BuildArgumentCharacter;
            if (currentCharacter == null)
            {
                ModHelper.LogNotExpected($"{nameof(BuildArgumentCharacter)} is null");
                return true;
            }

            ModHelper.LogDev($"ChoicePrefix: lang={lang}, idJump={idJump}, cancel={cancel}, currentCharacter={currentCharacter.Name}");

            // 添い寝の選択肢の直前に、移動許可の選択肢を差し込む
            // ここで待機してほしい Choice("disableMove", "_disableMove"); の選択肢と位置を合わせるための無理やり感
            if (idJump == JumpId.SleepBeside)
            {
                // [ELIN:DramaCustomSequence.Build]
                // -> if (c.IsPCParty) ... else if (!c.noMove)
                if (!currentCharacter.IsPCParty)
                {
                    if (currentCharacter.noMove)
                    {
                        ModHelper.LogDev("[add] LanguageId.EnableMove, JumpId.HookEnableMove");
                        instance.Choice2(LanguageId.EnableMove, JumpId.HookEnableMove);
                    }
                }
            }

            return true;
        }


        #endregion
    }
}
