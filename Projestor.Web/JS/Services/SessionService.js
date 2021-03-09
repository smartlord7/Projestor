Projestor.factory('sessionService', function ($window, $location, ajaxCaller) {
    var sessionObject = {
        getSessionJwt: function () {
            var parsed = JSON.parse($window.localStorage.getItem('sessionData'));
            if (parsed) return parsed.jwt;
        },

        getUserName: function () {
            var parsed = JSON.parse($window.localStorage.getItem('sessionData'));
            if (parsed) return parsed.userName;
        },

        getRole: function () {
            var parsed = JSON.parse($window.localStorage.getItem('sessionData'));
            if (parsed) return parsed.role;
        },

        updateSessionData: function (sessionData) {
            $window.localStorage.setItem('sessionData', JSON.stringify(sessionData));
        },

        checkSession: function () {
            if (!$window.localStorage.getItem('sessionData')) $location.path('/login');
            var sessionData = JSON.parse($window.localStorage.getItem('sessionData'));
            var loginDate = new Date(sessionData.loginDate);
            var expires_in = sessionData.expirationTime;
            var nowDate = new Date();
            var diff = (Math.abs(nowDate - loginDate)) / 60000;
            if (diff >= expires_in) {
                $window.localStorage.removeItem('sessionData');
                $location.path('/login')
            };
        }
    };

    sessionObject.userLogout = function () {
        $window.localStorage.removeItem('sessionData');
        sessionObject.checkSession();
    };

    return sessionObject; 
});