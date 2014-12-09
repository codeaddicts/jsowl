document.addEventListener("DOMContentLoaded", function (event) {
    // Main entry point
    function main() {
        var fx = new TestingFramework();
        fx.createTest('Equality', function () {
            return 1 == 1;
        },
        true);
        fx.createTest('Bitwise or', function () {
            return 0x02 | 0x20;
        },
        '34');
        fx.createTest('Bitwise and', function () {
            return 0x22 & 0x20;
        },
        '32');
        fx.createTest('Classes', function () {
            function sample() {
                this.Hello = function () {
                    return Say('Hello, World!');
                }
                function Say(something) {
                    return something;
                }
            }
            var x = new sample();
            return x.Hello();
        },
        'Hello, World!');
    }
    // A small testing class
    function TestingFramework() {
        this.createTest = function (name, func, expected) {
            log('Testing: ' + name);
            log('| Expected: ' + expected);
            var result = func();
            log('| Computed: ' + result);
            if (expected == result) {
                log('| Test passed');
            } else {
                log('| Test failed');
            }
        }
    }
    // A function which allows us to append
    // text to the html document
    function log(msg) {
        var elem = document.getElementById('results');
        elem.innerHTML = elem.innerHTML + msg + '<br/>';
    }
    main();
});