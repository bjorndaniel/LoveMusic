# Love music - Bringing your last.fm playlists to Spotify

This app allows the user to search for various playlists for a last.fm user and add those tracks (if found) to a new Spotify playlist.

No information about the user is saved, the user just has to sign in to Spotify and give permission for Love music to create playlists and read the public user information.

Built using .NET Core 5.0.100-preview.3.20216.6 and Blazor WebAssembly 3.2.0

To run this app you need developer accounts for Spotify and last.fm.
* Open appsettings.json
* Add your last.fm api-key to Api
* Add your Spotify clientid to Client
* Add the url to your site /callback to the Redirect-value
* Register the redirect url in the Spotify developer portal for the client you entered the key above.
* Open a console and type dotnet run

### Future ideas

  * Add selection view where the user can select which tracks to add
  * Add more playlist-types
  * See if it is possible to integrate setlist.fm
  * Fix dns...

