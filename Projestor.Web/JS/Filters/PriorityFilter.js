Projestor.filter("prioFilter", function () {
    return function (issue) {
        switch (issue.prio) {
            case 0:
                return "NA";
            case 1:
                return "LOW";
            case 2:
                return "INTERMEDIATE";
            case 3:
                return "HIGH";
        }
    };
})