var https = require('https');
module.exports = function (context, req) {
    context.log('LastFm tracks called.');
    var lastFmUser = req.query.user;
    var method = req.query.method;
    var perpage = req.query.perpage;
    var page = req.query.page
    if (lastFmUser && method && perpage && page) {
        var url = process.env['LastFmUrl'];
        var key = process.env['LastFmKey'];
        url = `${url}?method=${method}&user=${lastFmUser}&api_key=${key}&format=json&limit=${perpage}&page=${page}`;
        https.get(url, (resp) => {
            let data = '';
            resp.on('data', (chunk) => {
                data += chunk;
            });
            resp.on('end', () => {
                var json = JSON.parse(data);
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
            body: "Username,method,perpage,page is required"
        };
    }
};