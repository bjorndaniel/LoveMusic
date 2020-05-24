using System.ComponentModel.DataAnnotations;

namespace LoveMusic
{
    public enum PlaylistType
    {
        [Display(Name = "Loved tracks")]
        [LastFmMethod(Name = "user.getlovedtracks")]
        LovedTracks, [Display(Name = "Top tracks")]
        [LastFmMethod(Name = "user.gettoptracks")]
        TopTracks, [Display(Name = "Recent tracks")]
        [LastFmMethod(Name = "user.getrecenttracks")]
        RecentTracks
    }

    public enum UIUpdateType
    {
        Processing,
        Done
    }

    public enum SpotifyPlayerTask
    {
        play,
        pause,
        next,
        previous
    }
}