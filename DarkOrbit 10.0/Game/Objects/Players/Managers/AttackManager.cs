using Ow.Game.Objects;
using Ow.Game.Objects.Players.Techs;
using Ow.Game.Objects.Stations;
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
    class AttackManager : AbstractManager
    {
        public RocketLauncher RocketLauncher { get; set; }
        public bool Attacking = false;

        public AttackManager(Player player) : base(player) { RocketLauncher = new RocketLauncher(Player); }

        public DateTime lastAttackTime = new DateTime();
        public DateTime lastRSBAttackTime = new DateTime();
        public DateTime mineCooldown = new DateTime();

        public void LaserAttack()
        {
            if (Attacking)
            {
                var target = Player.Selected;

                if (target == null) return;
                if (!Player.TargetDefinition(target)) return;

                if (CheckLaserAttackTime())
                {
                    if (Player.Damage == 0)
                    {
                        Player.SendPacket("0|A|STM|no_lasers_on_board");
                        Player.DisableAttack(Player.Settings.InGameSettings.selectedLaser);
                        return;
                    }

                    var damage = RandomizeDamage((GetDamageMultiplier() * Player.Damage), (Player.Storage.underPLD8 ? 0.5 : 0.1));

                    if (Player.Storage.Spectrum)
                        damage -= Maths.GetPercentage(damage, 50);

                    if (target is Player)
                    {
                        if ((target as Player).Storage.Spectrum)
                            damage -= Maths.GetPercentage(damage, 80);
                    }

                    Damage(Player, target, DamageType.LASER, damage, Player.ShieldPenetration);

                    if (Player.Storage.AutoRocket)
                        RocketAttack();

                    if (Player.Storage.AutoRocketLauncher)
                        if (RocketLauncher.CurrentLoad != RocketLauncher.MaxLoad)
                            RocketLauncher.Reload();
                        else
                            LaunchRocketLauncher();
                    RocketLauncher.Reload();

                    UpdateAttacker(target, Player);

                    if (Player.Settings.InGameSettings.selectedLaser == AmmunitionManager.RSB_75)
                    {
                        lastRSBAttackTime = DateTime.Now;
                        Player.SendCooldown(AmmunitionManager.RSB_75, 3000);
                    }
                    else lastAttackTime = DateTime.Now;
                }
            }
        }

        public DateTime lastRocketAttack = new DateTime();
        public DateTime r_ic3Cooldown = new DateTime();
        public DateTime pld8Cooldown = new DateTime();
        public DateTime wiz_xCooldown = new DateTime();
        public DateTime dcr_250Cooldown = new DateTime();

        public async void RocketAttack()
        {
            var enemy = Player.SelectedCharacter;
            if (enemy == null) return;

            if (Player.Settings.InGameSettings.selectedRocket != AmmunitionManager.WIZ_X)
                if (!Player.TargetDefinition(enemy, true, true)) return;

            switch (GetSelectedRocket())
            {
                case 5:
                    if (pld8Cooldown.AddMilliseconds(TimeManager.PLD8_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
                    {
                        Player.SendCooldown(AmmunitionManager.PLD_8, TimeManager.PLD8_COOLDOWN);
                        pld8Cooldown = DateTime.Now;
                    } else return;
                    break;
                case 6:
                    if (wiz_xCooldown.AddMilliseconds(TimeManager.WIZARD_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
                    {
                        Player.SendCooldown(AmmunitionManager.WIZ_X, TimeManager.WIZARD_COOLDOWN);
                        wiz_xCooldown = DateTime.Now;
                    } else return;
                    break;
                case 10:
                    if (dcr_250Cooldown.AddMilliseconds(TimeManager.DCR_250_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
                    {
                        Player.SendCooldown(AmmunitionManager.DCR_250, TimeManager.DCR_250_COOLDOWN);
                        dcr_250Cooldown = DateTime.Now;
                    } else return;
                    break;
                case 18:
                    if (r_ic3Cooldown.AddMilliseconds(TimeManager.R_IC3_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
                    {
                        Player.SendCooldown(AmmunitionManager.R_IC3, TimeManager.R_IC3_COOLDOWN);
                        r_ic3Cooldown = DateTime.Now;
                    } else return;
                    break;
                default:
                    if (lastRocketAttack.AddSeconds(Player.RocketSpeed) < DateTime.Now)
                    {
                        Player.SendCooldown(AmmunitionManager.R_310, Player.Premium ? 1000 : 3000);
                        lastRocketAttack = DateTime.Now;
                    } else return;
                    break;
            }

            Player.CpuManager.DisableCloak();

            var rocketRunPacket = $"0|v|{Player.Id}|{enemy.Id}|H|{GetSelectedRocket()}|{(Player.SkillTree.rocketFusion == 5 ? 1 : 0)}|{(Player.Storage.PrecisionTargeter || Player.SkillTree.heatseekingMissiles == 5 ? 1 : 0)}";
            Player.SendPacket(rocketRunPacket);
            Player.SendPacketToInRangePlayers(rocketRunPacket);

            await Task.Delay(1000);

            UpdateAttacker(enemy, Player);

            switch (GetSelectedRocket())
            {
                case 5:
                case 6:
                case 18:
                    if (Player.RocketMissProbability < Randoms.random.NextDouble() && (!(enemy is Player) || (enemy is Player && (enemy as Player).Attackable())))
                    {
                        switch (GetSelectedRocket())
                        {
                            case 5:
                                enemy.Storage.underPLD8 = true;
                                enemy.Storage.underPLD8Time = DateTime.Now;

                                if (enemy is Player)
                                    (enemy as Player).SendPacket("0|n|MAL|SET|" + enemy.Id + "");

                                enemy.SendPacketToInRangePlayers("0|n|MAL|SET|" + enemy.Id + "");
                                break;
                            case 6:
                                var shipId = Ship.GetRandomShipId(enemy.Ship.Id);
                                enemy.AddVisualModifier(VisualModifierCommand.WIZARD_ATTACK, 0, GameManager.GetShip(shipId).LootId, 0, true);
                                break;
                            case 18:
                                enemy.Storage.underR_IC3 = true;
                                enemy.Storage.underR_IC3Time = DateTime.Now;

                                if (enemy is Player)
                                {
                                    (enemy as Player).SendPacket("0|n|fx|start|ICY_CUBE|" + enemy.Id + "");
                                    (enemy as Player).SendCommand(SetSpeedCommand.write(enemy.Speed, enemy.Speed));
                                }

                                enemy.SendPacketToInRangePlayers("0|n|fx|start|ICY_CUBE|" + enemy.Id + "");
                                break;
                            case 10:
                                if (enemy is Player enemyPlayer)
                                {
                                    enemyPlayer.Storage.underDCR_250 = true;
                                    enemyPlayer.Storage.underDCR_250Time = DateTime.Now;

                                    enemyPlayer.SendPacket("0|n|fx|start|SABOTEUR_DEBUFF|" + enemyPlayer.Id + "");
                                    enemyPlayer.SendCommand(SetSpeedCommand.write(enemyPlayer.Speed, enemyPlayer.Speed));

                                    enemyPlayer.SendPacketToInRangePlayers("0|n|fx|start|SABOTEUR_DEBUFF|" + enemyPlayer.Id + "");
                                }
                                break;
                        }
                    } else AttackMissed(enemy, DamageType.ROCKET);
                    break;
                default:
                    var damage = RandomizeDamage(Player.RocketDamage, Player.RocketMissProbability);
                    Damage(Player, enemy, DamageType.ROCKET, damage, 0);
                    break;
            }
        }

        public async void LaunchRocketLauncher()
        {
            var enemy = Player.Selected;
            if (enemy == null) return;
            if (!Player.TargetDefinition(enemy, false)) return;

            Player.SendPacket("0|RL|A|" + Player.Id + "|" + enemy.Id + "|" + RocketLauncher.CurrentLoad + "|" + GetSelectedLauncherId());
            Player.SendPacketToInRangePlayers("0|RL|A|" + Player.Id + "|" + enemy.Id + "|" + RocketLauncher.CurrentLoad + "|" + GetSelectedLauncherId());

            Player.SettingsManager.SendNewItemStatus(CpuManager.ROCKET_LAUNCHER);
            RocketLauncher.LastReloadTime = DateTime.Now;

            var damage = 0;
            DamageType damageType = GetSelectedLauncherId() == (int)DamageType.SHIELD_ABSORBER_ROCKET_URIDIUM ? DamageType.SHIELD_ABSORBER_ROCKET_URIDIUM : DamageType.ROCKET;

            for (var i = 0; i < RocketLauncher.CurrentLoad; i++)
            {
                damage += RandomizeDamage(GetRocketLauncherRocketDamage(), Player.RocketMissProbability);
            }

            RocketLauncher.CurrentLoad = 0;

            if (enemy.Invincible || (enemy is Satellite satellite && satellite.BattleStation.Invincible) || (enemy is Player && !(enemy as Player).Attackable()))
                damage = 0;

            await Task.Delay(1000);

            if (damage != 0)
            {
                if (GetSelectedLauncherId() == (int)DamageType.SHIELD_ABSORBER_ROCKET_URIDIUM)
                    Absorbation(Player, enemy, damageType, damage);
                else
                    Damage(Player, enemy, damageType, damage, 0);
            }

            UpdateAttacker(enemy, Player);
        }

        public void UpdateAttacker(Attackable target, Player player)
        {
            if (!target.Destroyed)
            {
                if (target.MainAttacker == null)
                    target.MainAttacker = player;

                if (!target.Attackers.ContainsKey(player.Id))
                    target.Attackers.TryAdd(player.Id, new Attacker(player));
                else
                    target.Attackers[player.Id].Refresh();
            }
        }

        public DateTime EmpCooldown = new DateTime();
        public void EMP()
        {
            if (EmpCooldown.AddMilliseconds(TimeManager.EMP_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                Player.SendCooldown(AmmunitionManager.EMP_01, TimeManager.EMP_COOLDOWN);
                EmpCooldown = DateTime.Now;

                Player.Storage.DeactiveR_RIC3();
                Player.Storage.DeactiveDCR_250();
                Player.Storage.DeactiveSLM_01();
                Player.Storage.DeactiveDrawFireEffect();

                string empPacket = "0|n|EMP|" + Player.Id;
                Player.SendPacket(empPacket);
                Player.SendPacketToInRangePlayers(empPacket);

                foreach (var otherPlayers in Player.Spacemap.Characters.Values)
                {
                    if (otherPlayers is Player otherPlayer && otherPlayer.Selected == Player)
                        otherPlayer.Deselection(true);
                }

                foreach (var otherPlayers in Player.InRangeCharacters.Values)
                {
                    if (otherPlayers is Player otherPlayer)
                    {
                        if (otherPlayer.Position.DistanceTo(Player.Position) > 700) continue;

                        if (otherPlayer.FactionId != Player.FactionId)
                            otherPlayer.CpuManager.DisableCloak();
                    }
                }
            }
        }

        public DateTime SmbCooldown = new DateTime();
        public void SMB()
        {
            if (Player.Storage.IsInDemilitarizedZone || Player.Storage.OnBlockedMinePosition || Player.CurrentInRangePortalId != -1) return;

            if (SmbCooldown.AddMilliseconds(TimeManager.SMB_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                Player.SendCooldown(AmmunitionManager.SMB_01, TimeManager.SMB_COOLDOWN);
                SmbCooldown = DateTime.Now;

                var smbPacket = "0|n|SMB|" + Player.Id;
                Player.SendPacket(smbPacket);
                Player.SendPacketToInRangePlayers(smbPacket);

                foreach (var otherPlayer in Player.InRangeCharacters.Values)
                {
                    if (otherPlayer == null || !(otherPlayer is Player)) continue;
                    if (otherPlayer.Position.DistanceTo(Player.Position) > 700) continue;
                    if (!Player.TargetDefinition(otherPlayer, false)) continue;

                    int damage = Maths.GetPercentage(otherPlayer.CurrentHitPoints, 20);
                    Damage(Player, otherPlayer as Player, DamageType.MINE, damage, false, true, false, false);
                }
            }
        }

        public DateTime IshCooldown = new DateTime();
        public void ISH()
        {
            if (IshCooldown.AddMilliseconds(TimeManager.ISH_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
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
            var damage = RandomizeDamage(ChainImpulse.DAMAGE);

            var targets = new Dictionary<int, Character>();

            foreach (var entry in Player.InRangeCharacters.Values)
            {
                if (entry != null && entry is Player && Player.TargetDefinition(entry, false) && entry.Position.DistanceTo(Player.Position) <= 1000)
                    if (targets.Count < 7)
                        targets.Add(entry.Id, entry);
            }

            foreach (var target in targets.Values)
            {
                if (target == null) continue;

                string eciPacket = "0|TX|ECI||" + Player.Id;
                eciPacket += "|" + target.Id;

                Player.SendPacket(eciPacket);
                Player.SendPacketToInRangePlayers(eciPacket);

                Damage(Player, target, DamageType.ECI, damage, true, true, false);
            }
        }

        public static int RandomizeDamage(int baseDmg, double missProbability = 1.00)
        {
            var value = baseDmg;

            switch (Randoms.random.Next(0, 8))
            {
                case 0:
                    value = (int)(baseDmg * 1.10);
                    break;
                case 1:
                    value = (int)(baseDmg * 0.98);
                    break;
                case 2:
                    value = (int)(baseDmg * 1.02);
                    break;
                case 3:
                    value = (int)(baseDmg * 1.05);
                    break;
                case 4:
                    value = (int)(baseDmg * 0.92);
                    break;
                case 5:
                    value = (int)(baseDmg * 0.99);
                    break;
                case 6:
                    value = (int)(baseDmg * 0.97);
                    break;
                default:
                    value = (int)(baseDmg * 1.01);
                    break;
            }

            if (missProbability > Randoms.random.NextDouble())
                value = 0;

            return value;
        }

        public void Absorbation(Player attacker, Attackable target, DamageType damageType, int damage)
        {
            if (attacker.Invincible)
                attacker.Storage.DeactiveInvincibilityEffect();

            Player.CpuManager.DisableCloak();

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
                     damage, false);

                Player.SendCommand(attackHitCommand);
                Player.SendCommandToInRangePlayers(attackHitCommand);
            }

            target.UpdateStatus();
            Player.UpdateStatus();
        }

        public void Damage(Player attacker, Attackable target, DamageType damageType, int damage, double shieldPenetration, bool deactiveCloak = true)
        {
            if (damageType == DamageType.MINE && target.Invincible) return;

            if (damageType == DamageType.LASER && Player.Settings.InGameSettings.selectedLaser == AmmunitionManager.SAB_50)
            {
                Absorbation(attacker, target, damageType, damage);
                return;
            }

            int damageShd = 0, damageHp = 0;

            if (attacker.Invincible)
                attacker.Storage.DeactiveInvincibilityEffect();

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

            if (target.Invincible || (target is Satellite satellite && satellite.BattleStation.Invincible) || (target is Player && !(target as Player).Attackable()))
            {
                damage = 0;
                damageShd = 0;
                damageHp = 0;
            }

            if (deactiveCloak)
                Player.CpuManager.DisableCloak();

            if (damageType == DamageType.LASER)
            {
                if (target is Player && (target as Player).Storage.Sentinel)
                    damageShd -= Maths.GetPercentage(damageShd, 30);

                if (Player.Storage.Diminisher)
                    if (target == Player.Storage.UnderDiminisherEntity)
                        damageShd += Maths.GetPercentage(damage, 50);

                if (target is Player && (target as Player).Storage.Diminisher)
                    if ((target as Player).Storage.UnderDiminisherEntity == Player)
                        damageShd += Maths.GetPercentage(damage, 30);

                var laserRunCommand = AttackLaserRunCommand.write(Player.Id, target.Id, GetSelectedLaser(), false, true);
                Player.SendCommand(laserRunCommand);
                Player.SendCommandToInRangePlayers(laserRunCommand);

                if (Player.Settings.InGameSettings.selectedLaser != AmmunitionManager.CBO_100)
                {
                    if (Player.Storage.EnergyLeech)
                        Player.TechManager.EnergyLeech.ExecuteHeal(damage);
                }
            }

            if (damage == 0)
            {
                if (damageType == DamageType.LASER || damageType == DamageType.ROCKET)
                    AttackMissed(target, damageType);
            }
            else
            {
                if (target is Npc)
                    (target as Npc).ReceiveAttack(Player);

                var attackHitCommand =
                        AttackHitCommand.write(new AttackTypeModule((short)damageType), Player.Id,
                                             target.Id, target.CurrentHitPoints,
                                             target.CurrentShieldPoints, target.CurrentNanoHull,
                                             damage > damageShd ? damage : damageShd, false);

                Player.SendCommand(attackHitCommand);
                Player.SendCommandToInRangePlayers(attackHitCommand);
            }

            if (Player.Settings.InGameSettings.selectedLaser == AmmunitionManager.CBO_100)
            {
                var sabDamage = RandomizeDamage(2 * Player.Damage, (Player.Storage.underPLD8 ? 0.5 : 0.1));

                if (Player.Storage.Spectrum)
                    sabDamage -= Maths.GetPercentage(sabDamage, 50);

                if (target is Player)
                {
                    if ((target as Player).Storage.Spectrum)
                        sabDamage -= Maths.GetPercentage(sabDamage, 80);
                }

                Player.CurrentShieldPoints += sabDamage;
            }

            if (damageHp >= target.CurrentHitPoints || target.CurrentHitPoints <= 0)
                target.Destroy(Player, DestructionType.PLAYER);
            else
            {
                if (target.CurrentNanoHull > 0)
                {
                    if (target.CurrentNanoHull - damageHp < 0)
                    {
                        var nanoDamage = damageHp - target.CurrentNanoHull;
                        target.CurrentNanoHull = 0;
                        target.CurrentHitPoints -= nanoDamage;
                    }
                    else
                        target.CurrentNanoHull -= damageHp;
                } else
                    target.CurrentHitPoints -= damageHp;
            }

            target.CurrentShieldPoints -= damageShd;
            target.LastCombatTime = DateTime.Now;

            if (Player.Settings.InGameSettings.selectedLaser == AmmunitionManager.CBO_100)
                Player.UpdateStatus();

            target.UpdateStatus();
        }

        public static void Damage(Player attacker, Attackable target, DamageType damageType, int damage, bool toDestroy, bool toHp, bool toShd, bool missedEffect = true)
        {
            if (damageType == DamageType.MINE && target.Invincible) return;

            if (attacker.Invincible && damageType != DamageType.RADIATION)
                attacker.Storage.DeactiveInvincibilityEffect();

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

            if (toHp && toDestroy && (damage >= target.CurrentHitPoints || target.CurrentHitPoints <= 0))
            {
                if (damageType == DamageType.RADIATION)
                    target.Destroy(null, DestructionType.RADIATION);
                else if (damageType == DamageType.MINE && attacker.Attackers.Count <= 0)
                    target.Destroy(null, DestructionType.MINE);
                else
                    target.Destroy(attacker, DestructionType.PLAYER);
            }
            else if (toHp)
            {
                if (target.CurrentNanoHull > 0)
                {
                    if (target.CurrentNanoHull - damage < 0)
                    {
                        var nanoDamage = damage - target.CurrentNanoHull;
                        target.CurrentNanoHull = 0;
                        target.CurrentHitPoints -= nanoDamage;
                    }
                    else
                        target.CurrentNanoHull -= damage;
                }
                else
                    target.CurrentHitPoints -= damage;
            }

            if (toShd)
                target.CurrentShieldPoints -= damage;

            var attackHitCommand =
                    AttackHitCommand.write(new AttackTypeModule((short)damageType), attacker.Id,
                                         target.Id, target.CurrentHitPoints,
                                         target.CurrentShieldPoints, target.CurrentNanoHull,
                                         damage, false);

            attacker.SendCommand(attackHitCommand);
            attacker.SendCommandToInRangePlayers(attackHitCommand);

            target.UpdateStatus();
        }

        public void AttackMissed(Attackable target, DamageType damageType)
        {
            var attackMissedCommand = AttackMissedCommand.write(new AttackTypeModule((short)damageType), target.Id, 0);
            var attackMissedCommandToInRange = AttackMissedCommand.write(new AttackTypeModule((short)damageType), target.Id, 1);
            Player.SendCommand(attackMissedCommand);
            Player.SendCommandToInRangePlayers(attackMissedCommandToInRange);
        }

        private bool CheckLaserAttackTime()
        {
            if (Player.Settings.InGameSettings.selectedLaser == AmmunitionManager.RSB_75)
            {
                return lastRSBAttackTime.AddSeconds(3) < DateTime.Now;
            }
            return lastAttackTime.AddSeconds(1) < DateTime.Now;
        }

        public int GetRocketRange()
        {
            switch (Player.Settings.InGameSettings.selectedRocket)
            {
                case AmmunitionManager.R_310:
                    return 400;
                case AmmunitionManager.PLT_2026:
                    return 600;
                case AmmunitionManager.PLT_2021:
                    return 800;
                case AmmunitionManager.PLT_3030:
                    return 800;
                case AmmunitionManager.PLD_8:
                case AmmunitionManager.DCR_250:
                case AmmunitionManager.R_IC3:
                    return 600;
                case AmmunitionManager.WIZ_X:
                    return 800;
                default:
                    return 0;
            }
        }

        private int GetSelectedRocket()
        {
            switch (Player.Settings.InGameSettings.selectedRocket)
            {
                case AmmunitionManager.R_310:
                    return 1;
                case AmmunitionManager.PLT_2026:
                    return 2;
                case AmmunitionManager.PLT_2021:
                    return 3;
                case AmmunitionManager.PLT_3030:
                    return 4;
                case AmmunitionManager.PLD_8:
                    return 5;
                case AmmunitionManager.WIZ_X:
                    return 6;
                case AmmunitionManager.DCR_250:
                    return 10;
                case AmmunitionManager.R_IC3:
                    return 18;
                default:
                    return 0;
            }
        }

        public int GetSelectedLauncherId()
        {
            switch (Player.Settings.InGameSettings.selectedRocketLauncher)
            {
                case AmmunitionManager.HSTRM_01:
                    return 7;
                case AmmunitionManager.UBR_100:
                    return 8;
                case AmmunitionManager.ECO_10:
                    return 9;
                case AmmunitionManager.SAR_01:
                    return 12;
                case AmmunitionManager.SAR_02:
                    return 13;
                case AmmunitionManager.CBR:
                    return 14;
                default:
                    return 7;
            }
        }

        public int GetRocketDamage()
        {
            switch (Player.Settings.InGameSettings.selectedRocket)
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

        private int GetRocketLauncherRocketDamage()
        {
            switch (Player.Settings.InGameSettings.selectedRocketLauncher)
            {
                case AmmunitionManager.HSTRM_01:
                case AmmunitionManager.UBR_100:
                case AmmunitionManager.SAR_02:
                    return 4000;
                default:
                    return 0;
            }
        }

        private int GetDamageMultiplier()
        {

            switch (Player.Settings.InGameSettings.selectedLaser)
            {
                case AmmunitionManager.LCB_10:
                    return 1;
                case AmmunitionManager.MCB_25:
                    return 2;
                case AmmunitionManager.CBO_100:
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
            switch (Player.Settings.InGameSettings.selectedLaser)
            {
                case AmmunitionManager.LCB_10:
                    return 0;
                case AmmunitionManager.MCB_25:
                    return 1;
                case AmmunitionManager.MCB_50:
                    return 2;
                case AmmunitionManager.UCB_100:
                    return 3;
                case AmmunitionManager.SAB_50:
                    return 4;
                case AmmunitionManager.RSB_75:
                    return 6;
                case AmmunitionManager.CBO_100:
                    return 8;
                case AmmunitionManager.JOB_100:
                    return 9;
                case AmmunitionManager.RB_214:
                    return 11;
                default:
                    return 0;
            }
        }
    }
}
