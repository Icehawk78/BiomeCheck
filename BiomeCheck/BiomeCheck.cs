using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;


namespace BiomeCheckPlugin
{
    [ApiVersion(1, 14)]
    public class BiomeCheck : TerrariaPlugin
    {
        public BiomeCheck(Main game) : base(game) { }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command(DoBiomeCheck, "biomecheck"));    // Check biome
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override Version Version
        {
            get { return new Version("1.0"); }
        }

        public override string Name
        {
            get { return "Biome Check"; }
        }

        public override string Author
        {
            get { return "Icehawk78"; }
        }

        public override string Description
        {
            get { return "Adds the ability to check what biome a user is in."; }
        }

        private static Player GetPlayer(CommandArgs args)
        {
            if (args == null || args.Parameters == null)
            {
                return null;
            }
            if (args.Parameters.Count == 1)
            {
                List<TSPlayer> results = TShock.Utils.FindPlayer(args.Parameters[0]);
                if (results.Count > 0)
                {
                    return results[0].TPlayer;
                }
                else
                {
                    return null;
                }
            }
            else if (args.Parameters.Count == 0)
            {
                return args.TPlayer;
            }
            else
            {
                return null;
            }
        }

        public void DoBiomeCheck(CommandArgs args)
        {
            Player player = GetPlayer(args);
            String output = "";
            if (player == null)
            {
                output = "Usage: /biomecheck [player]";
            }
            else
            {
                HashSet<KeyValuePair<string, bool>> zones = new HashSet<KeyValuePair<string, bool>>() {
                    {new KeyValuePair<string, bool> ("Crimson", player.zoneBlood)},
                    {new KeyValuePair<string, bool> ("Corruption", player.zoneEvil)},
                    {new KeyValuePair<string, bool> ("Hallow", player.zoneHoly)},
                    {new KeyValuePair<string, bool> ("Ice", player.zoneSnow)},
                    {new KeyValuePair<string, bool> ("Jungle", player.zoneJungle)},
                    {new KeyValuePair<string, bool> ("Dungeon", player.zoneDungeon)},
                    {new KeyValuePair<string, bool> ("Meteor", player.zoneMeteor)}
                };

                output = String.Join(", ",
                    zones.Where(zone => zone.Value == true)
                    .Select(zone => zone.Key)
                    .ToList());

                if (String.IsNullOrWhiteSpace(output))
                {
                    output = "None/Forest";
                }
                args.Player.SendMessage("Biomes: " + output, Color.Blue);
            }
        }
    }
}
