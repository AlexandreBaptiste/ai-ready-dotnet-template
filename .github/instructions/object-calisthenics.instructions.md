---
applyTo: '**/*.{cs,ts,java}'
description: Enforces Object Calisthenics principles for business domain code to ensure clean, maintainable, and robust code
---

# Object Calisthenics Rules

> ⚠️ **Warning:** This file contains the 9 original Object Calisthenics rules. No additional rules must be added, and none of these rules should be replaced or removed.

## Objective
This rule enforces the principles of Object Calisthenics to ensure clean, maintainable, and robust code in the backend, **primarily for business domain code**.

## Scope and Application
- **Primary focus**: Business domain classes (aggregates, entities, value objects, domain services)
- **Secondary focus**: Application layer services and use case handlers
- **Exemptions**: 
  - DTOs (Data Transfer Objects)
  - API models/contracts
  - Configuration classes
  - Simple data containers without business logic
  - Infrastructure code where flexibility is needed

## Key Principles

1. **One Level of Indentation per Method**: Ensure methods are simple and do not exceed one level of indentation.
   ```csharp
   // Bad Example - multiple levels of indentation
   public void SendNewsletter() {
       foreach (var user in users) {
           if (user.IsActive) {
               mailer.Send(user.Email);
           }
       }
   }

   // Good Example - filtered before sending
   public void SendNewsletter() {
       var activeUsers = users.Where(user => user.IsActive);
       foreach (var user in activeUsers) {
           mailer.Send(user.Email);
       }
   }
   ```

2. **Don't Use the ELSE Keyword**: Avoid using the `else` keyword to reduce complexity. Use early returns (Guard Clauses) instead.
   ```csharp
   // Bad Example
   public void ProcessOrder(Order order) {
       if (order.IsValid) {
           // Process order
       } else {
           // Handle invalid order
       }
   }

   // Good Example - Fail Fast with guard clause
   public void ProcessOrder(Order order) {
       if (order == null) throw new ArgumentNullException(nameof(order));
       if (!order.IsValid) throw new InvalidOperationException("Invalid order");
       // Process order
   }
   ```

3. **Wrap All Primitives and Strings**: Avoid using primitive types directly in your code. Wrap them in classes to provide meaningful context and behavior.
   ```csharp
   // Bad Example
   public class User { public string Name { get; set; } public int Age { get; set; } }

   // Good Example
   public class Age {
       private int value;
       public Age(int value) {
           if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Age cannot be negative");
           this.value = value;
       }
   }
   ```

4. **First Class Collections**: A class that contains a collection as an attribute should not contain any other attributes.
   ```csharp
   // Good Example
   public class Group {
       public int Id { get; private set; }
       public string Name { get; private set; }
       public GroupUserCollection UserCollection { get; private set; }
   }
   ```

5. **One Dot per Line**: Avoid violating the Law of Demeter — only have a single dot per line.
   ```csharp
   // Bad Example
   var userEmail = order.User.GetEmail().ToUpper().Trim();

   // Good Example
   public class Order {
       public NormalizedEmail ConfirmationEmail() => User.GetEmail();
   }
   var confirmationEmail = order.ConfirmationEmail();
   ```

6. **Don't Abbreviate**: Use meaningful names for classes, methods, and variables. Avoid abbreviations that can lead to confusion.

7. **Keep Entities Small**: Limit the size of classes and methods.
   - Maximum 10 methods per class
   - Maximum 50 lines per class
   - Maximum 10 classes per package or namespace

8. **No Classes with More Than Two Instance Variables**: Encourage single responsibility by limiting instance variables to two. *(ILogger or similar infrastructure loggers do not count.)*

9. **No Getters/Setters in Domain Classes**: Use private constructors and static factory methods for object creation. Domain classes should not expose public setters.
   ```csharp
   // Bad - domain class with public setters
   public class User { public string Name { get; set; } }

   // Good - domain class with encapsulation
   public class User {
       private string _name;
       private User(string name) { _name = name; }
       public static User Create(string name) => new User(name);
   }

   // Acceptable - DTO with public setters
   public class UserDto { public string Name { get; set; } }
   ```

## Implementation Guidelines
- **Domain Classes**: Apply all 9 rules strictly for business domain code.
- **Application Layer**: Apply these rules to use case handlers and application services.
- **DTOs and Data Objects**: Rules 3, 8, and 9 may be relaxed for DTOs. Public properties with getters/setters are acceptable for data transfer objects.
- **Testing**: Test classes may have relaxed rules for readability and maintainability.
- **Code Reviews**: Enforce these rules during code reviews for domain and application code. Be pragmatic about infrastructure and DTO code.
