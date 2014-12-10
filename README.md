Welcome.
=====

jsowl is a programming language, based on and compiling to JavaScript.   
It's much cleaner than JavaScript, supports classes and is generally a lot more beautiful.

Just convice yourself:
```scala
def Hello {
	let test = new Sample ();
    test.Hello ();
}

class Sample {
	
	// Public method
	public def Hello {
    	Say ("Hello, World!");
    }
    
    // Private method
    def Say (something) {
    	alert (something);
    }
    
}
```
versus
```javascript
function Hello () {
	var test = new Sample ();
    test.Hello ();
}

function Sample () {
	this.Hello = function () {
    	Say ("Hello, World!");
    }
    function Say (something) {
    	alert (something);
    }
}
```