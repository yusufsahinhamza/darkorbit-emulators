using Ow.Game.Events;
using Ow.Game.Movements;
using Ow.Game.Objects.Players.Managers;
using Ow.Game.Ticks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects
{
    abstract class Attackable : Tick
    {
        public int TickId { get; set; }

        public int Id { get; }

        public abstract Position Position { get; set; }

        public abstract Spacemap Spacemap { get; set; }

        public abstract int FactionId { get; set; }

        public abstract int CurrentHitPoints { get; set; }

        public abstract int MaxHitPoints { get; set; }

        public abstract int CurrentNanoHull { get; set; }

        public abstract int MaxNanoHull { get; set; }

        public abstract int CurrentShieldPoints { get; set; }

        public abstract int MaxShieldPoints { get; set; }

        public abstract double ShieldAbsorption { get; set; }

        public abstract double ShieldPenetration { get; set; }

        public DateTime LastCombatTime { get; set; }

        public virtual int AttackRange => 700;

        public virtual int RenderRange => 2000;

        public bool Invisible { get; set; }

        protected Attackable(int id)
        {
            Id = id;
            Invisible = false;
        }

        public abstract void Tick();

        public bool InRange(Attackable attackable, int range = 2000)
        {
            if (attackable == null || attackable.Spacemap.Id != Spacemap.Id) return false;
            if (attackable is Character character)
            {
                if (character == null || character.Destroyed) return false;

                if (this is Player thisPlayer && character is Player otherPlayer)
                {
                    if (thisPlayer.Spacemap.Id == Duel.Spacemap.Id)
                        if (thisPlayer.Storage.DuelOpponent != otherPlayer)
                            return false;
                }

            }
            if (range == -1 || attackable.Spacemap.Options.RangeDisabled) return true;
            return attackable.Id != Id && Position.DistanceTo(attackable.Position) <= range;
        }

        public abstract void Destroy(Character destroyer, DestructionType destructionType);
    }
}
