# Reflection 16

## 1. Lab ID and Derived Values

Lab ID = 7

- MAX_YEAR = (7 mod 4) + 1 = 3 + 1 = 4
- MIN_GPA = 2.5 + (7 mod 3 × 0.5)
  = 2.5 + (1 × 0.5)
  = 3.0
- INTAKE_CODE = itiB
  because 7 mod 3 = 1, which corresponds to B.

---

## 2. Route Constraint Rejection

When a request is made to `/students/top/5`, the middleware first logs the `[START]` message. The routing system then checks the route constraints. Since the value `5` is outside the allowed range `1–4`, the route does not match. The framework returns a 404 response, the middleware logs `[END]`, and the controller action is never executed. No Entity Framework query is sent to the database.

---

## 3. Custom Constraint vs Built-in Constraint

Both the custom `intakecode` constraint and the built-in `int` constraint are used by the routing system to determine whether a route matches a request before the controller action executes.

The difference is that the `int` constraint is built into ASP.NET Core, while the `intakecode` constraint was implemented by creating a class that implements `IRouteConstraint` and registering it in the `ConstraintMap`.

---

## 4. Attribute Routing

The `/Students/About` URL returns 404 because the action is reachable only through its attribute route (`/about/ahmed`). This is a guarantee rather than a limitation because it ensures that the action is accessible only through the URL explicitly defined by the developer.