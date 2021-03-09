Projestor.filter('stateFilter', function () {
    return function (issue) {
        switch (issue.state) {
            case 0:
                return "N/A";
            case 1:
                return "NOT STARTED";
            case 2:
                return "FROZEN"
            case 3:
                return "IN PROGRESS";
            case 4:
                return "FINISHED";
        }
    };
});