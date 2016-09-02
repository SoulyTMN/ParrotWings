var app = angular.module('app', []);

app.controller('TransactionsController', function ($scope, TransactionsService) {
    getTransactions();
    function getTransactions() {
        TransactionsService.getTransactions()
        .success(function (trs) {
            $scope.transactions = trs;
        })
        .error(function (error) {
            $scope.status = 'Unable to load transactions data: ' + error.message;
        });
    }
});

app.factory('TransactionsService', ['$http', function ($http) {
    var TransactionsService = {};
    TransactionsService.getTransactions = function () {
        var token = getCookie("token.cookie");
        var authHeader = {}
        authHeader.Authorization = "Bearer " + token;  
        var response =  $http({
            url: "api/Transactions/my",
            method: "GET",
            headers: authHeader
        });

        return response;
    };

    TransactionsService.addTransaction = function () {

    }

    return TransactionsService;
}]);

app.controller('TransactionCommitController', ['$scope', '$http', function ($scope, $http) {
    $scope.submit = function () {
        if ($scope.RecipientId && $scope.Amount) {
            var transaction = {
                "PayeeId": "dummy",
                "Date" : "01/01/01",
                "RecipientId": $scope.RecipientId,
                "Amount": $scope.Amount
            }
            var token = getCookie("token.cookie");

            var authHeader = {}
            authHeader.Authorization = "Bearer " + token;

            var config = {
                headers: authHeader
            }

            $http.post('api/Transactions/my', transaction, config).
            success(function (data, status, headers, config) {
                alert('Transaction commited successfully'); 
                $('#userBalance').text(data.MyResultingBalance);
            }).
            error(function (data, status, headers, config) {
                alert("There was error during the transaction");
            });
        }

    };
}]);

function updateUserAccount(userID, balance) {
    var uacData = {
        UserId: userID,
        Balance: balance
    };

    $.ajax({
        type: 'POST',
        url: 'api/UserAccounts',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(uacData)
    });
}