var app = angular.module('app', ['ngMaterial']);
var recipientId;

app.config(function ($mdThemingProvider) {
    $mdThemingProvider.theme('default').primaryPalette('lime').accentPalette('blue');
});

app.controller('TransactionsController', function ($rootScope, $scope, TransactionsService) {
    $scope.sortType = 'Date';
    $scope.sortReverse = true;

    getTransactions();
    function getTransactions() {
        TransactionsService.getTransactions()
        .success(function (trs) {
            $scope.transactions = trs;
            $('#myTransactionsDiv').mCustomScrollbar("update");
            $('#myTransactionsDiv').mCustomScrollbar({ theme: "dark" });
        })
        .error(function (error) {
            $scope.status = 'Unable to load transactions data: ' + error.message;
        });
    }

    $scope.$on('updateTransactionsList', function (event, data) {
        getTransactions();
    });

    $scope.copyTransaction = function (CorrespondentId, CorrespondentName, Amount) {
        var trs = {
            correspondentId: CorrespondentId,
            correspondentName: CorrespondentName,
            amount: Amount
        }
        $rootScope.$broadcast('copyTransaction', trs);
        window.scrollTo(0, 0);
    };

});

app.factory('TransactionsService', ['$http', function ($http) {
    var TransactionsService = {};
    TransactionsService.getTransactions = function () {
        var token = getCookie("token.cookie");
        var authHeader = {}
        authHeader.Authorization = "Bearer " + token;
        var response = $http({
            url: "api/Transactions/my",
            method: "GET",
            headers: authHeader
        });

        return response;
    };

    return TransactionsService;
}]);

app.controller('TransactionCommitController', ['$rootScope', '$scope', '$http', function ($rootScope, $scope, $http) {
    $scope.Amount;
    $scope.submit = function () {
        if (recipientId && $scope.Amount) {
            var transaction = {
                "PayeeId": "dummy",
                "Date": "01/01/01",
                "RecipientId": recipientId,
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
                updateUserAccounts(data.MyId, data.MyResultingBalance, data.CorrespondentId, data.CorrespondentResultingBalance);
                $('#userBalance').text(data.MyResultingBalance);
                $rootScope.$broadcast('updateTransactionsList', null);
            }).
            error(function (data, status, headers, config) {
                alert(data.ExceptionMessage);
            });
        }
        else
            alert("Please select recipient and transaction amount.");
    };

    $scope.$on('copyTransaction', function (event, data) {
        $scope.Amount = data.amount;
        recipientId = data.correspondentId;
    });
}]);

function updateUserAccounts(PayeeID, PayeeBalance, RecipientID, RecipientBalance) {
    var payeeData = {
        Id: 0,
        UserId: PayeeID,
        Balance: PayeeBalance
    };
    var recipientData = {
        Id: 0,
        UserId: RecipientID,
        Balance: RecipientBalance
    };

    $.ajax({
        type: 'PUT',
        url: 'api/UserAccounts/0',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(payeeData),
        beforeSend: function (xhr) {
            var token = getCookie("token.cookie");
            xhr.setRequestHeader("Authorization", "Bearer " + token);
        }
    }).success(function (data, status, headers, config) {
        $.ajax({
            type: 'PUT',
            url: 'api/UserAccounts/0',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(recipientData),
            beforeSend: function (xhr) {
                var token = getCookie("token.cookie");
                xhr.setRequestHeader("Authorization", "Bearer " + token);
            }
        }).success(function (data, status, headers, config) {
        }).
            error(function (data, status, headers, config) {
                alert(data.ExceptionMessage);
            });
    }).error(function (data, status, headers, config) {
        alert(data.ExceptionMessage);
    });
}

app.controller('aucCtrl', aucCtrl);
function aucCtrl($timeout, $q, $log, SearchUsersService, $scope, $rootScope) {
    var self = this;
    // list of `state` value/display objects
    self.states = loadAll();
    self.querySearch = querySearch;
    self.selectedItemChange = selectedItemChange;
    self.searchTextChange = searchTextChange;
    // ******************************
    // Internal methods
    // ******************************
    function querySearch(query) {
        loadAll();
        var results = query ? self.states.filter(function (item) { return (item.display.toLowerCase().indexOf(query.toLowerCase()) !== -1); }) : self.states;

        return results;

    }

    $scope.$on('copyTransaction', function (event, data) {
        var item = {
            display: data.correspondentName,
            value: data.correspondentId
        }
        selectedItemChange(item);
        self.selectedItem = item;
    });

    function searchTextChange(text) {
        recipientId = null;
    }

    function selectedItemChange(item) {
        if (item !== undefined)
            recipientId = item.value;
    }
    /**
     * Build `states` list of key/value pairs
     */
    function loadAll() {
        SearchUsersService.get().success(function (data) {
            var loadItems = [];
            loadItems = data.map(function (obj) {
                return {
                    display: obj.Name,
                    value: obj.Id
                };
            });

            self.states = loadItems;
        });
    }
}

app.factory('SearchUsersService', ['$http', function ($http) {
    return {
        get: function () {
            var token = getCookie("token.cookie");

            var authHeader = {}
            authHeader.Authorization = "Bearer " + token;

            var config = {
                headers: authHeader
            }

            return $http.get("api/Account/PwRecipients/my", config);
        }
    };
}]);