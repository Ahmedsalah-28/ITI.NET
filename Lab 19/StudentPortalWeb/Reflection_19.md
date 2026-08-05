# Reflection 19

## 1. My Lab Values

- Lab ID = 7

- MIN_GRADE_LAB
  = (7 mod 3) + 1.5
  = 1 + 1.5
  = **2.5**

- COURSE_COUNT
  = (7 mod 3) + 2
  = 1 + 2
  = **3**

---

## 2. Three Places Where Enrollment Can Be Rejected

An Enrollment can be rejected in three different places before it is stored in the database:

1. **Client-side validation** in the browser using unobtrusive validation.
2. **Server-side validation (ModelState)** using validation attributes such as `[Range]`.
3. **The database**, which enforces constraints such as the unique index on `(StudentId, CourseId)`.

My Part D change belongs to the **server-side ModelState validation** because I changed the `[Range]` attribute on the `Grade` property.

---

## 3. Why a Foreign Key Alone Is Not Enough

Using only a foreign key directly between `Student` and `Course` would only allow a simple relationship and would not store information about each enrollment. An `Enrollment` entity solves this by acting as a junction table that also stores additional data such as the enrollment date and the student's grade.

---

## 4. Choosing a Delete Behavior

If I added a future `Assignment` entity that belongs to a `Course`, I would choose **Cascade Delete**. When a course is deleted, its assignments should also be deleted because they have no meaning without their parent course.