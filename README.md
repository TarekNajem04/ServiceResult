
### **Dynamic Service Result Builder (Exception Pattern, Result Pattern, Decorator Pattern)**

What **type** will we **return** when calling **Methods of Services**? 
What **properties** will it contain? 
Will the **expected result** of this method be sufficient?

These questions are always in our minds when we start designing our special services for our program. In this topic we will try to talk about some scenarios about how the Methods of Services communicate with the ASP.NET Core 3.1 Controller.
At first you will think that this topic is special, but after you finish reading it, you will find that you can apply it in any scenario in which you need to return a meaningful value to the **Caller**.
We used to return one result for service methods, which is the possible result of that function (like Boolean, Integer, Models, etc.).

**Scenario 1 - Throw Exceptions Or Exception Pattern
Scenario 2 – Result Object Or Result Pattern
Scenario 3 – Aspect-Oriented Programming Or Decorator Pattern**
