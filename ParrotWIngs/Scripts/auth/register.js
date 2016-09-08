function register() {
    var uName = $('#name').val();
    var uEmail = $('#email').val();

    var data = {
        PwName: uName,
        Name: uEmail,
        Email: uEmail,
        Password: $('#password').val(),
        ConfirmPassword: $('#confirmpassword').val()
    };
    $.ajax({
        type: 'POST',
        url: '/api/Account/Register',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data)
    }).success(function (data) {
         
        initUserAccount(data);

        $('#after_registration').html('Successfully registered.');

    }).fail(function (data) {
        alert(parseValidationJSON(data));
    });

    return false;
}

function initUserAccount(userID) {
    var uacData = {
        UserId: userID,
        Balance: 500
    };

    $.ajax({
        type: 'POST',
        url: 'api/UserAccounts',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(uacData)
    }).success(function (data) {
        $('#emailLogin').val(data.Email);
    }).fail(function (data) {
        alert(parseValidationJSON(data));
    });
}