using TerrariaApi.Server;
using Terraria;
using TShockAPI;
using IL.Terraria.GameContent.Dyes;

namespace PvPManager;

[ApiVersion(2, 1)]
public class PvPManager: TerrariaPlugin {
  public override string Name => "PvPManager";
  public override Version Version => new Version(0, 1, 0);
  public override string Author => "jamesbt365";
  public override string Description => "A plugin to manage the state of PvP or teams for yourself and the server.";

  public PvPManager(Main game): base(game) {}

  public override void Initialize() {
    // command to switch your team/others teams
    // Make it possible to force pvp on?
    // Command to force teams?
    // command to get all hostile players.
    Commands.ChatCommands.Add(new Command("pvpmanager.pvp.enableall", EnablePvPAll, "pvp-all") {
        HelpText = "Enable PvP for everybody!"
    });
    // Need version to disable.
    Commands.ChatCommands.Add(new Command("pvpmanager.pvp.toggle", TogglePvP, "toggle-pvp", "pvp-toggle", "tpvp") {
        HelpText = "Toggle a users PvP status!"
    });
    Commands.ChatCommands.Add(new Command("pvpmanager.teams.setall", SetAllTeam, "team-set-all") {
        HelpText = "Change the team for everybody to a specified team!"
    });
  }

    private void SetAllTeam(CommandArgs args)
    {
        var player = args.Player;
        if (player == null) {
            return;
        }

        foreach(var ply in TShock.Players) {
            player.SendInfoMessage(ply?.Team.ToString());
            // 0 to 5?
        }
    }

    private void TogglePvP(CommandArgs args) {
    var player = args.Player;
    if (player == null) {
      return;
    }
    var silent = args.Silent;
    if (silent && args.Parameters.Count > 0 && !player.HasPermission("pvpmanager.pvp.toggle.silent")) {
        player.SendErrorMessage("You do not have permission to execute this silently.");
        return;
    }

    if (args.Parameters.Count == 0) {
      if (Main.player[args.Player.Index].hostile) {
        player.SetPvP(false);
      } else {
        player.SetPvP(true);
      }
      player.SendInfoMessage("Your PvP status has been toggled!");
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
          if (!silent) {
            matched_players[0].SendInfoMessage("Your PvP status has been toggled for you!");
          }
        }
      } else {
        player.SendErrorMessage("You do not have permission to use this on others.");
      }
    }
  }

  public void EnablePvPAll(CommandArgs args) {
    var player = args.Player;
    if (player == null) {
      return;
    }

    if (args.Silent) {
      if (player.HasPermission("pvpmanager.pvp.enableall.silent")) {
        player.SendSuccessMessage("Enabling PVP for everyone!");
        foreach(var ply in TShock.Players) {
          ply?.SetPvP(true);
        }
      } else {
        player.SendErrorMessage("You do not have permission to execute this silently.");
      }

    } else {
      TShock.Utils.Broadcast("Everyones PVP has been enabled!", 255, 255, 0);
      foreach(var ply in TShock.Players) {
        ply?.SetPvP(true);
      }
    }
  }
  protected override void Dispose(bool disposing) {
    if (disposing) {}
    base.Dispose(disposing);
  }
}
