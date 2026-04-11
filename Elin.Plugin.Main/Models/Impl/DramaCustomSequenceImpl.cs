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
            /// <remarks>Lang: daInvite</remarks>
            public const string InviteHome = "daInvite";

            /// <summary>
            /// もう待機する必要はない
            /// </summary>
            /// <remarks>Lang: enableMove</remarks>
            public const string EnableMove = "enableMove";
        }

        #endregion

        #region property

        #endregion

        #region function

        #endregion

        #region DramaCustomSequence

        public static bool Choice2Prefix(DramaCustomSequence instance, string lang, string idJump)
        {
            if (lang == LanguageId.InviteHome)
            {
                return false;
            }

            if (lang == LanguageId.EnableMove)
            {
                return false;
            }

            return true;
        }


        #endregion
    }
}
