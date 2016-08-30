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
            sessionStorage.setItem(tokenKey, data.access_token);
            console.log(data.access_token);
            alert("Login successful.");
        }).fail(function (data) {
            alert('Login failed.');
        }); 
}

function logout() {
    sessionStorage.removeItem(tokenKey);
}