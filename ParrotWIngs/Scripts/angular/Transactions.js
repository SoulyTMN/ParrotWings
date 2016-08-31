var app = angular.module('app', []);

app.controller('TransactionsController', function ($scope, TransactionsService) {
    getTransactions();
    function getTransactions() {
        TransactionsService.getTransactions()
        .success(function (trs) {
            $scope.transactions = trs;
            console.log($scope.transactions);
        })
        .error(function (error) {
            $scope.status = 'Unable to load transactions data: ' + error.message;
            console.log($scope.status);
        });
    }
});

app.factory('TransactionsService', ['$http', function ($http) {
    var TransactionsService = {};
    TransactionsService.getTransactions = function () {
        var token = sessionStorage.getItem("tokenInfo");  
        var authHeader = {}
        authHeader.Authorization = "Bearer " + token;  
        var response =  $http({
            url: "api/Transactions/my",
            method: "GET",
            headers: authHeader
        });

        return response;
    };
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
            var token = sessionStorage.getItem("tokenInfo");

            var authHeader = {}
            authHeader.Authorization = "Bearer " + token;

            /*$http({
                url: "api/Transactions/my",
                method: "POST",
                headers: authHeader
            });*/

            var config = {
                headers: authHeader
            }

            $http.post('api/Transactions/my', transaction, config).
            success(function (data, status, headers, config) {
                alert('Transaction commited successfully');
            }).
            error(function (data, status, headers, config) {
                alert("There was error during the transaction");
            });
        }
    };
}]);

app.controller('exitController', function ($scope, $window) {
    $scope.onExit = function () {
        $localStorage.$reset();
    };

    $window.onbeforeunload = $scope.onExit;
});