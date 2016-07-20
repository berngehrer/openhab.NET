namespace openhab.net.rest.Http
{
    internal class PoolingSession
    {
        public static PoolingSession False => new PoolingSession();

        public PoolingSession()
        {
        }

        public PoolingSession(long id)
        {
            Id = id;
            UsePooling = true;
        }

        public long Id { get; set; }
        public bool UsePooling { get; set; } = false;
        
        public override string ToString() => Id.ToString();
    }
}
