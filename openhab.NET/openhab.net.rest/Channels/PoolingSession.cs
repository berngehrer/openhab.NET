namespace openhab.net.rest.Channels
{
    internal class PoolingSession
    {
        public static PoolingSession False => new PoolingSession();

        public PoolingSession()
        {
        }

        public PoolingSession(int id)
        {
            Id = id;
            UsePooling = true;
        }

        public int Id { get; set; }
        public bool UsePooling { get; set; } = false;
        
        public override string ToString() => Id.ToString();
    }
}
