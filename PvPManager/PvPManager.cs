using TerrariaApi.Server;
using Terraria;
using TShockAPI;
using IL.Terraria.Testing.ChatCommands;

namespace PvPManager;

[ApiVersion(2, 1)]
public class PvPManager : TerrariaPlugin
{
  public override string Name => "Test";
  public override Version Version => new Version(0, 1, 0);
  public override string Author => "jamesbt365";
  public override string Description => "A plugin to manage the state of PvP or teams for yourself and the server.";

  public PvPManager(Main game) : base(game) {}

  public override void Initialize()
  {
    // command to switch your team/others teams
    // Make it possible to force pvp on?
    // Command to force teams?
    // command to get all hostile players.
    // command to view teams.
    Commands.ChatCommands.Add(new Command("pvpmanager.pvp.enableall", EnablePvPAll, "pvp-all"));
    Commands.ChatCommands.Add(new Command("permissions.pvp.toggle", TogglePvP, "toggle-pvp"));
  }

  private void TogglePvP(CommandArgs args) {
    var player = args.Player;
    if (player == null) {
      return;
    }
    var silent = args.Silent;
    if (silent == true && args.Parameters.Count > 0) {
      if (!player.HasPermission("pvpmanager.pvp.toggle.silent")) {
        player.SendErrorMessage("You do not have permission to execute this silently.");
      }
    }

    if (args.Parameters.Count == 0) {
      if (Main.player[args.Player.Index].hostile) {
        player.SetPvP(false);
      } else {
        player.SetPvP(true);
      }
      return;
    }

    if (args.Parameters.Count == 1) {
      if (player.HasPermission("pvpmanager.pvp.toggle.others")) {
        string plStr = args.Parameters[0];
        var matched_players = TSPlayer.FindByNameOrID(plStr);
        if (matched_players.Count == 0) {
          args.Player.SendErrorMessage("Player not found!");
        } else if (matched_players.Count > 1) {
          args.Player.SendMultipleMatchError(matched_players.Select(p => p.Name));
        } else {
          if (Main.player[matched_players[0].Index].hostile) {
            player.SetPvP(false);
          } else {
            player.SetPvP(true);
          }
          if (silent == false) {
            matched_players[0].SendInfoMessage("Your PvP status has been toggled!");
          }
        }
      } else {
        player.SendErrorMessage("You do not have permission to use this on others.");
      }
    }
  }

    public void EnablePvPAll(CommandArgs args)
  {
    var player = args.Player;
    if (player == null) {
      return;
    }

    if (args.Silent == true) {
      if (player.HasPermission("pvpmanager.pvp.enableall.silent")) {
        player.SendSuccessMessage("Enabling PVP for everyone!");
        foreach (var ply in TShock.Players) {
          ply?.SetPvP(true);
        }
      } else {
        player.SendErrorMessage("You do not have permission to execute this silently.");
      }

    } else {
      TShock.Utils.Broadcast("Everyones PVP has been enabled!", 255, 255, 0);
      foreach (var ply in TShock.Players) {
        ply?.SetPvP(true);
      }
    }
  }
  protected override void Dispose(bool disposing)
  {
    if (disposing) {}
    base.Dispose(disposing);
  }
}
