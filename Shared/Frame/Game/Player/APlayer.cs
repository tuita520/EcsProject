using Frame.Core.Base;

namespace Frame.Core.Register
{
    
    public abstract class APlayer:AEntity
    {
        public long Uid => Key;

        public long Key { get; private set; }

        public APlayer(int uid)
        {
            Key = uid;
        }
        
        public void SetUid(long uid)
        {
            Key = uid;
        }
    }
    
}