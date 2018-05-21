using System;

namespace Access2Justice.Shared
{
    public interface IApp
    {
        Uri LuisUrl { get;}
        string TopIntentsCount { get;}
    }
}
