# Reflection 17

## 1. Lab ID and derived values

Lab ID = 7

- MIN_GPA_EDIT = 2.0 + (7 mod 5 × 0.3)
- 7 mod 5 = 2
- 2 × 0.3 = 0.6
- MIN_GPA_EDIT = 2.6

- MAX_YEAR_EDIT = (7 mod 3) + 2
- 7 mod 3 = 1
- MAX_YEAR_EDIT = 3

---

## 2. The three places where invalid input can be rejected

Bad input can be rejected in three places, in this order:

1. Route matching and route constraints.
2. Model binding and model validation.
3. Database constraints when saving changes.

My Part D change belongs to the second stage, model validation, because I changed the Range validation attributes on the Student model.

---

## 3. Why pressing F5 no longer creates duplicate data

After a successful save, the browser receives a redirect response and performs a new GET request to the confirmation page. Because the current page is now a GET request instead of the original POST request, pressing F5 only repeats the GET request and does not submit the form again.

---

## 4. Required/MaxLength vs. Range

Both Required, MaxLength, and Range are written as validation attributes placed on model properties.

The difference is that Required and MaxLength can also influence the database schema when migrations are created, while Range is a validation-only attribute that checks values in the application and does not change the database schema.