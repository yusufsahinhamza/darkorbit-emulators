using Ow.Game.Objects;
using Ow.Game.Objects.Players.Techs;
using Ow.Managers;
using Ow.Net.netty;
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

        public AttackManager(Player player) { Player = player; RocketLauncher = new RocketLauncher(Player); }

        public DateTime lastAttackTime = new DateTime();
        public DateTime lastRSBAttackTime = new DateTime();
        public DateTime mineCooldown = new DateTime();

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

                    var damage = RandomizeDamage(GetDamageMultiplier() * Player.Damage, Player.Storage.underPLD8 ? 2 : 1);

                    if (Player.Storage.Spectrum)
                        damage -= Maths.GetPercentage(damage, 50);

                    if (enemy is Player)
                    {
                        if ((enemy as Player).Storage.Spectrum)
                            damage -= Maths.GetPercentage(damage, 80);
                    }

                    Damage(Player, enemy, DamageType.LASER, damage, Player.ShieldPenetration);

                    if (Player.Storage.AutoRocket)
                        RocketAttack();
                    
                    if (Player.Storage.AutoRocketLauncher)
                        if (RocketLauncher.CurrentLoad != RocketLauncher.MaxLoad)
                            RocketLauncher.Reload();
                        else
                            LaunchRocketLauncher();
                    RocketLauncher.Reload();                 

                    UpdateAttacker(enemy, Player);                  

                    if (Player.SettingsManager.SelectedLaser == AmmunitionTypeModule.RSB)
                    {
                        lastRSBAttackTime = DateTime.Now;
                        Player.SendCooldown(ServerCommands.RSB_COOLDOWN, 3000);
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

            if (Player.SettingsManager.SelectedRocket != AmmunitionTypeModule.WIZARD)
                if (!TargetDefinition(enemy, true, true)) return;

            switch (Player.SettingsManager.SelectedRocket)
            {
                case AmmunitionTypeModule.PLASMA:
                    PLD8();
                    break;
                case AmmunitionTypeModule.WIZARD:
                    WIZ_X();
                    break;
                case AmmunitionTypeModule.DECELERATION:
                    DCR_250();
                    break;
                default:
                    if (lastRocketAttack.AddSeconds(Player.RocketSpeed) < DateTime.Now)
                    {
                        var damage = RandomizeDamage(Player.RocketDamage, Player.Storage.PrecisionTargeter ? 0 : 1);
                        Damage(Player, enemy, DamageType.ROCKET, damage, 0);

                        String rocketRunPacket = "0|v|" + Player.Id + "|" + enemy.Id + "|H|" + GetSelectedRocket() + "|1|" + (Player.Storage.PrecisionTargeter ? 1 : 0);
                        Player.SendPacket(rocketRunPacket);
                        Player.SendPacketToInRangePlayers(rocketRunPacket);

                        Player.SendCooldown(ServerCommands.ROCKET_COOLDOWN, Player.RocketSpeed * 1000);
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

            if (GetSelectedLauncherId() == 13)
                Absorbation(Player, enemy, GetSelectedLauncherId() == 13 ? DamageType.SHIELD_ABSORBER_ROCKET_URIDIUM : DamageType.ROCKET, RandomizeDamage(GetRocketLauncherDamage()));
            else
                Damage(Player, enemy, DamageType.ROCKET, RandomizeDamage(GetRocketLauncherDamage()), 0);

            RocketLauncher.CurrentLoad = 0;
            Player.SendPacket("0|" + ServerCommands.ROCKETLAUNCHER + "|" + ServerCommands.ROCKETLAUNCHER_STATUS + "|2|" + Player.AttackManager.GetSelectedLauncherId() + "|" + RocketLauncher.CurrentLoad);
            RocketLauncher.LastReloadTime = DateTime.Now;

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
                case AmmunitionTypeModule.HELLSTORM:
                    return 7;
                case AmmunitionTypeModule.UBER_ROCKET:
                    return 8;
                case AmmunitionTypeModule.SAR02:
                    return 13;
                default:
                    return 7;
            }
        }

        public DateTime EmpCooldown = new DateTime();
        public void EMP()
        {
            if (EmpCooldown.AddMilliseconds(TimeManager.EMP_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                Player.SendCooldown(ServerCommands.EMP_COOLDOWN, TimeManager.EMP_COOLDOWN);
                EmpCooldown = DateTime.Now;

                Player.DeactiveDCR_250();
                Player.DeactiveSLM_01();

                string empPacket = "0|n|EMP|" + Player.Id;
                Player.SendPacket(empPacket);
                Player.SendPacketToInRangePlayers(empPacket);

                foreach (var otherPlayers in Player.Spacemap.Characters.Values)
                {
                    if (otherPlayers is Player otherPlayer)
                    {
                        if (otherPlayer.Selected == Player)
                        {
                            string empMessagePacket = "0|A|STM|msg_own_targeting_harmed";
                            otherPlayer.SendPacket(empMessagePacket);
                            otherPlayer.Selected = null;
                            otherPlayer.DisableAttack(otherPlayer.SettingsManager.SelectedLaser);
                            otherPlayer.SendCommand(ShipDeselectionCommand.write());
                            otherPlayer.SendPacket("0|UI|MM|NOISE");
                        }
                    }
                }

                foreach (var otherPlayers in Player.InRangeCharacters.Values)
                {
                    if (otherPlayers is Player otherPlayer)
                    {
                        if (otherPlayer.Position.DistanceTo(Player.Position) > 700) return;

                        if (Player.RankId == 21)
                            otherPlayer.AddVisualModifier(new VisualModifierCommand(otherPlayer.Id, VisualModifierCommand.MIRRORED_CONTROLS, 0, true));

                        if (otherPlayer.Invisible)
                            otherPlayer.CpuManager.DisableCloak();
                    }
                }
            }
        }

        public DateTime pld8Cooldown = new DateTime();
        public void PLD8()
        {
            var enemy = Player.SelectedCharacter;
            if (enemy == null) return;
            if (!TargetDefinition(enemy)) return;

            if (pld8Cooldown.AddMilliseconds(TimeManager.PLD8_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                if (Player.Invisible)
                    Player.CpuManager.DisableCloak();

                String rocketRunPacket = "0|v|" + Player.Id + "|" + enemy.Id + "|H|" + GetSelectedRocket() + "|1|" + (Player.Storage.PrecisionTargeter ? 1 : 0);
                Player.SendPacket(rocketRunPacket);
                Player.SendPacketToInRangePlayers(rocketRunPacket);

                Player.SendCooldown(ServerCommands.PLASMA_DISCONNECT_COOLDOWN, TimeManager.PLD8_COOLDOWN);
                pld8Cooldown = DateTime.Now;

                if (RandomizeDamage(1, 1) == 0)
                {
                    if (!(enemy is Player)) return;

                    var enemyPlayer = enemy as Player;
                    if (!enemyPlayer.Attackable()) return;

                    enemyPlayer.Storage.underPLD8 = true;
                    enemyPlayer.Storage.underPLD8Time = DateTime.Now;

                    enemyPlayer.SendPacket("0|n|MAL|SET|" + enemyPlayer.Id + "");
                    enemyPlayer.SendPacketToInRangePlayers("0|n|MAL|SET|" + enemyPlayer.Id + "");
                }
            }
        }

        public DateTime wiz_xCooldown = new DateTime();
        public void WIZ_X()
        {
            var enemy = Player.SelectedCharacter;
            if (enemy == null) return;

            if (wiz_xCooldown.AddMilliseconds(TimeManager.WIZARD_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                if (Player.Invisible)
                    Player.CpuManager.DisableCloak();

                String rocketRunPacket = "0|v|" + Player.Id + "|" + enemy.Id + "|H|" + GetSelectedRocket() + "|1|" + (Player.Storage.PrecisionTargeter ? 1 : 0);
                Player.SendPacket(rocketRunPacket);
                Player.SendPacketToInRangePlayers(rocketRunPacket);

                Player.SendCooldown(ServerCommands.WIZ_ROCKET, TimeManager.WIZARD_COOLDOWN);
                wiz_xCooldown = DateTime.Now;

                if (!(enemy is Player)) return;

                var enemyPlayer = enemy as Player;

                var ship = Ship.GetRandomShipLootId(enemyPlayer.Ship.Id);
                enemyPlayer.AddVisualModifier(new VisualModifierCommand(enemyPlayer.Id, VisualModifierCommand.WIZARD_ATTACK, ship, true));
            }
            
        }

        public DateTime dcr_250Cooldown = new DateTime();
        public void DCR_250()
        {
            var enemy = Player.SelectedCharacter;
            if (enemy == null) return;
            if (!TargetDefinition(enemy)) return;

            if (dcr_250Cooldown.AddMilliseconds(TimeManager.DCR_250_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                if (Player.Invisible)
                    Player.CpuManager.DisableCloak();

                String rocketRunPacket = "0|v|" + Player.Id + "|" + enemy.Id + "|H|" + GetSelectedRocket() + "|1|" + (Player.Storage.PrecisionTargeter ? 1 : 0);
                Player.SendPacket(rocketRunPacket);
                Player.SendPacketToInRangePlayers(rocketRunPacket);

                Player.SendCooldown(ServerCommands.DCR_ROCKET, TimeManager.DCR_250_COOLDOWN);
                dcr_250Cooldown = DateTime.Now;

                if (!(enemy is Player)) return;

                var enemyPlayer = enemy as Player;
                if (!enemyPlayer.Attackable()) return;

                enemyPlayer.Storage.underDCR_250 = true;
                enemyPlayer.Storage.underDCR_250Time = DateTime.Now;

                enemyPlayer.SendPacket("0|n|fx|start|SABOTEUR_DEBUFF|" + enemyPlayer.Id + "");
                enemyPlayer.SendPacketToInRangePlayers("0|n|fx|start|SABOTEUR_DEBUFF|" + enemyPlayer.Id + "");
                enemyPlayer.SendCommand(AttributeShipSpeedUpdateCommand.write(enemyPlayer.Speed));
            }
        }

        public DateTime SmbCooldown = new DateTime();
        public void SMB()
        {
            if (Player.Storage.IsInDemilitarizedZone) return;

            if (SmbCooldown.AddMilliseconds(TimeManager.SMB_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                Player.SendCooldown(ServerCommands.SMARTBOMB_COOLDOWN, TimeManager.SMB_COOLDOWN);
                SmbCooldown = DateTime.Now;

                var smbPacket = "0|n|SMB|" + Player.Id;
                Player.SendPacket(smbPacket);
                Player.SendPacketToInRangePlayers(smbPacket);

                foreach (var otherPlayer in Player.InRangeCharacters.Values)
                {
                    if (otherPlayer == null || !(otherPlayer is Player)) return;
                    if (!TargetDefinition(otherPlayer, false)) return;
                    if (otherPlayer.Position.DistanceTo(Player.Position) > 700) return;

                    int damage = Maths.GetPercentage(otherPlayer.CurrentHitPoints, 20);

                    Damage(Player, otherPlayer as Player, DamageType.MINE, damage, false, true, false);
                }
            }
        }

        public DateTime IshCooldown = new DateTime();
        public void ISH()
        {
            if (IshCooldown.AddMilliseconds(TimeManager.ISH_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                IshCooldown = DateTime.Now;
                Player.SendCooldown(ServerCommands.INSTASHIELD_COOLDOWN, TimeManager.ISH_COOLDOWN);

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

            /*
             * MANTIKEN YUKARIDAKI DAMAGEDE CAN AZSA ÖLMESİ LAZI???
            foreach (var target in targets.Values)
            {
                if (damage >= target.CurrentHitPoints || target.CurrentHitPoints == 0)
                    target.Destroy(Player, DestructionType.PLAYER);
            }
            */
        }

        public DateTime outOfRangeCooldown = new DateTime();
        public DateTime inAttackCooldown = new DateTime();
        public DateTime peaceAreaCooldown = new DateTime();
        public bool TargetDefinition(Character target, bool sendWarningMessage = true, bool isRocketAttack = false)
        {
            if (target == null) return false;

            if (target is Player && (target as Player).Group != null)
            {
                if (Player.Group != null && Player.Group.Members.ContainsKey(target.Id))
                {
                    if (sendWarningMessage)
                    {
                        Player.DisableAttack(Player.SettingsManager.SelectedLaser);
                        Player.SendPacket("0|A|STD|You can't attack members of your group!");
                    }
                    return false;
                }
            }

            short relationType = target.Clan != null && Player.Clan != null ? Player.Clan.GetRelation(target.Clan) : (short)0;
            if (target.FactionId == Player.FactionId && relationType != ClanRelationModule.AT_WAR && !(target is Pet pet && pet == Player.Pet) && !(EventManager.JackpotBattle.Active && EventManager.JackpotBattle.Players.ContainsKey(Player.Id)))
            {
                if (sendWarningMessage)
                {
                    Player.DisableAttack(Player.SettingsManager.SelectedLaser);
                    Player.SendPacket("0|A|STD|You can't attack members of your own company!");
                }
                return false;
            }

            if (target is Player && (target as Player).Storage.IsInDemilitarizedZone)
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

            if (Player.Position.DistanceTo(target.Position) > (isRocketAttack ? GetRocketRange() : Player.AttackRange))
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
                case AmmunitionTypeModule.R310:
                    return 400;
                case AmmunitionTypeModule.PLT2026:
                    return 600;
                case AmmunitionTypeModule.PLT2021:
                    return 800;
                case AmmunitionTypeModule.PLT3030:
                    return 800;
                default:
                    return 600;
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
            if (attacker.Storage.invincibilityEffect)
                attacker.DeactiveInvincibilityEffect();

            if (Player.Invisible)
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
            if (damageType == DamageType.MINE && target is Player && (target as Player).Storage.invincibilityEffect) return;

            if (damageType == DamageType.LASER && Player.SettingsManager.SelectedLaser == AmmunitionTypeModule.SAB)
            {
                Absorbation(attacker, target, damageType, damage);
                return;
            }

            int damageShd = 0, damageHp = 0;

            if (attacker.Storage.invincibilityEffect)
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
                    Player.CpuManager.DisableCloak();
            }

            if (damageType == DamageType.LASER)
            {
                if (target is Player && (target as Player).Storage.Sentinel)
                    damageShd -= Maths.GetPercentage(damageShd, 30);

                if (Player.Storage.Diminisher)
                    if (target == Player.Storage.UnderDiminisherPlayer)
                        damageShd += Maths.GetPercentage(damage, 50);

                if (target is Player && (target as Player).Storage.Diminisher)
                    if ((target as Player).Storage.UnderDiminisherPlayer == Player)
                        damageShd += Maths.GetPercentage(damage, 30);
            }

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

            if (Player.SettingsManager.SelectedLaser == AmmunitionTypeModule.CBO)
            {
                var sabDamage = RandomizeDamage(2 * Player.Damage, Player.Storage.underPLD8 ? 2 : 1);

                if (Player.Storage.Spectrum)
                    sabDamage -= Maths.GetPercentage(sabDamage, 50);

                if (target is Player)
                {
                    if ((target as Player).Storage.Spectrum)
                        sabDamage -= Maths.GetPercentage(sabDamage, 80);
                }

                Player.CurrentShieldPoints += sabDamage;
            }

            if (damageType == DamageType.LASER)
            {
                if (Player.SettingsManager.SelectedLaser != AmmunitionTypeModule.SAB)
                {
                    if (Player.Storage.EnergyLeech)
                        Player.TechManager.EnergyLeech.ExecuteHeal(damage);
                }
            }

            if (damageHp >= target.CurrentHitPoints || target.CurrentHitPoints == 0)
                target.Destroy(Player, DestructionType.PLAYER);
            else
                target.CurrentHitPoints -= damageHp;

            target.CurrentShieldPoints -= damageShd;
            target.LastCombatTime = DateTime.Now;

            if (Player.SettingsManager.SelectedLaser == AmmunitionTypeModule.CBO)
                Player.UpdateStatus();

            target.UpdateStatus();
        }

        public static void Damage(Player attacker, Character target, DamageType damageType, int damage, bool toDestroy, bool toHp, bool toShd, bool missedEffect = true)
        {
            if (damageType == DamageType.MINE && target is Player && (target as Player).Storage.invincibilityEffect) return;

            if (attacker.Storage.invincibilityEffect && damageType != DamageType.RADIATION)
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

            if (toHp && toDestroy && (damage >= target.CurrentHitPoints || target.CurrentHitPoints == 0))
            {
                if (damageType == DamageType.RADIATION)
                    target.Destroy(null, DestructionType.RADITATION);
                else if (damageType == DamageType.MINE && attacker.Attackers.Count <= 0)
                    target.Destroy(null, DestructionType.MINE);
                else
                    target.Destroy(attacker, DestructionType.PLAYER);
            }
            else target.CurrentHitPoints -= damage;

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

        public void AttackMissed(Character target, DamageType damageType)
        {
            var attackMissedCommand = AttackMissedCommand.write(new AttackTypeModule((short)damageType), target.Id, 0);
            var attackMissedCommandToInRange = AttackMissedCommand.write(new AttackTypeModule((short)damageType), target.Id, 1);
            Player.SendCommand(attackMissedCommand);
            Player.SendCommandToInRangePlayers(attackMissedCommandToInRange);
        }

        private bool CheckLaserAttackTime()
        {
            if (Player.SettingsManager.SelectedLaser == AmmunitionTypeModule.RSB)
            {
                return lastRSBAttackTime.AddSeconds(3) < DateTime.Now;
            }
            return lastAttackTime.AddSeconds(1) < DateTime.Now;
        }

        public int GetSelectedRocket()
        {
            switch (Player.SettingsManager.SelectedRocket)
            {
                case AmmunitionTypeModule.R310:
                    return 1;
                case AmmunitionTypeModule.PLT2026:
                    return 2;
                case AmmunitionTypeModule.PLT2021:
                    return 3;
                case AmmunitionTypeModule.PLT3030:
                    return 4;
                case AmmunitionTypeModule.PLASMA:
                    return 5;
                case AmmunitionTypeModule.DECELERATION:
                    return 10;
                case AmmunitionTypeModule.WIZARD:
                    return 6;
                default:
                    return 0;
            }
        }

        public int GetRocketDamage()
        {
            switch (Player.SettingsManager.SelectedRocket)
            {
                case AmmunitionTypeModule.R310:
                    return 1000;
                case AmmunitionTypeModule.PLT2026:
                    return 2000;
                case AmmunitionTypeModule.PLT2021:
                    return 4000;
                case AmmunitionTypeModule.PLT3030:
                    return 6000;
                default:
                    return 0;
            }
        }

        private int GetRocketLauncherDamage()
        {
            switch (Player.SettingsManager.SelectedRocketLauncher)
            {
                case AmmunitionTypeModule.HELLSTORM:
                    return 4000 * RocketLauncher.CurrentLoad;
                case AmmunitionTypeModule.UBER_ROCKET:
                    return 4000 * RocketLauncher.CurrentLoad;
                case AmmunitionTypeModule.SAR02:
                    return 4000 * RocketLauncher.CurrentLoad;
                default:
                    return 0;
            }
        }

        private int GetDamageMultiplier()
        {

            switch (Player.SettingsManager.SelectedLaser)
            {
                case AmmunitionTypeModule.X1:
                    return 1;
                case AmmunitionTypeModule.X2:
                    return 2;
                case AmmunitionTypeModule.CBO:
                case AmmunitionTypeModule.X3:
                    return 3;
                case AmmunitionTypeModule.X4:
                    return 4;
                case AmmunitionTypeModule.RSB:
                    return 5;
                case AmmunitionTypeModule.SAB:
                    return 2;
                default:
                    return 1;
            }
        }

        public int GetSelectedLaser()
        {
            switch (Player.SettingsManager.SelectedLaser)
            {
                case AmmunitionTypeModule.X1:
                    return 0;
                case AmmunitionTypeModule.X2:
                    return 1;
                case AmmunitionTypeModule.X3:
                    return 2;
                case AmmunitionTypeModule.X4:
                    return 3;
                case AmmunitionTypeModule.SAB:
                    return 4;
                case AmmunitionTypeModule.RSB:
                    return 6;
                case AmmunitionTypeModule.CBO:
                    return 8;
                default:
                    return 0;
            }
        }
    }
}
