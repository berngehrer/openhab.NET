using openhab.net.rest.Json;
using System;

namespace openhab.net.rest.Sitemaps
{
    public class OpenhabSitemap : IOpenhabElement, IEquatable<OpenhabSitemap>
    {
        internal OpenhabSitemap(SitemapObject original)
        {
            Name = original.Name;
            Description = original.Label;
            Link = new Uri(original.Link);
        }

        public Uri Link { get; }
        public string Name { get; }
        public string Description { get; }

        public string Value
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public static bool operator ==(OpenhabSitemap a, OpenhabSitemap b)
        {
            return a.Name.Equals(b.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool operator !=(OpenhabSitemap a, OpenhabSitemap b)
        {
            return !a.Name.Equals(b.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public bool Equals(OpenhabSitemap other) => this == other;

        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString() => Description;

        public override bool Equals(object obj)
        {
            var other = obj as OpenhabSitemap;
            if (other != null)
                return Equals(other);
            return base.Equals(obj);
        }
    }
}
