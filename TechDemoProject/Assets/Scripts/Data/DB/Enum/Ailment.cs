namespace Tech.Data.DB
{
    public enum Ailment
    {
        None, //Does nothing
        Burn, //Deal DOT cannot lower unit's HP below 1
        Poison, //Deal DOT cannot lower unit's HP below 1
        Freeze, //Render unit incapable to take any action. Can not be a target as well by skills or attack.
        Disease, //Reduce unit's base stats by 10%
        Paralyze, //Render unit incapable to take any action.
        Flinch, //Disables the Unit's next action (includes unit's special attack)
        Blind, //Reduce the unit's accuracy by 30% 
        Curse, //Reduces magic skill and ability output by 30%
        Sealed, //Prevent unit from using their signature ability. 
        Berserk, //Boost target unit attack by 30%, restrict the player to only attack.
        Silence, //Prevent unit from using skills.
        Sleep, //Render unit incapable to take any action until hit with basic attack. Unit also gain HOT per turn from sleep.
        Confuse, //Unit might attack ally or self
        Fear, //Immobilize unit (instant KO)
        Undead, //Take damage for the amount healed (heal and HOT) and revival result in instant KO.
    }
}