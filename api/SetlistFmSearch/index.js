var https = require('https');

module.exports = function (context, req) {
    context.log('SetlistFm search called');
    if (req.query.artist) {
        var key = process.env['SetlistFmKey'];
        var path = `/rest/1.0/search/setlists?artistName=${encodeURIComponent(req.query.artist)}&cityName=${encodeURIComponent(req.query.city)}&p=${req.query.page}`;
        if (req.query.venue) {
            path += `&venueName=${encodeURIComponent(req.query.venue)}`;
        }
        console.log(path);
        var optionsget = {
            headers: {
                'x-api-key': key,
                'accept': 'application/json'
            },
            host: process.env['SetlistFmUrl'],
            path: path,
            method: 'GET',
        };
        https.get(optionsget, (resp) => {
            let data = '';
            console.log(resp.statusCode);
            resp.on('data', (chunk) => {
                data += chunk;
            });
            resp.on('end', () => {
                var json = JSON.parse(data);
                console.log(json);
                context.res = {
                    body: json
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
            body: "Searchtext is required"
        };
    }
};