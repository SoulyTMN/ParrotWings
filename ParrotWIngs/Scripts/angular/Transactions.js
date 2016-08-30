var uTransactions = angular.module('uTransactions', []);

uTransactions.controller('TransactionsController', function ($scope, TransactionsService) {
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

uTransactions.factory('TransactionsService', ['$http', function ($http) {
    var TransactionsService = {};
    TransactionsService.getTransactions = function () {
        return $http.get('api/Transactions/my');
    };
    return TransactionsService;
}]);