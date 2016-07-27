using openhab.net.rest.Core;
using openhab.net.rest.Json;
using System;

namespace openhab.net.rest.Sitemaps
{
    public class OpenhabSitemap : IOpenhabElement, IElementObservable
    {
        IElementObserver _observer;

        public event EventHandler Changed;


        internal OpenhabSitemap(SitemapObject original, IElementObserver observer)
        {
            Name = original.Name;
            Description = original.Label;
            Link = new Uri(original.Link);
            (_observer = observer)?.Subscribe(this);
            Syncronize();
        }

        public Uri Link { get; }
        public string Name { get; }
        public string Description { get; }
        
        public override string ToString() => Description;


        public bool IsEqual(IOpenhabElement element)
        {
            return Equals(element as OpenhabSitemap);
        }

        public override bool Equals(object obj)
        {
            var other = obj as OpenhabSitemap;
            return other?.Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        protected void FireValueChanged()
        {
            Syncronize();
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Update from Observer
        /// </summary>
        public void OnNotify(IOpenhabElement element)
        {
            var other = element as OpenhabSitemap;
            if (!IsEqual(other)) {
                FireValueChanged();
            }
        }

        protected void Syncronize()
        {
        }
    }
}
