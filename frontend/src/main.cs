// This higher-order function takes an integer 'n' and returns a new function (Func<int, bool>)
Func<int, bool> isDivisible(int n)
{
    // The returned function uses 'n' in its scope (closure)
    return x => x % n == 0;
}

// Call the higher-order function to get specialized functions
var isDivisibleByThree = isDivisible(3);
var isDivisibleByFour = isDivisible(4);

// Use the specialized functions
var result1 = isDivisibleByThree(9); // true
var result2 = isDivisibleByFour(9); // false




// XUI MyUIComponent()
// {
//     // Create a new UI component
//     var component = new XUI();

//     // Set properties and add child elements
//     component.SetProperty("width", 200);
//     component.SetProperty("height", 100);
//     component.AddChild(new XUILabel("Hello, World!"));

//     return component;
// }