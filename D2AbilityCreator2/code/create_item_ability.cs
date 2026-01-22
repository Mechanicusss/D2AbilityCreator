using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static D2AbilityCreator2.Form1;

namespace D2AbilityCreator2
{
    public partial class Form1 : Form
    {
        public void CreateAbilityButtonClick(object sender, EventArgs e)
        {
            TextBox namebox = (TextBox)splitContainer1.Panel2.Tag;
            if (namebox.Text != "")
            {
                TreeNode[] neednode = treeView1.Nodes.Find(CreateAbility(namebox.Text, new Dictionary<string, string>(), null), true);
                treeView1.SelectedNode = neednode[0];
            }
            else
            {
                MessageBox.Show("Invalid name!", "Error");
            }
        }

        public string CreateAbility(string name, Dictionary<string, string> data, string nodename)
        {
            TreeNode newnode = new TreeNode();
            if (nodename != null)
            {
                TreeNode[] neednode = treeView1.Nodes.Find(nodename, true);
                newnode = neednode[0].Nodes.Add(name);
                if (!neednode[0].IsExpanded)
                    neednode[0].Toggle();
            }
            else
            {
                newnode = treeView1.Nodes.Add(name);
            }
            newnode.Name = "node" + nodenum;
            nodenum++;
            string[] ThisEventList = new string[2 + EventList.Length];
            new string[] { "AbilityValues", "Modifiers" }.CopyTo(ThisEventList, 0);
            EventList.CopyTo(ThisEventList, 2);
            object[] tagobj = new object[] {
                    name,
                    "ability",
                    new MyCheckboxString(){ name = "ID", check = data.TryGetValue("ID", out _), str = MyMiniF(data,"ID") ?? "" },
                    new MyStringSelect(){ name = "AbilityBehavior", str = MyMiniF(data,"AbilityBehavior") ?? "", selectlist = AbilityBehaviorSelectedList },
                    new MyCheckboxStringSelect(){ name = "AbilityType", check = data.TryGetValue("AbilityType", out _), str = MyMiniF(data,"AbilityType") ?? "", selectlist = AbilityTypeSelectedList },
                    new MyCheckboxStringSelect(){ name = "AbilityUnitTargetType", check = data.TryGetValue("AbilityUnitTargetType", out _), str = MyMiniF(data,"AbilityUnitTargetType") ?? "", selectlist = UnitTargetTypeSelectedList },
                    new MyCheckboxStringSelect(){ name = "AbilityUnitTargetTeam", check = data.TryGetValue("AbilityUnitTargetTeam", out _), str = MyMiniF(data,"AbilityUnitTargetTeam") ?? "", selectlist = TeamsList },
                    new MyCheckboxStringSelect(){ name = "AbilityUnitDamageType", check = data.TryGetValue("AbilityUnitDamageType", out _), str = MyMiniF(data,"AbilityUnitDamageType") ?? "", selectlist = DamageTypeSelectedList },
                    new MyCheckboxStringSelect(){ name = "SpellImmunityType", check = data.TryGetValue("SpellImmunityType", out _), str = MyMiniF(data,"SpellImmunityType") ?? "", selectlist = SpellImmunityTypeList },
                    new MyCheckboxStringSelect(){ name = "SpellDispellableType", check = data.TryGetValue("SpellDispellableType", out _), str = MyMiniF(data,"SpellDispellableType") ?? "", selectlist = Dispellable },
                    new MyCheckboxString(){ name = "CastFilterRejectCaster", check = data.TryGetValue("CastFilterRejectCaster", out _), str = MyMiniF(data,"CastFilterRejectCaster") ?? "1" },
                    new MyCheckboxString(){ name = "IsCastableWhileHidden", check = data.TryGetValue("IsCastableWhileHidden", out _), str = MyMiniF(data,"IsCastableWhileHidden") ?? "1" },
                    new MyCheckboxString(){ name = "IsOnCastBar", check = data.TryGetValue("IsOnCastBar", out _), str = MyMiniF(data,"IsOnCastBar") ?? "1" },
                    new MyCheckboxString(){ name = "IsOnLearnbar", check = data.TryGetValue("IsOnLearnbar", out _), str = MyMiniF(data,"IsOnLearnbar") ?? "1" },
                    new MyCheckboxString(){ name = "AbilityTextureName", check = data.TryGetValue("AbilityTextureName", out _), str = MyMiniF(data,"AbilityTextureName") ?? "" },
                    new MyCheckboxString(){ name = "MaxLevel", check = data.TryGetValue("MaxLevel", out _), str = MyMiniF(data,"MaxLevel") ?? "1" },
                    new MyCheckboxString(){ name = "RequiredLevel", check = data.TryGetValue("RequiredLevel", out _), str = MyMiniF(data,"RequiredLevel") ?? "" },
                    new MyCheckboxString(){ name = "LevelsBetweenUpgrades", check = data.TryGetValue("LevelsBetweenUpgrades", out _), str = MyMiniF(data,"LevelsBetweenUpgrades") ?? "" },
                    new MyCheckboxStringSelect(){ name = "AbilityCastAnimation", check = data.TryGetValue("AbilityCastAnimation", out _), str = MyMiniF(data,"AbilityCastAnimation") ?? "", selectlist = AbilityCastAnimationList },
                    new MyCheckboxString(){ name = "AnimationPlaybackRate", check = data.TryGetValue("AnimationPlaybackRate", out _), str = MyMiniF(data,"AnimationPlaybackRate") ?? "1" },
                    new MyCheckboxString(){ name = "AnimationIgnoresModelScale", check = data.TryGetValue("AnimationIgnoresModelScale", out _), str = MyMiniF(data,"AnimationIgnoresModelScale") ?? "1" },
                    new MyCheckboxString(){ name = "AbilityCastRange", check = data.TryGetValue("AbilityCastRange", out _), str = MyMiniF(data,"AbilityCastRange") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityCastRangeBuffer", check = data.TryGetValue("AbilityCastRangeBuffer", out _), str = MyMiniF(data,"AbilityCastRangeBuffer") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityCastMinimumRange", check = data.TryGetValue("AbilityCastMinimumRange", out _), str = MyMiniF(data,"AbilityCastMinimumRange") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityChannelTime", check = data.TryGetValue("AbilityChannelTime", out _), str = MyMiniF(data,"AbilityChannelTime") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityChannelledManaCostPerSecond", check = data.TryGetValue("AbilityChannelledManaCostPerSecond", out _), str = MyMiniF(data,"AbilityChannelledManaCostPerSecond") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityDuration", check = data.TryGetValue("AbilityDuration", out _), str = MyMiniF(data,"AbilityDuration") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityCastPoint", check = data.TryGetValue("AbilityCastPoint", out _), str = MyMiniF(data,"AbilityCastPoint") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityCooldown", check = data.TryGetValue("AbilityCooldown", out _), str = MyMiniF(data,"AbilityCooldown") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityManaCost", check = data.TryGetValue("AbilityManaCost", out _), str = MyMiniF(data,"AbilityManaCost") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityGoldCost", check = data.TryGetValue("AbilityGoldCost", out _), str = MyMiniF(data,"AbilityGoldCost") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityUpgradeGoldCost", check = data.TryGetValue("AbilityUpgradeGoldCost", out _), str = MyMiniF(data,"AbilityUpgradeGoldCost") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityDamage", check = data.TryGetValue("AbilityDamage", out _), str = MyMiniF(data,"AbilityDamage") ?? "0" },
                    new MyCheckboxString(){ name = "AOERadius", check = data.TryGetValue("AOERadius", out _), str = MyMiniF(data,"AOERadius") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityProcsMagicStick", check = data.TryGetValue("AbilityProcsMagicStick", out _), str = MyMiniF(data,"AbilityProcsMagicStick") ?? "1" },
                    new MyCheckboxString(){ name = "HotKeyOverride", check = data.TryGetValue("HotKeyOverride", out _), str = MyMiniF(data,"HotKeyOverride") ?? "" },
                    new MyCheckboxString(){ name = "DisplayAdditionalHeroes", check = data.TryGetValue("DisplayAdditionalHeroes", out _), str = MyMiniF(data,"DisplayAdditionalHeroes") ?? "1" },
                };
            data = DeleteItems(data, new string[] {
                "BaseClass",
                "ID",
                "AbilityBehavior",
                "AbilityType",
                "AbilityUnitTargetType",
                "AbilityUnitTargetTeam",
                "AbilityUnitDamageType",
                "SpellImmunityType",
                "SpellDispellableType",
                "CastFilterRejectCaster",
                "IsCastableWhileHidden",
                "IsOnCastBar",
                "IsOnLearnbar",
                "AbilityTextureName",
                "MaxLevel",
                "RequiredLevel",
                "LevelsBetweenUpgrades",
                "AbilityCastAnimation",
                "AnimationPlaybackRate",
                "AnimationIgnoresModelScale",
                "AbilityCastRange",
                "AbilityCastRangeBuffer",
                "AbilityCastMinimumRange",
                "AbilityChannelTime",
                "AbilityChannelledManaCostPerSecond",
                "AbilityDuration",
                "AbilityCastPoint",
                "AbilityCooldown",
                "AbilityManaCost",
                "AbilityGoldCost",
                "AbilityUpgradeGoldCost",
                "AbilityDamage",
                "AOERadius",
                "AbilityProcsMagicStick",
                "HotKeyOverride",
                "DisplayAdditionalHeroes"
            });
            Array.Resize(ref tagobj, tagobj.Length + data.Count + 1);
            if (data.Count > 0)
            {
                for (int i = 0; data.Count > i; i++)
                {
                    tagobj[tagobj.Length + i - (data.Count + 1)] = new MyCheckboxString() { name = data.ElementAt(i).Key, check = true, str = data.ElementAt(i).Value };
                }
            }
            tagobj[tagobj.Length - 1] = new MyAddNodes() { items = ThisEventList };
            newnode.Tag = tagobj;
            return newnode.Name;
        }

        public Dictionary<string, string> DeleteItems(Dictionary<string, string> data, string[] deletelist)
        {
            for (int i = 0; deletelist.Length > i; i++)
            {
                data.Remove(deletelist[i]);
            }
            return data;
        }

        public string CreateItem(string name, Dictionary<string, string> data, string nodename)
        {
            TreeNode newnode = new TreeNode();
            if (nodename != null)
            {
                TreeNode[] neednode = treeView1.Nodes.Find(nodename, true);
                newnode = neednode[0].Nodes.Add(name);
                if (!neednode[0].IsExpanded)
                    neednode[0].Toggle();
            }
            else
            {
                newnode = treeView1.Nodes.Add(name);
            }
            newnode.Name = "node" + nodenum;
            nodenum++;
            string[] ThisEventList = new string[3 + EventList.Length];
            new string[] { "ItemRequirements", "AbilityValues", "Modifiers" }.CopyTo(ThisEventList, 0);
            EventList.CopyTo(ThisEventList, 3);
            object[] tagobj = new object[] {
                    name,
                    "item",
                    new MyCheckboxString(){ name = "ID", check = data.TryGetValue("ID", out _), str = MyMiniF(data,"ID") ?? "" },
                    new MyCheckboxString(){ name = "ItemRecipe", check = data.TryGetValue("ItemRecipe", out _), str = MyMiniF(data,"ItemRecipe") ?? "0" },
                    new MyCheckboxString(){ name = "ItemResult", check = data.TryGetValue("ItemResult", out _), str = MyMiniF(data,"ItemResult") ?? "" },
                    new MyCheckboxString(){ name = "AbilityName", check = data.TryGetValue("AbilityName", out _), str = MyMiniF(data,"AbilityName") ?? "" },
                    new MyStringSelect(){ name = "AbilityBehavior", str = MyMiniF(data,"AbilityBehavior") ?? "", selectlist = AbilityBehaviorSelectedList },
                    new MyCheckboxStringSelect(){ name = "AbilityType", check = data.TryGetValue("AbilityType", out _), str = MyMiniF(data,"AbilityType") ?? "", selectlist = AbilityTypeSelectedList },
                    new MyCheckboxStringSelect(){ name = "AbilityUnitTargetType", check = data.TryGetValue("AbilityUnitTargetType", out _), str = MyMiniF(data,"AbilityUnitTargetType") ?? "", selectlist = UnitTargetTypeSelectedList },
                    new MyCheckboxStringSelect(){ name = "AbilityUnitTargetTeam", check = data.TryGetValue("AbilityUnitTargetTeam", out _), str = MyMiniF(data,"AbilityUnitTargetTeam") ?? "", selectlist = TeamsList },
                    new MyCheckboxStringSelect(){ name = "AbilityUnitDamageType", check = data.TryGetValue("AbilityUnitDamageType", out _), str = MyMiniF(data,"AbilityUnitDamageType") ?? "", selectlist = DamageTypeSelectedList },
                    new MyCheckboxStringSelect(){ name = "SpellImmunityType", check = data.TryGetValue("SpellImmunityType", out _), str = MyMiniF(data,"SpellImmunityType") ?? "", selectlist = SpellImmunityTypeList },
                    new MyCheckboxString(){ name = "CastFilterRejectCaster", check = data.TryGetValue("CastFilterRejectCaster", out _), str = MyMiniF(data,"CastFilterRejectCaster") ?? "" },
                    new MyCheckboxString(){ name = "Model", check = data.TryGetValue("Model", out _), str = MyMiniF(data,"Model") ?? "" },
                    new MyCheckboxString(){ name = "Effect", check = data.TryGetValue("Effect", out _), str = MyMiniF(data,"Effect") ?? "" },
                    new MyCheckboxString(){ name = "AbilityTextureName", check = data.TryGetValue("AbilityTextureName", out _), str = MyMiniF(data,"AbilityTextureName") ?? "" },
                    new MyCheckboxString(){ name = "MaxUpgradeLevel", check = data.TryGetValue("MaxUpgradeLevel", out _), str = MyMiniF(data,"MaxUpgradeLevel") ?? "1" },
                    new MyCheckboxString(){ name = "ItemBaseLevel", check = data.TryGetValue("ItemBaseLevel", out _), str = MyMiniF(data,"ItemBaseLevel") ?? "1" },
                    new MyCheckboxStringSelect(){ name = "AbilityCastAnimation", check = data.TryGetValue("AbilityCastAnimation", out _), str = MyMiniF(data,"AbilityCastAnimation") ?? "", selectlist = AbilityCastAnimationList },
                    new MyCheckboxString(){ name = "AbilityCastRange", check = data.TryGetValue("AbilityCastRange", out _), str = MyMiniF(data,"AbilityCastRange") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityCastPoint", check = data.TryGetValue("AbilityCastPoint", out _), str = MyMiniF(data,"AbilityCastPoint") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityCooldown", check = data.TryGetValue("AbilityCooldown", out _), str = MyMiniF(data,"AbilityCooldown") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityManaCost", check = data.TryGetValue("AbilityManaCost", out _), str = MyMiniF(data,"AbilityManaCost") ?? "0" },
                    new MyCheckboxString(){ name = "AbilityDamage", check = data.TryGetValue("AbilityDamage", out _), str = MyMiniF(data,"AbilityDamage") ?? "0" },
                    new MyCheckboxString(){ name = "AOERadius", check = data.TryGetValue("AOERadius", out _), str = MyMiniF(data,"AOERadius") ?? "0" },
                    new MyCheckboxStringSelect(){ name = "ItemDeclarations", check = data.TryGetValue("ItemDeclarations", out _), str = MyMiniF(data,"ItemDeclarations") ?? "0", selectlist = DeclarationList },
                    new MyCheckboxString(){ name = "ItemPurchasable", check = data.TryGetValue("ItemPurchasable", out _), str = MyMiniF(data,"ItemPurchasable") ?? "0" },
                    new MyCheckboxString(){ name = "ItemKillable", check = data.TryGetValue("ItemKillable", out _), str = MyMiniF(data,"ItemKillable") ?? "0" },
                    new MyCheckboxString(){ name = "ItemSellable", check = data.TryGetValue("ItemSellable", out _), str = MyMiniF(data,"ItemSellable") ?? "0" },
                    new MyCheckboxString(){ name = "ItemDroppable", check = data.TryGetValue("ItemDroppable", out _), str = MyMiniF(data,"ItemDroppable") ?? "0" },
                    new MyCheckboxString(){ name = "ItemStackable", check = data.TryGetValue("ItemStackable", out _), str = MyMiniF(data,"ItemStackable") ?? "0" },
                    new MyCheckboxString(){ name = "ItemPermanent", check = data.TryGetValue("ItemPermanent", out _), str = MyMiniF(data,"ItemPermanent") ?? "0" },
                    new MyCheckboxStringSelect(){ name = "ItemShareability", check = data.TryGetValue("ItemShareability", out _), str = MyMiniF(data,"ItemShareability") ?? "", selectlist = ShareabilityList },
                    new MyCheckboxStringSelect(){ name = "ItemQuality", check = data.TryGetValue("ItemQuality", out _), str = MyMiniF(data,"ItemQuality") ?? "", selectlist = QualityList },
                    new MyCheckboxStringSelect(){ name = "ItemDisassembleRule", check = data.TryGetValue("ItemDisassembleRule", out _), str = MyMiniF(data,"ItemDisassembleRule") ?? "", selectlist = DisassembleRuleList },
                    new MyCheckboxString(){ name = "ItemShopTags", check = data.TryGetValue("ItemShopTags", out _), str = MyMiniF(data,"ItemShopTags") ?? "" },
                    new MyCheckboxString(){ name = "ItemAliases", check = data.TryGetValue("ItemAliases", out _), str = MyMiniF(data,"ItemAliases") ?? "" },
                    new MyCheckboxString(){ name = "InvalidHeroes", check = data.TryGetValue("InvalidHeroes", out _), str = MyMiniF(data,"InvalidHeroes") ?? "" },
                    new MyCheckboxString(){ name = "ItemCost", check = data.TryGetValue("ItemCost", out _), str = MyMiniF(data,"ItemCost") ?? "0" },
                    new MyCheckboxString(){ name = "ItemStockMax", check = data.TryGetValue("ItemStockMax", out _), str = MyMiniF(data,"ItemStockMax") ?? "0" },
                    new MyCheckboxString(){ name = "ItemStockTime", check = data.TryGetValue("ItemStockTime", out _), str = MyMiniF(data,"ItemStockTime") ?? "0" },
                    new MyCheckboxString(){ name = "ItemStockInitial", check = data.TryGetValue("ItemStockInitial", out _), str = MyMiniF(data,"ItemStockInitial") ?? "0" },
                    new MyCheckboxString(){ name = "ItemInitialCharges", check = data.TryGetValue("ItemInitialCharges", out _), str = MyMiniF(data,"ItemInitialCharges") ?? "0" },
                    new MyCheckboxString(){ name = "ItemDisplayCharges", check = data.TryGetValue("ItemDisplayCharges", out _), str = MyMiniF(data,"ItemDisplayCharges") ?? "0" },
                    new MyCheckboxString(){ name = "ItemRequiresCharges", check = data.TryGetValue("ItemRequiresCharges", out _), str = MyMiniF(data,"ItemRequiresCharges") ?? "0" },
                    new MyCheckboxString(){ name = "ItemAlertable", check = data.TryGetValue("ItemAlertable", out _), str = MyMiniF(data,"ItemAlertable") ?? "0" },
                    new MyCheckboxString(){ name = "ItemCastOnPickup", check = data.TryGetValue("ItemCastOnPickup", out _), str = MyMiniF(data,"ItemCastOnPickup") ?? "0" },
                    new MyCheckboxString(){ name = "SideShop", check = data.TryGetValue("SideShop", out _), str = MyMiniF(data,"SideShop") ?? "0" },
                    new MyCheckboxString(){ name = "SecretShop", check = data.TryGetValue("SecretShop", out _), str = MyMiniF(data,"SecretShop") ?? "0" },
                    new MyCheckboxString(){ name = "UIPickupSound", check = data.TryGetValue("UIPickupSound", out _), str = MyMiniF(data,"UIPickupSound") ?? "" },
                    new MyCheckboxString(){ name = "UIDropSound", check = data.TryGetValue("UIDropSound", out _), str = MyMiniF(data,"UIDropSound") ?? "" },
                    new MyCheckboxString(){ name = "WorldDropSound", check = data.TryGetValue("WorldDropSound", out _), str = MyMiniF(data,"WorldDropSound") ?? "" },
                    new MyCheckboxString(){ name = "PingOverrideText", check = data.TryGetValue("PingOverrideText", out _), str = MyMiniF(data,"PingOverrideText") ?? "" },
                };
            data = DeleteItems(data, new string[] {
                "BaseClass",
                "ID",
                "ItemRecipe",
                "ItemResult",
                "AbilityName",
                "AbilityBehavior",
                "AbilityType",
                "AbilityUnitTargetType",
                "AbilityUnitTargetTeam",
                "AbilityUnitDamageType",
                "SpellImmunityType",
                "CastFilterRejectCaster",
                "Model",
                "Effect",
                "AbilityTextureName",
                "MaxUpgradeLevel",
                "ItemBaseLevel",
                "AbilityCastAnimation",
                "AbilityCastRange",
                "AbilityCastPoint",
                "AbilityCooldown",
                "AbilityManaCost",
                "AbilityDamage",
                "AOERadius",
                "ItemDeclarations",
                "ItemPurchasable",
                "ItemKillable",
                "ItemSellable",
                "ItemDroppable",
                "ItemStackable",
                "ItemPermanent",
                "ItemShareability",
                "ItemQuality",
                "ItemDisassembleRule",
                "ItemShopTags",
                "ItemAliases",
                "InvalidHeroes",
                "ItemCost",
                "ItemStockMax",
                "ItemStockTime",
                "ItemStockInitial",
                "ItemInitialCharges",
                "ItemDisplayCharges",
                "ItemRequiresCharges",
                "ItemAlertable",
                "ItemCastOnPickup",
                "SideShop",
                "SecretShop",
                "UIPickupSound",
                "UIDropSound",
                "WorldDropSound",
                "PingOverrideText"
            });
            Array.Resize(ref tagobj, tagobj.Length + data.Count + 1);
            if (data.Count > 0)
            {
                for (int i = 0; data.Count > i; i++)
                {
                    tagobj[tagobj.Length + i - (data.Count + 1)] = new MyCheckboxString() { name = data.ElementAt(i).Key, check = true, str = data.ElementAt(i).Value };
                }
            }
            tagobj[tagobj.Length - 1] = new MyAddNodes() { items = ThisEventList };
            newnode.Tag = tagobj;
            return newnode.Name;
        }

    }
}
