using System.Collections.Generic;
using Frame.Core.Base;

namespace Frame.Core.Register
{
    public class PlayerManagerComponent : AEntity
    {
        private Dictionary<long, APlayer> _players = new Dictionary<long, APlayer>();

        public PlayerManagerComponent()
        {
        }
        
        public void Awake()
        {
        }

        public void Add(APlayer player)
        {
            _players.Add(player.Key, player);
        }

        public AEntity Get(long id)
        {
            _players.TryGetValue(id, out var entity);
            return entity;
        }

        public void Remove(long id)
        {
            _players.Remove(id);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (var actor in _players.Values)
            {
                actor.Dispose();
            }
        }

   }


}