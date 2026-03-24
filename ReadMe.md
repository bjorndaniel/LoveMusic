![Build and deploy](https://github.com/bjorndaniel/LoveMusic/workflows/Build%20and%20deploy%20.NET%20Core%20application%20to%20Web%20App%20lovemusic/badge.svg)
# Love music - Bringing your last.fm playlists to Spotify

This app allows the user to search for various playlists for a last.fm user and add those tracks (if found) to a new Spotify playlist. It also supports setlist.fm integration to create playlists from concert setlists.

No information about the user is saved — the user only needs to sign in to Spotify and grant permission for Love music to create playlists and read public user information.

Built using .NET 9 and Blazor WebAssembly, deployed as an Azure Static Web App with an Azure Functions backend.

### Setup

To run this app you need developer accounts for Spotify and last.fm.

* Open `wwwroot/appsettings.json`
* Set `Client` to your Spotify client ID
* Set `Redirect` to your site's `/callback` URL
* Set `FunctionsApi` to the URL of your Azure Functions backend (which handles last.fm and setlist.fm API calls)
* Register the redirect URL in the Spotify developer portal for the client you entered above
* Run the Azure Functions project locally or deploy it to Azure
* Open a console and run `dotnet run`

### Future ideas

  * Add selection view where the user can select which tracks to add
  * Add more playlist-types
