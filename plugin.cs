using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.Toast;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.Game; // <--- The new magic ingredient

namespace InventoryCombatAlert
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Inventory Combat Alert";

        [PluginService] private static ICondition Condition { get; set; } = null!;
        [PluginService] private static IChatGui ChatGui { get; set; } = null!;
        [PluginService] private static IToastGui ToastGui { get; set; } = null!;

        // Warning threshold
        private const int LowSpaceThreshold = 3;

        public Plugin(IDalamudPluginInterface pluginInterface)
        {
            Condition.ConditionChange += OnConditionChange;
        }

        public void Dispose()
        {
            Condition.ConditionChange -= OnConditionChange;
        }

        private void OnConditionChange(ConditionFlag flag, bool value)
        {
            if (flag == ConditionFlag.InCombat && value)
            {
                CheckInventory();
            }
        }

        private unsafe void CheckInventory()
        {
            // NEW WAY: Access the game memory directly
            var manager = InventoryManager.Instance();
            if (manager == null) return;

            int freeSlots = 0;

            // The 4 main inventory bags are types 0, 1, 2, 3
            var bagTypes = new[] {
                InventoryType.Inventory1,
                InventoryType.Inventory2,
                InventoryType.Inventory3,
                InventoryType.Inventory4
            };

            foreach (var type in bagTypes)
            {
                var container = manager->GetInventoryContainer(type);
                if (container == null) continue;

                for (int i = 0; i < container->Size; i++)
                {
                    var item = container->GetInventorySlot(i);
                    // In the new system, we check if the ItemID is 0
                    if (item->ItemId == 0)
                    {
                        freeSlots++;
                    }
                }
            }

            // --- NOTIFICATION LOGIC ---
            if (freeSlots == 0)
            {
                ToastGui.ShowError("INVENTORY FULL! You will miss loot!");

                var errorBuilder = new SeString(new List<Payload>
                {
                    new UIForegroundPayload(500),
                    new TextPayload("[Inventory Alert] YOUR BAGS ARE COMPLETELY FULL!"),
                    new UIForegroundPayload(0)
                });
                ChatGui.Print(errorBuilder);
            }
            else if (freeSlots <= LowSpaceThreshold)
            {
                ChatGui.Print($"[Inventory Alert] Watch out! You only have {freeSlots} inventory slots left.");
            }
        }
    }
}