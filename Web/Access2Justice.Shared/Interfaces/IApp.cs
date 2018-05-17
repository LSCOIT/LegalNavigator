namespace Access2Justice.Shared
{
    using System;

    public interface IApp
    {
        Uri LuisUrl { get;}
        string TopIntentsCount { get;}
    }
}
