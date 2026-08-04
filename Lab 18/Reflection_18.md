# Reflection 18

## 1. Lab ID, CHIP_YEAR and CHIP_LABEL

My Lab ID is **7**.

CHIP_YEAR = (7 mod 4) + 1

= (3) + 1

= **4**

Since CHIP_YEAR is 4, CHIP_LABEL is **Final**.

---

## 2. Removing duplication

In Part B, I removed duplicated HTML markup by using a Partial View.

A similar idea appeared in previous sessions with validation, where the validation logic was written once in a custom validation attribute instead of being duplicated in multiple places.

---

## 3. Common pattern

A route constraint, a validation attribute, and a tag helper are all custom components that are written once and then used by the ASP.NET Core framework automatically.

The framework decides when to execute them based on where they are applied, allowing code to be reused instead of duplicated.

---

## 4. Rule vs Label

The **gpa-badge** contains a rule because it decides the student's classification based on the GPA value.

The **year-chip** contains a label because it only displays the student's academic year.

I would be more concerned about duplicating the GPA rule because if the classification criteria change, every duplicated copy would need to be updated.