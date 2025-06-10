var https = require('https');
module.exports = function (context, req) {
    context.log('LastFm count called.');
    var lastFmUser = req.query.user;
    var method = req.query.method;
    if (lastFmUser && method) {
        var url = process.env['LastFmUrl'];
        var key = process.env['LastFmKey'];
        url = `${url}?method=${method}&user=${lastFmUser}&api_key=${key}&format=json&limit=1&page=1`;
        https.get(url, (resp) => {
            let data = '';
            resp.on('data', (chunk) => {
                data += chunk;
            });
            resp.on('end', () => {
                console.log(data);
                var json = JSON.parse(data);
                context.res = {
                    body: {
                        "count": getCount(method, json)
                    }
                }
                context.done();
            });
        }).on("error", (err) => {
            console.log("Error: " + err.message);
            context.res = {
                status: 500,
                body: err.message
            };
            context.done();
        });

    }
    else {
        context.res = {
            status: 400,
            body: "Username is required"
        };
    }
    function getCount(method, json) {
        switch (method) {
            case 'user.gettoptracks':
                return parseInt(json.toptracks['@attr'].total, 10);
            case 'user.getrecenttracks':
                return parseInt(json.recenttracks['@attr'].total, 10);
            case 'user.getlovedtracks':
                return parseInt(json.lovedtracks['@attr'].total, 10);
            default:
                return 0
        }
    }
};