using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.Toast; // Required for the pop-ups and sounds
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace InventoryCombatAlert
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Inventory Combat Alert";

        [PluginService] private static ICondition Condition { get; set; } = null!;
        [PluginService] private static IChatGui ChatGui { get; set; } = null!;
        [PluginService] private static IToastGui ToastGui { get; set; } = null!;

        // Warning threshold (3 slots or fewer triggers the warning)
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
            var manager = InventoryManager.Instance();
            if (manager == null) return;

            int freeSlots = 0;
            
            // The 4 main inventory bags
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
                    if (item->ItemId == 0)
                    {
                        freeSlots++;
                    }
                }
            }

            // --- NOTIFICATION & SOUND LOGIC ---
            
            if (freeSlots == 0)
            {
                // 1. Play the "Error Buzz" sound + Big Error Message
                ToastGui.ShowError("INVENTORY FULL! You will miss loot! It may haunt you!");

                // 2. Also print to chat in Red
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
                // 1. Play "Quest Complete" Ding + Show pop-up
                // We use 'ShowQuest' because it has a nice clear sound effect.
                ToastGui.ShowQuest($"Low Inventory: {freeSlots} slots left!", new QuestToastOptions 
                { 
                    PlaySound = true,
                    DisplayCheckmark = true
                });

                // 2. Print to chat
                ChatGui.Print($"[Inventory Alert] Watch out! You only have {freeSlots} inventory slots left.");
            }
        }
    }
}