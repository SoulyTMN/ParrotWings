

function expireCookie(name) {
    document.cookie = name + '=;expires=Thu,01 Jan 1970 00:00:01 GMT;'
}

function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length === 2) return parts.pop().split(";").shift();
}

function parseValidationJSON(data) {
    if (data.responseJSON !== undefined)
    {
        var result = data.responseJSON.Message;

        if (result === undefined)
            result = data.responseJSON.error_description;

        for (var property in data.responseJSON.ModelState)
        {
            if (data.responseJSON.ModelState[property] !== undefined)
                result += "\n" + data.responseJSON.ModelState[property];
        }

        return result;
    }
    else
        return data.responseText;
}