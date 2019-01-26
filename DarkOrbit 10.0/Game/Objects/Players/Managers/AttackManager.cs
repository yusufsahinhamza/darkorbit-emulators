using Ow.Game.Objects;
using Ow.Game.Objects.Players.Techs;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Managers
{
    class AttackManager
    {
        public Player Player { get; set; }
        public RocketLauncher RocketLauncher { get; set; }
        public bool Attacking = false;
        public static int ATTACK_RANGE = 700;

        public AttackManager(Player player) { Player = player; RocketLauncher = new RocketLauncher(Player); }

        public DateTime lastAttackTime = new DateTime();
        public DateTime lastRSBAttackTime = new DateTime();

        public void LaserAttack()
        {
            if (Attacking)
            {
                var enemy = Player.SelectedCharacter;
                if (enemy == null) return;
                if (!TargetDefinition(enemy)) return;

                if (CheckLaserAttackTime())
                {
                    if (Player.Damage == 0)
                    {
                        Player.SendPacket("0|A|STM|no_lasers_on_board");
                        Player.DisableAttack(Player.SettingsManager.SelectedLaser);
                        return;
                    }

                    var damage = RandomizeDamage(GetDamageMultiplier() * Player.Damage);

                    if (Player.Spectrum)
                        damage -= Maths.GetPercentage(damage, 50);

                    if (enemy is Player)
                    {
                        if ((enemy as Player).Spectrum)
                            damage -= Maths.GetPercentage(damage, 80);
                    }

                    Damage(Player, enemy, DamageType.LASER, damage, Player.ShieldPenetration);

                    if (Player.AutoRocket)
                        RocketAttack();

                    if (Player.AutoRocketLauncher)
                        if (RocketLauncher.CurrentLoad != RocketLauncher.MaxLoad)
                            RocketLauncher.Reload();
                        else
                            LaunchRocketLauncher();

                    UpdateAttacker(enemy, Player);

                    if (Player.SettingsManager.SelectedLaser == AmmunitionManager.RSB_75)
                    {
                        lastRSBAttackTime = DateTime.Now;
                        Player.SendCooldown(AmmunitionManager.RSB_75, 3000);
                    }
                    else lastAttackTime = DateTime.Now;
                }
            }
            RefreshAttackers();
        }

        public DateTime lastRocketAttack = new DateTime();
        public void RocketAttack()
        {
            var enemy = Player.SelectedCharacter;
            if (enemy == null) return;
            if (!TargetDefinition(enemy, true, true)) return;

            switch (GetSelectedRocket())
            {
                case 6:
                    WIZ_X();
                    break;
                case 10:
                    DCR_250();
                    break;
                case 18:
                    R_IC3();
                    break;
                default:
                    if (lastRocketAttack.AddSeconds(Player.RocketSpeed) < DateTime.Now)
                    {
                        var damage = RandomizeDamage(Player.RocketDamage, Player.PrecisionTargeter ? 0 : 1);
                        Damage(Player, enemy, DamageType.ROCKET, damage, 0);

                        String rocketRunPacket = "0|v|" + Player.Id + "|" + enemy.Id + "|H|" + GetSelectedRocket() + "|1|" + (Player.PrecisionTargeter ? 1 : 0);
                        Player.SendPacket(rocketRunPacket);
                        Player.SendPacketToInRangePlayers(rocketRunPacket);

                        Player.SendCooldown(AmmunitionManager.R_310, Player.Premium ? 1000 : 3000);
                        lastRocketAttack = DateTime.Now;
                    }
                    break;
            }
        }

        public void LaunchRocketLauncher()
        {
            var enemy = Player.SelectedCharacter;
            if (enemy == null) return;
            if (!TargetDefinition(enemy, false)) return;

            Player.SendPacket("0|RL|A|" + Player.Id + "|" + enemy.Id + "|" + RocketLauncher.CurrentLoad + "|" + GetSelectedLauncherId());
            Player.SendPacketToInRangePlayers("0|RL|A|" + Player.Id + "|" + enemy.Id + "|" + RocketLauncher.CurrentLoad + "|" + GetSelectedLauncherId());

            if (GetSelectedLauncherId() == 12 || GetSelectedLauncherId() == 13)
                Absorbation(Player, enemy, GetSelectedLauncherId() == 12 ? DamageType.SHIELD_ABSORBER_ROCKET_CREDITS : GetSelectedLauncherId() == 13 ? DamageType.SHIELD_ABSORBER_ROCKET_URIDIUM : DamageType.ROCKET, RandomizeDamage(GetRocketLauncherDamage()));
            else
                Damage(Player, enemy, DamageType.ROCKET, RandomizeDamage(GetRocketLauncherDamage()), 0);

            RocketLauncher.CurrentLoad = 0;
            RocketLauncher.LastReloadTime = DateTime.Now;
            RocketLauncher.ReloadingActive = Player.AutoRocketLauncher;
            Player.SettingsManager.SendNewItemStatus(CpuManager.ROCKET_LAUNCHER);

            UpdateAttacker(enemy, Player);
        }

        public void UpdateAttacker(Character target, Player player)
        {
            if (target.MainAttacker == null)
            {
                target.MainAttacker = player;
            }
            if (!target.Attackers.ContainsKey(Player.Id))
            {
                target.Attackers.TryAdd(Player.Id, new Attacker(player));
            }
            else
            {
                target.Attackers[player.Id].Refresh();
            }
        }

        public void RefreshAttackers()
        {
            foreach (var attacker in Player.Attackers)
            {
                if (attacker.Value?.Player != null && attacker.Value.LastRefresh.AddSeconds(10) > DateTime.Now)
                {
                    if (attacker.Value.FadedToGray && Player.MainAttacker == attacker.Value.Player)
                    {
                        attacker.Value.Player.SendPacket($"0|n|USH|{Player.Id}");
                        attacker.Value.FadedToGray = false;
                    }
                    if (!attacker.Value.FadedToGray && Player.MainAttacker != attacker.Value.Player)
                    {
                        attacker.Value.Player.SendPacket($"0|n|LSH|{Player.Id}|{Player.Id}");
                        attacker.Value.FadedToGray = true;
                    }
                    continue;
                }
                Attacker removedAttacker;
                Player.Attackers.TryRemove(attacker.Key, out removedAttacker);
            }
            if (Player.MainAttacker != null)
            {
                if (!Player.Attackers.ContainsKey(Player.MainAttacker.Id))
                {
                    Player.MainAttacker = null;
                }
            }
        }

        public int GetSelectedLauncherId()
        {
            switch (Player.SettingsManager.SelectedRocketLauncher)
            {
                case AmmunitionManager.HSTRM_01:
                    return 10;
                case AmmunitionManager.SAR_02:
                    return 13;
                default:
                    return 9;
            }
        }

        public DateTime EmpCooldown = new DateTime();
        public void EMP()
        {
            if (EmpCooldown.AddMilliseconds(TimeManager.EMP_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                Player.SendCooldown(AmmunitionManager.EMP_01, TimeManager.EMP_COOLDOWN);
                EmpCooldown = DateTime.Now;

                Player.DeactiveR_RIC3();
                Player.DeactiveDCR_250();
                Player.DeactiveSLM_01();

                string empPacket = "0|n|EMP|" + Player.Id;
                Player.SendPacket(empPacket);
                Player.SendPacketToInRangePlayers(empPacket);

                foreach (var otherPlayers in Player.InRangeCharacters.Values)
                {
                    if (otherPlayers == null || !(otherPlayers is Player)) return;
                    if (otherPlayers.Position.DistanceTo(Player.Position) > 700) return;

                    var otherPlayer = otherPlayers as Player;
                    if (otherPlayer.Selected == Player)
                    {
                        string empMessagePacket = "0|A|STM|msg_own_targeting_harmed";
                        otherPlayer.SendPacket(empMessagePacket);
                        otherPlayer.Selected = null;
                        otherPlayer.DisableAttack(otherPlayer.SettingsManager.SelectedLaser);
                        otherPlayer.SendCommand(ShipDeselectionCommand.write());

                        if (Player.RankId == 21)
                        {
                            otherPlayer.AddVisualModifier(new VisualModifierCommand(otherPlayer.Id, VisualModifierCommand.MIRRORED_CONTROLS, 0, "", 0, true));
                        }
                    }

                    if (otherPlayer.Invisible)
                    {
                        otherPlayer.SendPacket("0|A|STM|msg_uncloaked");
                        otherPlayer.CpuManager.DisableCloak();
                    }
                }
            }
        }

        public DateTime r_ic3Cooldown = new DateTime();
        public void R_IC3()
        {
            var enemy = Player.SelectedCharacter;
            if (!TargetDefinition(enemy)) return;

            if (r_ic3Cooldown.AddMilliseconds(TimeManager.R_IC3_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                String rocketRunPacket = "0|v|" + Player.Id + "|" + enemy.Id + "|H|" + GetSelectedRocket() + "|1|" + (Player.PrecisionTargeter ? 1 : 0);
                Player.SendPacket(rocketRunPacket);
                Player.SendPacketToInRangePlayers(rocketRunPacket);

                Player.SendCooldown(AmmunitionManager.R_IC3, TimeManager.R_IC3_COOLDOWN);
                r_ic3Cooldown = DateTime.Now;

                if (!(enemy is Player)) return;

                var enemyPlayer = enemy as Player;
                if (!enemyPlayer.Attackable()) return;

                enemyPlayer.underR_IC3 = true;
                enemyPlayer.underR_IC3Time = DateTime.Now;

                enemyPlayer.SendPacket("0|n|fx|start|ICY_CUBE|" + enemyPlayer.Id + "");
                enemyPlayer.SendPacketToInRangePlayers("0|n|fx|start|ICY_CUBE|" + enemyPlayer.Id + "");
                enemyPlayer.SendCommand(SetSpeedCommand.write(enemyPlayer.Speed, enemyPlayer.Speed));
            }
        }

        public DateTime wiz_xCooldown = new DateTime();
        public void WIZ_X()
        {
            var enemy = Player.SelectedCharacter;
            if (enemy == null) return;

            if (wiz_xCooldown.AddMilliseconds(TimeManager.WIZARD_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                String rocketRunPacket = "0|v|" + Player.Id + "|" + enemy.Id + "|H|" + GetSelectedRocket() + "|1|" + (Player.PrecisionTargeter ? 1 : 0);
                Player.SendPacket(rocketRunPacket);
                Player.SendPacketToInRangePlayers(rocketRunPacket);

                Player.SendCooldown(AmmunitionManager.WIZ_X, TimeManager.WIZARD_COOLDOWN);
                wiz_xCooldown = DateTime.Now;

                if (!(enemy is Player)) return;

                var enemyPlayer = enemy as Player;

                var ship = Ship.GetRandomShipLootId(enemyPlayer.Ship.LootId);
                enemyPlayer.AddVisualModifier(new VisualModifierCommand(enemyPlayer.Id, VisualModifierCommand.WIZARD_ATTACK, 0, ship, 0, true));
            }
        }

        public DateTime dcr_250Cooldown = new DateTime();
        public void DCR_250()
        {
            var enemy = Player.SelectedCharacter;
            if (enemy == null) return;
            if (!TargetDefinition(enemy)) return;

            if (dcr_250Cooldown.AddMilliseconds(TimeManager.DCR_250_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                String rocketRunPacket = "0|v|" + Player.Id + "|" + enemy.Id + "|H|" + GetSelectedRocket() + "|1|" + (Player.PrecisionTargeter ? 1 : 0);
                Player.SendPacket(rocketRunPacket);
                Player.SendPacketToInRangePlayers(rocketRunPacket);

                Player.SendCooldown(AmmunitionManager.DCR_250, TimeManager.DCR_250_COOLDOWN);
                dcr_250Cooldown = DateTime.Now;

                if (!(enemy is Player)) return;

                var enemyPlayer = enemy as Player;
                if (!enemyPlayer.Attackable()) return;

                enemyPlayer.underDCR_250 = true;
                enemyPlayer.underDCR_250Time = DateTime.Now;

                enemyPlayer.SendPacket("0|n|fx|start|SABOTEUR_DEBUFF|" + enemyPlayer.Id + "");
                enemyPlayer.SendPacketToInRangePlayers("0|n|fx|start|SABOTEUR_DEBUFF|" + enemyPlayer.Id + "");
                enemyPlayer.SendCommand(SetSpeedCommand.write(enemyPlayer.Speed, enemyPlayer.Speed));
            }
        }

        public DateTime SmbCooldown = new DateTime();
        public void SMB()
        {
            if (Player.IsInDemilitarizedZone) return;

            if (SmbCooldown.AddMilliseconds(TimeManager.SMB_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                Player.SendCooldown(AmmunitionManager.SMB_01, TimeManager.SMB_COOLDOWN);
                SmbCooldown = DateTime.Now;

                var smbPacket = "0|n|SMB|" + Player.Id;
                Player.SendPacket(smbPacket);
                Player.SendPacketToInRangePlayers(smbPacket);

                foreach (var otherPlayer in Player.InRangeCharacters.Values)
                {
                    if (otherPlayer == null || !(otherPlayer is Player)) return;
                    if (!TargetDefinition(otherPlayer, false)) return;
                    if (otherPlayer.Position.DistanceTo(Player.Position) > 700) return;

                    if (!(otherPlayer as Player).Attackable())
                    {
                        AttackMissed(otherPlayer, DamageType.MINE);
                        return;
                    }

                    int damage = Maths.GetPercentage(otherPlayer.CurrentHitPoints, 20);

                    Damage(Player, otherPlayer as Player, DamageType.MINE, damage, false, true, false);
                }
            }
        }

        public DateTime IshCooldown = new DateTime();
        public void ISH()
        {
            if (IshCooldown.AddMilliseconds(TimeManager.ISH_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                IshCooldown = DateTime.Now;
                Player.SendCooldown(AmmunitionManager.ISH_01, TimeManager.ISH_COOLDOWN);

                var ishPacket = "0|n|ISH|" + Player.Id;
                Player.SendPacket(ishPacket);
                Player.SendPacketToInRangePlayers(ishPacket);
            }
        }

        public void ECI()
        {
            var damage = RandomizeDamage(ChainImpulse.DAMAGE, 0);

            var targets = new Dictionary<int, Character>();

            foreach (var entry in Player.InRangeCharacters.Values)
            {
                if (entry != null)
                    if(entry is Player)
                        if (TargetDefinition(entry, false))
                            if (entry.Position.DistanceTo(Player.Position) <= 1000)
                                if (targets.Count < 7)
                                    targets.Add(entry.Id, entry);
            }

            foreach (var target in targets.Values)
            {
                if (target == null) return;

                string eciPacket = "0|TX|ECI||" + Player.Id;
                eciPacket += "|" + target.Id;

                Player.SendPacket(eciPacket);
                Player.SendPacketToInRangePlayers(eciPacket);

                Damage(Player, target, DamageType.ECI, damage, true, true, false);
            }

            foreach (var target in targets.Values)
            {
                if (damage >= target.CurrentHitPoints || target.CurrentHitPoints == 0)
                    target.Destroy(Player, DestructionType.PLAYER);
            }
        }

        public DateTime outOfRangeCooldown = new DateTime();
        public DateTime inAttackCooldown = new DateTime();
        public DateTime peaceAreaCooldown = new DateTime();
        public bool TargetDefinition(Character target, bool sendWarningMessage = true, bool isRocketAttack = false)
        {
            if (target == null) return false;

            short relationType = target.Clan != null && Player.Clan != null ? Player.Clan.GetRelation(target.Clan) : (short)0;
            if (target.FactionId == Player.FactionId && relationType != ClanRelationModule.AT_WAR && (EventManager.JackpotBattle.Active && Player.Spacemap.Id != EventManager.JackpotBattle.Spacemap.Id))
            {
                if (sendWarningMessage)
                {
                    Player.DisableAttack(Player.SettingsManager.SelectedLaser);
                    Player.SendPacket("0|A|STD|You can't attack members of your own company!");
                }
                return false;
            }

            if (target is Player && (target as Player).IsInDemilitarizedZone)
            {
                Player.DisableAttack(Player.SettingsManager.SelectedLaser);
                if (peaceAreaCooldown.AddSeconds(10) < DateTime.Now)
                {
                    if (sendWarningMessage)
                    {
                        Player.SendPacket("0|A|STM|peacearea");

                        if (target is Player)
                            (target as Player).SendPacket("0|A|STM|peacearea");

                        peaceAreaCooldown = DateTime.Now;
                    }
                }
                return false;
            }

            if (Player.Position.DistanceTo(target.Position) > (isRocketAttack ? GetRocketRange() : ATTACK_RANGE))
            {
                if (outOfRangeCooldown.AddSeconds(5) < DateTime.Now)
                {
                    if (sendWarningMessage)
                    {
                        Player.SendPacket("0|A|STM|outofrange");

                        if (target is Player)
                            (target as Player).SendPacket("0|A|STM|attescape");

                        outOfRangeCooldown = DateTime.Now;
                    }
                }
                return false;
            }

            if (inAttackCooldown.AddSeconds(10) < DateTime.Now)
            {
                if (sendWarningMessage)
                {
                    Player.SendPacket("0|A|STM|oppoatt|%!|" + (EventManager.JackpotBattle.Active && Player.Spacemap.Id == EventManager.JackpotBattle.Spacemap.Id ? EventManager.JackpotBattle.Name : target.Name));
                    inAttackCooldown = DateTime.Now;
                }
            }
            return true;
        }

        private int GetRocketRange()
        {
            switch (Player.SettingsManager.SelectedRocket)
            {
                case AmmunitionManager.R_310:
                    return 400;
                case AmmunitionManager.PLT_2026:
                    return 600;
                case AmmunitionManager.PLT_2021:
                    return 800;
                case AmmunitionManager.PLT_3030:
                    return 800;
                case AmmunitionManager.DCR_250:
                    return 600;
                case AmmunitionManager.R_IC3:
                    return 600;
                case AmmunitionManager.WIZ_X:
                    return 800;
                default:
                    return 0;
            }
        }

        public int RandomizeDamage(int baseDmg, double missProbability = 1.00)
        {
            var random = new Random();

            var randNums = random.Next(0, 6);

            if (missProbability == 0)
                randNums = random.Next(0, 3) | random.Next(4, 7);
            if (missProbability < 1.00 && missProbability != 0)
                randNums = random.Next(0, 7);
            if (missProbability > 1.00 && missProbability < 2.00)
                randNums = random.Next(0, 4);
            if (missProbability >= 2.00)
                randNums = random.Next(2, 4);

            switch (randNums)
            {
                case 0:
                    return (int)(baseDmg * 1.10);
                case 1:
                    return (int)(baseDmg * 0.98);
                case 2:
                    return (int)(baseDmg * 1.02);
                case 3:
                    return 0;
                case 4:
                    return (int)(baseDmg * 0.92);
                case 5:
                    return (int)(baseDmg * 0.99);
                default:
                    return baseDmg;
            }
        }

        public void Absorbation(Player attacker, Character target, DamageType damageType, int damage)
        {
            if (attacker.invincibilityEffect)
                attacker.DeactiveInvincibilityEffect();

            if (Player.Invisible)
            {
                Player.SendPacket("0|A|STM|msg_uncloaked");
                Player.CpuManager.DisableCloak();
            }

            if (target is Player && !(target as Player).Attackable())
                damage = 0;

            if ((target.CurrentShieldPoints - damage) < 0)
                damage = target.CurrentShieldPoints;

            target.CurrentShieldPoints -= damage;
            Player.CurrentShieldPoints += damage;
            target.LastCombatTime = DateTime.Now;

            if (damageType == DamageType.LASER)
            {
                var laserRunCommand = AttackLaserRunCommand.write(Player.Id, target.Id, GetSelectedLaser(), false, true);
                Player.SendCommand(laserRunCommand);
                Player.SendCommandToInRangePlayers(laserRunCommand);
            }

            if (damage == 0 && target.CurrentShieldPoints != 0)
            {
                if (damageType == DamageType.LASER)
                    AttackMissed(target, damageType);
            }
            else
            {
                var attackHitCommand =
                AttackHitCommand.write(new AttackTypeModule((short)damageType), Player.Id,
                     target.Id, target.CurrentHitPoints,
                     target.CurrentShieldPoints, target.CurrentNanoHull,
                     damage, false);

                Player.SendCommand(attackHitCommand);
                Player.SendCommandToInRangePlayers(attackHitCommand);
            }

            target.UpdateStatus();
            Player.UpdateStatus();
        }

        public void Damage(Player attacker, Character target, DamageType damageType, int damage, double shieldPenetration, bool deactiveCloak = true)
        {
            if (damageType == DamageType.MINE && target is Player && (target as Player).invincibilityEffect) return;

            if (damageType == DamageType.LASER && Player.SettingsManager.SelectedLaser == AmmunitionManager.SAB_50)
            {
                Absorbation(attacker, target, damageType, damage);
                return;
            }

            int damageShd = 0, damageHp = 0;

            if (attacker.invincibilityEffect)
                attacker.DeactiveInvincibilityEffect();

            if (target is Spaceball)
            {
                var spaceball = target as Spaceball;
                spaceball.AddDamage(attacker, damage);
            }

            double shieldAbsorb = System.Math.Abs(target.ShieldAbsorption - shieldPenetration);

            if (shieldAbsorb > 1)
                shieldAbsorb = 1;

            if ((target.CurrentShieldPoints - damage) >= 0)
            {
                damageShd = (int)(damage * shieldAbsorb);
                damageHp = damage - damageShd;
            }
            else
            {
                int newDamage = damage - target.CurrentShieldPoints;
                damageShd = target.CurrentShieldPoints;
                damageHp = (int)(newDamage + (damageShd * shieldAbsorb));
            }

            if ((target.CurrentHitPoints - damageHp) < 0)
            {
                damageHp = target.CurrentHitPoints;
            }

            if (target is Player && !(target as Player).Attackable())
            {
                damage = 0;
                damageShd = 0;
                damageHp = 0;
            }

            if (deactiveCloak)
            {
                if (Player.Invisible)
                    Player.SendPacket("0|A|STM|msg_uncloaked");
                Player.CpuManager.DisableCloak();
            }

            if (damageType == DamageType.LASER)
            {
                if (target is Player && (target as Player).Sentinel)
                    damageShd -= Maths.GetPercentage(damageShd, 30);

                if (Player.Diminisher)
                    if (target == Player.UnderDiminisherPlayer)
                        damageShd += Maths.GetPercentage(damage, 50);

                if (target is Player && (target as Player).Diminisher)
                    if ((target as Player).UnderDiminisherPlayer == Player)
                        damageShd += Maths.GetPercentage(damage, 30);
            }

            target.CurrentHitPoints -= damageHp;
            target.CurrentShieldPoints -= damageShd;
            target.LastCombatTime = DateTime.Now;
      
            if (damageType == DamageType.LASER)
            {
                var laserRunCommand = AttackLaserRunCommand.write(Player.Id, target.Id, GetSelectedLaser(), false, true);
                Player.SendCommand(laserRunCommand);
                Player.SendCommandToInRangePlayers(laserRunCommand);
            }

            if (damage == 0)
            {
                if (damageType == DamageType.LASER)
                    AttackMissed(target, damageType);
            }
            else
            {
                var attackHitCommand =
                        AttackHitCommand.write(new AttackTypeModule((short)damageType), Player.Id,
                                             target.Id, target.CurrentHitPoints,
                                             target.CurrentShieldPoints, target.CurrentNanoHull,
                                             damage > damageShd ? damage : damageShd, false);

                Player.SendCommand(attackHitCommand);
                Player.SendCommandToInRangePlayers(attackHitCommand);
            }

            if (damageType == DamageType.LASER)
            {
                if (Player.SettingsManager.SelectedLaser != AmmunitionManager.SAB_50)
                {
                    if (Player.EnergyLeech)
                        Player.TechManager.EnergyLeech.ExecuteHeal(damage);
                }
            }

            if (damageHp >= target.CurrentHitPoints || target.CurrentHitPoints == 0)
                target.Destroy(Player, DestructionType.PLAYER);

            target.UpdateStatus();
        }

        public static void Damage(Player attacker, Character target, DamageType damageType, int damage, bool toDestroy, bool toHp, bool toShd, bool missedEffect = true)
        {
            if (damageType == DamageType.MINE && target is Player && (target as Player).invincibilityEffect) return;

            if (attacker.invincibilityEffect)
                attacker.DeactiveInvincibilityEffect();

            if (target is Player && !(target as Player).Attackable())
            {
                if (missedEffect)
                {
                    var attackMissedCommandToInRange = AttackMissedCommand.write(new AttackTypeModule((short)damageType), target.Id, 0);
                    var attackMissedCommand = AttackMissedCommand.write(new AttackTypeModule((short)damageType), target.Id, 0);
                    attacker.SendCommand(attackMissedCommand);
                    attacker.SendCommandToInRangePlayers(attackMissedCommandToInRange);
                }
                damage = 0;
                return;
            }

            target.LastCombatTime = DateTime.Now;

            if (toHp)
                target.CurrentHitPoints -= damage;

            if (toShd)
                target.CurrentShieldPoints -= damage;

            var attackHitCommand =
                    AttackHitCommand.write(new AttackTypeModule((short)damageType), attacker.Id,
                                         target.Id, target.CurrentHitPoints,
                                         target.CurrentShieldPoints, target.CurrentNanoHull,
                                         damage, false);

            attacker.SendCommand(attackHitCommand);
            attacker.SendCommandToInRangePlayers(attackHitCommand);

            if (toDestroy)
            {
                if (damage >= target.CurrentHitPoints || target.CurrentHitPoints == 0)
                {
                    if (damageType == DamageType.RADIATION)
                        target.Destroy(null, DestructionType.RADITATION);
                    else if (damageType == DamageType.MINE && attacker.Attackers.Count <= 0)
                        target.Destroy(null, DestructionType.MINE);
                    else
                        target.Destroy(attacker, DestructionType.PLAYER);
                }
            }

            target.UpdateStatus();
        }

        public void AttackMissed(Character target, DamageType damageType)
        {
            var attackMissedCommand = AttackMissedCommand.write(new AttackTypeModule((short)damageType), target.Id, 0);
            var attackMissedCommandToInRange = AttackMissedCommand.write(new AttackTypeModule((short)damageType), target.Id, 1);
            Player.SendCommand(attackMissedCommand);
            Player.SendCommandToInRangePlayers(attackMissedCommandToInRange);
        }

        private bool CheckLaserAttackTime()
        {
            if (Player.SettingsManager.SelectedLaser == AmmunitionManager.RSB_75)
            {
                return lastRSBAttackTime.AddSeconds(3) < DateTime.Now;
            }
            return lastAttackTime.AddSeconds(1) < DateTime.Now;
        }

        private int GetSelectedRocket()
        {
            switch (Player.SettingsManager.SelectedRocket)
            {
                case AmmunitionManager.R_310:
                    return 1;
                case AmmunitionManager.PLT_2026:
                    return 2;
                case AmmunitionManager.PLT_2021:
                    return 3;
                case AmmunitionManager.PLT_3030:
                    return 4;
                case AmmunitionManager.DCR_250:
                    return 10;
                case AmmunitionManager.R_IC3:
                    return 18;
                case AmmunitionManager.WIZ_X:
                    return 6;
                default:
                    return 0;
            }
        }

        public int GetRocketDamage()
        {
            switch (Player.SettingsManager.SelectedRocket)
            {
                case AmmunitionManager.R_310:
                    return 1000;
                case AmmunitionManager.PLT_2026:
                    return 2000;
                case AmmunitionManager.PLT_2021:
                    return 4000;
                case AmmunitionManager.PLT_3030:
                    return 6000;
                default:
                    return 0;
            }
        }

        private int GetRocketLauncherDamage()
        {
            switch (Player.SettingsManager.SelectedRocketLauncher)
            {
                case AmmunitionManager.HSTRM_01:
                    return 4000 * RocketLauncher.CurrentLoad;
                case AmmunitionManager.SAR_02:
                    return 4000 * RocketLauncher.CurrentLoad;
                default:
                    return 0;
            }
        }

        private int GetDamageMultiplier()
        {

            switch (Player.SettingsManager.SelectedLaser)
            {
                case AmmunitionManager.LCB_10:
                    return 1;
                case AmmunitionManager.MCB_25:
                    return 2;
                case AmmunitionManager.MCB_50:
                    return 3;
                case AmmunitionManager.UCB_100:
                    return 4;
                case AmmunitionManager.RSB_75:
                    return 5;
                case AmmunitionManager.SAB_50:
                    return 2;
                default:
                    return 1;
            }
        }

        public int GetSelectedLaser()
        {
            switch (Player.SettingsManager.SelectedLaser)
            {
                case AmmunitionManager.LCB_10:
                    return 0;
                case AmmunitionManager.MCB_25:
                    return 1;
                case AmmunitionManager.MCB_50:
                    return 2;
                case AmmunitionManager.UCB_100:
                    return 3;
                case AmmunitionManager.RSB_75:
                    return 6;
                case AmmunitionManager.SAB_50:
                    return 4;
                case AmmunitionManager.JOB_100:
                    return 9;
                case AmmunitionManager.RB_214:
                    return 10;
                case AmmunitionManager.CBO_100:
                    return 8;
                default:
                    return 0;
            }
        }
    }
}
