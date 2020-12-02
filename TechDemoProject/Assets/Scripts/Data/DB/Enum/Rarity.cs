using System;

namespace Tech.Data.DB
{
    [Serializable] //Just in case enum uses nonstandard enum values
    public enum Rarity
    {
        N, //Normal
        U, //Uncommon
        UR, //Uncommon Rare
        R, //Rare
        SR, //Secret Rare
        SS, //Super Secret
        SSR, //Super Secret Relic
        SSS, //Super Secret S tier Relic
        SSSPlus // Super Secret S tier Relic Plus
    }
}