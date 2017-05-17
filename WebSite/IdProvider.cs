using System;

namespace WebSite
{
    public static class IdProvider
    {
        private static readonly Func<Guid> DefaultGuidProvider = () => Guid.NewGuid();

        public static Guid NewGuidId()
        {
            return DefaultGuidProvider();
        }
    }
}