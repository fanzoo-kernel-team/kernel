namespace FluentNHibernate
{
    public static class FluentNHibernateExtensions
    {
        /// <summary>
        /// Indicates that the collection is never loaded. This is useful for collections that you never want to load, but want to add new values to.
        /// </summary>
        /// <remarks>
        /// Note: This is not the same as lazy loading. The resulting collection will always be empty until new values are added to it.
        /// </remarks>
        public static OneToManyPart<TChild> NeverLoad<TChild>(this OneToManyPart<TChild> part) => part.Where("1 = 0");
    }
}
