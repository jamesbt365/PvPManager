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
  public override string Description => "No idea!";

  public PvPManager(Main game) : base(game) {}

  public override void Initialize()
  {
    // command to toggle yourself/others, no arg = self, can ask for a specific user, and a specific state.
    // command to switch your team/others teams
    // Make it possible to force pvp on?
    // Command to force teams?
    Commands.ChatCommands.Add(new Command("pvpmanager.pvp.enableall", EnablePvPAll, "pvp-all"));
  }

  public void EnablePvPAll(CommandArgs args)
  {
    var player = args.Player;
    if (args.Player == null) {
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