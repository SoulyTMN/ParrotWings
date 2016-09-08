var tokenKey = "tokenInfo";
function login() {
    //...........................
        var loginData = {
            grant_type: 'password',
            username: $('#emailLogin').val(),
            password: $('#passwordLogin').val()
        };
        $.ajax({
            type: 'POST',
            url: '/Token',
            data: loginData
        }).success(function (data) {
            $('.userName').text(data.userName);
            $('.userInfo').css('display', 'block');
            $('.loginForm').css('display', 'none'); 
            document.cookie = "token.cookie=" + data.access_token + ";";
        }).fail(function (data) {
            alert(parseValidationJSON(data));
        }); 
}

function logout() {
    $.ajax({
        type: 'POST',
        url: 'api/Account/Logout',
        beforeSend: function (xhr) {
            var token = getCookie("token.cookie");
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        }
    }).success(function (data) {
        expireCookie("token.cookie");
        location.reload();
    }).fail(function (data) {
        alert(parseValidationJSON(data));
    });
}
