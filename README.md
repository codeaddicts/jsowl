Welcome.
=====

jsowl is a programming language, based on and compiling to JavaScript.   
It's much cleaner than JavaScript, supports classes and is generally a lot more beautiful.

Just convice yourself:
```scala
def main {
	let test = new Test ();
	test.Say ("Hello, World!");

	class Test {
		public def Say (msg) {
			alert (msg);
		}
	}
}
```
versus
```javascript
(function () {
	function main () {
		var test = new Test ();
		test.Say ('Hello, World!');
		function Test () {
			this.Say = function (msg) {
				alert (msg);
			}
		}
	}
	main ();
}) ();
```