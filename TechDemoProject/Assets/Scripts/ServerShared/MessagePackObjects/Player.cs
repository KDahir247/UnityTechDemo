using MessagePack;

namespace Tech.Network.Param
{
    /*
     * Order
     * ID GUID or Ulid,
     * Icon Image,
     * Name String, 
     * Level unsigned int,
     * Guild String,
     * Bio String,
     * Gold unsigned int
     * Gem unsigned int
     * Resource unsigned int
     * stamina unsigned int
     * 
     */

    [MessagePackObject]
    public class Player
    {
        // [Key(0)][MessagePackObject()] public Ulid ID { get; set; }

        [Key(0)] public string Name { get; set; }

        [Key(1)] public uint Level { get; set; }

        // [Key(3)] public string GuildName { get; set; }
        //
        // [Key(4)] public string Bio { get; set; }
        //
        // [Key(5)] public uint GoldAmount { get; set; }
        //
        // [Key(6)] public uint GemAmount { get; set; }
        //
        // [Key(7)] public uint ResourceAmount { get; set; }
        //
        // [Key(8)] public uint StaminaAmount { get; set; }
    }
}